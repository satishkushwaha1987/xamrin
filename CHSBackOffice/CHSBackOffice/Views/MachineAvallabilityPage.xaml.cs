using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using System;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MachineAvallabilityPage : ReportPage
    {
        public MachineAvallabilityPage ()
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