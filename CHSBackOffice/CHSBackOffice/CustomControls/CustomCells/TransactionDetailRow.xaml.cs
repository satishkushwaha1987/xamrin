using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls.CustomCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionDetailRow : FastCell, IParentContext
    {
        #region Properties
        private BaseMatcher<string, string> stringStatusToColorMatcher;
        #endregion

        #region .CTOR
        public TransactionDetailRow()
        {
            this.PrepareCell();
        }
        #endregion

        #region FastCell
        protected override void InitializeCell()
        {
            try
            {
                InitializeComponent();
                stringStatusToColorMatcher = FactoryMatcher.GetTransactionStatusToColorMatcher();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        protected override void SetupCell(bool isRecycled)
        {
            var row = BindingContext as CHBackOffice.ApiServices.ChsProxy.PatronTransaction;
            if (row == null && BindingContext is ViewModels.TransactionsDetailsFrameViewModel)
                row = (BindingContext as ViewModels.TransactionsDetailsFrameViewModel).SelectedTransaction;

            if (row != null)
            {
                bool IsFailed = row.Status == "Failed" || row.Status == "Partial Dispense" ? true : false;
                id.Text = row.Id;
                type.Text = row.Type;
                startDate.Text = row.StartDate.ToDateTimeStr();
                endDate.Text = row.EndDate.ToDateTimeStr();
                unitId.Text = row.KioskId;
                sequenceId.Text = row.SequenceId;
                patron.Text = row.Patron;
                reqAmount.Text = "$" + (1.0 * row.AmountRequested / 100).ToString("0.00");
                disAmount.Text = "$" + (1.0 * row.AmountDispensed / 100).ToString("0.00");
                ticket.Text = "$" + (1.0 * row.AmountPrinted / 100).ToString("0.00");
                fee.Text = "$" + (1.0 * row.Fee / 100).ToString("0.00");
                amountDeb.Text = "$" + (1.0 * row.AmountRequested / 100).ToString("0.00");
                hostDate.Text = row.HostDate.ToDateTimeStr();
                hostResponce.Text = row.HostStatus;
                hostSequence.Text = row.HostSequenceId;
                status.Text = row.Status;
                status.TextColor = StaticResourceManager.GetColor(stringStatusToColorMatcher.ToValue(row.Status));
            }
        }
        #endregion

        #region IParentContext
        public object Context
        {
            set
            {
                ParentContext = value;
            }
            get { return ParentContext; }
        }

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        public static readonly BindableProperty ParentContextProperty =
           BindableProperty.Create("ParentContext", typeof(object), typeof(TransactionDetailRow), null, propertyChanged: OnParentContextPropertyChanged);

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as TransactionDetailRow).ParentContext = newValue;
            }
        }
        #endregion

        private void button_Clicked(object sender, EventArgs e)
        {
           
        }

        #region "PRIVATE METHODS"

        //private string ToDateTimeStr(DateTime? dateTime)
        //{
        //    if (!dateTime.HasValue || dateTime.Value < DateTime.UtcNow.AddYears(-50))
        //        return String.Empty;

        //    return dateTime.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
        //}

        #endregion
    }
}