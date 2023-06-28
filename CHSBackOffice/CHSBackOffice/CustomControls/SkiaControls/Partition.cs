using CHSBackOffice.Models.Chart;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHSBackOffice.CustomControls.SkiaControls
{
    public class Partition
    {
        #region Properties

        #region MinValue
        private float? _minValue;
        public float MinValue
        {
            get 
            {
                if (this._minValue == null)
                {
                    return Math.Min(0, this.DataEntry.Capacity);
                }
                return Math.Min(this._minValue.Value, this.DataEntry.Capacity);
            }

            set => this._minValue = value;
        }

        #endregion

        #region MaxValue
        private float? _maxValue;
        public float MaxValue
        {
            get 
            {
                if (this._maxValue == null)
                {
                    return Math.Max(0, this.DataEntry.Capacity);
                }
                return Math.Max(this._maxValue.Value, this.DataEntry.Capacity);
            }

            set => this._maxValue = value;
        }
        #endregion
        private float ValueRange => this.MaxValue - this.MinValue;
        public SKColor ChartAreaColor { get; set; } = Xamarin.Forms.Color.FromHex("#EBF3F9").ToSKColor();
        public SKColor ChartAreaBorderColor { get; set; } = Xamarin.Forms.Color.FromHex("#5C9ECD").ToSKColor();
        public float BorderWidth { get; set; } = 2.0f;
        public DataEntry DataEntry { get; set; }
        public double KScale { get; set; }
        #endregion

        #region .CTOR
        public Partition()
        { }

        public Partition(DataEntry dataEntry)
        {
            DataEntry = dataEntry;
        }
        #endregion

        #region Methods
        public void Draw(SKCanvas canvas, int width, int height, double kScale)
        {
            KScale = kScale;
            this.DrawLayout(canvas, width, height);
        }

        private void DrawLayout(SKCanvas canvas, int width, int height)
        {
            var itemSize = CalculateItemSize(width, height);
            var origin = CalculateXOrigin(itemSize.Width);
            var point = CalculatePoint(itemSize);

            this.DrawGridAreas(canvas, point, itemSize, origin);
            this.DrawGrid(canvas, point, itemSize, origin);
        }

        private SKSize CalculateItemSize(int width, int height)
        {
            return new SKSize(width, height);
        }

        private float CalculateXOrigin(float itemWidth)
        {
            return (this.MaxValue / this.ValueRange) * itemWidth;
        }

        private SKPoint CalculatePoint(SKSize itemSize)
        {
            var percent = (this.MaxValue - this.DataEntry.Value) / this.ValueRange;
            if (this.DataEntry.Value == 0)
                percent = 1;
            var x = percent * itemSize.Width;
            var y = 0;
            return new SKPoint(x, y);
        }

        private void DrawGridAreas(SKCanvas canvas, SKPoint point, SKSize itemSize, float origin)
        {
            try 
            {
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = ChartAreaBorderColor
                })
                {
                    var max = 0;
                    var x = Math.Abs(origin - point.X);
                    var width = Math.Abs(max - point.X);

                    var rect = SKRect.Create(x, point.Y, width, itemSize.Height);
                    canvas.DrawRect(rect, paint);
                }

                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = ChartAreaColor
                })
                {
                    var max = 0;
                    var x = Math.Abs(origin - point.X);
                    var width = Math.Abs(max - point.X);

                    var rect = SKRect.Create(x + BorderWidth, point.Y + BorderWidth, width - BorderWidth * 2, itemSize.Height - BorderWidth * 2);
                    canvas.DrawRect(rect, paint);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            
        }

        private void DrawGrid(SKCanvas canvas, SKPoint point, SKSize itemSize, float origin)
        {
            try 
            {
                const float MinBarWidth = 1;
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = this.DataEntry.Color
                })
                {
                    var max = 0;
                    var x = Math.Min(max, point.X);
                    var y = point.Y;
                    float width = 0;
                    if (DataEntry.Value > 0)
                    {
                        width = Math.Max(MinBarWidth, Math.Abs(origin - point.X));
                    }
                    else
                    {
                        width = Math.Abs(origin - point.X);
                    }

                    var rect = SKRect.Create(x, point.Y, width, itemSize.Height);
                    canvas.DrawRect(rect, paint);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion
    }
}
