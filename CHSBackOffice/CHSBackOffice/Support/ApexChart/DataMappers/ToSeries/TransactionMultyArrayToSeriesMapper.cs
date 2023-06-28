using CHBackOffice.ApiServices.ChsProxy;
using CHSBackOffice.Extensions;
using CHSBackOffice.Models.ApexCharts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHSBackOffice.Support.ApexChart.DataMappers.ToSeries
{
    public class TransactionMultyArrayToSeriesMapper : IDataToSeriesMapper
    {
        readonly Transaction[][] _transactionArrays;
        readonly string _state;
        readonly DateTime _utcNow;

        #region ".CTOR"

        public TransactionMultyArrayToSeriesMapper(Transaction[][] transactionArrays, string state, DateTime utcNow)
        {
            _transactionArrays = transactionArrays;
            _state = state;
            _utcNow = utcNow;
        }

        #endregion

        #region "IDataToSeriesMapper implementation"

        public ApexChartConfigSeries[] Map()
        {
            var series = new List<ApexChartConfigSeries>();
            foreach (var transactions in _transactionArrays)
            {
                series.Add(new ApexChartConfigSeries
                {
                    Name = transactions.First().Type,
                    Data = GetData(transactions).ToArray()
                });
            }

            return series.ToArray();
        }

        #endregion

        #region "PRIVATE METHODS"

        private IEnumerable<object> GetData(Transaction[] transactions)
        {


            if (_state == State.Yesterday)
            {
                var transactionsDict = transactions
                    .GetWithMinShift()
                    .ToDictionary(t => int.Parse(t.Time), t => t);

                for (int i = 0; i < 24; i++)
                {
                    if (transactionsDict.ContainsKey(i))
                        yield return transactionsDict[i].Amount.ToDouble();
                    else
                        yield return 0.0;
                }
            }

            if (_state == State.Days7)
            {
                var transactionsDict = transactions
                    .GetWithMinShift()
                    .ToDictionary(t => t.Time, t => t);

                foreach (var day in ApexChartHelper.WeekDays)
                {
                    if (transactionsDict.ContainsKey(day))
                        yield return transactionsDict[day].Amount.ToDouble();
                    else
                        yield return 0.0;
                }
            }

            if (_state == State.Days31)
            {
                var transactionsDict = transactions
                    .GetWithMinShift()
                    .ToDictionary(t => int.Parse(t.Time), t => t);

                for (int i = 1; i <= 31; i++)
                {
                    if (transactionsDict.ContainsKey(i))
                        yield return transactionsDict[i].Amount.ToDouble();
                    else
                        yield return 0.0;
                }
            }

            if (_state == State.Days366)
            {
                var transactionsDict = transactions
                    .GetWithMinShift()
                    .ToDictionary(t => int.Parse(t.Time), t => t);

                for (int i = 11; i >= 0; i--)
                {
                    var month = _utcNow.AddMonths(-i).Month;
                    if (transactionsDict.ContainsKey(month))
                        yield return transactionsDict[month].Amount.ToDouble();
                    else
                        yield return 0.0;
                }
            }

            yield break;
        }

        #endregion
    }
}
