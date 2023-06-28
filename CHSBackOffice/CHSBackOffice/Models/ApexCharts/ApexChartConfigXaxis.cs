using CHSBackOffice.Models.ApexCharts.Enums;

namespace CHSBackOffice.Models.ApexCharts
{
    public sealed class ApexChartConfigXaxis
    {
        public ApexChartConfigXaxisType Type { set; get; }
        public string[] Categories { set; get; }
        public ApexChartConfigXaxisLabels Labels { set; get; }
        public ApexChartAxisBorder AxisBorder { set; get; }
        public ApexChartAxisTicks AxisTicks { set; get; }
        public ApexChartConfigXaxisCrosshairs Crosshairs { set; get; }
    }
}
