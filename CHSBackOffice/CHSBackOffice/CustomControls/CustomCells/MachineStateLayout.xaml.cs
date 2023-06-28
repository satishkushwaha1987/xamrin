using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls.CustomCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MachineStateLayout : FastCell, INotifyPropertyChanged
    {
        #region .CTOR
        public MachineStateLayout ()
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
                ExtendedNaviPage.OnOrientartionChanged += MachineStateLayout_OnOrientartionChanged;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        protected override void SetupCell(bool isRecycled)
        {
            var row = BindingContext as CHBackOffice.ApiServices.ChsProxy.Kiosk;
            if (row != null)
            {
                ColorState = MatchStateStatusToColor(row);
            }
        }

        private Color MatchStateStatusToColor(CHBackOffice.ApiServices.ChsProxy.Kiosk row)
        {
            if (row.State == CHBackOffice.ApiServices.ChsProxy.KioskState.InService ||
                   row.State == CHBackOffice.ApiServices.ChsProxy.KioskState.ONLINE)
            {
                if (row.Status == CHBackOffice.ApiServices.ChsProxy.KioskStatus.Normal)
                {
                    return StaticResourceManager.GetColor("StatusNormal");
                }
                else if (row.Status == CHBackOffice.ApiServices.ChsProxy.KioskStatus.Warning)
                {
                    return StaticResourceManager.GetColor("StatusWarning");
                }
                else
                {
                    return StaticResourceManager.GetColor("StatusCritical");
                }
            }
            else if (row.State == CHBackOffice.ApiServices.ChsProxy.KioskState.Offline)
            {
                return StaticResourceManager.GetColor("StatusOffline");
            }
            else if (row.State == CHBackOffice.ApiServices.ChsProxy.KioskState.OutOfServiceSOP)
            {
                return StaticResourceManager.GetColor("StatusSOP");
            }
            else
            {
                return StaticResourceManager.GetColor("StatusOOS");
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

        #region Methods

        protected override void OnParentSet()
        {
            base.OnParentSet();
            CalculateSpace();
        }

        private void MachineStateLayout_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            try 
            {
                CalculateSpace();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void CalculateSpace()
        {
            var iosPlatform = Device.Idiom == TargetIdiom.Phone ? 60 : 100;
            var androidPlatform = Device.Idiom == TargetIdiom.Phone ? 60 : 100;
            var space = LayoutResizer.Instance.InitColumnSpace(
                                DeviceDisplay.MainDisplayInfo.WidthInDp(),
                                Device.RuntimePlatform == Device.Android ?
                                androidPlatform :
                                iosPlatform);
            cell.Margin = new Thickness(space , space, 0, 0);
        }
        #endregion
    }
}