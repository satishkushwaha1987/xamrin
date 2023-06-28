using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHSBackOffice.Models.Popup
{
    public class PopoverItem : BindableBase
    {
        string _key;
        public string Key
        {
            get { return _key; }
            set
            {
                SetProperty(ref _key, value);
                RaisePropertyChanged(nameof(Key));
            }
        }

        string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                SetProperty(ref _value, value);
                RaisePropertyChanged(nameof(Value));
            }
        }

        bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                SetProperty(ref _selected, value);
                RaisePropertyChanged(nameof(Selected));
            }
        }
        
    }

    public class PopoverItemEqualityComparer : IEqualityComparer<PopoverItem>
    {
        public bool Equals(PopoverItem x, PopoverItem y)
        {
            bool result = x.Key == y.Key;
            return result;
        }
        public int GetHashCode(PopoverItem obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
