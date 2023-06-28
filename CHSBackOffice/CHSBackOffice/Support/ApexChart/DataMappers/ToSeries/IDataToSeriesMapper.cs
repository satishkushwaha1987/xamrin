using CHSBackOffice.Models.ApexCharts;

namespace CHSBackOffice.Support.ApexChart.DataMappers.ToSeries
{
    public interface IDataToSeriesMapper
    {
        ApexChartConfigSeries[] Map();
    }
}
