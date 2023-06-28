using System;
using Android.Content;
using Android.Graphics;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Droid.Rendereres;
using CHSBackOffice.Support;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(RoundedCornerView),typeof(RoundedCornerViewRenderer))]
namespace CHSBackOffice.Droid.Rendereres
{
    public class RoundedCornerViewRenderer : ViewRenderer
    {
        public RoundedCornerViewRenderer(Context context) : base(context)
        {
        }

        protected override bool DrawChild(Canvas canvas, Android.Views.View child, long drawingTime)
        {
            if (Element == null)
                return false;
            RoundedCornerView rcv = Element as RoundedCornerView;
            if (rcv != null)
            {
                this.SetClipChildren(true);
                rcv.Padding = new Thickness(0);
                int radius = (int)rcv.RoundedCornerRadius;
                if (rcv.MakeCircle)
                {
                    radius = Math.Min(Width, Height) / 2;
                }
                radius *= 2;
                try
                {
                    var path = new Path();
                    path.AddRoundRect(new RectF(0, 0, Width, Height),
                        new float[] { radius, radius, radius, radius, radius, radius, radius, radius },
                        Path.Direction.Ccw);
                    canvas.Save();
                    canvas.ClipPath(path);

                    var result = base.DrawChild(canvas, child, drawingTime);
                    canvas.Restore();

                    if (rcv.BorderWidth > 0)
                    {
                        var paint = new Paint();
                        paint.AntiAlias = true;
                        paint.StrokeWidth = rcv.BorderWidth * 2;
                        paint.SetStyle(Paint.Style.Stroke);
                        paint.Color = rcv.BorderColor.ToAndroid();

                        canvas.DrawPath(path, paint);

                        paint.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
            return base.DrawChild(canvas, child, drawingTime);
        }
    }
}