using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Database;
using CHSBackOffice.Events;
using CHSBackOffice.Support;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    internal abstract class ExtendedNaviPageViewModelBase : BindableBase
    {
        #region "PUBLIC PROPERTIES"

        private bool _locationSwitcherVisible;
        public bool LocationSwitcherVisible
        {
            get => _locationSwitcherVisible;
            set => SetProperty(ref _locationSwitcherVisible, value);
        }

        private bool _isLoadingVisible;
        public bool IsLoadingVisible
        {
            get => _isLoadingVisible;
            set
            {
                SetProperty(ref _isLoadingVisible, value);
                OnIsLoadingVisibleChanged?.Invoke();
                CommonViewObjects.Instance.IsLoadingVisible = value;
            }
        }

        #region LoadingIndicatorColor

        private Color _LoadingIndicatorColor;
        public Color LoadingIndicatorColor
        {
            get => _LoadingIndicatorColor;
            set => SetProperty(ref _LoadingIndicatorColor, value);

        }

        #endregion


        #region TitleIsVisible

        private bool _TitleIsVisible;
        public bool TitleIsVisible
        {
            get => _TitleIsVisible;
            set => SetProperty(ref _TitleIsVisible, value);

        }

        #endregion

        #region SearchIsVisible

        private bool _SearchIsVisible;
        public bool SearchIsVisible
        {
            get => _SearchIsVisible;
            set => SetProperty(ref _SearchIsVisible, value);

        }

        #endregion

        #endregion

        #region "PRIVATE/PROTECTED/INTERNAL FIELDS"

        protected delegate void OnIsLoadingVisibleChangedHandler();
        protected event OnIsLoadingVisibleChangedHandler OnIsLoadingVisibleChanged;

        protected readonly ICHSServiceAgent _serviceAgent;
        /// <summary>
        /// Default is True
        /// </summary>
        internal bool NeedToRefreshDataOnNetworkConnected = true;

        /// <summary>
        /// Sets to True if Location button enebled
        /// </summary>
        internal bool NeedToRefreshDataOnLocationChanged = false;
        
        /// <summary>
        /// Default is False
        /// </summary>
        internal bool NeedToRefreshDataOrientationChanged = false;

        private readonly Dictionary<string, Action<ToolbarButton>> _toolbarItemsCommands;
        private Semaphore _refreshDataSemaphore;
        internal static DisplayOrientation CurrentDisplayOrientation;

        #endregion

        #region ".CTOR"

        public ExtendedNaviPageViewModelBase(ICHSServiceAgent serviceAgent)
        {
            LoadingIndicatorColor = Color.Black;

            _serviceAgent = serviceAgent;
            _refreshDataSemaphore = new Semaphore(1, 1);

            CurrentDisplayOrientation = DeviceDisplay.MainDisplayInfo.Orientation;

            HideSearch();

            _toolbarItemsCommands = new Dictionary<string, Action<ToolbarButton>>
            {
                { ExtendedNaviPage.ChangeLocationBtnName, new Action<ToolbarButton>(LocationSwitcherBtnPressed)},
                { ExtendedNaviPage.SearchBtnName, new Action<ToolbarButton>(SearchBtnPressed)},
            };
        }

        #endregion

        #region "COMMANDS"

        public ICommand ToolbarButtonPressedCommand => new Command<ToolbarButton>(ToolbarPressed);
        public ICommand OnLocationChangedCommand => new Command<object>(OnLocationChanged);

        #region Search Commands

        public ICommand HideSearchCommand => new Command(HideSearch);
        public ICommand ShowSearchCommand => new Command(ShowSearch);

        #endregion

        #endregion

        #region "COMMAND HANDLERS"

        void ToolbarPressed(ToolbarButton button)
        {
            if (_toolbarItemsCommands.TryGetValue(button.Name, out Action<ToolbarButton> command))
                command.Invoke(button);
        }

        void OnLocationChanged(object paramObj)
        {
            Task.Run(async () =>
            {
                try
                {
                    if (paramObj is string paramStr && int.TryParse(paramStr, out int locationId))
                    {
                        var success = await _serviceAgent.SwitchProperty(StateInfoService.SessionId, locationId);
                        if (success)
                        {
                            StateInfoService.CurentLocationId = locationId.ToString();
                            App.SharedEventAggregator.GetEvent<LocationChanged>().Publish();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            });
        }

        #endregion

        #region "ToolbarItems Commands Handlers"

        void LocationSwitcherBtnPressed(ToolbarButton button)
        {
            LocationSwitcherVisible = !LocationSwitcherVisible;
        }

        void SearchBtnPressed(ToolbarButton button)
        {
            ShowSearchCommand?.Execute(null);
        }

       

        #endregion

        #region "PROTECTED METHODS"

        protected void AddToolbarItemCommand(string btnName, Action<ToolbarButton> action)
        {
            if (_toolbarItemsCommands.ContainsKey(btnName))
                return;

            _toolbarItemsCommands.Add(btnName, action);
        }

        #endregion

        #region Private methods

        private void ShowSearch()
        {
            TitleIsVisible = false;
            SearchIsVisible = true;
        }

        private void HideSearch()
        {
            TitleIsVisible = true;
            SearchIsVisible = false;
        }

        #endregion

        #region Page events methods

        internal void OnPageAppearing()
        {
            App.SharedEventAggregator.GetEvent<NeedToRefreshPageData>().Subscribe(RefreshDataOnNetworkEvent);
            App.SharedEventAggregator.GetEvent<LocationChanged>().Subscribe(RefreshDataOnLocatinChanged);
            DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
        }

        

        internal void OnPageDisappearing()
        {
            App.SharedEventAggregator.GetEvent<NeedToRefreshPageData>().Unsubscribe(RefreshDataOnNetworkEvent);
            App.SharedEventAggregator.GetEvent<LocationChanged>().Unsubscribe(RefreshDataOnLocatinChanged);
            DeviceDisplay.MainDisplayInfoChanged -= DeviceDisplay_MainDisplayInfoChanged;
        }

        #endregion

        #region DataRefresh

        private void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            CurrentDisplayOrientation = e.DisplayInfo.Orientation;
            if (NeedToRefreshDataOrientationChanged)
                SafeRefreshDataAsync();
        }

        void RefreshDataOnLocatinChanged()
        {
            if (NeedToRefreshDataOnLocationChanged)
                SafeRefreshData(true, true);
        }

        void RefreshDataOnNetworkEvent()
        {
            if (NeedToRefreshDataOnNetworkConnected)
                SafeRefreshDataAsync();
        }

        internal void SafeRefreshDataSync()
        {
            SafeRefreshData(false);
        }

        internal void SafeRefreshDataAsync()
        {
            SafeRefreshData(true);
        }

        async void SafeRefreshData(bool async, bool isLocationChanged = false)
        {
            if (async)
            {
                await Task.Run(() => SafeRefreshData(false, isLocationChanged));
                return;
            }

            if (!_refreshDataSemaphore.WaitOne(1))
                return;

            try
            {
                IsLoadingVisible = true;

                await RefreshData(isLocationChanged);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            finally
            {
                _refreshDataSemaphore.Release();
                IsLoadingVisible = false;
            }
        }

        internal abstract Task RefreshData(bool isLocationChanged = false);

        #endregion
    }
}
