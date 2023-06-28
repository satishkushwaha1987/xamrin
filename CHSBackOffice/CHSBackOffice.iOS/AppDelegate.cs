using CHSBackOffice.Database;
using CHSBackOffice.iOS.InterfacesImplementation;
using CHSBackOffice.iOS.Rendereres;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Device;
using CHSBackOffice.Support.Interfaces;
using CHSBackOffice.ViewModels;
using CHSBackOffice.Views;
using Foundation;
using Prism;
using Prism.Ioc;
using System;
using UIKit;


namespace CHSBackOffice.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() { TextColor = UIColor.White });
            UIBarButtonItem.Appearance.TintColor = UIColor.White;
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Forms.Forms.Init();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DevExpress.Mobile.Forms.Init();
            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ExceptionProcessor.ProcessException(e.ExceptionObject as Exception);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            //containerRegistry.RegisterForNavigation<TransactionDetailsPageIos, TransactionDetailsPageViewModel>();
            containerRegistry.RegisterInstance<ISqLite>(new SqLiteImp());
            containerRegistry.RegisterInstance<ICloseBehavior>(new IosCloseBehavior());
            containerRegistry.RegisterInstance<ISoftwareKeyboardService>(new SoftwareKeyboardService());
            containerRegistry.RegisterInstance<IOrientationHandler>(new OrientationHandlerImpl());
        }
    }
}
