using CHBackOffice.ApiServices.ChsProxy;
using CHSBackOffice.Models.ApexCharts;
using System.Collections.Generic;
using System.Linq;

namespace CHSBackOffice.Support.ApexChart.DataMappers.ToSeries
{
    public class DashboardUtilizationToSeriesMapper : IDataToSeriesMapper
    {
        readonly DashboardUtilization _utilization;

        #region ".CTOR"

        public DashboardUtilizationToSeriesMapper(DashboardUtilization utilization)
        {
            _utilization = utilization;
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
            return _utilization
                .CashUtil
                .Select(s => s.Value)
                .Cast<object>();
        }

        #endregion
    }
}
