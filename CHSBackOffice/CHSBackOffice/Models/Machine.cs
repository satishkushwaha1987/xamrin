using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CHSBackOffice.Models
{
    public class Machine : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string Id { get; set; }
        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
            }
        }
    }
}
