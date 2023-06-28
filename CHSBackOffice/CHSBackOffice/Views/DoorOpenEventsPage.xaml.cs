using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using System;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DoorOpenEventsPage : ExtendedNaviPage
    {
        #region ".CTOR"

        public DoorOpenEventsPage ()
		{
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }			
        }

        #endregion
    }
}