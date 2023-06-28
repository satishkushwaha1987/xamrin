using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls.CustomCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionLayoutTablet : FastCell, INotifyPropertyChanged
    {
        #region Properties
        BaseMatcher<string, string> stringStatusToColorMatcher;
        #endregion

        #region .CTOR
        public TransactionLayoutTablet ()
		{
            try
            {
                this.PrepareCell();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
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
            if (row != null)
            {
                ColorState = StaticResourceManager.GetColor(stringStatusToColorMatcher.ToValue(row.Status));
            }
        }
        #endregion

        #region Bindable Properties
        private Color stateColor;
        public Color ColorState
        {
            get => stateColor;
            set
            {
                stateColor = value;
                NotifyPropertyChanged(nameof(ColorState));
            }
        }
        #endregion

        #region INotifyPropertyChanged
        internal void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}