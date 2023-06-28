using Android.Content;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Droid.Rendereres;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(ExtendedEditor),typeof(ExtendedEditorRenderer))]
namespace CHSBackOffice.Droid.Rendereres
{
    public class ExtendedEditorRenderer : EditorRenderer
    {
        public ExtendedEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            Control.Background = Android.App.Application.Context.GetDrawable(Resource.Drawable.RoundCornerbutton);
            Control.Gravity = Android.Views.GravityFlags.Start;
            Control.SetPadding(10, 0, 0, 0);
        }
    }
}