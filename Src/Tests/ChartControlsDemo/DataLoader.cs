﻿using ChartControls;
using Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChartControlsDemo
{
    class StockVolumnList
    {
        public List<StockItem> Prices;
        public List<VolumnItem> Volumns;
    }

    class DataLoader
    {
        private const string sFileName = "s.json";

        private List<CompleteShare> shares;

        public DataLoader()
        {
            Load();
        }


        public List<ChartItem> GetChartItems(string id)
        {
            List<ChartItem> result = null;

            var share = shares.FirstOrDefault(s => s.StockId == id);
            if (share != null)
            {
                result = FromSimpleStock(share);
            }

            return result;
        }

        public StockVolumnList GetStockItems(string id)
        {
            StockVolumnList result = null;

            var share = shares.FirstOrDefault(s => s.StockId == id);
            if (share != null)
            {
                result = FromCompleteStock(share);
            }

            return result;
        }

        private void Load()
        {
            shares = new List<CompleteShare>();

            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            location = Path.Combine(location, sFileName);

            var content = File.ReadAllText(location, Encoding.UTF8);

            var jObjects = JArray.Parse(content);

            foreach (var jObject in jObjects)
            {
                CompleteShare share = jObject.ToObject<CompleteShare>();
                if(share != null)
                    shares.Add(share);
            }
        }

        private List<ChartItem> FromSimpleStock(SimpleShare s)
        {
            List<ChartItem> ctList = null;
            if (s.Dates != null)
            {
                ctList = new List<ChartItem>(s.Dates.Count);

                double preClose = 0;
                for (int i = 0; i < s.Dates.Count; i++)
                {
                    ctList.Add(new ChartItem()
                    {
                        Date = s.Dates[i],
                        Value = s.Closes[i],
                        ValueChange = i != 0 ? (s.Closes[i] - preClose) / preClose : 0
                    });

                    preClose = s.Closes[i];
                }
            }

            return ctList;
        }

        private StockVolumnList FromCompleteStock(CompleteShare s)
        {
            StockVolumnList svList = new StockVolumnList();

            if (s.Dates != null)
            {
                svList.Prices = new List<StockItem>(s.Dates.Count);
                svList.Volumns = new List<VolumnItem>(s.Dates.Count);

                double preClose = 0;
                for (int i = 0; i < s.Dates.Count; i++)
                {
                    var sItem = new StockItem()
                    {
                        Date = s.Dates[i],
                        Value = s.Closes[i],
                        ValueChange = i != 0 ? (s.Closes[i] - preClose) / preClose : 0,
                        High = s.Highs[i],
                        Low = s.Lows[i],
                        Open = s.Opens[i]
                    };

                    svList.Prices.Add(sItem);

                    svList.Volumns.Add(new VolumnItem()
                    {
                        Date = s.Dates[i],
                        Value = s.Volumns[i],
                        Turnover = s.Turnovers[i],
                        IsRaise = s.Closes[i] > s.Opens[i] || (s.Closes[i] == s.Opens[i] && sItem.CloseChange > 0)
                    });
                    preClose = s.Closes[i];
                }
            }

            return svList;
        }
    }
}
