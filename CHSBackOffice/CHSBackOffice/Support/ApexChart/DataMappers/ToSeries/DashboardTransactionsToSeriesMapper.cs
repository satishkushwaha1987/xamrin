using CHBackOffice.ApiServices.ChsProxy;
using CHSBackOffice.Extensions;
using CHSBackOffice.Models.ApexCharts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHSBackOffice.Support.ApexChart.DataMappers.ToSeries
{
    public class DashboardTransactionsToSeriesMapper : IDataToSeriesMapper
    {
        readonly DashboardTransactions _transactions;
        readonly string _state;

        #region ".CTOR"

        public DashboardTransactionsToSeriesMapper(DashboardTransactions transactions, string state)
        {
            _transactions = transactions;
            _state = state;
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
            if (_state == State.Yesterday)
            {
                var transactions = _transactions
                    .Hourly
                    .GetWithMinShift()
                    .ToDictionary(t => int.Parse(t.Time), t => t);

                for (int i = 0; i < 24; i++)
                {
                    if (transactions.ContainsKey(i))
                        yield return transactions[i].Amount.ToDouble();
                    else
                        yield return 0.0;
                }
            }

            if (_state == State.Days7)
            {
                var transactions = _transactions
                    .Hourly
                    .GetWithMinShift()
                    .ToDictionary(t => t.Time, t => t);

                foreach (var day in ApexChartHelper.WeekDays)
                {
                    if (transactions.ContainsKey(day))
                        yield return transactions[day].Amount.ToDouble();
                    else
                        yield return 0.0;
                }
            }

            if (_state == State.Days31)
            {
                var transactions = _transactions
                    .Hourly
                    .GetWithMinShift()
                    .ToDictionary(t => int.Parse(t.Time), t => t);

                for (int i = 1; i <= 31; i++)
                {
                    if (transactions.ContainsKey(i))
                        yield return transactions[i].Amount.ToDouble();
                    else
                        yield return 0.0;
                }
            }

            if (_state == State.Days366)
            {
                var transactions = _transactions
                    .Hourly
                    .GetWithMinShift()
                    .ToDictionary(t => int.Parse(t.Time), t => t);

                for (int i = 1; i <= 12; i++)
                {
                    if (transactions.ContainsKey(i))
                        yield return transactions[i].Amount.ToDouble();
                    else
                        yield return 0.0;
                }
            }

            yield break;
        }

        #endregion
    }
}
