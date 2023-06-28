using CHSBackOffice.Extensions;
using CHSBackOffice.Support.StaticResources;
using DevExpress.Mobile.DataGrid;
using DevExpress.Mobile.DataGrid.Theme;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CHSBackOffice.Support
{
    class DxGridHelper
    {
        #region "STATIC METHODS"

        internal static void InitParametersGrid(
            ref GridControl gridControl,
            Color titleColor,
            bool hasEvenOddRows = true,
            bool selectionEnabled = false,
            Color? selectedRowColor = null)
        {
            ThemeManager.ThemeName = Themes.Light;

            #region "Header customization"

            ThemeManager.Theme.HeaderCustomizer.BackgroundColor = titleColor;
            ThemeFontAttributes headerFont = new ThemeFontAttributes(StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular),
                                        StaticResourceManager.GetFontSize(AppFontSize.Medium),
                                        FontAttributes.None, Color.White);
            ThemeManager.Theme.HeaderCustomizer.Font = headerFont;

            #endregion

            #region "Cell customization"

            if (selectedRowColor.HasValue)
                ThemeManager.Theme.CellCustomizer.SelectionColor = selectedRowColor.Value;

            ThemeFontAttributes cellFont = new ThemeFontAttributes(StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular),
                                        StaticResourceManager.GetFontSize(AppFontSize.Small),
                                        FontAttributes.None, Color.Black);

            ThemeManager.Theme.CellCustomizer.Font = cellFont;
            ThemeManager.Theme.CellCustomizer.Padding = new Thickness(10, 2);

            #endregion

            if (hasEvenOddRows)
                gridControl.CustomizeCell += GridControl_CustomizeCell;

            if (!selectionEnabled)
                gridControl.PropertyChanged += DisableSelection_PropertyChanged;

            ThemeManager.RefreshTheme();
        }

        internal static void SetDefaultSettings(ref GridControl gridControl)
        {
            gridControl.IsReadOnly = true;
            gridControl.AllowResizeColumns = true;
            gridControl.ColumnsAutoWidth = true;
            gridControl.AllowGroup = true;
            gridControl.AllowGroupCollapse = true;
            gridControl.GroupsInitiallyExpanded = false;
            gridControl.IsColumnChooserEnabled = false;
            gridControl.ColumnHeadersHeight = Xamarin.Forms.Device.Idiom == TargetIdiom.Phone ? 35 : 50;
            gridControl.RowHeight = Xamarin.Forms.Device.Idiom == TargetIdiom.Phone ? 55 : 80;
            gridControl.Margin = -1;
        }

        internal static void UpdateDemension(ref GridControl gridControl)
        {
            gridControl.WidthRequest = DeviceDisplay.MainDisplayInfo.WidthInDp();
            gridControl.Redraw(true);
        }

        #endregion

        #region "EVENT HANDLERS"

        private static void DisableSelection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GridControl.SelectedDataObject))
                ((GridControl)sender).SelectedDataObject = null;
        }

        private static void GridControl_CustomizeCell(CustomizeCellEventArgs e)
        {
            if (e.RowHandle % 2 == 0)
            {
                e.BackgroundColor = StaticResourceManager.GetColor(AppColor.OddRowBackgroundColor);
                e.Handled = true;
            }
        }

        #endregion
    }
}
