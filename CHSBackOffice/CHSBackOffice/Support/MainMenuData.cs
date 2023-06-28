using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.Support
{
    public class MainMenuData : INotifyPropertyChanged
    {
        #region "PRIVATE FIELDS"

        private static Dictionary<string, bool> _menuState;

        #endregion

        #region "PUBLIC PROPS"

        public static MainMenuData Instance = new MainMenuData();
        public event PropertyChangedEventHandler PropertyChanged;

        public string MainMenuUserSiluetText => Constants.CHSIcons.UserSiluet;
        public Color MainMenuUserSiluetColor => Constants.CHSIconColors.UserSiluet;
        public Color MainMenuBackgroundColor => Color.FromHex("#04783B");

        #endregion

        #region "BINDABLE PROPS"

        private bool _menuIsActive;
        public bool MenuIsActive
        {
            get => _menuIsActive;
            set 
            {
                _menuIsActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuIsActive)));
            }
        }

        private ObservableCollection<Models.MenuItem> _mainMenuItems;
        public ObservableCollection<Models.MenuItem> MainMenuItems
        {
            get
            {
                return _mainMenuItems;
            }
            set
            {
                _mainMenuItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MainMenuItems)));
            }
        }

        private ObservableCollection<Models.MenuItem> _dashBoardItems;
        public ObservableCollection<Models.MenuItem> DashBoardItems
        {
            get => _dashBoardItems;
            set
            {
                _dashBoardItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MainMenuItems)));
            }
        }

        private ObservableCollection<Models.MenuItem> _employeeManagementItems;
        public ObservableCollection<Models.MenuItem> EmployeeManagementItems
        {
            get => _employeeManagementItems;
            set
            {
                _employeeManagementItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmployeeManagementItems)));
            }
        }

        #endregion

        #region ".CTOR"

        public MainMenuData()
        {
            _menuState = new Dictionary<string, bool>();
            InitMenu();
            MenuIsActive = true;
        }

        #endregion

        #region "COMMANDS"

        public ICommand MenuItemTappedCommand => new Command<Models.MenuItem>(InternalMenuItemTapped);

        #region "COMMAND HANDLERS"

        void InternalMenuItemTapped(Models.MenuItem menuItem)
        {
            try
            {
                var commandType = menuItem.CommandParameter as Type;
                if (commandType == null)
                    throw new NotSupportedException("Command parameter must be a type");

                switch (menuItem.MenuActionType)
                {
                    case Models.MenuActionTypes.NavigateDetailPage:
                        Services.Navigation.NavigateDetailPage(menuItem.CommandParameter as Type);
                        break;
                    case Models.MenuActionTypes.SetDetailPage:
                        Services.Navigation.SetDetailPage(menuItem.CommandParameter as Type);
                        break;
                    case Models.MenuActionTypes.SetMainPage:
                        Services.Navigation.SetMainPage(menuItem.CommandParameter as Type);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #endregion

        #region "PRIVATE MEMBERS"

        internal void InitMenu()
        {
            #region Main Menu

            List<Models.MenuItem> menuItems = new List<Models.MenuItem>();

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Title = "Machine Status",
                Icon = Constants.CHSIcons.MachineStatus,
                IconColor = Constants.CHSIconColors.MachineStatus,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.MachineStatusSkiaPage),
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Title = "Events",
                Icon = Constants.CHSIcons.Events,
                IconColor = Constants.CHSIconColors.Events,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.EventsPage),
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Title = "Door Open Events",
                Icon = Constants.CHSIcons.DoorOpenEvents,
                IconColor = Constants.CHSIconColors.DoorOpenEvents,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.DoorOpenEventsPage)
            });

            #region Dashboards menu group

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Title = "Dashboard",
                Icon = Constants.CHSIcons.Dashboard,
                IconColor = Constants.CHSIconColors.Dashboard,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.DashboardPage)
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Level = 2,
                Title = "Cash On Hand",
                Icon = Constants.CHSIcons.CashOnHand,
                IconColor = Constants.CHSIconColors.CashOnHand,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.CashOnHandPage)
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Level = 2,
                Title = "Cash Utilization",
                Icon = Constants.CHSIcons.CashUtilization,
                IconColor = Constants.CHSIconColors.CashUtilization,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.CashUtilizationPage)
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Level = 2,
                Title = "All Transactions",
                Icon = Constants.CHSIcons.AllTransactions,
                IconColor = Constants.CHSIconColors.AllTransactions,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.AllTransactionsPage)
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Level = 2,
                Title = "Transaction by Type",
                Icon = Constants.CHSIcons.TransactionByType,
                IconColor = Constants.CHSIconColors.TransactionByType,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.TransactionByTypePage)
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Level = 2,
                Title = "Machine Availability",
                Icon = Constants.CHSIcons.MachineAvalabitity,
                IconColor = Constants.CHSIconColors.MachineAvalabitity,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.MachineAvallabilityPage)
            });

            #endregion

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Title = "Remote Control",
                Icon = Constants.CHSIcons.RemoteControl,
                IconColor = Constants.CHSIconColors.RemoteControl,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.RemoteControlPage),
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Title = "Active Floats",
                Icon = Constants.CHSIcons.ActiveFloats,
                IconColor = Constants.CHSIconColors.ActiveFloats,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.ActiveFloatsPage)
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Title = "System Parameters",
                Icon = Constants.CHSIcons.SystemParameters,
                IconColor = Constants.CHSIconColors.SystemParameters,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.SystemParametersPage)
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Title = "Transactions",
                Icon = Constants.CHSIcons.Transactions,
                IconColor = Constants.CHSIconColors.Transactions,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.TransactionsPage)
            });

            #region Employees management group

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Title = "Employee Managment",
                Icon = Constants.CHSIcons.EmployeeManagment,
                IconColor = Constants.CHSIconColors.EmployeeManagment,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.EmployeeManagmentPage)
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Level = 2,
                Title = "Users",
                Icon = Constants.CHSIcons.Users,
                IconColor = Constants.CHSIconColors.EmployeeManagment,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.UsersPage)
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Level = 2,
                Title = "SOP Users",
                Icon = Constants.CHSIcons.SOPUsers,
                IconColor = Constants.CHSIconColors.EmployeeManagment,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.SOPUsersPage)
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Level = 2,
                Title = "Employees",
                Icon = Constants.CHSIcons.Employees,
                IconColor = Constants.CHSIconColors.EmployeeManagment,
                MenuActionType = Models.MenuActionTypes.SetDetailPage,
                CommandParameter = typeof(Views.EmployeesPage)
            });

            #endregion

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                 Title = "Settings",
                 Icon = Constants.CHSIcons.Settings,
                 IconColor = Constants.CHSIconColors.Settings,
                 MenuActionType = Models.MenuActionTypes.SetDetailPage,
                 CommandParameter = typeof(Views.SettingsPage)
            });

            CheckAndAdd(ref menuItems, new Models.MenuItem
            {
                Title = "Logout",
                Icon = Constants.CHSIcons.Logout,
                IconColor = Constants.CHSIconColors.Logout,
                MenuActionType = Models.MenuActionTypes.SetMainPage,
                CommandParameter = typeof(Views.LoginPage),
            });

            MainMenuItems = new ObservableCollection<Models.MenuItem>(menuItems);

            #endregion

            #region Dashboards Page

            var dashBoardItems = new List<Models.MenuItem>();

            CheckAndAdd(ref dashBoardItems, new Models.MenuItem
            {
                Title = "Events",
                Icon = Constants.CHSIcons.Events,
                IconColor = Constants.CHSIconColors.DashBoardColor,
                MenuActionType = Models.MenuActionTypes.NavigateDetailPage,
                CommandParameter = typeof(Views.EventsPage),
            });

            CheckAndAdd(ref dashBoardItems, new Models.MenuItem
            {
                Title = "Door Open Events",
                Icon = Constants.CHSIcons.DoorOpenEvents,
                IconColor = Constants.CHSIconColors.DashBoardColor,
                MenuActionType = Models.MenuActionTypes.NavigateDetailPage,
                CommandParameter = typeof(Views.DoorOpenEventsPage)
            });

            CheckAndAdd(ref dashBoardItems, new Models.MenuItem
            {
                Title = "Cash On Hand",
                Icon = Constants.CHSIcons.CashOnHand,
                IconColor = Constants.CHSIconColors.DashBoardColor,
                MenuActionType = Models.MenuActionTypes.NavigateDetailPage,
                CommandParameter = typeof(Views.CashOnHandPage)
            });

            CheckAndAdd(ref dashBoardItems, new Models.MenuItem
            {
                Title = "Cash Utilization",
                Icon = Constants.CHSIcons.CashUtilization,
                IconColor = Constants.CHSIconColors.DashBoardColor,
                MenuActionType = Models.MenuActionTypes.NavigateDetailPage,
                CommandParameter = typeof(Views.CashUtilizationPage)
            });

            CheckAndAdd(ref dashBoardItems, new Models.MenuItem
            {
                Title = "All Transactions",
                Icon = Constants.CHSIcons.AllTransactions,
                IconColor = Constants.CHSIconColors.DashBoardColor,
                MenuActionType = Models.MenuActionTypes.NavigateDetailPage,
                CommandParameter = typeof(Views.AllTransactionsPage)
            });

            CheckAndAdd(ref dashBoardItems, new Models.MenuItem
            {
                Title = "Transaction by Type",
                Icon = Constants.CHSIcons.TransactionByType,
                IconColor = Constants.CHSIconColors.DashBoardColor,
                MenuActionType = Models.MenuActionTypes.NavigateDetailPage,
                CommandParameter = typeof(Views.TransactionByTypePage)
            });

            CheckAndAdd(ref dashBoardItems, new Models.MenuItem
            {
                Title = "Machine Availability",
                Icon = Constants.CHSIcons.MachineAvalabitity,
                IconColor = Constants.CHSIconColors.DashBoardColor,
                MenuActionType = Models.MenuActionTypes.NavigateDetailPage,
                CommandParameter = typeof(Views.MachineAvallabilityPage)
            });

            CheckAndAdd(ref dashBoardItems, new Models.MenuItem
            {
                Title = "Machine Status",
                Icon = Constants.CHSIcons.MachineStatus,
                IconColor = Constants.CHSIconColors.DashBoardColor,
                MenuActionType = Models.MenuActionTypes.NavigateDetailPage,
                CommandParameter = typeof(Views.MachineStatusSkiaPage)
            });

            DashBoardItems = new ObservableCollection<Models.MenuItem>(dashBoardItems);

            #endregion

            #region Employees management page

            var employeesItems = new List<Models.MenuItem>();

            CheckAndAdd(ref employeesItems, new Models.MenuItem
            {
                Title = "Users",
                Icon = Constants.CHSIcons.Users,
                IconColor = Constants.CHSIconColors.EmployeeManagment,
                MenuActionType = Models.MenuActionTypes.NavigateDetailPage,
                CommandParameter = typeof(Views.UsersPage)
            });

            CheckAndAdd(ref employeesItems, new Models.MenuItem
            {
                Title = "SOP Users",
                Icon = Constants.CHSIcons.SOPUsers,
                IconColor = Constants.CHSIconColors.EmployeeManagment,
                MenuActionType = Models.MenuActionTypes.NavigateDetailPage,
                CommandParameter = typeof(Views.SOPUsersPage)
            });

            CheckAndAdd(ref employeesItems, new Models.MenuItem
            {
                Title = "Employees",
                Icon = Constants.CHSIcons.Employees,
                IconColor = Constants.CHSIconColors.EmployeeManagment,
                MenuActionType = Models.MenuActionTypes.NavigateDetailPage,
                CommandParameter = typeof(Views.EmployeesPage)
            });

            EmployeeManagementItems = new ObservableCollection<Models.MenuItem>(employeesItems);

            #endregion
        }

        private void CheckAndAdd(ref List<Models.MenuItem> list, Models.MenuItem value)
        {
            if (PagesPermissions.CheckRights(value.CommandParameter as Type))
                list.Add(value);
        }

        #endregion
    }
}
