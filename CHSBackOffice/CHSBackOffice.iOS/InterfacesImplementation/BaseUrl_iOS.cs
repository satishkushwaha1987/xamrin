using CHSBackOffice.iOS.InterfacesImplementation;
using CHSBackOffice.Support.Device;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_iOS))]
namespace CHSBackOffice.iOS.InterfacesImplementation
{
    public class BaseUrl_iOS : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath; // to load resources in the iOS app itself
        }
    }
}