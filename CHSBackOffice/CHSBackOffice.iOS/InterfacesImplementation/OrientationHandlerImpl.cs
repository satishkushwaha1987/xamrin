
using CHSBackOffice.Support.Interfaces;
using Foundation;
using UIKit;

namespace CHSBackOffice.iOS.InterfacesImplementation
{
    public class OrientationHandlerImpl : IOrientationHandler
    {
        public void ForceLandscape()
        {
            UIDevice.CurrentDevice.SetValueForKeyPath(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft),
                new NSString("orientation"));
        }

        public void ForcePortrait()
        {
            UIDevice.CurrentDevice.SetValueForKeyPath(new NSNumber((int)UIInterfaceOrientation.Portrait),
                new NSString("orientation"));
        }

        public void ForceUnspecified()
        {
            UIDevice.CurrentDevice.SetValueForKeyPath(new NSNumber((int)UIInterfaceOrientation.Unknown),
                new NSString("orientation"));
        }
    }
}