using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Models;
using CHSBackOffice.Models.Popup;
using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    internal class RemoteControlPageViewModel : ExtendedNaviPageViewModelBase
    {
        const string AllMachinesFilterKey = "All";

        #region "PUBLIC PROPS"

        public string ViewTypeText => Constants.CHSIcons.TopMenu;

        public string ShutdownString => GetEnumCaption(MachineAction.Shutdown);
        public string RebootString => GetEnumCaption(MachineAction.Reboot);
        public string CloseString => GetEnumCaption(MachineAction.Close);
        public string InServiceString => GetEnumCaption(MachineAction.In_service);
        public string OutOfServiceString => GetEnumCaption(MachineAction.Out_of_service);


        public bool ShutdownEnabled => StateInfoService.UserPermissions.RemoteControl.Shutdown;
        public bool RebootEnabled => StateInfoService.UserPermissions.RemoteControl.Reboot;
        public bool CloseEnabled => StateInfoService.UserPermissions.RemoteControl.Close;
        public bool InServiceEnabled => StateInfoService.UserPermissions.RemoteControl.InService;
        public bool OutOfServiceEnabled => StateInfoService.UserPermissions.RemoteControl.OutOfService;


        public string SelectAllText => "SELECT ALL";
        public string InvertSelectionText => "INVERT";
        public string ClearAllText => "CLEAR ALL";

        #endregion

        #region "BINDABLE PROPS"

        #region "Machines"

        private List<Machine> _allMachines;

        private ObservableCollection<Machine> _machines;
        public ObservableCollection<Machine> Machines
        {
            get => _machines;
            set => SetProperty(ref _machines, value);
        }

        #endregion

        #region "IsRemoteCommandButtonsVisible"

        private bool _isRemoteCommandButtonsVisible;
        public bool IsRemoteCommandButtonsVisible
        {
            get => _isRemoteCommandButtonsVisible;
            set => SetProperty(ref _isRemoteCommandButtonsVisible, value);
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

        #region AdditionalWindowsVisible

        private bool _AdditionalWindowsVisible;
        public bool AdditionalWindowsVisible
        {
            get => _AdditionalWindowsVisible;
            set => SetProperty(ref _AdditionalWindowsVisible, value);
        }

        #endregion

        #region FilterGridVisible

        private bool _FilterGridVisible;
        public bool FilterGridVisible
        {
            get => _FilterGridVisible;
            set => SetProperty(ref _FilterGridVisible, value);

        }

        #endregion

        #region ConfirmVisible

        private bool _ConfirmVisible;
        public bool ConfirmVisible
        {
            get => _ConfirmVisible;
            set => SetProperty(ref _ConfirmVisible, value);

        }

        #endregion

        #region CommandConfirmationText

        private string _CommandConfirmationText;
        public string CommandConfirmationText
        {
            get => _CommandConfirmationText;
            set => SetProperty(ref _CommandConfirmationText, value);

        }

        #endregion

        #region CommandCommentText

        private string _CommandCommentText;
        public string CommandCommentText
        {
            get => _CommandCommentText;
            set => SetProperty(ref _CommandCommentText, value);

        }

        #endregion

        #region CommandSendedVisible

        private bool _CommandSendedVisible;
        public bool CommandSendedVisible
        {
            get => _CommandSendedVisible;
            set => SetProperty(ref _CommandSendedVisible, value);

        }

        #endregion

        #region SelectedCommandType

        CHBackOffice.ApiServices.ChsProxy.CommandType _selectedCommandType;
        CHBackOffice.ApiServices.ChsProxy.CommandType SelectedCommandType
        {
            get => _selectedCommandType;
            set
            {
                _selectedCommandType = value;
                CommandConfirmationText = $"Do you want to send {Enum.GetName(typeof(CHBackOffice.ApiServices.ChsProxy.CommandType), _selectedCommandType)} command?";
            }
        }

        #endregion

        #region CommandSendResultText

        private string _CommandSendResultText;
        public string CommandSendResultText
        {
            get => _CommandSendResultText;
            set => SetProperty(ref _CommandSendResultText, value);

        }

        #endregion

        #region FilterItems

        private List<PopoverItem> _FilterItems;
        public List<PopoverItem> FilterItems
        {
            get => _FilterItems;
            set => SetProperty(ref _FilterItems, value);

        }

        #endregion

        #region SelectedFilterItem

        private PopoverItem _SelectedFilterItem;
        public PopoverItem SelectedFilterItem
        {
            get => _SelectedFilterItem;
            set
            {
                if (_SelectedFilterItem != null)
                    _SelectedFilterItem.Selected = false;

                SetProperty(ref _SelectedFilterItem, value);

                if (_SelectedFilterItem != null)
                {
                    _SelectedFilterItem.Selected = true;

                    SafeRefreshDataAsync();
                }

                HideAdditionalWindowsCommand?.Execute(null);
            }

        }

        #endregion

        #endregion

        #region ".CTOR"

        public RemoteControlPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            AddToolbarItemCommand("but1", new Action<CustomControls.ToolbarButton>(SearchPressed));
            AddToolbarItemCommand("filterToolbarButton", new Action<CustomControls.ToolbarButton>(FilterPressed));

            FilterItems = new List<PopoverItem>
            {
                new PopoverItem { Key = AllMachinesFilterKey },
                new PopoverItem { Key = CHBackOffice.ApiServices.ChsProxy.KioskStatus.Normal.ToString()},
                new PopoverItem { Key = CHBackOffice.ApiServices.ChsProxy.KioskStatus.Critical.ToString() },
                new PopoverItem { Key = CHBackOffice.ApiServices.ChsProxy.KioskStatus.Warning.ToString() }
            };

            SelectedFilterItem = FilterItems[0];


            
            //LoadData();

        }

        #endregion

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            _allMachines = new List<Machine>();
            var allMachines = await _serviceAgent.GetMachineList(StateInfoService.SessionId);
            foreach (var t in allMachines)
            {
                if (SelectedFilterItem == null || SelectedFilterItem.Key == "All" || SelectedFilterItem.Key == t.Status.ToString())
                    _allMachines.Add(new Machine
                    {
                        Name = t.Id,
                        Id = t.Id,
                        Selected = false
                    });
            }
            Machines = new ObservableCollection<Machine>(_allMachines);
        }

        #endregion

        #region "COMMANDS"

        public ICommand MachineTappedCommand => new Command<Machine>(SelectMachine);
        public ICommand SelectAllCommand => new Command(SelectAll);
        public ICommand InvertSelectionCommand => new Command(InvertSelection);
        public ICommand ClearAllCommand => new Command(ClearAll);

        public ICommand MachineActionCommand => new Command<string>(ProcessMachineAction, MachineActionAllowed);

        public ICommand SearchCommand => new Command(SearchExecute);

        public ICommand HideAdditionalWindowsCommand => new Command(() =>
        {
            CommandSendedVisible = false;
            ConfirmVisible = false;
            AdditionalWindowsVisible = false;
            CommandCommentText = "";
            FilterGridVisible = false;
        });

        public ICommand SendRemoteCommand => new Command(SendToMachines);

        #region "COMMAND HANDLERS"

        void ProcessMachineAction(string actionText)
        {
            try
            {
                MachineAction action = (MachineAction)Enum.Parse(typeof(MachineAction), actionText.Replace(" ", "_"));
                var selectedMachines = Machines.Where(m => m.Selected).ToList();
                if (selectedMachines.Count > 0)
                    DoWithMachines(selectedMachines, action);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        bool MachineActionAllowed(string actionText)
        {
            try
            {
                if (String.IsNullOrEmpty(actionText)) return false;
                MachineAction action = (MachineAction)Enum.Parse(typeof(MachineAction), actionText.Replace(" ", "_"));
                switch (action)
                {
                    case MachineAction.Shutdown:
                        return ShutdownEnabled;
                    case MachineAction.Reboot:
                        return RebootEnabled;
                    case MachineAction.Close:
                        return CloseEnabled;
                    case MachineAction.In_service:
                        return InServiceEnabled;
                    case MachineAction.Out_of_service:
                        return OutOfServiceEnabled;
                }
                return false;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return false;
            }
        }

        void SelectMachine(Machine machine)
        {
            machine.Selected = !machine.Selected;
            SetRemoteCommandButtonsVisibility();
        }

        void SelectAll()
        {
            if (Machines == null)
                return;

            foreach (var t in Machines) t.Selected = true;
            SetRemoteCommandButtonsVisibility();
        }

        void InvertSelection()
        {
            if (Machines == null)
                return;

            foreach (var t in Machines) t.Selected = !t.Selected;
            SetRemoteCommandButtonsVisibility();
        }

        void ClearAll()
        {
            if (Machines == null)
                return;

            foreach (var t in Machines) t.Selected = false;
            SetRemoteCommandButtonsVisibility();
        }

        void SearchExecute()
        {
            if (_allMachines != null)
            {
                var allMachines = _allMachines.ToList();
                allMachines.ForEach(m => m.Selected = false);
                Machines = new ObservableCollection<Machine>(allMachines.Where(m => string.IsNullOrEmpty(SearchText) || m.Id.ToUpper().Contains(SearchText.ToUpper())));
            }

        }

        #endregion

        #endregion

        #region "PRIVATE METHODS"

        void FilterPressed(CustomControls.ToolbarButton button)
        {
            AdditionalWindowsVisible = true;
            FilterGridVisible = true;
        }

        void SearchPressed(CustomControls.ToolbarButton button)
        {
            button?.Command?.Execute(null);
        }

        string GetEnumCaption(MachineAction action)
        {
            return Enum.GetName(typeof(MachineAction), action).Replace("_", " ");
        }

        void DoWithMachines(List<Machine> machines, MachineAction action)
        {
            SelectedCommandType = CHBackOffice.ApiServices.ChsProxy.CommandType.Unknown;
            switch (action)
            {
                case MachineAction.Shutdown:
                    SelectedCommandType = CHBackOffice.ApiServices.ChsProxy.CommandType.Shutdown;
                    break;
                case MachineAction.Reboot:
                    SelectedCommandType = CHBackOffice.ApiServices.ChsProxy.CommandType.Reboot;
                    break;
                case MachineAction.Close:
                    SelectedCommandType = CHBackOffice.ApiServices.ChsProxy.CommandType.ShutdownApplication;
                    break;
                case MachineAction.In_service:
                    SelectedCommandType = CHBackOffice.ApiServices.ChsProxy.CommandType.InService;
                    break;
                case MachineAction.Out_of_service:
                    SelectedCommandType = CHBackOffice.ApiServices.ChsProxy.CommandType.OutOfService;
                    break;
            }
            AdditionalWindowsVisible = true;
            ConfirmVisible = true;


        }

        async void SendToMachines()
        {
            try
            {
                var machines = Machines.Where(m => m.Selected).ToList();

                bool wasSuccess = false;
                bool allSuccess = true;
                foreach (var t in machines)
                {
                    var result = await _serviceAgent.RemoteControl(StateInfoService.SessionId, SelectedCommandType, t.Id, CommandCommentText);
                    wasSuccess = wasSuccess || result;
                    allSuccess = allSuccess && result;
                }

                ConfirmVisible = false;
                CommandSendedVisible = true;

                CommandSendResultText = allSuccess ? "Sended succesfully" : (wasSuccess ? "Sended with errors" : "All failed");
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void SetRemoteCommandButtonsVisibility()
        {
            IsRemoteCommandButtonsVisible = Machines.Any(m => m.Selected);
        }

        #endregion
    }
}