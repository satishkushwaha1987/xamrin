using System.ComponentModel;
using Android.Graphics;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Droid.Rendereres;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace CHSBackOffice.Droid.Rendereres
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        BorderlessEntry element;
        public BorderlessEntryRenderer(Android.Content.Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (BorderlessEntry)this.Element;

            SetBorder(element);
            if (Control != null)
            {
                Control.ImeOptions = (Android.Views.InputMethods.ImeAction)Android.Views.InputMethods.ImeFlags.NoExtractUi;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            element = (BorderlessEntry)this.Element;

            if (e.PropertyName == BorderlessEntry.BorderlessProperty.PropertyName)
            {
                SetBorder(element);
            }
        }

        private void SetBorder(BorderlessEntry entry)
        {
            if (entry == null)
            {
                return;
            }

            if (entry.RoundedStyle == RoundedEntryStyle.White)
            {
                Control.Background = Android.App.Application.Context.GetDrawable(Resource.Drawable.RoundCornerbutton);
                Control.Gravity = Android.Views.GravityFlags.CenterVertical;
                Control.SetPadding(10, 0, 0, 0);
            }
            if (entry.RoundedStyle == RoundedEntryStyle.Oppacity)
            {
                Control.Background = Android.App.Application.Context.GetDrawable(Resource.Drawable.RoundedCornerbuttonOpacity);
                Control.Gravity = Android.Views.GravityFlags.CenterVertical;
                Control.SetPadding(10, 0, 0, 0);
            }

            if (entry.RoundedStyle == RoundedEntryStyle.None)
            {
                Control.Background = null;
                Control.Gravity = Android.Views.GravityFlags.CenterVertical;
                Control.SetPadding(10, 0, 0, 0);
            }

            

            if (!entry.Borderless)
            {
                Control.Background.SetColorFilter(element.LineColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
        }
    }
}