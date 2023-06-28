using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using System;
using System.Diagnostics;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BackOfficeHostAddressPage : ExtendedNaviPage
    {
		public BackOfficeHostAddressPage()
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
	}
}