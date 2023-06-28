using CHBackOffice.ApiServices.ChsProxy;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Models.ApexCharts.Common;
using CHSBackOffice.Models.ApexCharts.Enums;
using CHSBackOffice.Support;
using CHSBackOffice.Support.ApexChart;
using CHSBackOffice.Support.ApexChart.DataMappers.ToCategories;
using CHSBackOffice.Support.ApexChart.DataMappers.ToSeries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CHSBackOffice.ViewModels
{
    internal class AllTransactionsPageViewModel : ReportPageViewModel<ReportItem>
    {
        #region .CTOR

        public AllTransactionsPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
        }

        #endregion

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

        internal override async Task<ReportItem> GetReport(string state, DisplayOrientation displayOrientation)
        {
            ReportItem report = null;

            var chartData = await GetDashboardData(state);

            if (chartData != null && chartData.Hourly.Length > 0)
            {
                var utcNow = DateTime.UtcNow;
                var series = ApexChart.MapDataToSeries(new DashboardTransactionsToSeriesMapper(chartData, state));
                var categories = ApexChart.MapDataToCategories(new DashboardTransactionsToCategoriesMapper(chartData, state, utcNow));

                var dataLabelsFormatter = ApexChart.NumberToMoneyFormatter;
                var tooltipYFormatter = ApexChart.NumberToMoneyFormatter;

                var columnCount = series[0].Data.Length;
                var columnWidth = ApexChartHelper.GetColumnWidthByColumnCount(columnCount, displayOrientation);
                var labelsFontSize = ApexChartHelper.GetLabelsFontSize();
                report = new ReportItem
                {
                    Html = ApexChart
                        .New(ApexChartType.bar)
                        .Init(
                            series,
                            categories,
                            dataLabelsFormatter,
                            tooltipYFormatter,
                            labelsFontSize,
                            columnWidth
                        ).GetHtml(),
                    ColumnCount = columnCount
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

        private async Task<DashboardTransactions> GetDashboardData(string state)
        {
            try
            {
                if (state == State.Yesterday)
                    return await _serviceAgent.GetDashboardTransactions(StateInfoService.SessionId, GraphCriteria.Yesterday);
                if (state == State.Days7)
                    return await _serviceAgent.GetDashboardTransactions(StateInfoService.SessionId, GraphCriteria.LastWeek);
                if (state == State.Days31)
                    return await _serviceAgent.GetDashboardTransactions(StateInfoService.SessionId, GraphCriteria.MonthToDay);
                if (state == State.Days366)
                    return await _serviceAgent.GetDashboardTransactions(StateInfoService.SessionId, GraphCriteria.LastYear);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }
    }
}
