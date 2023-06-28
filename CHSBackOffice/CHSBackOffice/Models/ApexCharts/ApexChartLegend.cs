using CHSBackOffice.Models.ApexCharts.Enums;

namespace CHSBackOffice.Models.ApexCharts
{
    public sealed class ApexChartLegend
    {
        public bool? Show { set; get; }
        public bool? ShowForSingleSeries { set; get; }
        public bool? ShowForNullSeries { set; get; }
        public bool? ShowForZeroSeries { set; get; }
        public ApexChartLegendPosition Position { set; get; }
        public ApexChartLegendHorizontalAlign HorizontalAlign {set;get;}
        public string FontSize { set; get; }
        public string FontFamily { set; get; }
    }
}
