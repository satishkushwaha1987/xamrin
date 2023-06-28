using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using static CHSBackOffice.Support.CommonViewObjects;

namespace CHSBackOffice.Models.Popup
{
    public class PopupRow : BindableBase
    {
        public LineBreakMode TextLineBreakMode { get; set; } = LineBreakMode.TailTruncation;
        public string Title { get; set; }
        public Color TitleTextColor { get; set; } = Color.Black;
        public Color BackgroundColor { get; set; } = Color.Transparent;
        private Type _typeOfValue;

        private string _key;
        public string Key
        {
            get => String.IsNullOrEmpty(_key) ? (String.IsNullOrEmpty(Title) ? (new Random()).Next(Int32.MaxValue).ToString() : Title) : _key;
            set => _key = value;
        }

        private object _value;
        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_typeOfValue == null)
                    _typeOfValue = _value?.GetType();
                if (_isDate)
                {
                    DateOfValue = (DateTime)_value;
                    TimeOfValue = (DateTime)_value;
                }
            }
        }

        public bool _failedEditor;
        public bool IsFailedEditor
        {
            get => _failedEditor;
            set
            {
                _failedEditor = value;
                RaisePropertyChanged(nameof(IsFailedEditor));
            }
        }

        public bool _isButton;
        public bool IsButton
        {
            get => _isButton;
            set
            {
                _isButton = value;
                RaisePropertyChanged(nameof(IsButton));
            }
        }

        public bool _isPicker;
        public bool IsPicker
        {
            get => _isPicker;
            set
            {
                _isPicker = value;
                RaisePropertyChanged(nameof(IsPicker));
            }
        }

        public bool BoolValue
        {
            get => (_isBool ? (bool)Value : false);
            set
            {
                if (_isBool)
                    Value = value;
            }
        }


        public DateTime DateValue
        {
            get => (_isDate ? (DateTime)Value : DateTime.MinValue);
            set
            {
                if (_isDate)
                {
                    Value = value;
                }      
            }
        }

        DateTime dateOfValue;
        public DateTime DateOfValue
        {
            get => (_isDate ? dateOfValue : DateTime.MinValue);
            set
            {
                if (_isDate)
                {
                    dateOfValue = value;
                    RaisePropertyChanged(nameof(DateOfValue));
                }
            }
        }

        DateTime timeOfValue;
        public DateTime TimeOfValue
        {
            get=>(_isDate ? timeOfValue : DateTime.MinValue);
            set
            {
                if (_isDate)
                {
                    timeOfValue = value;
                    RaisePropertyChanged(nameof(TimeOfValue));
                }
            }
        }

        Models.Popup.PopoverItem pickerSelectedItem;
        public Models.Popup.PopoverItem PickerSelectedItem
        {
            get => pickerSelectedItem;
            set
            {
                if (pickerSelectedItem != null)
                {
                    pickerSelectedItem.Selected = false;
                }

                SetProperty(ref pickerSelectedItem, value);
                RaisePropertyChanged(nameof(PickerSelectedItem));

                if (pickerSelectedItem != null)
                {
                    pickerSelectedItem.Selected = true;
                }
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> pickerItemSource;
        public ObservableCollection<Models.Popup.PopoverItem> PickerItemSource
        {
            get => pickerItemSource;
            set
            {
                SetProperty(ref pickerItemSource, value);
                RaisePropertyChanged(nameof(PickerItemSource));
            }
        }


        public string TextValue
        {
            get => (_isText ? (Value != null ? Value.ToString() : (AllowedValues != null && AllowedValues.Count > 0 ? AllowedValues[0].ToString() : "")) : "");
            set
            {
                if (_isText)
                    Value = value;
            }
        }

        public object ObjectValue
        {
            get => IsSelectable ? Value ?? AllowedValues[0] : "";
            set
            {
                if (IsSelectable)
                    Value = value;
            }
        }

        private bool _isBool => _typeOfValue == typeof(bool);
        private bool _isDate => _typeOfValue == typeof(DateTime);
        private bool _isText => !_isBool && !_isDate;
        private bool _isEditor => _typeOfValue == typeof(Editor);


        public Color ValueTextColor { get; set; } = Color.Black;
        
        public string Placeholder { get; set; }
        public bool IsPassword { get; set; }
        
        public bool ReadOnly { get; set; }

        public bool IsEnabled => !ReadOnly;

        public bool IsSwitch => _isBool;
        public bool IsLabel => !IsSwitch && ReadOnly;
        public bool IsDatePicker => !IsSelectable && !IsSwitch && _isDate;
        public bool IsEntry => !IsSelectable && !IsSwitch && !IsLabel && !IsDatePicker;

        private Keyboard _inputKeyboard;
        public Keyboard InputKeyboard
        {
            get => _inputKeyboard ?? (_typeOfValue == typeof(Int32) || _typeOfValue == typeof(Double) || _typeOfValue == typeof(float) ? Keyboard.Numeric : Keyboard.Default)  ;
            set => _inputKeyboard = value;
        }

        public bool IsSelectable => !ReadOnly && AllowedValues != null && AllowedValues.Count > 0;

        public List<object> AllowedValues { get; set; }

    }
}
