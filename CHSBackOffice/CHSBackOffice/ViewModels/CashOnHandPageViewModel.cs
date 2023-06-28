using CHBackOffice.ApiServices.ChsProxy;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Extensions;
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
    internal class CashOnHandPageViewModel : ReportPageViewModel<CachOnHandReportItem>
    {
        #region .CTOR

        public CashOnHandPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
        }

        #endregion

        #region Get report implementation

        internal override async Task<CachOnHandReportItem> GetReport(string state, DisplayOrientation displayOrientation)
        {
            CachOnHandReportItem report = null;

            var chartData = await GetDashboardData(state);


            if (chartData != null && chartData.Summary.Length > 0)
            {
                var series = ApexChart.MapDataToSeries(new DashboardCashSummaryToSeriesMapper(chartData));
                var categories = ApexChart.MapDataToCategories(new DashboardCashSummaryToCategoriesMapper(chartData));

                var dataLabelsFormatter = ApexChart.NumberToMoneyFormatter;
                var tooltipYFormatter = ApexChart.NumberToMoneyFormatter;

                var columnCount = series[0].Data.Length;
                var columnWidth = ApexChartHelper.GetColumnWidthByColumnCount(columnCount, displayOrientation);
                var labelsFontSize = ApexChartHelper.GetLabelsFontSize();
                report = new CachOnHandReportItem
                {
                    AcceptedCach = chartData.AcceptedCashAmount.ToMoney(),
                    AcceptedTickets = chartData.AcceptedTicketAmount.ToMoney(),
                    Remaining = chartData.RemainingAmount.ToMoney(),
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
                report = new CachOnHandReportItem
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
                new KeyValuePair<string, string>(State.MachineGroup, State.GetLabel(State.MachineGroup)),
                new KeyValuePair<string, string>(State.Machine, State.GetLabel(State.Machine))
            };
        }

        internal override void AfterRefreshDataActions(CachOnHandReportItem report)
        {
            AcceptedCach = report.AcceptedCach;
            AcceptedTickets = report.AcceptedTickets;
            Remaining = report.Remaining;
        }

        #endregion

        #region Additional public properties

        private string _acceptedCach;
        public string AcceptedCach
        {
            get => _acceptedCach;
            set => SetProperty(ref _acceptedCach, value);
        }

        private string _acceptedTickets;
        public string AcceptedTickets
        {
            get => _acceptedTickets;
            set => SetProperty(ref _acceptedTickets, value);
        }

        private string _remaining;
        public string Remaining
        {
            get => _remaining;
            set => SetProperty(ref _remaining, value);
        }

        #endregion

        #region Private methods

        private async Task<DashboardCashSummary> GetDashboardData(string state)
        {
            try
            {
                if (state == State.Machine)
                    return await _serviceAgent.GetDashboardCashSummary(StateInfoService.SessionId, 1);
                if (state == State.MachineGroup)
                    return await _serviceAgent.GetDashboardCashSummary(StateInfoService.SessionId, 2);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }

            return null;
        }

        #endregion
    }
}
