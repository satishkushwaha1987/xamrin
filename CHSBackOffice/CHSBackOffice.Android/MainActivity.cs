using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using CHSBackOffice.Database;
using CHSBackOffice.Droid.InterfacesImplementation;
using CHSBackOffice.Droid.InterfacesImplementation.Keyboard;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Device;
using CHSBackOffice.Support.Interfaces;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;
using System;

namespace CHSBackOffice.Droid
{
    [Activity(Label = "CHS BackOffice", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static AssetManager AssetManager;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            AssetManager = this.Assets;
            global::Xamarin.Forms.Forms.SetFlags(new[] { "FastRenderers_Experimental", "CollectionView_Experimental", "Shell_Experimental" });
            CrossCurrentActivity.Current.Init(this, bundle);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            UserDialogs.Init(this);
            //CarouselViewRenderer.Init();
            DevExpress.Mobile.Forms.Init();
            

            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ExceptionProcessor.ProcessException(e.ExceptionObject as Exception);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            //containerRegistry.RegisterForNavigation<TransactionDetailsPage, TransactionDetailsPageViewModel>();
            //containerRegistry.RegisterForNavigation<TransactionDetailsPageIos, TransactionDetailsPageViewModel>();
            containerRegistry.RegisterInstance<ISqLite>(new SqLiteImp());
            containerRegistry.RegisterInstance<ISetToolbarColor>(new SetToolbarColorImpl());
            containerRegistry.RegisterInstance<ICloseBehavior>(new AndroidCloseBehavior());
            containerRegistry.RegisterInstance<ISoftwareKeyboardService>(new SoftwareKeyboardService());
            containerRegistry.RegisterInstance<ILocationFetcher>(new LocationFetcher());
            containerRegistry.RegisterInstance<IOrientationHandler>(new OrientationHandlerImpl());
            containerRegistry.RegisterInstance<IFontManager>(new FontManager());
        }
    }
}

