using CHSBackOffice.CustomControls.CustomCells;
using CHSBackOffice.CustomControls.SkiaControls;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MachineInfoSkiaContent : FastCell, INotifyPropertyChanged
    {
        #region .CTOR
        public MachineInfoSkiaContent()
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

        #region Methods
        #region FastCell
        protected override void InitializeCell()
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

        protected override void SetupCell(bool isRecycled)
        {
            CHSBackOffice.CustomControls.SkiaControls.Container container =
                new SkiaControls.Container()
                {
                    MarginHeader = 20.0f,
                    LabelTextSize = 24.0f,
                    MarginInner = Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet ?
                        20.0f : 20.0f,
                    PaddingOuter = 20.0f
                };
            Series series = new Series()
            {
                MarginRightInner = 10.0f,
                MarginLeftInner = 50.0f,
                MarginTopInner = 30.0f,
                MarginBottomInner = 10.0f,
                MarginFooter = 0.0f,
                LabelTextSize = 35.0f,
                WidthItem = Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet ?
                    40.0f : 38.0f
            };
            machine.Container = container;
            machine.Series = series;
            var row = BindingContext as KioskExtended;
            if (row != null)
            {
                machine.DataSource = row.DataSetWithoutId;
            }
        }
        #endregion
        #endregion

        #region Bindable Properties

        #region WidthExcludeMenu
        private double _width;
        public double WidthExcludeMenu
        {
            get => _width;
            set
            {
                _width = value;
                NotifyPropertyChanged(nameof(WidthExcludeMenu));
            }
        }
        #endregion

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