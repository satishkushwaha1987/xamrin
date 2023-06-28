using CHBackOffice.ApiServices.ChsProxy;
using CHSBackOffice.Models.Chart;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls.SkiaControls
{
    public class MachineSkiaView : SKCanvasView
    {
        #region Private Fields
        KioskState state = default(KioskState);
        KioskStatus status = default(KioskStatus);
        SKColor color = default(SKColor);
        string id;
        #endregion


        #region .CTOR
        public MachineSkiaView()
        {
            Container = new Container();
            this.PaintSurface += OnPaintCanvas;
        }
        #endregion

        #region Bindable Properties

        public new float Scale => Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet ? (float)2 : (float)3;

        public Container Container { get; set; }

        public Series Series { get; set; }

        #region DataSource
        public Tuple<List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>, KioskState, KioskStatus, string> DataSource
        {
            get { return (Tuple<List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>, KioskState, KioskStatus, string>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly BindableProperty DataSourceProperty = BindableProperty.Create(
            nameof(DataSource), typeof(Tuple<List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>, KioskState, KioskStatus, string>), typeof(MachineSkiaView), default(Tuple<List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>, KioskState, KioskStatus, string>), propertyChanged: OnDataSourceChanged);

        private static void OnDataSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((MachineSkiaView)bindable).InvalidateSurface();
        }

        #endregion

        #region Sections

        public List<Series> Sections
        {
            get { return (List<Series>)GetValue(SectionsProperty); }
            set { SetValue(SectionsProperty, value); }
        }

        public static readonly BindableProperty SectionsProperty = BindableProperty.Create(
            nameof(Sections), typeof(List<Series>), typeof(MachineSkiaView), null, propertyChanged: OnSeriesChanged);

        private static void OnSeriesChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }
        #endregion

        #region SeriesOrientation
        public SeriesOrientation SeriesOrientation
        {
            get { return (SeriesOrientation)GetValue(SeriesOrientationProperty); }
            set { SetValue(SeriesOrientationProperty, value); }
        }

        public static readonly BindableProperty SeriesOrientationProperty = BindableProperty.Create(
            nameof(SeriesOrientation), typeof(SeriesOrientation), typeof(MachineSkiaView), SeriesOrientation.Vertical);
        #endregion

        #endregion

        #region Event Handling
        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear();
            var originScale = CanvasSize.Width / Width;
            var kScale = originScale / Scale;
            Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height, kScale);

        }

        #endregion

        #region Methods
        private void Draw(SKCanvas sKCanvas, int width, int height, double kScale)
        {
            try 
            {
                if (DataSource != default(Tuple<List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>, KioskState, KioskStatus, string>))
                {
                    if (Sections == null)
                    {
                        Sections = new List<Series>();
                    }
                    if (Sections.Count > 0)
                    {
                        Sections.Clear();
                    }
                    InitSections();
                }
                if (Sections != null)
                {
                    float xOffset = 0;
                    float yOffset = 0;
                    if (Container != null)
                    {
                        SKColor color = MatchStateStatusToColor(state, status);
                        Container.Draw(sKCanvas, width, height, kScale, color, id);
                        xOffset = Container.MarginInner;
                        yOffset = Container.HeaderHeight;
                        DrawSections(sKCanvas, xOffset, yOffset, kScale);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void InitSections()
        {
            try 
            {
                Type dataSourceType = DataSource.GetType();
                IEnumerable<PropertyInfo> dataSourceProperties = dataSourceType.GetTypeInfo().DeclaredProperties;
                IEnumerable<object> collections = dataSourceProperties.ElementAt(0).GetValue(DataSource, null) as IEnumerable<object>;

                Enum.TryParse(dataSourceProperties.ElementAt(1).GetValue(DataSource, null).ToString(), out state);
                Enum.TryParse(dataSourceProperties.ElementAt(2).GetValue(DataSource, null).ToString(), out status);
                id = dataSourceProperties.ElementAt(3).GetValue(DataSource, null).ToString();

                Type collType = collections.ElementAt(0).GetType();
                IEnumerable<PropertyInfo> properties = collType.GetTypeInfo().DeclaredProperties;
                foreach (var val in collections)
                {
                    IEnumerable<object> entities;
                    string label;
                    entities = properties.ElementAt(0).GetValue(val, null) as IEnumerable<object>;

                    DataEntryCollection entries = new DataEntryCollection();
                    foreach (var entity in entities)
                    {
                        Type entityType = entity.GetType();
                        IEnumerable<PropertyInfo> properties2 = entityType.GetTypeInfo().DeclaredProperties;
                        float count, capacity;
                        float.TryParse(properties2.ElementAt(0).GetValue(entity, null).ToString(), out count);
                        float.TryParse(properties2.ElementAt(1).GetValue(entity, null).ToString(), out capacity);
                        Func<float, float, Color> func = properties2.ElementAt(2).GetValue(entity, null) as Func<float, float, Color>;
                        if (func != null)
                        {
                            color = func.Invoke(count, capacity).ToSKColor();
                        }
                        entries.Add(new DataEntry(count, capacity, color));
                    }
                    label = properties.ElementAt(1).GetValue(val, null).ToString();
                    if (entities.Count() == 0)
                        label = string.Empty;
                    Sections.Add(new Series(entries, label)
                    {
                        MarginLeftInner = Series.MarginLeftInner,
                        MarginTopInner = Series.MarginTopInner,
                        MarginBottomInner = Series.MarginBottomInner,
                        MarginRightInner = Series.MarginRightInner,
                        MarginFooter = Series.MarginFooter,
                        LabelTextSize = Series.LabelTextSize,
                        WidthItem = Series.WidthItem
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void DrawSections(SKCanvas sKCanvas, float xOffset, float yOffset, double kScale)
        {
            if (SeriesOrientation == SeriesOrientation.Vertical)
            {
                int iHeight = Container.InnerHeight / Sections.Count;
                int iWidth = Container.InnerWidth;


                foreach (var section in Sections)
                {
                    section.Draw(sKCanvas, iWidth, iHeight, kScale, xOffset, yOffset);
                    yOffset += iHeight;
                }
            }
            else
            {
                int iHeight = Container.InnerHeight;
                int iWidth = Container.InnerWidth / Sections.Count;

                foreach (var section in Sections)
                {
                    section.Draw(sKCanvas, iWidth, iHeight, kScale, xOffset, yOffset);
                    xOffset += iWidth;
                }
            }
        }
        #region Color Graphs

        private SKColor MatchStateStatusToColor(KioskState state, KioskStatus status)
        {
            try 
            {
                if (state == CHBackOffice.ApiServices.ChsProxy.KioskState.InService ||
                   state == CHBackOffice.ApiServices.ChsProxy.KioskState.ONLINE)
                {
                    if (status == CHBackOffice.ApiServices.ChsProxy.KioskStatus.Normal)
                    {
                        return StaticResourceManager.GetColor("StatusNormal").ToSKColor();
                    }
                    else if (status == CHBackOffice.ApiServices.ChsProxy.KioskStatus.Warning)
                    {
                        return StaticResourceManager.GetColor("StatusWarning").ToSKColor();
                    }
                    else
                    {
                        return StaticResourceManager.GetColor("StatusCritical").ToSKColor();
                    }
                }
                else if (state == CHBackOffice.ApiServices.ChsProxy.KioskState.Offline)
                {
                    return StaticResourceManager.GetColor("StatusOffline").ToSKColor();
                }
                else if (state == CHBackOffice.ApiServices.ChsProxy.KioskState.OutOfServiceSOP)
                {
                    return StaticResourceManager.GetColor("StatusSOP").ToSKColor();
                }
                else
                {
                    return StaticResourceManager.GetColor("StatusOOS").ToSKColor();
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return default(SKColor);
            }
        }
        #endregion

        #endregion
    }

    public enum SeriesOrientation
    {
        Vertical,
        Horizontal
    }
}
