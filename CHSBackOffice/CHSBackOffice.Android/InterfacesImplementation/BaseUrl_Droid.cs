using CHSBackOffice.Droid.InterfacesImplementation;
using CHSBackOffice.Support.Device;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_Droid))]
namespace CHSBackOffice.Droid.InterfacesImplementation
{
    public class BaseUrl_Droid : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
    }
}