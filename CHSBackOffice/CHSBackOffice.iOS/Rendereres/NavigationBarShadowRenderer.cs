using CHSBackOffice.iOS.Rendereres;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationBarShadowRenderer))]
namespace CHSBackOffice.iOS.Rendereres
{
    public class NavigationBarShadowRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (this.Element == null) return;

            NavigationBar.Layer.MasksToBounds = false;
            NavigationBar.Layer.ShadowColor = UIColor.Black.CGColor;
            NavigationBar.Layer.ShadowOffset = new CGSize(0, Device.Idiom == TargetIdiom.Phone ? 2 : 3);
            NavigationBar.Layer.ShadowRadius = Device.Idiom == TargetIdiom.Phone ? 2 : 3;
            NavigationBar.Layer.ShadowOpacity = 0.4F;
        }
    }
}