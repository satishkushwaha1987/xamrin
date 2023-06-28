using Newtonsoft.Json;

namespace CHSBackOffice.Models.ApexCharts
{
    public sealed class ApexChartDataLabels
    {
        public bool Enabled { set; get; }
        public int OffsetX { set; get; }
        public int OffsetY { set; get; }
        public ApexChartDataLabelsStyle Style { set; get; }

        [JsonIgnore]
        public string Formatter { set; get; }
    }
}
