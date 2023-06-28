using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CHSBackOffice.Support
{
    public class PopupData : INotifyPropertyChanged
    {
        public static PopupData Instance = new PopupData();

        public event PropertyChangedEventHandler PropertyChanged;

        private Models.Popup.PopupParameters _popupParameters;
        public Models.Popup.PopupParameters PopupParameters
        {
            get => _popupParameters;
            set
            {
                _popupParameters = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PopupParameters)));
            }
        }

        private bool _popupIsActive;
        public bool PopupIsActive
        {
            get => _popupIsActive;
            set
            {
                _popupIsActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PopupIsActive)));
            }
        }
    }
}
