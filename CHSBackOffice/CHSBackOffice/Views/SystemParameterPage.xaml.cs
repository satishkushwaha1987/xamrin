using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CHSBackOffice.Views
{
    public partial class SystemParameterPage : ExtendedNaviPage
    {
        #region .CTOR
        public SystemParameterPage()
        {
            try
            {
                InitializeComponent();
                InitLayout(DeviceDisplay.MainDisplayInfo.Orientation);
                ExtendedNaviPage.OnOrientartionChanged += ExtendedNaviPage_OnOrientartionChanged;
                var popupLayout = PopupLayout.AddToPage(this);
                popover.ParentContext = popupLayout;
                popoverBottom.ParentContext = popupLayout;
                
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region "Events Handling"
        private void ExtendedNaviPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            InitLayout(e.Orientation == PageOrientation.Horizontal ? DisplayOrientation.Landscape : DisplayOrientation.Portrait);
        }

        #endregion

        #region Methods

        private void InitLayout(DisplayOrientation orientation)
        {
            if (orientation == DisplayOrientation.Landscape)
            {
                ContentGrid.Spacing = Device.Idiom == TargetIdiom.Tablet ? 25 : 5;
            }
            else
            {
                ContentGrid.Spacing = 25;
            }
        }
        #endregion
    }
}
