
using Android.App;
using CHSBackOffice.Droid.InterfacesImplementation;
using CHSBackOffice.Support.Interfaces;
using Xamarin.Forms;

[assembly:Xamarin.Forms.Dependency(typeof(OrientationHandlerImpl))]
namespace CHSBackOffice.Droid.InterfacesImplementation
{
    public class OrientationHandlerImpl : IOrientationHandler
    {
        public void ForceLandscape()
        {
            GetActivity().RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
        }

        public void ForcePortrait()
        {
            GetActivity().RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
        }

        public void ForceUnspecified()
        {
            GetActivity().RequestedOrientation = Android.Content.PM.ScreenOrientation.Unspecified;
        }

        private Activity GetActivity()
        {
            return (Activity)Forms.Context;
        }
    }
}