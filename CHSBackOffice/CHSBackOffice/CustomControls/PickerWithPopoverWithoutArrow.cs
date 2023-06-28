
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class PickerWithPopoverWithoutArrow : PickerWithPopover
    {
        public PickerWithPopoverWithoutArrow()
        {
            #region Content
            _contentGrid.Children.Clear();
            _contentGrid.Children.Add(_pickerText, 0, 0);
            Grid.SetColumnSpan(_pickerText, 2);
            #endregion
        }
    }
}
