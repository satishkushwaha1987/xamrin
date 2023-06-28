using CHSBackOffice.CustomControls;
using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using CHSBackOffice.Support.ApexChart;
using CHSBackOffice.Support.Classes;
using CHSBackOffice.Support.Device;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CashOnHandPage : ReportPage
    {
        #region ".CTOR"

        public CashOnHandPage()
		{
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
			
        }

        #endregion

        #region "PRIVATE METHODS"

        internal override void OnReportClearChart(Type senderType)
        {
            if (!senderType.Name.Contains(GetType().Name))
                return;

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    graphScrollContainer.Content = null;
                    await graphScrollContainer.ScrollToAsync(0, 0, false);
                }
                catch { }
            });
        }

        internal override double GetPaddingsWidth(ScrollView scrollView)
        {
            double retValue = 0;
            var parentGrid = scrollView.Parent as Layout;
            while (parentGrid != null)
            {
                retValue += parentGrid.Padding.Left + parentGrid.Padding.Right;
                parentGrid = parentGrid.Parent as Layout;
            }

            return retValue;
        }

        internal override void OnReportHtmlIsReady(ReportData reportData)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var reportHtml = reportData.ReportHtml;
                    if (reportHtml.IsNullOrEmpty())
                    {
                        dashboardContainer.IsVisible = false;
                        noDataAvailable.IsVisible = true;
                        noDataAvailable.Opacity = 1;
                    }
                    else
                    {
                        var paddingsWidth = GetPaddingsWidth(graphScrollContainer);
                        var optimalColumnCount = ApexChartHelper.GetOptimalColumnCount(DeviceDisplay.MainDisplayInfo.Orientation);

                        var needToIncreaseWidth = false;
                        if (reportData.ColumnsCount > optimalColumnCount)
                            needToIncreaseWidth = true;

                        var webView = new WebView
                        {
                            BackgroundColor = Color.White,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Source = new HtmlWebViewSource
                            {
                                Html = reportHtml,
                                BaseUrl = DependencyService.Get<IBaseUrl>().Get()
                            }
                        };

                        if (needToIncreaseWidth)
                        {
                            webView.HorizontalOptions = LayoutOptions.Start;

                            var columnWidth = (DeviceDisplay.MainDisplayInfo.WidthInDp() - paddingsWidth) / optimalColumnCount;
                            var webViewWidth = reportData.ColumnsCount * columnWidth;
                            webView.WidthRequest = webViewWidth;
                        }

                        noDataAvailable.IsVisible = false;
                        dashboardContainer.IsVisible = true;

                        graphScrollContainer.Content = webView;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            });
        }

        #endregion
    }
}