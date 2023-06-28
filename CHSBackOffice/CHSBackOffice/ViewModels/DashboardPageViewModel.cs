using CHBackOffice.ApiServices.Interfaces;
using System.Threading.Tasks;

namespace CHSBackOffice.ViewModels
{
    internal class DashboardPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region "PUBLIC PROPERTIES"

        public string Title => "BackOffice";

        #endregion

        #region ".CTOR"

        public DashboardPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            // use the protected _serviceAgent field for your own needs
        }

        #endregion

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            await Task.Delay(1);
            // Load Page Data and Set Models Here
        }

        #endregion
    }
}
