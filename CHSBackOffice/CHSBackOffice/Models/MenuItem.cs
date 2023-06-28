using Xamarin.Forms;

namespace CHSBackOffice.Models
{
    public class MenuItem
    {
        public string Icon { get; set; }
        public Color IconColor { get; set; }
        public string Title { get; set; }
        public int Level { set; get; }
        public Thickness Margin => new Thickness(15 + Level * 10, 1, 5, 1);

        public MenuActionTypes MenuActionType;
        public object CommandParameter { get; set; }
        
    }
}
