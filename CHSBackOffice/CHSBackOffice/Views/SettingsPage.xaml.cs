using CHSBackOffice.CustomControls;
using CHSBackOffice.Events;
using CHSBackOffice.Support;
using System;
using Xamarin.Forms;

namespace CHSBackOffice.Views
{
    public partial class SettingsPage : ExtendedNaviPage
    {

        #region .CTOR
        public SettingsPage()
        {
            try
            {
                InitializeComponent();
                OnOrientartionChanged += SettingsPage_OnOrientartionChanged;
                scroll.Scrolled += Scroll_Scrolled;
                var popupLayout = PopupLayout.AddToPage(this);
                popover.ParentContext = popupLayout;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }
        #endregion

        #region "Event Handling"
        private void SettingsPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            App.SharedEventAggregator.GetEvent<OrientationChanged>().Publish();
        }

        private void Scroll_Scrolled(object sender, ScrolledEventArgs e)
        {
            App.SharedEventAggregator.GetEvent<ScrollYChanged>().Publish(e.ScrollY);
        }
        #endregion
    }
}
