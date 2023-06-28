using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using System;
using Xamarin.Forms;

namespace CHSBackOffice.Views
{
    public partial class DashboardPage : ExtendedNaviPage
    {
        #region "PRIVATE FIELDS"

        private double heightExcludeNavBar;

        #endregion

        #region ".CTOR"

        public DashboardPage()
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
