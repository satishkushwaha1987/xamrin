using CHSBackOffice.Models.ApexCharts.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using Xamarin.Forms;

namespace CHSBackOffice.Models.ApexCharts
{
    public sealed class ApexChartConfig
    {
        public ApexChartModel Chart { set; get; }
        public string[] Colors { set; get; }
        public ApexChartDataLabels DataLabels { set; get; }
        public ApexChartGrid Grid { set; get; }
        public ApexChartFill Fill { set; get; }
        public ApexChartLegend Legend { set; get; }
        public ApexChartPlotOptions PlotOptions { set; get; }
        public ApexChartConfigSeries[] Series { set; get; }
        public ApexChartStroke Stroke { set; get; }
        public ApexChartTooltip Tooltip { set; get; }
        public ApexChartConfigXaxis Xaxis { set; get; }
        public ApexChartConfigYaxis Yaxis { set; get; }

        #region "Fabric Methods"

        public static ApexChartConfig CreateBarChart()
        {
            return new ApexChartConfig
            {
                Chart = new ApexChartModel
                {
                    Type = ApexChartType.bar,
                    Height = Device.Idiom == TargetIdiom.Phone ? "96%" : "98%",
                    Width = "100%",
                    Toolbar = new ApexChartToolbar
                    {
                        Show = false
                    }
                },
                Colors = new string[] { "#C56C28" },
                DataLabels = new ApexChartDataLabels
                {
                    Enabled = true,
                    OffsetY = Device.Idiom == TargetIdiom.Phone ? -14 : -20,
                    Style = new ApexChartDataLabelsStyle
                    {
                        Colors = new string[] { "#304758" },
                        FontFamily = "KlavikaRegular"
                    }
                },
                Fill = new ApexChartFill
                {
                    Type = ApexChartFillType.gradient,
                    Gradient = new ApexChartFillGradient
                    {
                        Type = ApexChartFillGradientType.vertical,
                        ShadeIntensity = 0.2F,
                        OpacityFrom = 0.8F,
                        OpacityTo = 1F,
                        Stops = new ushort[] { 30, 100 }
                    }
                },
                PlotOptions = new ApexChartPlotOptions
                {
                    Bar = new ApexChartPlotOptionsBar
                    {
                        DataLabels = new ApexChartPlotOptionsBarDataLabels
                        {
                            Position = ApexChartPlotOptionsBarDataLabelsPosition.top
                        }
                    }
                },
                Tooltip = new ApexChartTooltip
                {
                    Style = new ApexChartTooltipStyle
                    {
                        FontFamily = "KlavikaRegular"
                    },
                    Y = new ApexChartTooltipY(),
                    Marker = new ApexChartTooltipMarker
                    {
                        Show = false
                    }
                },
                Xaxis = new ApexChartConfigXaxis
                {
                    Labels = new ApexChartConfigXaxisLabels
                    {
                        OffsetY = Device.Idiom == TargetIdiom.Phone ? 3 : 5,
                        Style = new ApexChartConfigXaxisLabelsStyle
                        {
                            FontFamily = "KlavikaRegular"
                        }
                    },
                    AxisBorder = new ApexChartAxisBorder
                    {
                        Show = false
                    },
                    AxisTicks = new ApexChartAxisTicks
                    {
                        Show = false
                    },
                    Crosshairs = new ApexChartConfigXaxisCrosshairs
                    {
                        Fill = new ApexChartConfigXaxisCrosshairsFill
                        {
                            Type = ApexChartConfigXaxisCrosshairsFillType.gradient,
                            Gradient = new ApexChartConfigXaxisCrosshairsFillGradient
                            {
                                ColorFrom = "",
                                ColorTo = "",
                                OpacityFrom = 0.4F,
                                OpacityTo = 0.5F,
                                Stops = new ushort[] {0, 100}
                            }
                        }
                    }
                },
                Yaxis = new ApexChartConfigYaxis
                {
                    AxisBorder = new ApexChartAxisBorder
                    {
                        Show = false
                    },
                    AxisTicks = new ApexChartAxisTicks
                    {
                        Show = false
                    },
                    Labels = new ApexChartConfigYaxisLabels
                    {
                        Show = false
                    }
                }
            };
        }

