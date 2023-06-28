using CHSBackOffice.Events;
using System;
using Xamarin.Forms;

namespace CHSBackOffice.Views
{
    public partial class MainMenuPage : MasterDetailPage
    {
        public MainMenuPage()
        {
            try
            {
                InitializeComponent();
                Support.MainMenuData.Instance.InitMenu();
            }
            catch (Exception ex)
            {
                Support.ExceptionProcessor.ProcessException(ex);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.SharedEventAggregator.GetEvent<SetDetailPage>().Subscribe(SetDetailPage);
            App.SharedEventAggregator.GetEvent<NavigateDetailPage>().Subscribe(NavigateToDetailPage);
            App.SharedEventAggregator.GetEvent<GoBackDetailPage>().Subscribe(GoBackDetail);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.SharedEventAggregator.GetEvent<SetDetailPage>().Unsubscribe(SetDetailPage);
            App.SharedEventAggregator.GetEvent<NavigateDetailPage>().Unsubscribe(NavigateToDetailPage);
            App.SharedEventAggregator.GetEvent<GoBackDetailPage>().Unsubscribe(GoBackDetail);
        }

        async void GoBackDetail()
        {
            try
            {
                await Detail.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Support.ExceptionProcessor.ProcessException(ex);
            }
        }

        void SetDetailPage(Page page)
        {
            try
            {
                Detail = new NavigationPage(page);
                IsPresented = false;
                IsGestureEnabled = true;
            }
            catch (Exception ex)
            {
                Support.ExceptionProcessor.ProcessException(ex);
            }
        }

        void NavigateToDetailPage(Page page)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    page.Disappearing += Page_Disappearing;
                    await Detail.Navigation.PushAsync(page);
                    if (page.GetType() == typeof(Views.ATMInfoPage))
                        (page as ATMInfoPage).CreateContent();
                    IsPresented = false;
                    IsGestureEnabled = false;
                }
                catch (Exception ex)
                {
                    Support.ExceptionProcessor.ProcessException(ex);
                }
                finally
                {
                    Support.CommonViewObjects.Instance.IsPageCreating = false;
                }
            });
        }

        private void Page_Disappearing(object sender, EventArgs e)
        {
            IsGestureEnabled = (Detail.Navigation.NavigationStack.Count <= 2);
        }
    }
}