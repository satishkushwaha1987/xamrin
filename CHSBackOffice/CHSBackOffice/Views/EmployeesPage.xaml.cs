using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using CHSBackOffice.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EmployeesPage : ExtendedNaviPage
    {
        #region .CTOR
        public EmployeesPage ()
		{
            try
            {
                InitializeComponent();
                InitSpace(DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape ?
                    PageOrientation.Horizontal : PageOrientation.Vertical);
                ExtendedNaviPage.OnOrientartionChanged += ExtendedNaviPage_OnOrientartionChanged;
                ExtendedNaviPage.OnDemensionChanged += ExtendedNaviPage_OnDemensionChanged;
                var popUpLayout = PopupLayout.AddToPage(this);
                (popUpLayout as PopupLayout).HasInnerPopover = true;
                detailPopup.ParentContext = popUpLayout;
                addPopup.ParentContext = popUpLayout;
                (this.BindingContext as EmployeesPageViewModel).UserDetailPopup = detailPopup;
                (this.BindingContext as EmployeesPageViewModel).AddPopup = addPopup;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }
        #endregion

        #region "Event Handling"

        private void ExtendedNaviPage_OnDemensionChanged(object sender, PageDemensionEventsArgs e)
        {
            layout.Measure(e.DemensionWidth, e.DemensionHeight);
        }

        private void ExtendedNaviPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            InitSpace(e.Orientation);
        }

        #endregion

        #region Methods


        private void InitSpace(PageOrientation orientation)
        {

            if (orientation == PageOrientation.Horizontal)
            {
                layout.RowSpacing = Device.Idiom == TargetIdiom.Phone ? 20 : 20;
                layout.ColumnSpacing = Device.Idiom == TargetIdiom.Phone ? 20 : 10;
            }
            else
            {
                layout.ColumnSpacing = Device.Idiom == TargetIdiom.Phone ? 20 : 10;
                layout.RowSpacing = Device.Idiom == TargetIdiom.Phone ? 5 : 10;
            }
        }
        #endregion
    }
}