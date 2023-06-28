using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MachineStatusSkiaPage : ExtendedNaviPage
    {
        #region ".CTOR"
        public MachineStatusSkiaPage()
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