using CHBackOffice.ApiServices.Interfaces;
using CHBackOffice.Service;
using CHSBackOffice.Database;
using CHSBackOffice.Events;
using CHSBackOffice.Support;
using CHSBackOffice.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CHSBackOffice.Support.Settings;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CHSBackOffice
{
    public partial class App
    {
        internal static INavigationService NaviService;
        internal static IEventAggregator SharedEventAggregator;
        public static SettingInfo<bool> RememberMeSetting;

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            try
            {
                InitializeComponent();
                //for loading the Deexpress library
                var g = DevExpress.Mobile.Core.GlobalServices.Instance;

                NaviService = NavigationService;
                SharedEventAggregator = IocContainer.EventAggregator;
                Repository.DatabasePath = IocContainer.SqLite.GetPathToDatabase("bo.db");
                Repository.Init();
                Settings.InitSettings();

                Settings.IgnoreCertificate.ValueChangedSyncAction?.Invoke(Settings.IgnoreCertificate.BoolValue);

                CHSServiceAgent.Init();
                StartPluginConnectivity();

                // MainPage = new NavigationPage(new TransactionsCarouselPage());

                if (StateInfoService.HasBackOfficeHostAddress)
                    MainPage = new NavigationPage(new LoginPage());
                else
                    MainPage = new NavigationPage(new BackOfficeHostAddressPage());

                DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void StartPluginConnectivity()
        {
            Network.Current.ConnectivityTypeChanged += Current_ConnectivityTypeChanged;
            Network.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        private void Current_ConnectivityTypeChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityTypeChangedEventArgs e)
        {
            SharedEventAggregator.GetEvent<NetworkStateChanged>().Publish();
        }

        private void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            SharedEventAggregator.GetEvent<NetworkStateChanged>().Publish();
        }

        protected override void OnStart()
        {
            base.OnStart();
            AppCenter.Start("android=86edeab1-c508-4f49-a5c8-9e82f9796d63;" +
                  "ios=ec6260a9-425e-4687-b261-18700a3cc451;",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            try
            {
                containerRegistry.RegisterInstance<ICHSServiceAgent>(new CHSServiceAgent());

                containerRegistry.RegisterForNavigation<NavigationPage>();
                containerRegistry.RegisterForNavigation<DashboardPage>();
                containerRegistry.RegisterForNavigation<DoorOpenEventsPage>();
                containerRegistry.RegisterForNavigation<TransactionsPage>();
                containerRegistry.RegisterForNavigation<MachineStatusPage>();
                containerRegistry.RegisterForNavigation<MachineStatusSkiaPage>();
                containerRegistry.RegisterForNavigation<CashOnHandPage>();
                containerRegistry.RegisterForNavigation<CashUtilizationPage>();
                containerRegistry.RegisterForNavigation<TransactionByTypePage>();
                containerRegistry.RegisterForNavigation<MachineAvallabilityPage>();
                containerRegistry.RegisterForNavigation<ActiveFloatsPage>();
                containerRegistry.RegisterForNavigation<RemoteControlPage>();
                containerRegistry.RegisterForNavigation<AllTransactionsPage>();
                containerRegistry.RegisterForNavigation<EmployeesPage>();
                containerRegistry.RegisterForNavigation<EventsPage>();
                containerRegistry.RegisterForNavigation<EmployeeManagmentPage>();
                containerRegistry.RegisterForNavigation<UsersPage>();
                containerRegistry.RegisterForNavigation<SOPUsersPage>();
                containerRegistry.RegisterForNavigation<SystemParametersPage>();
                containerRegistry.RegisterForNavigation<SettingsPage>();
                containerRegistry.RegisterForNavigation<EventsFilterPage>();
                containerRegistry.RegisterForNavigation<TransactionFilterPage>();
                containerRegistry.RegisterForNavigation<BackOfficeHostAddressPage>();
                containerRegistry.RegisterForNavigation<LoginPage>();
                containerRegistry.RegisterForNavigation<MainMenuPage>();
                containerRegistry.RegisterForNavigation<ATMInfoPage>();
                containerRegistry.RegisterForNavigation<SystemParameterPage>();
                containerRegistry.RegisterForNavigation<TransactionsCarouselPage>();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            App.SharedEventAggregator.GetEvent<MainDisplayInfoChanged<DisplayInfo>>().Publish(e.DisplayInfo);
        }
    }
}
