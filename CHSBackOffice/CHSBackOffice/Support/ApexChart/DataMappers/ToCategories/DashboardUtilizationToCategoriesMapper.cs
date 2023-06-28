using CHBackOffice.ApiServices.ChsProxy;
using System;
using System.Linq;

namespace CHSBackOffice.Support.ApexChart.DataMappers.ToCategories
{
    public class DashboardUtilizationToCategoriesMapper : IDataToCategoriesMapper
    {
        readonly DashboardUtilization _utilization;

        #region ".CTOR"

        public DashboardUtilizationToCategoriesMapper(DashboardUtilization utilization)
        {
            _utilization = utilization;
        }

        #endregion

        #region "IDataToSeriesMapper implementation"

        public string[] Map()
        {
            return _utilization.CashUtil.Select(u => GetCatogory(u.Id)).ToArray();
        }

        #endregion

        #region "PRIVATE METHODS"

        private string GetCatogory(string id)
        {
            if (String.IsNullOrEmpty(id))
                return string.Empty;

            return id;
        }

        #endregion
    }
}
