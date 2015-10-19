﻿using ChartControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Common;
using ChartControls.Drawing;

namespace ChartControlsDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string id1 = "000001";
        private const string id2 = "399001";

        private const string priceFormat = "F2";
        private const string percentFormat = "P2";
        private const string volumnFormat = "N2";
        private MainViewModel model;
        private DataLoader loader;
        public MainWindow()
        {
            InitializeComponent();

            DataContext = model = new MainViewModel();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            loader = new DataLoader();
            AdjustViews(true);
            QueryChartCollection(id1);
        }

        private void price_CursorMoved(object sender, CursorMovedRoutedEventArgs e)
        {
            if (e != null && e.CurrentItems != null)
            {
                int iIndex = 1;
                foreach(var item in e.CurrentItems)
                {
                    var collection = price.FindCollection(item.Id);
                    if (collection == null)
                        continue;
                    model.Date = item.Item.Date;
                    var realItem = item.Item as StockItem;
                    if (realItem != null)
                    {
                        var sRealItem = item.Item as StockValuesItem;

                        model.SetValue<string>(iIndex, MainViewModel.priceName, realItem.Close.ToString(priceFormat));
                        model.SetValue<Brush>(iIndex, MainViewModel.priceClrName, collection.Pen.Brush);
                        iIndex++;

                        model.SetValue<string>(iIndex, MainViewModel.priceName, realItem.CloseChange.ToString(percentFormat));
                        model.SetValue<Brush>(iIndex, MainViewModel.priceClrName, collection.Pen.Brush);
                        iIndex++;

                        model.SetValue<string>(iIndex, MainViewModel.priceName, realItem.Open.ToString(priceFormat));
                        model.SetValue<Brush>(iIndex, MainViewModel.priceClrName, collection.Pen.Brush);
                        iIndex++;

                        model.SetValue<string>(iIndex, MainViewModel.priceName, realItem.High.ToString(priceFormat));
                        model.SetValue<Brush>(iIndex, MainViewModel.priceClrName, collection.Pen.Brush);
                        iIndex++;

                        model.SetValue<string>(iIndex, MainViewModel.priceName, realItem.Low.ToString(priceFormat));
                        model.SetValue<Brush>(iIndex, MainViewModel.priceClrName, collection.Pen.Brush);
                        iIndex++;

                        if (sRealItem != null)
                        {
                            var sCollection = collection as StockValuesItemCollection;
                            for (int i = 0; i < sRealItem.Values.Count; i ++)
                            {
                                model.SetValue<string>(iIndex, MainViewModel.priceName, sRealItem.Values[i].ToString(priceFormat));
                                model.SetValue<Brush>(iIndex, MainViewModel.priceClrName, sCollection.Pens[i].Brush);

                                iIndex++;

                                model.SetValue<string>(iIndex, MainViewModel.priceName, sRealItem.ValueChanges[i].ToString(percentFormat));
                                model.SetValue<Brush>(iIndex, MainViewModel.priceClrName, sCollection.Pens[i].Brush);

                                iIndex++;
                            }
                        }
                    }
                    else
                    {
                        model.SetValue<string>(iIndex, MainViewModel.priceName, item.Item.Value.ToString(priceFormat));
                        model.SetValue<Brush>(iIndex, MainViewModel.priceClrName, collection.Pen.Brush);
                        iIndex++;

                        if (iIndex == 2)
                        {
                            model.SetValue<string>(iIndex, MainViewModel.priceName, item.Item.ValueChange.ToString(percentFormat));
                            model.SetValue<Brush>(iIndex, MainViewModel.priceClrName, collection.Pen.Brush);
                            iIndex++;
                        }

                        var mRealItem = item.Item as MultipleChartItem;
                        if (mRealItem != null)
                        {
                            var mCollection = collection as MultipleChartItemCollection;
                            for (int i = 0; i < mRealItem.Values.Count; i ++)
                            {
                                model.SetValue<string>(iIndex, MainViewModel.priceName, mRealItem.Values[i].ToString(priceFormat));
                                model.SetValue<Brush>(iIndex, MainViewModel.priceClrName, mCollection.Pens[i].Brush);
                                iIndex++;
                            }
                        }
                    }
                }

                
            }
        }

        private void volumn_CursorMoved(object sender, CursorMovedRoutedEventArgs e)
        {
            if (e != null && e.CurrentItems != null)
            {
                foreach (var item in e.CurrentItems)
                {
                    var collection = volumn.FindCollection(item.Id) as VolumnItemCollection;
                    if (collection != null && item.Item != null)
                    {
                        var realItem = item.Item as VolumnItem;
                        if (realItem != null)
                        {
                            model.SetValue<string>(1, MainViewModel.volumnName, realItem.Volumn.ToString(volumnFormat));
                            model.SetValue<Brush>(1, MainViewModel.volumnClrName, realItem.IsRaise ? collection.RaisePen.Brush : collection.FallPen.Brush);

                            model.SetValue<string>(2, MainViewModel.volumnName, realItem.Turnover.ToString(volumnFormat));
                            model.SetValue<Brush>(2, MainViewModel.volumnClrName, realItem.IsRaise ? collection.RaisePen.Brush : collection.FallPen.Brush);
                        }
                    }
                }
               
            }
        }

        private void AdjustViews(bool isHideVolumView)
        {
            if (isHideVolumView)
            {
                price.SetValue(Grid.RowSpanProperty, 3);
                volumn.Visibility = Visibility.Collapsed;
                volumnTitle.Visibility = Visibility.Collapsed;
            }
            else
            {
                if(volumn.Visibility != Visibility.Visible)
                {
                    volumn.Visibility = Visibility.Visible;
                    volumnTitle.Visibility = Visibility.Visible;
                    price.SetValue(Grid.RowSpanProperty, 1);
                }
            }
        }

    
        private void OnClickView(object sender, RoutedEventArgs e)
        {
            var menu = this.FindResource("ViewsMenu") as ContextMenu;
            if(menu != null)
            {
                menu.IsOpen = true;
            }
        }

        private void OnViewCurve(object sender, RoutedEventArgs e)
        {
            model.Reset();
            AdjustViews(true);
            QueryChartCollection(id1);
            
        }

        private void OnViewCandle(object sender, RoutedEventArgs e)
        {
            model.Reset();
            AdjustViews(false);
            QueryCandleCollection(id1);
        }

        private void OnViewMultiple(object sender, RoutedEventArgs e)
        {
            model.Reset();
            AdjustViews(true);
            QueryMultipleCollection(id1);
        }

        private void OnViewComparison(object sender, RoutedEventArgs e)
        {
            model.Reset();
            AdjustViews(true);
            QueryCandleComparisonCollection(id1, id2);
        }

        private void OnViewOverlap(object sender, RoutedEventArgs e)
        {
            model.Reset();
            AdjustViews(true);
            QueryChartOverlapCollection(id1, id2);
        }

        private void QueryChartCollection(string id)
        {
            var chartItems = loader.GetChartItems(id);
            if (chartItems == null || !chartItems.Any())
            {
                Debug.WriteLine("Can not load chart items");
                return;
            }

            var collection = CreateCollection(id, chartItems, ChartItemType.Linear, DrawingObjectFactory.CreatePen(model.RaiseBrush, 1), null);
            price.SetMainCollection(collection);
            price.ForceDraw();
        }

        private void QueryCandleCollection(string id)
        {
            var svItems = loader.GetStockItems(id);
            if (svItems == null || svItems.Prices == null || svItems.Volumns == null)
            {
                Debug.WriteLine("Can not load candle items");
                return;
            }

            var collection = CreateCollection(id, svItems.Prices, ChartItemType.Candle, 
                DrawingObjectFactory.CreatePen(model.RaiseBrush, 1), DrawingObjectFactory.CreatePen(model.FallBrush, 1));
            price.SetMainCollection(collection);

            var maCollections = CreateMVCollections(svItems.Prices);
            foreach (var maColl in maCollections)
            {
                price.AddAssistCollection(maColl);
            }

            var vCollection = CreateCollection(id, svItems.Volumns, ChartItemType.Volumn, 
                DrawingObjectFactory.CreatePen(model.RaiseBrush, 1), DrawingObjectFactory.CreatePen(model.FallBrush, 1));
            volumn.SetMainCollection(vCollection);
            volumn.AddConnection(price);
            price.ForceDraw();
        }

        private void QueryMultipleCollection(string id)
        {
            var chartItems = loader.GetStockItems(id);
            if (chartItems == null || chartItems.Prices == null)
            {
                Debug.WriteLine("Can not load chart items");
                return;
            }

            var maColls = CreateMVCollections(chartItems.Prices).ToArray();
            IEnumerable<ChartItem>[] cItemsList = new IEnumerable<ChartItem>[maColls.Count() + 1];
            IPen[] pens = new IPen[maColls.Count() + 1];

            cItemsList[0] = chartItems.Prices;
            pens[0] = DrawingObjectFactory.CreatePen(model.RaiseBrush, 1);

            for(int i = 1; i < cItemsList.Length; i ++)
            {
                cItemsList[i] = maColls[i - 1].Items;
                pens[i] = maColls[i - 1].Pen;
            }

            var collection = CreateCollection(id, cItemsList, pens);
            price.SetMainCollection(collection);
            price.ForceDraw();
        }

        private void QueryCandleComparisonCollection(string id, string id2)
        {
            var svItems = loader.GetStockItems(id);
            if (svItems == null || svItems.Prices == null)
            {
                Debug.WriteLine("Can not load chart items");
                return;
            }

            var chartItems2 = loader.GetChartItems(id2);
            if (chartItems2 == null || !chartItems2.Any())
            {
                Debug.WriteLine("Can not load chart items");
                return;
            }

            var collection = CreateCollection(id, svItems.Prices, chartItems2, ChartItemType.Candle, 
                DrawingObjectFactory.CreatePen(model.RaiseBrush, 1), DrawingObjectFactory.CreatePen(model.FallBrush, 1), 
                new IPen[] { DrawingObjectFactory.CreatePen(model.ContrastBrush, 1) }
                );
            price.SetMainCollection(collection);
            price.CoordinateType = CoordinateType.Percentage;
            price.ForceDraw();
        }

        private void QueryChartOverlapCollection(string id, string id2)
        {
            var chartItems = loader.GetChartItems(id);
            if (chartItems == null || !chartItems.Any())
            {
                Debug.WriteLine("Can not load chart items");
                return;
            }

            var chartItems2 = loader.GetChartItems(id2);
            if (chartItems2 == null || !chartItems2.Any())
            {
                Debug.WriteLine("Can not load chart items");
                return;
            }

            var collection = CreateCollection(id, chartItems, ChartItemType.Linear, DrawingObjectFactory.CreatePen(model.RaiseBrush, 1), null);
            var collection2 = CreateCollection(id2, chartItems2, ChartItemType.Linear, DrawingObjectFactory.CreatePen(model.FallBrush, 1), null);
            price.SetMainCollection(collection);
            price.AddAssistCollection(collection2, true);
            price.ForceDraw();
        }

        enum ChartItemType { Linear, Candle, Volumn };

        private ChartItemCollection CreateCollection(string id, IEnumerable<ChartItem> chartItems, ChartItemType type, IPen pen1, IPen pen2)
        {
            CollectionId cId = new CollectionId(id);
            ChartItemCollection coll = CreateChartItemCollection(type, chartItems, cId, pen1, pen2);
            return coll;
        }

        private ChartItemCollection CreateCollection(string id, IEnumerable<ChartItem> chartItems, IEnumerable<ChartItem> chartItems2, ChartItemType type, IPen pen1, IPen pen2, IPen[] otherPens)
        {
            CollectionId cId = new CollectionId(id);
            ChartItemCollection coll = CreateSingleStockCollection(type, chartItems, chartItems2, cId, pen1, pen2, otherPens);
            return coll;
        }

        private ChartItemCollection CreateCollection(string id, IEnumerable<ChartItem>[] chartItemsList, IPen[] pens)
        {
            CollectionId cId = new CollectionId(id);
            ChartItemCollection coll = CreateMultipleItemCollection(chartItemsList, cId, pens);
            return coll;
        }

        private ChartItemCollection CreateChartItemCollection(ChartItemType type, IEnumerable<ChartItem> chartItems, CollectionId id, IPen pen1, IPen pen2)
        {
            ChartItemCollection coll = null;
            if (type == ChartItemType.Linear)
            {
                coll = new ChartItemCollection(id, chartItems, pen1, null, true);
            }
            else if (type == ChartItemType.Candle)
            {
                coll = new StockItemCollection(id, chartItems.OfType<StockItem>(), pen1, pen2, null);
            }
            else if (type == ChartItemType.Volumn)
            {
                coll = new VolumnItemCollection(id, chartItems.OfType<VolumnItem>(), pen1, pen2);
            }

            return coll;
        }

        private ChartItemCollection CreateMultipleItemCollection(IEnumerable<ChartItem>[] chartItemsList, CollectionId id, IPen[] pens)
        {
            List<MultipleChartItem> mItems = new List<MultipleChartItem>(chartItemsList[0].Count());

            IEnumerator<ChartItem>[] its = new IEnumerator<ChartItem>[chartItemsList.Length];

            for (int i = 1; i < chartItemsList.Length; i++)
            {
                its[i] = chartItemsList[i].GetEnumerator();
                its[i].MoveNext();
            }

            double[] invalidValues = new double[chartItemsList.Length - 1];
            for(int i = 0; i < invalidValues.Length; i ++)
            {
                invalidValues[i] = ChartItemCollection.valueNA;
            }
            foreach (var cItem in chartItemsList[0])
            {
                MultipleChartItem mItem = new MultipleChartItem()
                {
                    Date = cItem.Date,
                    Value = cItem.Value,
                    ValueChange = cItem.ValueChange,
                    Values = new List<double>(invalidValues),
                    ValueChanges = new List<double>(invalidValues)
                };

                for(int i = 1; i < chartItemsList.Length; i ++)
                {
                    if (its[i].Current.Date > cItem.Date)
                    {
                        continue;
                    }

                    mItem.Values[i - 1] = its[i].Current.Value;
                    mItem.ValueChanges[i - 1] = its[i].Current.ValueChange;
                    its[i].MoveNext();
                }
                mItems.Add(mItem);
            }

            return new MultipleChartItemCollection(id, mItems, pens);    
        }

        private ChartItemCollection CreateSingleStockCollection(ChartItemType type, IEnumerable<ChartItem> chartItems, IEnumerable<ChartItem> chartItems2, CollectionId id, IPen pen1, IPen pen2, IPen[] otherPens)
        {
            List<StockValuesItem> ssItems = new List<StockValuesItem>(chartItems.Count());

            var it = chartItems2.GetEnumerator();
            
            foreach (var cItem in chartItems)
            {
                it.MoveNext();
                var sItem = cItem as StockItem;
                StockValuesItem ssItem = new StockValuesItem()
                {
                    Date = sItem.Date,
                    High = sItem.High,
                    Low = sItem.Low,
                    Open = sItem.Open,
                    Close = sItem.Close,
                    CloseChange = sItem.CloseChange,
                    Values = new List<double>(new double[] { it.Current.Value }),
                    ValueChanges = new List<double>(new double[] { it.Current.ValueChange })
                };

                ssItems.Add(ssItem);
            }
            var coll = new StockValuesItemCollection(id, ssItems, pen1, pen2, null, otherPens);
            
                
            return coll;
        }

        public List<double> MA(int period, List<StockItem> chartItems)
        {
            int count = chartItems.Count - period + 1;
            if (count < 0)
                count = 0;

            List<double> maResult = new List<double>(count);
            if (count == 0)
                return maResult;

            double total = 0;

            for (int i = 0; i < period; i++)
            {
                total += chartItems[i].Value;
            }
            maResult.Add(total / period);

            for (int i = 0; i < count - 1; i++)
            {
                total -= chartItems[i].Value;
                total += chartItems[i + period].Value;
                maResult.Add(total / (float)period);
            }

            return maResult;
        }

        private int mvCount = 3;
        private int[] mvUnits = new int[] { 10, 20, 30, 60, 120, 250 };
        //{ 10, 20, 30, 60, 120, 250};
        private Brush[] mvBrushs = new Brush[] { Brushes.Salmon, Brushes.Purple, Brushes.Orange, Brushes.Salmon, Brushes.Purple, Brushes.Orange };

        public IEnumerable<ChartItemCollection> CreateMVCollections(List<StockItem> chartItems, int mvStart = 0)
        {
            for (int i = mvStart; i < mvStart + mvCount; i++)
            {
                var maPeriod = mvUnits[i];
                var maData = MA(maPeriod, chartItems);

                List<ChartItem> mvItems = new List<ChartItem>(maData.Count);
                for (int j = 0; j < maData.Count; j++)
                {
                    mvItems.Add(new ChartItem() { Value = maData[j], Date = chartItems[j + maPeriod - 1].Date });
                }

                var id = new CollectionId(maPeriod.ToString());
                ChartItemCollection collMa = new ChartItemCollection(id, mvItems, DrawingObjectFactory.CreatePen(mvBrushs[i], 1), null, true, false);
                yield return collMa;
            }
        }

        private void price_SelectItems(object sender, SelectItemsEventArgs e)
        {
            if (e.Items.Any())
            {
                System.Windows.MessageBox.Show(string.Format("Select items range {0}-{1}",
                    e.Items.First().Date.ToString("yyyyMMdd"), e.Items.Last().Date.ToString("yyyyMMdd")), "Selection");

            }
        }
    }
}
