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
	public partial class EmployeesLayout : Frame
	{
        #region .CTOR
        public EmployeesLayout ()
		{
            try
            {
                InitializeComponent();
                ExtendedNaviPage.OnOrientartionChanged += ExtendedNaviPage_OnOrientartionChanged;
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
            WidthRequest = Device.Idiom == TargetIdiom.Phone ? 150 : 350;
            HeightRequest = Device.Idiom == TargetIdiom.Phone ? 60 : 120;
        }
        #endregion
    }
}