using CHBackOffice.ApiServices.ChsProxy;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Models.ApexCharts.Common;
using CHSBackOffice.Models.ApexCharts.Enums;
using CHSBackOffice.Support.ApexChart;
using CHSBackOffice.Support.ApexChart.DataMappers.ToCategories;
using CHSBackOffice.Support.ApexChart.DataMappers.ToSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CHSBackOffice.ViewModels
{
    internal class TransactionByTypePageViewModel : ReportPageViewModel<ReportItem>
    {
        #region .CTOR

        public TransactionByTypePageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
        }

        #endregion

        #region Get report implementation

        internal override async Task<ReportItem> GetReport(string state, DisplayOrientation displayOrientation)
        {
            ReportItem report = null;

            var data = await GetDashboardData(state);
            var chartData = data?
                .Where(a => a.Any(t => t.Amount != "$0.00") &&
                            (state == State.Days7 && a.All(t => ApexChartHelper.WeekDays.Contains(t.Time)) || state != State.Days7))
                .ToArray();


            if (chartData != null && chartData.Length > 0)
            {
                var utcNow = DateTime.UtcNow;
                var series = ApexChart.MapDataToSeries(new TransactionMultyArrayToSeriesMapper(chartData, state, utcNow));
                var categories = ApexChart.MapDataToCategories(new TransactionMultyArrayToCategoriesMapper(chartData, state, utcNow));

                var tooltipYFormatter = ApexChart.NumberToMoneyTooltipFormatter;

                var labelsFontSize = ApexChartHelper.GetLabelsFontSize();
                report = new ReportItem
                {
                    Html = ApexChart
                        .New(ApexChartType.area)
                        .Init(
                            series,
                            categories,
                            null,
                            tooltipYFormatter,
                            labelsFontSize,
                            null
                        ).GetHtml(),
                    ColumnCount = 1
                };
            }
            else
            {
                report = new ReportItem
                {
                    Html = String.Empty,
                    ColumnCount = 1
                };
            }

            return report;
        }

        internal override List<KeyValuePair<string, string>> GetStateCollection()
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(State.Yesterday, State.GetLabel(State.Yesterday)),
                new KeyValuePair<string, string>(State.Days7, State.GetLabel(State.Days7)),
                new KeyValuePair<string, string>(State.Days31, State.GetLabel(State.Days31)),
                new KeyValuePair<string, string>(State.Days366, State.GetLabel(State.Days366)),
            };
        }

        #endregion

        #region Private methods

        private async Task<Transaction[][]> GetDashboardData(string state)
        {
            if (state == State.Yesterday)
                return await _serviceAgent.GetDashboardTransactionTypes(StateInfoService.SessionId, GraphCriteria.Yesterday);
            if (state == State.Days7)
                return await _serviceAgent.GetDashboardTransactionTypes(StateInfoService.SessionId, GraphCriteria.LastWeek);
            if (state == State.Days31)
                return await _serviceAgent.GetDashboardTransactionTypes(StateInfoService.SessionId, GraphCriteria.LastMonth);
            if (state == State.Days366)
                return await _serviceAgent.GetDashboardTransactionTypes(StateInfoService.SessionId, GraphCriteria.LastYear);

            return null;
        }

        #endregion
    }
}
