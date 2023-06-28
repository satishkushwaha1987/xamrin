using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls.CustomCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardTileLayout : Frame, IParentContext
    {
        #region "Private Fields"
        private int tileColumns;
        private int tileRows;
        private double widthExcludeMenu;
        private double heightExcludeNavBar;
        private double minSpace = 10;
        private double height = (Device.Idiom == TargetIdiom.Tablet) ? 180 : 110;
        #endregion

        #region CTOR
        public DashboardTileLayout()
        {
            InitializeComponent();
            
            ExtendedNaviPage.OnOrientartionChanged += ExtendedNaviPage_OnOrientartionChanged;
            ExtendedNaviPage.OnDemensionChanged += ExtendedNaviPage_OnDemensionChanged;
        }

        #endregion

        #region Bindable Properties

        #region IParentContext
        public object Context
        {
            set
            {
                ParentContext = value;
            }
            get { return ParentContext; }
        }

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        public static readonly BindableProperty ParentContextProperty =
           BindableProperty.Create("ParentContext", typeof(object), typeof(DashboardTileLayout), null, propertyChanged: OnParentContextPropertyChanged);

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as DashboardTileLayout).ParentContext = newValue;
            }
        }

        #endregion

        #endregion

        #region "Event Handling"

        private void ExtendedNaviPage_OnDemensionChanged(object sender, PageDemensionEventsArgs e)
        {
            heightExcludeNavBar = e.DemensionHeight;
            widthExcludeMenu = e.DemensionWidth;
            InitLayout(DeviceDisplay.MainDisplayInfo.Orientation);
            CalculateTile();
        }
        private void ExtendedNaviPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            var orientation = e.Orientation == PageOrientation.Vertical ? DisplayOrientation.Portrait : DisplayOrientation.Landscape;
            InitLayout(orientation);
            CalculateTile();
        }

        #endregion

        #region Methods

        private void InitLayout(DisplayOrientation orientation)
        {
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                TabletGridDemensions(orientation);
            }
            else
            {
                PhoneGridDemensions(orientation);
            }
        }

        private void PhoneGridDemensions(DisplayOrientation orientation)
        {
            tileColumns = orientation == DisplayOrientation.Landscape ? 4 : 2;
            tileRows = orientation == DisplayOrientation.Landscape ? 2 : 4;
        }

        private void TabletGridDemensions(DisplayOrientation orientation)
        {
            tileColumns = orientation == DisplayOrientation.Landscape ? 3 : 2;
            tileRows = orientation == DisplayOrientation.Landscape ? 3 : 4;
        }

        private void CalculateTile()
        {
            try 
            {
                double calcContainer = (height + minSpace) * tileRows;
                if (calcContainer > heightExcludeNavBar)
                {
                    PhoneGridDemensions(DeviceDisplay.MainDisplayInfo.Orientation);
                }

                var width = LayoutResizer.Instance.InitDemension(widthExcludeMenu - 10, tileColumns, 10);
                tile.WidthRequest = width;

                double space = LayoutResizer.Instance.InitRowSpace(
                     heightExcludeNavBar,
                     height,
                     tileRows);

                tile.Margin = new Thickness(tile.Margin.Left, 0, tile.Margin.Right, space);
                var parent = ParentContext as Layout;
                if (parent != null)
                {
                    parent.Margin = new Thickness(
                        parent.Margin.Left,
                        space,
                        parent.Margin.Right,
                        0);
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