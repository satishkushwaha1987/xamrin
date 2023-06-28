using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Support;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CHSBackOffice.ViewModels
{
    internal class EmployeeManagmentPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region "BINDABLE PROPS"

        private ObservableCollection<Models.MenuItem> _menuItems;
        public ObservableCollection<Models.MenuItem> MenuItems
        {
            get => _menuItems;
            set => SetProperty(ref _menuItems, value);
        }

        #region "IsNoDataLabelVisible"

        private bool _isNoDataLabelVisible;
        public bool IsNoDataLabelVisible
        {
            get => _isNoDataLabelVisible;
            set => SetProperty(ref _isNoDataLabelVisible, value);
        }

        #endregion


        #endregion

        #region ".CTOR"

        public EmployeeManagmentPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            SafeRefreshDataAsync();
        }

        #endregion

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            IsNoDataLabelVisible = false;
            MenuItems = MainMenuData.Instance.EmployeeManagementItems;
            IsNoDataLabelVisible = MenuItems.Count == 0;
            await Task.Delay(1);
        }

        #endregion

    }
}
