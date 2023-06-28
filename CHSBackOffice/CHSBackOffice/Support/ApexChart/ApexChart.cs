using CHSBackOffice.Models.ApexCharts;
using CHSBackOffice.Models.ApexCharts.Enums;
using CHSBackOffice.Support.ApexChart.DataMappers.ToCategories;
using CHSBackOffice.Support.ApexChart.DataMappers.ToSeries;
using CHSBackOffice.Support.ResourceFiles;
using System;
using System.Linq;

namespace CHSBackOffice.Support.ApexChart
{
    public sealed class ApexChart
    {
        private ApexChartConfig _apexChartConfig;

        #region ".CTOR"

        private ApexChart(ApexChartConfig apexChartConfig)
        {
            _apexChartConfig = apexChartConfig;
        }

        #endregion

        #region "FACTORY METHODS"

        public static ApexChart New(ApexChartType apexChartType)
        {
            ApexChartConfig config;

            switch (apexChartType)
            {
                case ApexChartType.area:
                    config = ApexChartConfig.CreateAreaChart();
                    break;
                default:
                    config = ApexChartConfig.CreateBarChart();
                    break;
            }

            return new ApexChart(config);
        }

        public ApexChart Init(
            ApexChartConfigSeries[] series,
            string[] categories,
            string dataLabelsFormatter = null,
            string tooltipYFormatter = null,
            string labelsFontSize = "12px",
            string columnWidth = null)
        {
            _apexChartConfig
                .SetSeries(series)
                .SetXaxisCategories(categories)
                .SetDataLabelsFormatter(dataLabelsFormatter)
                .SetTooltipYFormatter(tooltipYFormatter)
                .SetLabelsFontSize(labelsFontSize)
                .SetColumnWidth(columnWidth);

            return this;
        }

        #endregion

        #region "FORMATTERS"

        public static string NumberToMoneyFormatter => @"
function (value) { 
    if (value == 0)
        return '';

    return accounting.formatMoney(value); 
}";

        public static string NumberToMoneyTooltipFormatter => @"
function (value) { 
    return accounting.formatMoney(value); 
}";

        public static string NumberToPercentFormatter => @"
function (value) { 
    if (value == 0)
        return '';

    return value + ' %'; 
}";

        public static string NumberToPercentTooltipFormatter => @"
function (value) { 
    if (value == 0)
        return '';

    return value + ' %'; 
}";

        #endregion

        #region "PUBLIC METHODS"

        public static ApexChartConfigSeries[] MapDataToSeries(IDataToSeriesMapper dataToSeriesMapper)
        {
            if (dataToSeriesMapper == null)
                throw new ArgumentNullException(nameof(dataToSeriesMapper));

            return dataToSeriesMapper.Map();
        }

        public static string[] MapDataToCategories(IDataToCategoriesMapper dataToCategoriesMapper)
        {
            if (dataToCategoriesMapper == null)
                throw new ArgumentNullException(nameof(dataToCategoriesMapper));

            return dataToCategoriesMapper.Map();
        }

        public  string GetHtml()
        {
            var chartConfigScript = GetChartConfigScript();
            return GetHtmlWithChartConfig(chartConfigScript);
        }

        #region "Mock Data"

        public static ApexChartConfigSeries[] GetMockSeries()
        {
            return new ApexChartConfigSeries[]
            {
                new ApexChartConfigSeries {
                    Name = "Inflation",
                    Data = new float[] {
                        2.3F,
                        3.1F,
                        4.0F,
                        10.1F,
                        0.0F,
                        3.6F,
                        3.2F,
                        2.3F,
                        1.4F,
                        0.8F,
                        0.5F,
                        0.2F,
                        2.3F,
                        3.1F,
                        4.0F,
                        10.1F,
                        0.0F,
                        3.6F,
                        3.2F,
                        2.3F,
                        1.4F,
                        0.8F,
                        0.5F,
                        0.2F
                    }.Cast<object>().ToArray()
                }
            };
        }

        public static string[] GetMockCategories()
        {
            return new string[] {
                "Jan,2018",
                "Feb,2018",
                "Mar,2018",
                "Apr,2018",
                "May,2018",
                "Jun,2018",
                "Jul,2018",
                "Aug,2018",
                "Sep,2018",
                "Oct,2018",
                "Nov,2018",
                "Dec,2018",
                "Jan,2019",
                "Feb,2019",
                "Mar,2019",
                "Apr,2019",
                "May,2019",
                "Jun,2019",
                "Jul,2019",
                "Aug,2019",
                "Sep,2019",
                "Oct,2019",
                "Nov,2019",
                "Dec,2019"
            };
        }

        #endregion

        #endregion

        #region "PRIVATE METHODS"

        private string GetHtmlWithChartConfig(string chartConfig)
        {
            var apexChartLib = $"<script>{ResourceFilesManager.GetFileContent("apexcharts.min.js")}</script>";
            var accountingtLib = $"<script>{ResourceFilesManager.GetFileContent("accounting.min.js")}</script>";
            var chartConfigScript = $"<script>{chartConfig}</script>";

            var chartDomContainer = $"<div id='chart-container'></div>";
            return $@"
<html style='background-color:#ffffff;'>
  <head>
    <style type='text/css'>
        @font-face {{
            font-family: KlavikaRegular;
            src: url('KlavikaCHRegular.otf')
        }}
    </style>
    {apexChartLib}
    {accountingtLib}
  </head>
  <body style='margin:0;padding:0;'>
    {chartDomContainer}
    {chartConfigScript}
  </body>
</html>";

        }

        private string GetChartConfigScript()
        {
            var dataLabelsFormatter =
                string.IsNullOrEmpty(_apexChartConfig.DataLabels.Formatter) ?
                string.Empty :
                $"config.dataLabels.formatter = {_apexChartConfig.DataLabels.Formatter};";

            var tooltipYFormatter =
                string.IsNullOrEmpty(_apexChartConfig.Tooltip.Y.Formatter) ?
                string.Empty :
                $"config.tooltip.y.formatter = {_apexChartConfig.Tooltip.Y.Formatter};";


            var script = $@"
var config = {_apexChartConfig.ToJson()};
{dataLabelsFormatter}
{tooltipYFormatter}

window.onload = function() {{
  var chart = new ApexCharts(document.querySelector('#chart-container'), config);
  chart.render();
}};";
            return script;
        }

        #endregion
    }
}
