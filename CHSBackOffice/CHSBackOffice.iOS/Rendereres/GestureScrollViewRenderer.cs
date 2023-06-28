
using CHSBackOffice.CustomControls;
using CHSBackOffice.iOS.Rendereres;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GestureScrollView), typeof(GestureScrollViewRenderer))]
namespace CHSBackOffice.iOS.Rendereres
{
    public class GestureScrollViewRenderer: ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            this.ScrollEnabled = false;
            
        }
    }
}