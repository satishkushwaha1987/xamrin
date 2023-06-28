using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.Models.Popup
{
    public class PopupParameters
    {
        public string Title { get; set; }
        public string Status { get; set; }
        public Color TitleBackground { get; set; } = Color.FromHex("#F99D52");
        public Color TitleTextColor { get; set; } = Color.White;
        public bool CanChangeValues { get; set; }
        public List<PopupRow> Rows { get; set; }
    }
}