        public static ApexChartConfig CreateAreaChart()
        {
            return new ApexChartConfig
            {
                Chart = new ApexChartModel
                {
                    Type = ApexChartType.area,
                    Height = Device.Idiom == TargetIdiom.Phone ? "96%" : "97%",
                    ParentHeightOffset = 0,
                    Width = "100%",
                    Toolbar = new ApexChartToolbar
                    {
                        Show = false
                    }
                },
                DataLabels = new ApexChartDataLabels
                {
                    Enabled = false,
                    Style = new ApexChartDataLabelsStyle()
                },
                Legend = new ApexChartLegend
                {
                    Show = true,
                    Position = ApexChartLegendPosition.top,
                    HorizontalAlign = ApexChartLegendHorizontalAlign.center,
                    FontFamily = "KlavikaRegular"
                },
                Tooltip = new ApexChartTooltip
                {
                    Style = new ApexChartTooltipStyle
                    {
                        FontFamily = "KlavikaRegular"
                    },
                    Y = new ApexChartTooltipY(),
                    Marker = new ApexChartTooltipMarker
                    {
                        Show = true
                    }
                },
                Stroke = new ApexChartStroke
                {
                    Curve = ApexChartStrokeCurve.straight
                },
                Xaxis = new ApexChartConfigXaxis
                {
                    Labels = new ApexChartConfigXaxisLabels
                    {
                        RotateAlways = true,
                        MinHeight = Device.Idiom == TargetIdiom.Phone? 40 : 50,
                        Style = new ApexChartConfigXaxisLabelsStyle
                        {
                            FontFamily = "KlavikaRegular"
                        }
                    },
                    AxisBorder = new ApexChartAxisBorder
                    {
                        Show = false
                    },
                    AxisTicks = new ApexChartAxisTicks
                    {
                        Show = false
                    },
                    Crosshairs = new ApexChartConfigXaxisCrosshairs
                    {
                        Fill = new ApexChartConfigXaxisCrosshairsFill
                        {
                            Type = ApexChartConfigXaxisCrosshairsFillType.gradient,
                            Gradient = new ApexChartConfigXaxisCrosshairsFillGradient
                            {
                                ColorFrom = "",
                                ColorTo = "",
                                OpacityFrom = 0.4F,
                                OpacityTo = 0.5F,
                                Stops = new ushort[] { 0, 100 }
                            }
                        }
                    }
                },
                Yaxis = new ApexChartConfigYaxis
                {
                    Labels = new ApexChartConfigYaxisLabels
                    {
                        Show = true,
                        Style = new ApexChartConfigYaxisLabelsStyle
                        {
                            FontFamily = "KlavikaRegular"
                        }
                    }
                }
            };
        }

        public ApexChartConfig SetSeries(ApexChartConfigSeries[] series)
        {
            Series = series;

            try
            {
                var maxValue = series.SelectMany(_ => _.Data).Max(_ => _);
                if (maxValue != null)
                {
                    var k = 1.1;
                    if (maxValue is double)
                        Yaxis.Max = (double)maxValue * k;
                    else if (maxValue is Int32)
                        Yaxis.Max = (Int32)maxValue * k;
                    else if (maxValue is Int64)
                        Yaxis.Max = (Int64)maxValue * k;
                }
            }
            catch (Exception) 
            { }

            return this;
        }

        public ApexChartConfig SetXaxisCategories(string[] categories)
        {
            if (Xaxis != null)
                Xaxis.Categories = categories;

            return this;
        }

        public ApexChartConfig SetDataLabelsFormatter(string formatter)
        {
            if (!string.IsNullOrEmpty(formatter) && DataLabels != null)
                DataLabels.Formatter = formatter;

            return this;
        }

        public ApexChartConfig SetTooltipYFormatter(string formatter)
        {
            if (!string.IsNullOrEmpty(formatter) && Tooltip?.Y != null)
                Tooltip.Y.Formatter = formatter;

            return this;
        }

        public ApexChartConfig SetLabelsFontSize(string fontSize)
        {
            if (!string.IsNullOrEmpty(fontSize))
            {
                if (DataLabels?.Style != null)
                    DataLabels.Style.FontSize = fontSize;
                if (Tooltip?.Style != null)
                    Tooltip.Style.FontSize = fontSize;
                if (Xaxis?.Labels?.Style != null)
                    Xaxis.Labels.Style.FontSize = fontSize;
                if (Yaxis?.Labels?.Style != null)
                    Yaxis.Labels.Style.FontSize = fontSize;
                if (Legend != null)
                    Legend.FontSize = fontSize;
            }

            return this;
        }

        public ApexChartConfig SetColumnWidth(string columnWidth)
        {
            if (!string.IsNullOrEmpty(columnWidth) && PlotOptions?.Bar != null && columnWidth.EndsWith("%"))
                PlotOptions.Bar.ColumnWidth = columnWidth; // columnWidth is the percentage of the available width in the grid-rect

            return this;
        }

        #endregion

        #region "Support Methods"

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[] { new StringEnumConverter() },
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        #endregion
    }
}