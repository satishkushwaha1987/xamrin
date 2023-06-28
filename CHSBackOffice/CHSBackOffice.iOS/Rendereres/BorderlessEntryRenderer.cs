using CHSBackOffice.CustomControls;
using CHSBackOffice.iOS.Rendereres;
using CoreAnimation;
using CoreGraphics;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace CHSBackOffice.iOS.Rendereres
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        BorderlessEntry element;
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            element = (BorderlessEntry)this.Element;
            var textField = this.Control;

            SetBorder(element);
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

            var textField = this.Control;
            if (entry.RoundedStyle == RoundedEntryStyle.None)
            {
                
                textField.BorderStyle = UITextBorderStyle.None;
                textField.Layer.BorderWidth = 0;
            }
            if (entry.RoundedStyle == RoundedEntryStyle.White)
            {
                textField.BorderStyle = UITextBorderStyle.RoundedRect;
                textField.Layer.BorderColor = UIColor.White.CGColor;
                textField.BackgroundColor = UIColor.White;
            }
            if (entry.RoundedStyle == RoundedEntryStyle.Oppacity)
            {
                textField.BorderStyle = UITextBorderStyle.RoundedRect;
                textField.Layer.BorderColor = Color.FromHex("#557768").ToCGColor();
                textField.BackgroundColor = Color.FromHex("#557768").ToUIColor();
            }
            
            if (!element.Borderless)
            {
                CALayer bottomBorder = new CALayer
                {
                    Frame = new CGRect(0.0f, element.HeightRequest - 1, this.Frame.Width * 4, 1.0f),
                    BorderWidth = 2.0f,
                    BorderColor = element.LineColor.ToCGColor()
                };

                textField.Layer.AddSublayer(bottomBorder);
                textField.Layer.MasksToBounds = true;
            }
        }
    }
}