using CHBackOffice.ApiServices.ChsProxy;
using System;

namespace CHSBackOffice.Support.ApexChart.DataMappers.ToCategories
{
    public class TransactionMultyArrayToCategoriesMapper : IDataToCategoriesMapper
    {
        readonly Transaction[][] _transactions;
        readonly string _state;
        readonly DateTime _utcNow;

        #region ".CTOR"

        public TransactionMultyArrayToCategoriesMapper(Transaction[][] transactions, string state, DateTime utcNow)
        {
            _transactions = transactions;
            _state = state;
            _utcNow = utcNow;
        }

        #endregion

        #region "IDataToCategoriesMapper implementation"

        public string[] Map()
        {
            if (_state == State.Yesterday)
                return ApexChartHelper.GetDayLabels();

            if (_state == State.Days7)
                return ApexChartHelper.GetWeekLabels();

            if (_state == State.Days31)
                return ApexChartHelper.GetMonthLabels(_utcNow);

            if (_state == State.Days366)
                return ApexChartHelper.GetYearLabels(_utcNow);

            return null;
        }

        #endregion
    }
}
