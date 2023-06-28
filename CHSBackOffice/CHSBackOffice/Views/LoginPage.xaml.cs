using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ExtendedNaviPage
    {
        #region .CTOR
        public LoginPage ()
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