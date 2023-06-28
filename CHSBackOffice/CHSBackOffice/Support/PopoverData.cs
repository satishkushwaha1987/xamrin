using CHSBackOffice.Views;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace CHSBackOffice.Support
{
    public class PopoverData : BindableBase
    {
        public static PopoverData Instance = new PopoverData();

        public bool FilterAction { get; set; } = false;
        public bool LoadMoreAction { get; set; } = true;


        private ObservableCollection<Models.Popup.PopoverItem> _settingsPopoverItems;
        public ObservableCollection<Models.Popup.PopoverItem> SettingsPopoverItems
        {
            get => _settingsPopoverItems;
            set
            {
                SetProperty(ref _settingsPopoverItems, value);
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _machineIDs;
        public ObservableCollection<Models.Popup.PopoverItem> MachineIDs
        {
            get => _machineIDs;
            set
            {
                SetProperty(ref _machineIDs, value);
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _eventDescriptions;
        public ObservableCollection<Models.Popup.PopoverItem> EventDescriptions
        {
            get => _eventDescriptions;
            set
            {
                SetProperty(ref _eventDescriptions, value);
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _machineStatuses;
        public ObservableCollection<Models.Popup.PopoverItem> MachineStatuses
        {
            get => _machineStatuses;
            set
            {
                SetProperty(ref _machineStatuses, value);
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _eventSeverites;
        public ObservableCollection<Models.Popup.PopoverItem> EventSeverites
        {
            get => _eventSeverites;
            set
            {
                SetProperty(ref _eventSeverites, value);
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _machineStates;
        public ObservableCollection<Models.Popup.PopoverItem> MachineStates
        {
            get => _machineStates;
            set
            {
                SetProperty(ref _machineStates, value);
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _machineTypes;
        public ObservableCollection<Models.Popup.PopoverItem> MachineTypes
        {
            get => _machineTypes;
            set
            {
                SetProperty(ref _machineTypes, value);
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _transactionTypes;
        public ObservableCollection<Models.Popup.PopoverItem> TransactionTypes
        {
            get => _transactionTypes;
            set
            {
                SetProperty(ref _transactionTypes, value);
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _transactionStatuses;
        public ObservableCollection<Models.Popup.PopoverItem> TransactionStatuses
        {
            get => _transactionStatuses;
            set
            {
                SetProperty(ref _transactionStatuses, value);
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _groups;
        public ObservableCollection<Models.Popup.PopoverItem> Groups
        {
            get => _groups;
            set
            {
                SetProperty(ref _groups, value);
            }
        }

        public DateTime EndDateTimeSelected { get; set; }

        public DateTime StartDateTimeSelected { get; set; }

        public int CheckedIndexRadioButtonGroup { get; set; } = 2;

        public string TransactionIdSelected { get; set; }

        public string SequenceIdSelected { get; set; }

        void CheckAndAdd(ref List<Models.Popup.PopoverItem> list, string pageCaption, Type pageType)
        {
            if (PagesPermissions.CheckRights(pageType))
                list.Add(new Models.Popup.PopoverItem { Key = pageCaption, Value = pageType.Name, Selected = false });
        }

        internal void InitMenu()
        {
            List<Models.Popup.PopoverItem> list = new List<Models.Popup.PopoverItem>();
            CheckAndAdd(ref list, "Machine Status", typeof(MachineStatusSkiaPage));
            CheckAndAdd(ref list, "Events", typeof(EventsPage));
            CheckAndAdd(ref list, "Door Open Events", typeof(DoorOpenEventsPage));
            CheckAndAdd(ref list, "Dash Board", typeof(DashboardPage));
            CheckAndAdd(ref list, "Cash On Hand", typeof(CashOnHandPage));
            CheckAndAdd(ref list, "Cash Utilization", typeof(CashUtilizationPage));
            CheckAndAdd(ref list, "All Transactions", typeof(AllTransactionsPage));
            CheckAndAdd(ref list, "Transaction by Type", typeof(TransactionByTypePage));
            CheckAndAdd(ref list, "Machine Availability", typeof(MachineAvallabilityPage));
            CheckAndAdd(ref list, "Active Floats", typeof(ActiveFloatsPage));
            CheckAndAdd(ref list, "Remote Control", typeof(RemoteControlPage));
            CheckAndAdd(ref list, "System Parameters", typeof(SystemParametersPage));
            CheckAndAdd(ref list, "Employee Management", typeof(EmployeeManagmentPage));
            CheckAndAdd(ref list, "Users", typeof(UsersPage));
            CheckAndAdd(ref list, "SOP Users", typeof(SOPUsersPage));
            CheckAndAdd(ref list, "Employees", typeof(EmployeesPage));
            CheckAndAdd(ref list, "Transactions", typeof(TransactionsPage));
            CheckAndAdd(ref list, "Settings", typeof(SettingsPage));

            SettingsPopoverItems = new ObservableCollection<Models.Popup.PopoverItem>(list);
        }
    }
}
