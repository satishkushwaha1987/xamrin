using CHSBackOffice.CustomControls.SkiaControls;
using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Classes;
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls.CustomCells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MachineStateSkiaLayout : FastCell, INotifyPropertyChanged
    {
        #region .CTOR
        public MachineStateSkiaLayout()
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
            CHSBackOffice.CustomControls.SkiaControls.Container container = 
                new CHSBackOffice.CustomControls.SkiaControls.Container()
            {
                MarginHeader = 25.0f,
                LabelTextSize = 24.0f,
                MarginInner = Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet ?
                10.0f : 15.0f,
                PaddingOuter = 20.0f
            };
            Series series = new Series()
            {
                MarginRightInner = Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet ?
                9.0f : 6.0f,
                MarginLeftInner = Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet ?
                9.0f : 6.0f,
                MarginTopInner = 10.0f,
                MarginBottomInner = 10.0f,
                MarginFooter = 5.0f,
                LabelTextSize = 20.0f,
                WidthItem = Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet ?
                20.0f : 18.0f
            };
            machine.Container = container;
            machine.Series = series;
            var row = BindingContext as KioskExtended;
            if (row != null)
            {
                machine.DataSource = row.DataSet;
            }
        }

        #endregion

        #region Bindable Properties
        
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
            set { SetValue(ParentContextProperty, value); }
            get { return GetValue(ParentContextProperty); }
        }

        public static readonly BindableProperty ParentContextProperty = BindableProperty.Create(
            nameof(ParentContext), typeof(object), typeof(MachineStateSkiaLayout), null, propertyChanged: OnParentContextChanged);

        private static void OnParentContextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as MachineStateSkiaLayout).ParentContext = newValue;
            }
        }

        #endregion

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
                                iosPlatform,
                                5);
            cell.Margin = new Thickness(0, 0, space, 10);
            var parent = ParentContext as Layout;
            if (parent != null)
            {
                parent.Margin = new Thickness(
                    space,
                    space,
                    parent.Margin.Right,
                    0);
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