using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using System;

namespace CHSBackOffice.Views
{
    public partial class TransactionsPage : ExtendedNaviPage
    {
        public TransactionsPage()
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
