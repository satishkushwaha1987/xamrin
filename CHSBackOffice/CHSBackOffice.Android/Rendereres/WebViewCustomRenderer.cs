using Android.Content;
using CHSBackOffice.Droid.Rendereres;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WebView), typeof(WebViewCustomRenderer))]

namespace CHSBackOffice.Droid.Rendereres
{
    class WebViewCustomRenderer : WebViewRenderer
    {
        public WebViewCustomRenderer(Context context) : base(context)
        {
        }
    }
}