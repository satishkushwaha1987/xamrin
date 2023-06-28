using CHSBackOffice.CustomControls;
using CHSBackOffice.iOS.Rendereres;
using CoreGraphics;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(RoundedCornerView),typeof(RoundedCornerViewRenderer))]
namespace CHSBackOffice.iOS.Rendereres
{
    public class RoundedCornerViewRenderer : ViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (this.Element == null)
                return;

            this.Element.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (NativeView != null)
                {
                    NativeView.SetNeedsDisplay();
                    NativeView.SetNeedsLayout();
                }
            }
            catch (ObjectDisposedException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            this.LayoutIfNeeded();
            RoundedCornerView rcv = Element as RoundedCornerView;
            rcv.Padding = new Thickness(0);
            this.ClipsToBounds = true;
            this.Layer.BackgroundColor = rcv.FillColor.ToCGColor();
            this.Layer.MasksToBounds = true;
            this.Layer.CornerRadius = (nfloat)rcv.RoundedCornerRadius;
            if (rcv.MakeCircle)
            {
                this.Layer.CornerRadius = (int)(Math.Min(Element.Width, Element.Height) / 2);
            }
            this.Layer.BorderWidth = 0;
            if (rcv.BorderWidth > 0 && rcv.BorderColor.A > 0.0)
            {
                this.Layer.BorderWidth = rcv.BorderWidth;
                this.Layer.BorderColor = new UIKit.UIColor((nfloat)rcv.BorderColor.R,
                                                           (nfloat)rcv.BorderColor.G,
                                                           (nfloat)rcv.BorderColor.B,
                                                           (nfloat)rcv.BorderColor.A).CGColor;
            }
        }
    }
}