using CHSBackOffice.CustomControls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SystemParametersPage : ExtendedNaviPage
    {
		public SystemParametersPage ()
		{
            try
            {
                InitializeComponent();
                Support.DxGridHelper.InitParametersGrid(ref parametersGrid, Color.FromHex("#874845"));
                Support.DxGridHelper.SetDefaultSettings(ref parametersGrid);
                OnOrientartionChanged += SystemParametersPage_OnOrientartionChanged;
                parametersGrid.GroupsInitiallyExpanded = true;
            }
            catch (Exception ex)
            {
                Support.ExceptionProcessor.ProcessException(ex);
            }
		}

        #region "EVENT HANDLERS"
        private void SystemParametersPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            Support.DxGridHelper.UpdateDemension(ref parametersGrid);
        }

        #endregion
    }
}