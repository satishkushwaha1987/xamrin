using Xamarin.Forms;

namespace CHSBackOffice.Support.Classes
{
    public class ListViewItemBase<T>
    {
        public T BaseObject;
        public int Number { get; set; }
        public Color BackColor => Number % 2 == 0 ? Color.White : Color.FromHex("#F4F4F4");
    }
}
