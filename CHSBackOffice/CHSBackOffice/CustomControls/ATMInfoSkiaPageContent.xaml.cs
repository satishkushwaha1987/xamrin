using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ATMInfoSkiaPageContent : ContentView, INotifyPropertyChanged
    {
        #region Fieds
        internal event Action<int> ListViewItemAppearing;
        internal event Action<int> ListViewItemTapped;
        private double _width;
        private double _height;
        #endregion

        #region Bindable Properties
        private double _widthDisplay;
        public double WidthDisplay
        {
            get => _widthDisplay;
            set
            {
                _widthDisplay = value;
                RaisePropertyChanged(nameof(WidthDisplay));
            }
        }

        private double _heightDisplay;
        public double HeightDisplay
        {
            get => _heightDisplay;
            set
            {
                _heightDisplay = value;
                RaisePropertyChanged(nameof(HeightDisplay));
            }
        }
        #endregion

        #region .CTOR
        public ATMInfoSkiaPageContent()
        {
            InitializeComponent();
            DxGridHelper.InitParametersGrid(ref parametersGrid, Color.FromHex("#30A9D6"));
            DxGridHelper.SetDefaultSettings(ref parametersGrid);
        }
        #endregion

        #region Methods

        protected override void OnSizeAllocated(double width, double height)
        {
            var oldWidth = _width;
            const double sizenotallocated = -1;

            base.OnSizeAllocated(width, height);

            if (Equals(_width, width) && Equals(_height, height)) return;

            _width = width;
            _height = height;

            if (Equals(oldWidth, sizenotallocated)) return;
            if (_width < 0 || _height < 0)
            {
                WidthDisplay = DeviceDisplay.MainDisplayInfo.Width;
                HeightDisplay = DeviceDisplay.MainDisplayInfo.Height;
            }
            else
            {
                WidthDisplay = _width;
                HeightDisplay = _height;
            }
            
        }

        internal void SetPopupParentContext(View layout)
        {
            popover.ParentContext = layout;
        }

        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            ListViewItemAppearing?.Invoke(e.ItemIndex);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListViewItemTapped?.Invoke(e.ItemIndex);
        }

        internal void RefreshGrid()
        {
            DxGridHelper.UpdateDemension(ref parametersGrid);
        }
        #endregion


        #region INotifyPropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged(string nameOfProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameOfProperty));
        }

        void SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "")
        {
            backingStore = value;

            RaisePropertyChanged(propertyName);
        }

        #endregion
    }
}