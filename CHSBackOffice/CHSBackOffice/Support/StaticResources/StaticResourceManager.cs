using System.Linq;
using Xamarin.Forms;

namespace CHSBackOffice.Support.StaticResources
{
    public sealed class StaticResourceManager
    {
        /// <summary>
        /// Get Font Name from App.xaml resources
        /// </summary>
        public static string GetFontName(AppFont fontKey)
        {
            if (!(Application.Current.Resources[fontKey.ToString()] is OnPlatform<string> resource))
                return null;

            return resource.Platforms.FirstOrDefault(p => p.Platform[0] == Xamarin.Forms.Device.RuntimePlatform)?.Value as string;
        }

        /// <summary>
        /// Get Font Size from App.xaml resources
        /// </summary>
        public static double GetFontSize(AppFontSize fontSize)
        {
            if (!(Application.Current.Resources[fontSize.ToString()] is OnIdiom<double> resource))
                return 0;

            switch (Xamarin.Forms.Device.Idiom)
            {
                case TargetIdiom.Desktop:
                    return resource.Desktop;
                case TargetIdiom.Phone:
                    return resource.Phone;
                case TargetIdiom.Tablet:
                    return resource.Tablet;
                case TargetIdiom.TV:
                    return resource.TV;
                case TargetIdiom.Watch:
                    return resource.Watch;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Get Color from App.xaml resources
        /// </summary>
        public static Color GetColor(AppColor color)
        {
            return GetColor(color.ToString());
        }

        /// <summary>
        /// Get Color from App.xaml resources
        /// </summary>
        public static Color GetColor(string resourceName)
        {
            if ((Application.Current.Resources[resourceName] is Color resource))
                return resource;

            return Color.Transparent;
        }
    }
}
