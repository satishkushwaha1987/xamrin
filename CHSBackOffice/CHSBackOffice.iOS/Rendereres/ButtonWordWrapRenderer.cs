using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CHSBackOffice.iOS.Rendereres;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Button), typeof(ButtonWordWrapRenderer))]
namespace CHSBackOffice.iOS.Rendereres
{
    [Preserve(AllMembers = true)]
    public class ButtonWordWrapRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
                Control.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
        }
    }
}