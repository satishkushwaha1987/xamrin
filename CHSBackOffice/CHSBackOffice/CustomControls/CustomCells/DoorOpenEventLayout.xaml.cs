using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls.CustomCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DoorOpenEventLayout : FastCell, INotifyPropertyChanged
    {
        #region Properties
        private BaseMatcher<CHBackOffice.ApiServices.ChsProxy.SeverityType, string> severityTypeToColorMatcher;
        #endregion

        #region .CTOR
        public DoorOpenEventLayout ()
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
                severityTypeToColorMatcher = FactoryMatcher.GetSeverityTypeToColorMatcher();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }

        }

        protected override void SetupCell(bool isRecycled)
        {
            var row = BindingContext as CHBackOffice.ApiServices.ChsProxy.DoorOpenEvent;
            if (row != null)
            {
                ColorState = StaticResourceManager.GetColor(severityTypeToColorMatcher.ToValue(row.Severity));
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
    }
}