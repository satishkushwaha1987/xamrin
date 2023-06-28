using CHSBackOffice.Models.ApexCharts.Enums;

namespace CHSBackOffice.Models.ApexCharts
{
    public sealed class ApexChartFillGradient
    {
        public ApexChartFillGradientType Type { set; get; }
        public float ShadeIntensity { set; get; }
        public float OpacityFrom { set; get; }
        public float OpacityTo { set; get; }
        public ushort[] Stops { set; get; }
    }
}
