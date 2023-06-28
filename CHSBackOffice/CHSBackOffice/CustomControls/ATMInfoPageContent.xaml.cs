using CHSBackOffice.Support;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ATMInfoPageContent : ContentView
    {
        internal event Action<int> ListViewItemAppearing;
        internal event Action<int> ListViewItemTapped;

        public ATMInfoPageContent()
        {
            InitializeComponent();

            
            //popover.VerticalPosition = PickerWithPopover.LocationOption.Centred;
            DxGridHelper.InitParametersGrid(ref parametersGrid, Color.FromHex("#30A9D6"));
            DxGridHelper.SetDefaultSettings(ref parametersGrid);
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
    }
}