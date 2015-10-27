﻿using ChartControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ChartViewU8
{
    public class ResultEventArgs : EventArgs
    {
        public ResultEventArgs(bool isOk)
        {
            IsOk = isOk;
        }

        public bool IsOk
        {
            set;
            get;
        }
    }
    /// <summary>
    /// Interaction logic for SettingsEditor.xaml
    /// </summary>
    public partial class SettingsEditor : UserControl
    {
        public static readonly DependencyProperty BorderThickness1Property;
        public static readonly DependencyProperty BorderProperty;
        public static readonly DependencyProperty YScaleDockProperty;
        public static readonly DependencyProperty XScaleDockProperty;
        public static readonly DependencyProperty ControlBackgroundProperty;
        public static readonly DependencyProperty YScaleWidthProperty;
        public static readonly DependencyProperty XScaleHeightProperty;
        public static readonly DependencyProperty CoordinateTypeProperty;
        public static readonly DependencyProperty CursorLinesProperty;
        public static readonly DependencyProperty CursorLinesThicknessProperty;
        public static readonly DependencyProperty CursorLinesDashesProperty;
        public static readonly DependencyProperty ScaleLineColorProperty;
        public static readonly DependencyProperty ScaleLineThicknessProperty;
        public static readonly DependencyProperty ScaleLineDashesProperty;
        public static readonly DependencyProperty FontFamilyProperty;
        public static readonly DependencyProperty FontSizeProperty;

        public static readonly DependencyProperty widthArrayProperty;
        public static readonly DependencyProperty brushesProperty;
        public static readonly DependencyProperty yScaleDocksProperty;
        public static readonly DependencyProperty xScaleDocksProperty;
        public static readonly DependencyProperty bgBrushesProperty;
        public static readonly DependencyProperty dockWidthArrayProperty;
        public static readonly DependencyProperty coordinateTypesProperty;
        public static readonly DependencyProperty dashArrayProperty;
        public static readonly DependencyProperty fontNamesProperty;
        public static readonly DependencyProperty fontSizesProperty;

        static SettingsEditor()
        {
            YScaleWidthProperty = DependencyProperty.Register("YScaleWidth", typeof(int), typeof(SettingsEditor), new PropertyMetadata(60));
            XScaleHeightProperty = DependencyProperty.Register("XScaleHeight", typeof(int), typeof(SettingsEditor), new PropertyMetadata(15));
            BorderThickness1Property = DependencyProperty.Register("BorderThickness1", typeof(int), typeof(SettingsEditor), new PropertyMetadata(1));
            BorderProperty = DependencyProperty.Register("Border", typeof(string), typeof(SettingsEditor), new PropertyMetadata("Black"));
            YScaleDockProperty = DependencyProperty.Register("YScaleDock", typeof(YScaleDock), typeof(SettingsEditor), new PropertyMetadata(YScaleDock.Right));
            ControlBackgroundProperty = DependencyProperty.Register("ControlBackground", typeof(string), typeof(SettingsEditor), new PropertyMetadata("White"));
            XScaleDockProperty = DependencyProperty.Register("XScaleDock", typeof(XScaleDock), typeof(SettingsEditor), new PropertyMetadata(XScaleDock.Bottom));
            CoordinateTypeProperty = DependencyProperty.Register("CoordinateType", typeof(CoordinateType), typeof(SettingsEditor), new PropertyMetadata(CoordinateType.Linear));
            CursorLinesProperty = DependencyProperty.Register("CursorLines", typeof(string), typeof(SettingsEditor), new PropertyMetadata("Gray"));
            CursorLinesThicknessProperty = DependencyProperty.Register("CursorLinesThickness", typeof(int), typeof(SettingsEditor), new PropertyMetadata(1));
            CursorLinesDashesProperty = DependencyProperty.Register("CursorLinesDashes", typeof(string), typeof(SettingsEditor), new PropertyMetadata(null));

            ScaleLineColorProperty = DependencyProperty.Register("ScaleLineColor", typeof(string), typeof(SettingsEditor), new PropertyMetadata("Gray"));
            ScaleLineThicknessProperty = DependencyProperty.Register("ScaleLineThickness", typeof(int), typeof(SettingsEditor), new PropertyMetadata(1));
            ScaleLineDashesProperty = DependencyProperty.Register("ScaleLineDashes", typeof(string), typeof(SettingsEditor), new PropertyMetadata(null));

            FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(string), typeof(SettingsEditor), new PropertyMetadata("Arial"));
            FontSizeProperty = DependencyProperty.Register("FontSize", typeof(int), typeof(SettingsEditor), new PropertyMetadata(10));


            widthArrayProperty = DependencyProperty.Register("widthArray", typeof(int[]), typeof(SettingsEditor), new PropertyMetadata(null));
            brushesProperty = DependencyProperty.Register("brushes", typeof(string[]), typeof(SettingsEditor), new PropertyMetadata(null));
            yScaleDocksProperty = DependencyProperty.Register("yScaleDocks", typeof(YScaleDock[]), typeof(SettingsEditor), new PropertyMetadata(null));
            xScaleDocksProperty = DependencyProperty.Register("xScaleDocks", typeof(XScaleDock[]), typeof(SettingsEditor), new PropertyMetadata(null));
            bgBrushesProperty = DependencyProperty.Register("bgBrushes", typeof(string[]), typeof(SettingsEditor), new PropertyMetadata(null));
            dockWidthArrayProperty = DependencyProperty.Register("dockWidthArray", typeof(int[]), typeof(SettingsEditor), new PropertyMetadata(null));
            coordinateTypesProperty = DependencyProperty.Register("coordinateTypes", typeof(CoordinateType[]), typeof(SettingsEditor), new PropertyMetadata(null));
            dashArrayProperty = DependencyProperty.Register("dashArray", typeof(string[]), typeof(SettingsEditor), new PropertyMetadata(null));
            fontNamesProperty = DependencyProperty.Register("fontNames", typeof(string[]), typeof(SettingsEditor), new PropertyMetadata(null));
            fontSizesProperty = DependencyProperty.Register("fontSizes", typeof(int[]), typeof(SettingsEditor), new PropertyMetadata(null));
        }

        public SettingsEditor()
        {
            this.InitializeComponent();
        }

        public int BorderThickness1
        {
            get
            {
                return (int)GetValue(BorderThickness1Property);
            }
            set
            {
                SetValue(BorderThickness1Property, value);
            }
        }

        public string Border
        {
            get
            {
                return (string)GetValue(BorderProperty);
            }
            set
            {
                SetValue(BorderProperty, value);
            }
        }

        public YScaleDock YScaleDock
        {
            get
            {
                return (YScaleDock)GetValue(YScaleDockProperty);
            }
            set
            {
                SetValue(YScaleDockProperty, value);
            }
        }

        public XScaleDock XScaleDock
        {
            get
            {
                return (XScaleDock)GetValue(XScaleDockProperty);
            }
            set
            {
                SetValue(XScaleDockProperty, value);
            }
        }

        public CoordinateType CoordinateType
        {
            get
            {
                return (CoordinateType)GetValue(CoordinateTypeProperty);
            }
            set
            {
                SetValue(CoordinateTypeProperty, value);
            }
        }
        public string ControlBackground
        {
            get
            {
                return (string)GetValue(ControlBackgroundProperty);
            }
            set
            {
                SetValue(ControlBackgroundProperty, value);
            }
        }

        public int YScaleWidth
        {
            get
            {
                return (int)GetValue(YScaleWidthProperty);
            }
            set
            {
                SetValue(YScaleWidthProperty, value);
            }
        }

        public int XScaleHeight
        {
            get
            {
                return (int)GetValue(XScaleHeightProperty);
            }
            set
            {
                SetValue(XScaleHeightProperty, value);
            }
        }

        public string CursorLines
        {
            get
            {
                return (string)GetValue(CursorLinesProperty);
            }
            set
            {
                SetValue(CursorLinesProperty, value);
            }
        }

        public double CursorLinesThickness
        {
            get { return (int)GetValue(CursorLinesThicknessProperty); }
            set { SetValue(CursorLinesThicknessProperty, value); }
        }

        public string CursorLinesDashes
        {
            get { return (string)GetValue(CursorLinesDashesProperty); }
            set { SetValue(CursorLinesDashesProperty, value); }
        }

        public string ScaleLineColor
        {
            get
            {
                return (string)GetValue(ScaleLineColorProperty);
            }
            set
            {
                SetValue(ScaleLineColorProperty, value);
            }
        }

        public double ScaleLineThickness
        {
            get { return (int)GetValue(ScaleLineThicknessProperty); }
            set { SetValue(ScaleLineThicknessProperty, value); }
        }

        public string ScaleLineDashes
        {
            get { return (string)GetValue(ScaleLineDashesProperty); }
            set { SetValue(ScaleLineDashesProperty, value); }
        }

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public int[] widthArray
        {
            get { return (int[])GetValue(widthArrayProperty); }
            set { SetValue(widthArrayProperty, value); }
        }

        public string[] brushes
        {
            get { return (string[])GetValue(brushesProperty); }
            set { SetValue(brushesProperty, value); }
        }
        public YScaleDock[] yScaleDocks
        {
            get { return (YScaleDock[])GetValue(yScaleDocksProperty); }
            set { SetValue(yScaleDocksProperty, value); }
        }
        public XScaleDock[] xScaleDocks
        {
            get { return (XScaleDock[])GetValue(xScaleDocksProperty); }
            set { SetValue(xScaleDocksProperty, value); }
        }
        public string[] bgBrushes
        {
            get { return (string[])GetValue(bgBrushesProperty); }
            set { SetValue(bgBrushesProperty, value); }
        }
        public int[] dockWidthArray
        {
            get { return (int[])GetValue(dockWidthArrayProperty); }
            set { SetValue(dockWidthArrayProperty, value); }
        }
        public CoordinateType[] coordinateTypes
        {
            get { return (CoordinateType[])GetValue(coordinateTypesProperty); }
            set { SetValue(coordinateTypesProperty, value); }
        }
        public string[] dashArray
        {
            get { return (string[])GetValue(dashArrayProperty); }
            set { SetValue(dashArrayProperty, value); }
        }
        public string[] fontNames
        {
            get { return (string[])GetValue(fontNamesProperty); }
            set { SetValue(fontNamesProperty, value); }
        }
        public int[] fontSizes
        {
            get { return (int[])GetValue(fontSizesProperty); }
            set { SetValue(fontSizesProperty, value); }
        }

        public event EventHandler<ResultEventArgs> ResultReturned;

        private void RevokeOk()
        {
            if (ResultReturned != null)
                ResultReturned(this, new ResultEventArgs(true));
        }

        private void RevokeCancel()
        {
            if (ResultReturned != null)
                ResultReturned(this, new ResultEventArgs(false));
        }

        private void OnClickOk(object sender, RoutedEventArgs e)
        {
            RevokeOk();
            
        }

        private void OnClickCancel(object sender, RoutedEventArgs e)
        {
            RevokeCancel();
        }
    }
}
