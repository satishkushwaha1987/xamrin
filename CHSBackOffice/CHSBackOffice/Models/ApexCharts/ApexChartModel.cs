using CHSBackOffice.Models.ApexCharts.Enums;

namespace CHSBackOffice.Models.ApexCharts
{
    public sealed class ApexChartModel
    {
        public string Background { set; get; }
        public string Height { set; get; }
        public int? ParentHeightOffset { set; get; }
        public ApexChartToolbar Toolbar { set; get; }
        public ApexChartType Type { set; get; }
        public string Width { set; get; }
    }
}
