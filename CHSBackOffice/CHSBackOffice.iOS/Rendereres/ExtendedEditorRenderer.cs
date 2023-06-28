using CHSBackOffice.CustomControls;
using CHSBackOffice.iOS.Rendereres;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]
namespace CHSBackOffice.iOS.Rendereres
{
    public class ExtendedEditorRenderer: EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            Control.Layer.CornerRadius = 8;
        }
    }
}