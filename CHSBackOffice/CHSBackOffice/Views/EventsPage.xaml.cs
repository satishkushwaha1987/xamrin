using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using System;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EventsPage : ExtendedNaviPage
    {
		public EventsPage ()
		{
            try
            {
                InitializeComponent();
                var popUpLayout = PopupLayout.AddToPage(this);
                detailPopup.ParentContext = popUpLayout;

            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
		}
	}
}