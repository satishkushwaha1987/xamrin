using CHBackOffice.ApiServices.ChsProxy;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Database;
using CHSBackOffice.Events;
using CHSBackOffice.Models.Popup;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Classes;
using CHSBackOffice.Support.StaticResources;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static CHSBackOffice.Support.Constants;

namespace CHSBackOffice.ViewModels
{
    internal class ATMInfoPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region Constants

        Color ActiveLabelColor = Color.FromHex("#0D72F4");
        Color InActiveLabelColor = Color.FromHex("#8F8E93");

        public string ToolbarButtonText => ParametersGridVisible ? CHSIcons.Add : CHSIcons.Remote;

        public const string OverviewLabelNameConst = "Overview";
        public const string DetailsLabelNameConst = "Details";
        public const string TransactionsLabelNameConst = "Transactions";
        public const string ParametersLabelNameConst = "Parameters";

        internal bool CanLoadMore = true;
        #endregion

        #region BINDABLE PROPS

        #region PageTitle
        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }
        #endregion

        #region Machine information (Overview and details)

        private string _location;
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        private KioskState _state;
        public KioskState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        private KioskStatus _status;
        public KioskStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private Color stateColor;
        public Color ColorState
        {
            get => stateColor;
            set => SetProperty(ref stateColor, value);
        }

        private string _chsVersion;
        public string CHSVersion
        {
            get => _chsVersion;
            set => SetProperty(ref _chsVersion, value);
        }

        private string _runtimeStatus;
        public string RuntimeStatus
        {
            get => _runtimeStatus;
            set => SetProperty(ref _runtimeStatus, value);
        }

        private string _extendedStatus;
        public string ExtendedStatus
        {
            get => _extendedStatus;
            set => SetProperty(ref _extendedStatus, value);
        }

        private string _group;
        public string Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }

        private string _iPAddress;
        public string IPAddress
        {
            get => _iPAddress;
            set => SetProperty(ref _iPAddress, value);
        }

        private string _keywords;
        public string Keywords
        {
            get => _keywords;
            set => SetProperty(ref _keywords, value);
        }

        private string _serialNumber;
        public string SerialNumber
        {
            get => _serialNumber;
            set => SetProperty(ref _serialNumber, value);
        }

        private List<KeyValuePair<string, string>> _deviceAndInterfaceVersion;
        public List<KeyValuePair<string, string>> DeviceAndInterfaceVersion
        {
            get => _deviceAndInterfaceVersion;
            set => SetProperty(ref _deviceAndInterfaceVersion, value);
        }

        #endregion

        #region Events List

        private List<EventExtended> _eventsList;
        public List<EventExtended> EventsList
        {
            get => _eventsList;
            set
            {
                SetProperty(ref _eventsList, value);
                RaisePropertyChanged(nameof(EventsListHasNoData));
            }
        }
        public bool EventsListHasNoData => EventsList?.Count == 0;

        #endregion

        #region Recycler details

        private List<RecyclerExtended> _recyclerDetails;
        public List<RecyclerExtended> RecyclerDetails
        {
            get => _recyclerDetails;
            set
            {
                SetProperty(ref _recyclerDetails, value);
                RaisePropertyChanged(nameof(Recycler));
                RaisePropertyChanged(nameof(RecyclerDetailsIsVisible));
            }
        }

        public bool RecyclerDetailsIsVisible => RecyclerDetails?.Count > 0;
        public Recycler[] Recycler => RecyclerDetails?.Select(c => c.BaseObject).ToArray();

        #endregion

        #region Cash dispenser details

        private List<DispenserElementExtended> _cashDispenserDetails;
        public List<DispenserElementExtended> CashDispenserDetails
        {
            get => _cashDispenserDetails;
            set
            {
                SetProperty(ref _cashDispenserDetails, value);
                RaisePropertyChanged(nameof(CashDispenser));
                RaisePropertyChanged(nameof(CashDispenserDetailsIsVisible));
            }
        }

        public bool CashDispenserDetailsIsVisible => CashDispenserDetails?.Count > 0;
        public ArrayOfDispenserDispenser[] CashDispenser => CashDispenserDetails?.Select(c => c.BaseObject).ToArray();

        #endregion

        #region Coin hopper details

        private List<DispenserElementExtended> _coinHopperDetails;
        public List<DispenserElementExtended> CoinHopperDetails
        {
            get => _coinHopperDetails;
            set
            {
                SetProperty(ref _coinHopperDetails, value);
                RaisePropertyChanged(nameof(CoinHopper));
                RaisePropertyChanged(nameof(CoinHopperDetailsIsVisible));
            }
        }

        public bool CoinHopperDetailsIsVisible => CoinHopperDetails?.Count > 0;
        public ArrayOfDispenserDispenser[] CoinHopper => CoinHopperDetails?.Select(c => c.BaseObject).ToArray();

        #endregion

        #region Bill acceptor details

        private List<BillAcceptorExtended> _billAcceptorDetails;
        public List<BillAcceptorExtended> BillAcceptorDetails
        {
            get => _billAcceptorDetails;
            set
            {
                SetProperty(ref _billAcceptorDetails, value);
                RaisePropertyChanged(nameof(BillAcceptor));
                RaisePropertyChanged(nameof(BillAcceptorDetailsIsVisible));
            }
        }

        public bool BillAcceptorDetailsIsVisible => BillAcceptorDetails?.Count > 0;
        public ArrayOfAcceptorAcceptor[] BillAcceptor => BillAcceptorDetails?.Select(c => c.BaseObject).ToArray();

        #endregion

        #region Parameters list

        private List<ParameterInfoClass> _parametersList;
        public List<ParameterInfoClass> ParametersList
        {
            get => _parametersList;
            set
            {
                SetProperty(ref _parametersList, value);
                RaisePropertyChanged(nameof(ParametersListHasNoData));
            }
        }

        public bool ParametersListHasNoData => ParametersList?.Count == 0;

        #endregion

        private KioskExtended _machineInfo;
        public KioskExtended MachineInfo 
        {
            get => _machineInfo;
            set => SetProperty(ref _machineInfo, value);
        }

        #region Remote control

        #region Commands buttons availibility

        public bool ShutdownCommandEnabled => StateInfoService.UserPermissions.RemoteControl.Shutdown;
        public bool RebootCommandEnabled => StateInfoService.UserPermissions.RemoteControl.Reboot;
        public bool CloseCommandEnabled => StateInfoService.UserPermissions.RemoteControl.Close;
        public bool InServiceCommandEnabled => StateInfoService.UserPermissions.RemoteControl.InService;
        public bool OutOfServiceCommandEnabled => StateInfoService.UserPermissions.RemoteControl.OutOfService;

        #endregion

        public string CommandConfirmationText => $"Do you want to send {Enum.GetName(typeof(CommandType), _selectedCommandType)} command?";

        private string _commandCommentText;
        public string CommandCommentText
        {
            get => _commandCommentText;
            set => SetProperty(ref _commandCommentText, value);
        }

        private bool _remoteControlSendedVisible;
        public bool RemoteControlSendedVisible
        {
            get => _remoteControlSendedVisible;
            set => SetProperty(ref _remoteControlSendedVisible, value);
        }

        private bool _remoteControlVisible;
        public bool RemoteControlVisible
        {
            get => _remoteControlVisible;
            set => SetProperty(ref _remoteControlVisible, value);
        }
        

        private bool _remoteControlMenuVisible;
        public bool RemoteControlMenuVisible
        {
            get => _remoteControlMenuVisible;
            set => SetProperty(ref _remoteControlMenuVisible, value);
        }
        

        private bool _remoteControlConfirmVisible;
        public bool RemoteControlConfirmVisible
        {
            get => _remoteControlConfirmVisible;
            set => SetProperty(ref _remoteControlConfirmVisible, value);
        }

        CommandType _selectedCommandType;
        CommandType SelectedCommandType
        {
            get => _selectedCommandType;
            set
            {
                _selectedCommandType = value;
                RaisePropertyChanged(nameof(CommandConfirmationText));
            }
        }

        private string _CommandSendResultText;
        public string CommandSendResultText
        {
            get => _CommandSendResultText;
            set => SetProperty(ref _CommandSendResultText, value);

        }

        #endregion

        #region Parameters page

        public bool EditParamVisible
        {
            get => _editParamVisible;
            set => SetProperty(ref _editParamVisible, value);
        }
        private bool _editParamVisible;

        private Parameter _selectedParameter;
        public Parameter SelectedParameter
        {
            get => _selectedParameter;
            set => SetProperty(ref _selectedParameter, value);
        }

        private string _addOrEditParameterCaption;
        public string AddOrEditParameterCaption
        {
            get => _addOrEditParameterCaption;
            set => SetProperty(ref _addOrEditParameterCaption, value);
        }

        private IEnumerable<PopoverItem> _parameterIDs;
        public IEnumerable<PopoverItem> ParameterIDs
        {
            get => _parameterIDs;
            set { SetProperty(ref _parameterIDs, value);
                RaisePropertyChanged(nameof(ParameterIDs));
            }
        }

        private bool _isParameterAdd;
        public bool IsParameterAdd
        {
            get => _isParameterAdd;
            set { 
                SetProperty(ref _isParameterAdd, value);
                RaisePropertyChanged(nameof(IsParameterAdd));
            }
        }

        private PopoverItem _selectedParameterId;
        public PopoverItem SelectedParameterId
        {
            get => _selectedParameterId;
            set
            {
                if (value == null) return;

                if (_selectedParameterId != null)
                    _selectedParameterId.Selected = false;

                SetProperty(ref _selectedParameterId, value);
                RaisePropertyChanged(nameof(SelectedParameterId));

                if (_selectedParameterId != null)
                    _selectedParameterId.Selected = true;
            }
        }

        #endregion

        #region Transactions list

        private ObservableCollection<TransactionsExtended> _transactionsList;
        public ObservableCollection<TransactionsExtended> TransactionsList
        {
            get => _transactionsList;
            set
            {
                SetProperty(ref _transactionsList, value);
                RaisePropertyChanged(nameof(TransactionsListHasNoData));
            }
        }

        public bool TransactionsListHasNoData => !CommonViewObjects.Instance.IsLoadingVisible && TransactionsList?.Count == 0;

        #endregion

        #endregion

        #region Pages

        enum Pages
        {
            Overview,
            Details,
            Transactions,
            Parameters
        }

        #region Titles

        public string OverviewLabelName => OverviewLabelNameConst;
        public string DetailsLabelName => DetailsLabelNameConst;
        public string TransactionsLabelName => TransactionsLabelNameConst;
        public string ParametersLabelName => ParametersLabelNameConst;

        #endregion

        Pages _currentPage;
        Pages CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                RaisePropertyChanged(nameof(OverviewLableColor));
                RaisePropertyChanged(nameof(OverviewGridVisible));

                RaisePropertyChanged(nameof(DetailsLableColor));
                RaisePropertyChanged(nameof(DetailsGridVisible));

                RaisePropertyChanged(nameof(TransactionsLableColor));
                RaisePropertyChanged(nameof(TransactionsGridVisible));

                RaisePropertyChanged(nameof(ParametersLableColor));
                RaisePropertyChanged(nameof(ParametersGridVisible));

                RaisePropertyChanged(nameof(ToolbarButtonText));
            }
        }

        public Color OverviewLableColor => _currentPage == Pages.Overview ? ActiveLabelColor : InActiveLabelColor;
        public bool OverviewGridVisible => _currentPage == Pages.Overview;

        public Color DetailsLableColor => _currentPage == Pages.Details ? ActiveLabelColor : InActiveLabelColor;
        public bool DetailsGridVisible => _currentPage == Pages.Details;

        public Color TransactionsLableColor => _currentPage == Pages.Transactions ? ActiveLabelColor : InActiveLabelColor;
        public bool TransactionsGridVisible => _currentPage == Pages.Transactions;

        public Color ParametersLableColor => _currentPage == Pages.Parameters ? ActiveLabelColor : InActiveLabelColor;
        public bool ParametersGridVisible => _currentPage == Pages.Parameters;

        #endregion

        #region Fields

        Kiosk _machineData;
        static Thread _refreshThread;
        static bool _isPageActive;
        #endregion

        #region .CTOR

        public ATMInfoPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            PageTitle = "Kiosk ID - " + CommonViewObjects.Instance.CurrentUnit.Id;

            OnIsLoadingVisibleChanged += ATMInfoPageViewModel_OnIsLoadingVisibleChanged;
            CurrentPage = Pages.Overview;
            AddToolbarItemCommand("but1", ToolbarPressed);

            TransactionsCache.Instance.ClearCache();
            TransactionsCache.Instance.SetMachineId(CommonViewObjects.Instance.CurrentUnit.Id);

            if (Settings.AutoRefresh.BoolValue && _refreshThread == null)
            {
                _refreshThread = new Thread(RefreshPageInfo) { IsBackground = true };
                _refreshThread.Start();
            }

            SafeRefreshDataAsync();
        }

        #endregion

        #region Commands

        public ICommand ParameterTappedCommand => new Command<int>(async (parameterNumber) => await AddOrEditParameter(parameterNumber));
        public ICommand HideEditParamCommand => new DelegateCommand(HideParameterEdit);
        public ICommand SaveParameterCommand => new DelegateCommand(SaveParameter);

        public ICommand LabelTappedCommand => new DelegateCommand<string>(LabelTapped);
        public ICommand HideRemoteCommand => new DelegateCommand(() =>
        {
            RemoteControlVisible = false;
            RemoteControlMenuVisible = false;
            RemoteControlConfirmVisible = false;
            RemoteControlSendedVisible = false;
        });

        public ICommand RemoteCommand => new DelegateCommand<string>(SelectRemote, CanRemoteExecute);
        public ICommand SendCommand => new DelegateCommand(DoSelectedRemote);

        public ICommand DisappearingCommand => new DelegateCommand(DisappearingCommandExecuce);
        public ICommand AppearingCommand => new DelegateCommand(AppearingCommandExecuce);

        #region COMMAND HANDLERS

        private void AppearingCommandExecuce()
        {
            _isPageActive = Settings.AutoRefresh.BoolValue;
        }

        private void DisappearingCommandExecuce()
        {
            _isPageActive = false;
        }

        private async Task AddOrEditParameter(int parameterNumber = -1)
        {
            try
            {
                IsParameterAdd = ParametersList == null || ParametersList.Count <= parameterNumber || parameterNumber == -1;
                var parameterNames = new List<PopoverItem>();

                if (IsParameterAdd)
                {
                    SelectedParameter = new Parameter();
                    AddOrEditParameterCaption = "Add parameter details";
                    var parameterNamesArray = await _serviceAgent.GetParameterNames(StateInfoService.SessionId);
                    foreach (var t in parameterNamesArray)
                        if (!ParametersList.Any(p => p.Name == t))
                            parameterNames.Add(new PopoverItem { Key = t.Trim(), Value = t.Trim(), Selected = false });
                    if (SelectedParameterId != null)
                    {
                        _selectedParameterId = null;
                        //SelectedParameterId.Selected = false;
                        RaisePropertyChanged(nameof(SelectedParameterId));
                    }
                }
                else
                {
                    SelectedParameter = ParametersList[parameterNumber].BaseObject;
                    AddOrEditParameterCaption = "Edit " + SelectedParameter.Id + " parameter";
                    parameterNames = new List<PopoverItem> { new PopoverItem { Key = SelectedParameter.Id, Value = SelectedParameter.Id, Selected = true } };
                    SelectedParameterId = parameterNames[0];
                }
                ParameterIDs = parameterNames;

                //SelectedParameterId = ParameterIDs[0];
                EditParamVisible = true;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void HideParameterEdit()
        {
            EditParamVisible = false;
        }

        private async void SaveParameter()
        {
            try
            {
                string addRes = "";
                bool updateRes = false;
                if (IsParameterAdd)
                    addRes = await _serviceAgent.AddMachineParameter(StateInfoService.SessionId, _machineData.Id, SelectedParameterId.Key, SelectedParameter.Value);
                else
                    updateRes = await _serviceAgent.UpdateMachineParameter(StateInfoService.SessionId, _machineData.Id, SelectedParameter.Id, SelectedParameter.Value);

                SafeRefreshDataAsync();

                HideParameterEdit();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void LabelTapped(string labelName)
        {
            switch (labelName)
            {
                case OverviewLabelNameConst:
                    CurrentPage = Pages.Overview;
                    break;
                case DetailsLabelNameConst:
                    CurrentPage = Pages.Details;
                    break;
                case TransactionsLabelNameConst:
                    CurrentPage = Pages.Transactions;
                    break;
                case ParametersLabelNameConst:
                    CurrentPage = Pages.Parameters;
                    break;
            }

            if (CurrentPage == Pages.Transactions && TransactionsList == null)
                SafeRefreshDataAsync();
        }

        private bool CanRemoteExecute(string par)
        {
            switch (par)
            {
                case "Shutdown":
                    return ShutdownCommandEnabled;
                case "Reboot":
                    return RebootCommandEnabled;
                case "Close":
                    return CloseCommandEnabled;
                case "InService":
                    return InServiceCommandEnabled;
                case "OutOfService":
                    return OutOfServiceCommandEnabled;
            }
            return false;
        }

        private void SelectRemote(string par)
        {
            SelectedCommandType = CommandType.Unknown;
            switch (par)
            {
                case "Shutdown":
                    SelectedCommandType = CommandType.Shutdown;
                    break;
                case "Reboot":
                    SelectedCommandType = CommandType.Reboot;
                    break;
                case "Close":
                    SelectedCommandType = CommandType.ShutdownApplication;
                    break;
                case "InService":
                    SelectedCommandType = CommandType.InService;
                    break;
                case "OutOfService":
                    SelectedCommandType = CommandType.OutOfService;
                    break;
            }

            if (SelectedCommandType != CommandType.Unknown)
            {
                RemoteControlMenuVisible = false;
                RemoteControlConfirmVisible = true;
            }
        }

        private async void DoSelectedRemote()
        {
            try
            {
                var result = await _serviceAgent.RemoteControl(StateInfoService.SessionId, SelectedCommandType, _machineData.Id, CommandCommentText);

                RemoteControlConfirmVisible = false;
                CommandSendResultText = result ? "Sended" : "Failed";

                //if (result && !CommonViewObjects.MachinesIdNeedsToRefresh.Contains(_machineData.Id))
                //    CommonViewObjects.MachinesIdNeedsToRefresh.Add(_machineData.Id);

                RaisePropertyChanged(nameof(CommandSendResultText));
                RemoteControlSendedVisible = true;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private async void ToolbarPressed(ToolbarButton _)
        {
            switch (_.Name)
            {
                case "but1":
                    if (ParametersGridVisible)
                        await AddOrEditParameter();
                    else
                    {
                        RemoteControlConfirmVisible = false;
                        RemoteControlMenuVisible = true;
                        RemoteControlVisible = true;
                        RemoteControlSendedVisible = false;
                    }
                    break;
            }
        }

        #endregion

        #endregion

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            if (CurrentPage != Pages.Transactions)
            {
                var tmp = await _serviceAgent.GetKioskDetails(StateInfoService.SessionId, CommonViewObjects.Instance.CurrentUnit.Id);
                Kiosk.FillFromDetails(ref _machineData, tmp);
                UpdateViewData();
                MachineInfo = new KioskExtended { BaseObject = _machineData };
            }
            else
            {
                if (TransactionsListHasNoData)
                    return;

                if (TransactionsList == null)
                    TransactionsList = new ObservableCollection<TransactionsExtended>();

                var transactionList = new List<TransactionsExtended>(TransactionsList);

                int i = transactionList != null ? transactionList.Count - 1 : 0;
                
                var datapart = await TransactionsCache.Instance.LoadMore(); //await _serviceAgent.GetMachineTransactions(StateInfoService.SessionId, _machineData.Id, _offset, TransactionsPageSize);
                foreach (var t in datapart)
                    transactionList.Add(new TransactionsExtended { BaseObject = t, Number = i++ });
                CanLoadMore = datapart.Length >= TransactionsCache.Instance.PageSize;

                TransactionsList = new ObservableCollection<TransactionsExtended>(transactionList);
            }

        }

        #endregion

        #region PRIVATE METHODS

        async void RefreshPageInfo()
        {
            //int lastVal = 0;
            while (true)
            {
                await Task.Delay(Support.Constants.AutoRefreshInterval);
                if (Support.CommonViewObjects.Instance.IsLoadingVisible || !_isPageActive || _machineData == null) continue;

                try
                {
                    var kioskDetails = await _serviceAgent.GetKioskDetails(StateInfoService.SessionId, _machineData.Id);

                    //if (kioskDetails.BillAcceptor != null && kioskDetails.BillAcceptor.Length > 0)
                    //{
                    //    kioskDetails.BillAcceptor[0].BillsCount += lastVal;
                    //    kioskDetails.BillAcceptor[0].TicketCount += lastVal;
                    //    kioskDetails.BillAcceptor[0].Count = kioskDetails.BillAcceptor[0].BillsCount + kioskDetails.BillAcceptor[0].TicketCount;
                    //    lastVal += 10;
                    //}
                    

                    if (!Support.CommonViewObjects.Instance.IsLoadingVisible && kioskDetails != null && _machineData != null && kioskDetails.Id == _machineData.Id)
                    {
                        _machineData.BillAcceptor = kioskDetails.BillAcceptor;
                        _machineData.CashDispenser = kioskDetails.CashDispenser;
                        _machineData.CoinHopper = kioskDetails.CoinHopper;
                        _machineData.RecyclerCassettes = kioskDetails.RecyclerCassettes;
                        _machineData.LastCommunication = kioskDetails.LastCommunication;
                        _machineData.State = kioskDetails.State;
                        _machineData.Status = kioskDetails.Status;
                        _machineData.TicketNumbers = kioskDetails.TicketNumbers;
                        _machineData.RecyclerCassettes = kioskDetails.RecyclerCassettes;
                        _machineData.Location = kioskDetails.Location;

                        UpdateViewData();
                    }
                }
                catch
                { }
            }

        }

        private void ATMInfoPageViewModel_OnIsLoadingVisibleChanged()
        {
            RaisePropertyChanged(nameof(TransactionsListHasNoData));
        }

        private void UpdateViewData()
        {
            try
            {
                Location = _machineData.Location;
                State = _machineData.State;
                Status = _machineData.Status;
                ColorState = MatchStateStausToColor(_machineData);

                CHSVersion = _machineData.Version;
                RuntimeStatus = _machineData.LastCommunication;

                ExtendedStatus = State + " - " + Status;
                Group = _machineData.Group;
                IPAddress = _machineData.IpAddress;
                Keywords = String.Join(",", new string[] { _machineData.Keyword1, _machineData.Keyword1 }).Trim(new[] { ' ', ',' });
                SerialNumber = _machineData.SerialNumber;

                var deviceAndInterfaceVersion = new List<KeyValuePair<string, string>>();
                foreach (var t in _machineData.Versions)
                    deviceAndInterfaceVersion.Add(new KeyValuePair<string, string>(t.Id, t.Version));
                DeviceAndInterfaceVersion = deviceAndInterfaceVersion;
                /*kioskDetails.Versions*/

                int i = 0;
                if (_machineData.BillAcceptor != null)
                {
                    var billAcceptorDetails = new List<BillAcceptorExtended>();
                    foreach (var t in _machineData.BillAcceptor)
                        billAcceptorDetails.Add(new BillAcceptorExtended { BaseObject = t, Number = i++ });
                    BillAcceptorDetails = billAcceptorDetails;
                }

                if (_machineData.Events != null)
                {
                    i = 0;
                    var events = new List<EventExtended>();
                    foreach (var t in _machineData.Events)
                    {
                        t.EventDate = GetDateTimeStringFromHtml(t.EventDate);
                        events.Add(new EventExtended { BaseObject = t, Number = i++ });
                    }
                    EventsList = events;
                }

                if (_machineData.RecyclerCassettes != null)
                {
                    i = 0;
                    var recyclerDetails = new List<RecyclerExtended>();
                    foreach (var t in _machineData.RecyclerCassettes)
                        recyclerDetails.Add(new RecyclerExtended { BaseObject = t, Number = i++ });
                    RecyclerDetails = recyclerDetails;
                }

                if (_machineData.CashDispenser != null)
                {
                    i = 0;
                    var cashDispenserDetails = new List<DispenserElementExtended>();
                    foreach (var t in _machineData.CashDispenser)
                        cashDispenserDetails.Add(new DispenserElementExtended { BaseObject = t, Number = i++ });
                    CashDispenserDetails = cashDispenserDetails;
                }
                
                if (_machineData.CoinHopper != null)
                {
                    i = 0;
                    var coinHopperDetails = new List<DispenserElementExtended>();
                    foreach (var t in _machineData.CoinHopper)
                        coinHopperDetails.Add(new DispenserElementExtended { BaseObject = t, Number = i++ });
                    CoinHopperDetails = coinHopperDetails;
                }
                
                if (_machineData.ParameterCollection != null)
                {
                    i = 0;
                    var parametersList = new List<ParameterInfoClass>();
                    foreach (var t in _machineData.ParameterCollection)
                        parametersList.Add(new ParameterInfoClass { BaseObject = t, Number = i++ });
                    ParametersList = parametersList;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            
        }

        private Color MatchStateStausToColor(Kiosk _machineData)
        {
            if (_machineData.State == CHBackOffice.ApiServices.ChsProxy.KioskState.InService ||
                   _machineData.State == CHBackOffice.ApiServices.ChsProxy.KioskState.ONLINE)
            {
                if (_machineData.Status == CHBackOffice.ApiServices.ChsProxy.KioskStatus.Normal)
                {
                    return StaticResourceManager.GetColor("StatusNormal");
                }
                else if (_machineData.Status == CHBackOffice.ApiServices.ChsProxy.KioskStatus.Warning)
                {
                    return StaticResourceManager.GetColor("StatusWarning");
                }
                else
                {
                    return StaticResourceManager.GetColor("StatusCritical");
                }
            }
            else if (_machineData.State == CHBackOffice.ApiServices.ChsProxy.KioskState.Offline)
            {
                return StaticResourceManager.GetColor("StatusOffline");
            }
            else if (_machineData.State == CHBackOffice.ApiServices.ChsProxy.KioskState.OutOfServiceSOP)
            {
                return StaticResourceManager.GetColor("StatusSOP");
            }
            else
            {
                return StaticResourceManager.GetColor("StatusOOS");
            }
        }
        string GetDateTimeStringFromHtml(string dateTimeHtml)
        {
            try
            {
                Regex r = new Regex("((?<=<b><i>)(.*?)(?=\\s*</i></b>))|((?<=</b>).*)", RegexOptions.IgnoreCase);

                var matchValues = r.Matches(dateTimeHtml)
                    .Cast<Match>()
                    .Select(m => m.Value)
                    .ToArray();

                if (matchValues.Length == 2)
                {
                    var date = Regex.Replace(matchValues[1], @"\s+", "");
                    var time = matchValues[0];

                    return $"{date} {time}";
                }

                return dateTimeHtml.Replace("<b>", "").Replace("</b>", "").Replace("<i>", "").Replace("</i>", "");
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return string.Empty;
            }
        }

        #endregion

    }
}
