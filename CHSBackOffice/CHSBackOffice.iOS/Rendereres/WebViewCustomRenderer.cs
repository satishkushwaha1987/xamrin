using CHSBackOffice.iOS.Rendereres;
using Foundation;
using System.Linq;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebView), typeof(WebViewCustomRenderer))]

namespace CHSBackOffice.iOS.Rendereres
{
    [Preserve(AllMembers = true)]
    class WebViewCustomRenderer : WkWebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            
            if (!(this is WKWebView browser) || browser.ScrollView == null)
                return;

            var recogrizers = browser
                .ScrollView
                .GestureRecognizers
                .Where(_ => _.GetType() != typeof(UIPanGestureRecognizer)).ToArray(); // fix issue with frozen swipe in some IOs devices.
            browser.ScrollView.GestureRecognizers = recogrizers;
        }
    }
}