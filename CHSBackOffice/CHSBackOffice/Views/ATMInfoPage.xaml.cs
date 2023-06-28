using CHSBackOffice.CustomControls;
using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using CHSBackOffice.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ATMInfoPage : ExtendedNaviPage
    {
        bool _contentCreated = false;
        //ATMInfoPageContent _content;
        ATMInfoSkiaPageContent _content;

        #region ".CTOR"

        public ATMInfoPage()
        {
            try
            {
                InitializeComponent();

                OnOrientartionChanged += ExtendedNaviPage_OnOrientartionChanged;
                NavigationPage.SetHasNavigationBar(this, true);

            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            
        }

        internal void CreateContent()
        {
            if (_contentCreated && _content != null) return;

            try
            {
                var popupLayout = PopupLayout.AddToPage(this);
                //_content = new ATMInfoPageContent();
                _content = new ATMInfoSkiaPageContent();
                _content.ListViewItemAppearing += ProcessItemAppearing;
                _content.ListViewItemTapped += ProcessItemTapped;
                _content.SetPopupParentContext(popupLayout);

                PageMainGrid.Children.Add(_content);
            }
            catch
            { }
        }

        #endregion

        #region "COMMAND HANDLERS"

        ATMInfoPageViewModel _aTMInfoPageViewModel;

        void ProcessItemAppearing(int index)
        {
            if (_aTMInfoPageViewModel == null)
                _aTMInfoPageViewModel = (BindingContext as ATMInfoPageViewModel);

            if (_aTMInfoPageViewModel == null)
                return;

            if (
                _aTMInfoPageViewModel.TransactionsList != null 
                && _aTMInfoPageViewModel.TransactionsList.Count > 1 
                && _aTMInfoPageViewModel.CanLoadMore 
                && index >= _aTMInfoPageViewModel.TransactionsList.Count - 1)
                _aTMInfoPageViewModel.SafeRefreshDataAsync();
        }

        void ProcessItemTapped(int index)
        {
            CommonViewObjects.Instance.SelectedTransactionNumber = index;
            Services.Navigation.NavigateDetailPage(typeof(TransactionsCarouselPage));

        }

        #endregion

        #region "EVENT HANDLERS"
        private void ExtendedNaviPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            if (_content != null)
                _content.RefreshGrid();
        }

        #endregion
    }
}
