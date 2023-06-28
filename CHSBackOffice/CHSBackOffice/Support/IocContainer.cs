using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Support.Device;
using CHSBackOffice.Support.Interfaces;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;

namespace CHSBackOffice.Support
{
    public class IocContainer
    {
        public static IEventAggregator EventAggregator => Container.Resolve<IEventAggregator>();
        public static INavigationService NavigationService => Container.Resolve<INavigationService>();
        public static Database.ISqLite SqLite => Container.Resolve<Database.ISqLite>();
        public static ICHSServiceAgent CHSServiceAgent => Container.Resolve<ICHSServiceAgent>();
        public static ICloseBehavior CloseBehavior => Container.Resolve<ICloseBehavior>();
        public static ILocationFetcher LocationFetcher => Container.Resolve<ILocationFetcher>();
        public static Interfaces.ISetToolbarColor SetToolbarColor => Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android ? Container.Resolve<Interfaces.ISetToolbarColor>() : null;

        public static Interfaces.ISoftwareKeyboardService KeyboardService => Container.Resolve<Interfaces.ISoftwareKeyboardService>();

        public static Interfaces.IOrientationHandler OrientationHandler => Container.Resolve<Interfaces.IOrientationHandler>();

        public static Interfaces.IFontManager FontManager => Container.Resolve<Interfaces.IFontManager>();
        #region "PRIVATE METHODS"

        private static IContainerProvider Container
        {
            get => ((Prism.DryIoc.PrismApplication)App.Current).Container;
        }

        #endregion
    }
}
