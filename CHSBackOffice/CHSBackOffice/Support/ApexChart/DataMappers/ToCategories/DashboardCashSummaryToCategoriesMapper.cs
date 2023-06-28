using CHBackOffice.ApiServices.ChsProxy;
using System;
using System.Linq;

namespace CHSBackOffice.Support.ApexChart.DataMappers.ToCategories
{
    public class DashboardCashSummaryToCategoriesMapper : IDataToCategoriesMapper
    {
        readonly DashboardCashSummary _summary;

        #region ".CTOR"

        public DashboardCashSummaryToCategoriesMapper(DashboardCashSummary summary)
        {
            _summary = summary;
        }

        #endregion

        #region "IDataToSeriesMapper implementation"

        public string[] Map()
        {
            return _summary.Summary.Select(s => GetCatogory(s.Id)).ToArray();
        }

        #endregion

        #region "PRIVATE METHODS"

        private string GetCatogory(string id)
        {
            if (String.IsNullOrEmpty(id))
                return "NA";

            return id;
        }

        #endregion
    }
}
