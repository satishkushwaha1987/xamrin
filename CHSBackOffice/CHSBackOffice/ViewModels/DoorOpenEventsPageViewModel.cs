using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Support;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CHSBackOffice.ViewModels
{
    internal class DoorOpenEventsPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region Fields

        #region "Private Fields"

        private CHBackOffice.ApiServices.ChsProxy.DoorOpenEvent[] events;

        #endregion

        #endregion

        #region "PUBLIC PROPS"

        #region "IsNoDataLabelVisible"

        private bool _isNoDataLabelVisible;
        public bool IsNoDataLabelVisible
        {
            get => _isNoDataLabelVisible;
            set
            {
                SetProperty(ref _isNoDataLabelVisible, value);
            }
        }

        #endregion

        #region "SearchText"

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                SearchCommand?.Execute(null);
            }
        }

        #endregion

        #region "Events"
        private ObservableCollection<CHBackOffice.ApiServices.ChsProxy.DoorOpenEvent> _events;
        public ObservableCollection<CHBackOffice.ApiServices.ChsProxy.DoorOpenEvent> Events
        {
            get => _events;
            set => SetProperty(ref _events, value);
        }
        #endregion

        #endregion

        #region ".CTOR"

        public DoorOpenEventsPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            SafeRefreshDataAsync();
        }

        #endregion

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            Events = null;
            IsNoDataLabelVisible = false;

            events = await _serviceAgent.GetAllOpenDoorEvents(StateInfoService.SessionId);
            
            if (events != null && events.Length > 0)
                Events = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.DoorOpenEvent>(events);
            else
                IsNoDataLabelVisible = true;
        }

        #endregion

        #region "COMMANDS"

        public ICommand SearchCommand => new Xamarin.Forms.Command(SearchExecute);
        void SearchExecute()
        {
            try
            {
                Func<CHBackOffice.ApiServices.ChsProxy.DoorOpenEvent, bool> predicate = @event =>
                     string.IsNullOrEmpty(SearchText) ||
                     @event.Id.ToUpper().Contains(SearchText.ToUpper()) ||
                     @event.Description.ToUpper().Contains(SearchText.ToUpper());
                if (events != null)
                    Events = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.DoorOpenEvent>(
                        events.AsParallel().Where(predicate));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

    }
}
