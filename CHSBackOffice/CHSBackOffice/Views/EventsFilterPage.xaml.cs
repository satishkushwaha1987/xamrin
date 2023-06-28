using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using System;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EventsFilterPage : ExtendedNaviPage
    {
        #region .CTOR
        public EventsFilterPage()
		{
            try
            {
                InitializeComponent();
                InitLayout(DeviceDisplay.MainDisplayInfo.Orientation);
                ExtendedNaviPage.OnOrientartionChanged += ExtendedNaviPage_OnOrientartionChanged;
                var popupLayout = PopupLayout.AddToPage(this);
                machineIdPopover.ParentContext = popupLayout;
                eventDescriptionPopover.ParentContext = popupLayout;
                machineStatusPopover.ParentContext = popupLayout;
                machineStatusPopover.ParentContext = popupLayout;
                eventSeverityPopover.ParentContext = popupLayout;
                machineStatePopover.ParentContext = popupLayout;
                machineTypePopover.ParentContext = popupLayout;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }


        #endregion

        #region "Event Handling"
        private void ExtendedNaviPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            var orientation = e.Orientation == PageOrientation.Vertical ? DisplayOrientation.Portrait : DisplayOrientation.Landscape;
            InitLayout(orientation);
        }


        private void InitLayout(DisplayOrientation orientation)
        {
            if (orientation == DisplayOrientation.Landscape)
            {
                secondRow.Height = new Xamarin.Forms.GridLength(0.3, Xamarin.Forms.GridUnitType.Star);
            }
            else
            {
                secondRow.Height = new Xamarin.Forms.GridLength(1, Xamarin.Forms.GridUnitType.Star);
            }
        }

        #endregion
    }
}