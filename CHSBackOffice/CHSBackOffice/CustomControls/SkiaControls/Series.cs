using CHSBackOffice.Models.Chart;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls.SkiaControls
{
    public class Series
    {
        #region Properties

        #region MarginRightInner
        private float _marginRightInner;
        public float MarginRightInner
        {
            set { _marginRightInner = value; }
            get { return _marginRightInner * (float)KScale; }
        }
        #endregion

        #region MarginLeftInner
        private float _marginLeftRightInner;
        public float MarginLeftInner
        {
            set { _marginLeftRightInner = value; }
            get { return _marginLeftRightInner * (float)KScale; }
        }
        #endregion

        #region MarginTopInner
        private float _marginTopInner;
        public float MarginTopInner
        {
            set { _marginTopInner = value; }
            get { return _marginTopInner * (float)KScale; }
        }
        #endregion

        #region MarginBottomInner
        private float _marginBottomInner;
        public float MarginBottomInner
        {
            set { _marginBottomInner = value; }
            get { return _marginBottomInner * (float)KScale; }
        }
        #endregion

        #region MarginFooter
        private float _marginFooter;
        public float MarginFooter
        {
            set { _marginFooter = value; }
            get { return _marginFooter * (float)KScale; }
        }
        #endregion

        #region LabelTextSize
        private float _labelTextSize;
        public float LabelTextSize
        {
            set { _labelTextSize = value; }
            get { return _labelTextSize * (float)KScale; }
        }
        #endregion

        #region WidthItem
        private float _widthItem;
        public float WidthItem
        {
            set { _widthItem = value; }
            get { return _widthItem * (float)KScale; }
        }
        #endregion

        #region MinValue
        private float? _minValue;
        public float MinValue
        {
            get
            {
                if (!this.DataEntries.Any())
                {
                    return 0;
                }

                if (this._minValue == null)
                {
                    return Math.Min(0, this.DataEntries.Min(x => x.Capacity));
                }

                return Math.Min(this._minValue.Value, this.DataEntries.Min(x => x.Capacity));
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
                if (!this.DataEntries.Any())
                {
                    return 0;
                }

                if (this._maxValue == null)
                {
                    return Math.Max(0, this.DataEntries.Max(x => x.Capacity));
                }

                return Math.Max(this._maxValue.Value, this.DataEntries.Max(x => x.Capacity));
            }

            set => this._maxValue = value;
        }
        #endregion
        private float ValueRange => this.MaxValue - this.MinValue;
        public string FooterLabel { get; set; }
        public SKColor CriticalColor { get; set; } = StaticResourceManager.GetColor("ChartCritical").ToSKColor();
        public SKColor ChartAreaColor { get; set; } = StaticResourceManager.GetColor("ChartCapacity").ToSKColor();
        public SKColor TextColor { get; set; } = SKColors.Black;
        public DataEntryCollection DataEntries { get; set; }
        public double KScale { get; set; } = 1;

        #endregion

        #region .CTOR
        public Series()
        {
        }

        public Series(DataEntryCollection dataEntries, string label)
        {
            DataEntries = dataEntries;
            FooterLabel = label;
        }

        #endregion

        #region Methods

        public void Draw(SKCanvas canvas, int width, int height, double kScale, float xOffset = 0, float yOffset = 0)
        {
            KScale = kScale;
            this.DrawLayout(canvas, width, height, xOffset, yOffset);
        }

        private void DrawLayout(SKCanvas canvas, int width, int height, float xOffset, float yOffset)
        {

            var headerHeight = CalculateHeaderHeight();
            var footerHeight = CalculateFooterHeight();
            var itemSize = CalculateItemSize(width, height, footerHeight, headerHeight);
            var origin = CalculateYOrigin(itemSize.Height, headerHeight);
            var points = this.CalculatePoints(itemSize, headerHeight);

            this.DrawGridAreas(canvas, points, itemSize, headerHeight, xOffset, yOffset);
            this.DrawGrids(canvas, points, itemSize, origin, headerHeight, xOffset, yOffset);
            this.DrawFooter(canvas, height, xOffset, yOffset);
        }

        private float CalculateHeaderHeight()
        {
            var result = this.MarginTopInner;
            return result;
        }

        private float CalculateFooterHeight()
        {
            var result = this.MarginBottomInner;
            if (!string.IsNullOrEmpty(FooterLabel))
            {
                result += this.LabelTextSize + this.MarginFooter;
            }

            return result;
        }

        private SKSize CalculateItemSize(int width, int height, float footerHeight, float headerHeight)
        {
            var w = WidthItem; 
            var h = height - this.MarginTopInner - footerHeight - headerHeight;
            return new SKSize(w, h);
        }

        private float CalculateYOrigin(float itemHeight, float headerHeight)
        {
            return headerHeight + ((this.MaxValue / this.ValueRange) * itemHeight);
        }

        private SKPoint[] CalculatePoints(SKSize itemSize, float headerHeight)
        {
            var result = new List<SKPoint>();
            try 
            {
                for (int i = 0; i < this.DataEntries.Count(); i++)
                {
                    var entry = this.DataEntries.ElementAt(i);

                    var x = this.MarginLeftInner + (itemSize.Width / 2) + (i * (itemSize.Width + this.MarginRightInner));
                    var value = entry.Value;
                    var capacity = entry.Capacity;
                    if (value > capacity)
                    {
                        value = capacity;
                    }
                    var percent = ((this.MaxValue - value) / this.ValueRange);

                    ///Because if count == 0  we can not display setted color indicator
                    if (value == 0)
                        percent = 1;
                    var y = headerHeight + (percent * itemSize.Height);
                    var point = new SKPoint(x, y);
                    result.Add(point);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            return result.ToArray();
        }

        private void DrawGridAreas(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float headerHeight, float xOffset, float yOffset)
        {
            try 
            {
                if (points.Length > 0)
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        var entry = this.DataEntries.ElementAt(i);
                        var point = points[i];

                        using (var paint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = ChartAreaColor,
                        })
                        {
                            var max = headerHeight;
                            var height = Math.Abs(max - point.Y);
                            var y = Math.Min(max, point.Y);
                            var rect = SKRect.Create(point.X - (itemSize.Width / 2) + xOffset, y + yOffset, itemSize.Width, height);
                            canvas.DrawRect(rect, paint);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void DrawGrids(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float origin, float headerHeight, float xOffset, float yOffset)
        {
            try 
            {
                const float MinBarHeight = 1;
                if (points.Length > 0)
                {
                    for (int i = 0; i < this.DataEntries.Count(); i++)
                    {
                        var entry = this.DataEntries.ElementAt(i);
                        var point = points[i];

                        using (var paint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = entry.Color,
                        })
                        {
                            var x = point.X - (itemSize.Width / 2);
                            var y = Math.Min(origin, point.Y);
                            float height = 0;
                            if (DataEntries[i].Value > 0)
                            {
                                height = Math.Max(MinBarHeight, Math.Abs(origin - point.Y));
                            }
                            else
                            {
                                if (DataEntries[i].Color == CriticalColor)
                                {
                                    height = Math.Max(origin, point.Y) - headerHeight;
                                    if (y + height > this.MarginTopInner + itemSize.Height)
                                    {
                                        y = headerHeight + itemSize.Height - height;
                                    }
                                }
                                else
                                {
                                    height = Math.Abs(origin - point.Y);
                                }

                            }

                            var rect = SKRect.Create(x + xOffset, y + yOffset, itemSize.Width, height);
                            canvas.DrawRect(rect, paint);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void DrawFooter(SKCanvas canvas, int height, float xOffset, float yOffset)
        {
            this.DrawLabel(canvas, height, xOffset, yOffset);
        }

        private void DrawLabel(SKCanvas canvas, int height, float xOffset, float yOffset)
        {
            try 
            {
                if (!string.IsNullOrEmpty(FooterLabel))
                {
                    using (var paint = new SKPaint())
                    {
                        paint.TextSize = this.LabelTextSize;
                        paint.IsAntialias = true;
                        paint.Color = TextColor;
                        paint.IsStroke = false;
                        paint.Typeface = Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android ?
                            SKTypeface.FromFile(IocContainer.FontManager.GetFontFilePath("KlavikaCHRegular.otf"))
                            : SKTypeface.FromFile("KlavikaCHRegular.otf");
                        var bounds = new SKRect();
                        var text = FooterLabel;
                        paint.MeasureText(text, ref bounds);


                        canvas.DrawText(text, this.MarginLeftInner + xOffset, height - (this.MarginBottomInner + (this.LabelTextSize / 2)) + this.MarginFooter + yOffset, paint);
                    }
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
