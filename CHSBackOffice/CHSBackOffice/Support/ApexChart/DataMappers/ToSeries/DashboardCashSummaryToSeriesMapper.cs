using CHBackOffice.ApiServices.ChsProxy;
using CHSBackOffice.Extensions;
using CHSBackOffice.Models.ApexCharts;
using System.Collections.Generic;
using System.Linq;

namespace CHSBackOffice.Support.ApexChart.DataMappers.ToSeries
{
    public class DashboardCashSummaryToSeriesMapper : IDataToSeriesMapper
    {
        readonly DashboardCashSummary _summary;

        #region ".CTOR"

        public DashboardCashSummaryToSeriesMapper(DashboardCashSummary summary)
        {
            _summary = summary;
        }

        #endregion

        #region "IDataToSeriesMapper implementation"

        public ApexChartConfigSeries[] Map()
        {
            return new ApexChartConfigSeries[]
            {
                new ApexChartConfigSeries
                {
                    Name = "Value",
                    Data = GetData().ToArray()
                }
            };
        }

        #endregion

        #region "PRIVATE METHODS"

        private IEnumerable<object> GetData()
        {
            return _summary
                .Summary
                .Select(s => s.Amount.ToDouble())
                .Cast<object>();
        }

        #endregion
    }
}
