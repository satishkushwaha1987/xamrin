using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Droid.Rendereres;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ResolutionGroupName("CHSBackOffice.CustomControls")]
[assembly:ExportEffect(typeof(AddDatePickerPlatformEffect),nameof(AddDataPicker))]
namespace CHSBackOffice.Droid.Rendereres
{
    public class AddDatePickerPlatformEffect : PlatformEffect
    {
        Android.Views.View _view;
        DatePickerDialog _dialog;
        ICommand _command;
        ICommand _tapCommand;
        bool _opened = false;

        protected override void OnAttached()
        {
            
            _view = Control ?? Container;
            _view.Clickable = true;
            _view.Touch += OnTouchView;
            var effect = (Element.Effects.FirstOrDefault(e => e is Touch));
            if(effect is Touch)
                (effect as Touch).TouchAction += OnShowDialog;

            
            UpdateCommand();
            UpdateTapCommand();
        }

        protected override void OnDetached()
        {
            _view.Touch -= OnTouchView;
            _dialog.Dispose();
            _dialog = null;
            _view = null;
            _command = null;
            _tapCommand = null;

        }

       
        void OnTouchView(object sender, Android.Views.View.TouchEventArgs e)
        {
            if (e.Event.Action == Android.Views.MotionEventActions.Move)
            {
                
                return;
            }
            if (_dialog != null)
            {
                _dialog.Dispose();
            }
            if (!_opened)
            {
                _tapCommand?.Execute(null);
                CreateDialog();

                UpdateMinDate();
                UpdateMaxDate();

                _dialog.CancelEvent += OnCancelButtonClicked;
                _dialog.Show();
                _opened = true;
            }
        }

        void OnShowDialog(object sender, TouchActionEventArgs e)
        {
            if (e.Type == TouchActionType.Pressed)
            {
                if (!_opened)
                {
                    CreateDialog();

                    UpdateMinDate();
                    UpdateMaxDate();

                    _dialog.CancelEvent += OnCancelButtonClicked;
                    _dialog.Show();
                    _opened = true;
                }
            }
            if (e.Type != TouchActionType.Pressed || e.Type != TouchActionType.Entered)
            {
                _view.ClearFocus();
                _opened = false;
            }

        }
        protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);
            if (e.PropertyName == AddDataPicker.CommandProperty.PropertyName)
            {
                UpdateCommand();
            }
            else if (e.PropertyName == AddDataPicker.TapCommandProperty.PropertyName)
            {
                UpdateTapCommand();
            }
        }

        void CreateDialog()
        {
            var date = AddDataPicker.GetDate(Element);
            _dialog = new DatePickerDialog(_view.Context, (o, e) => 
            {
                AddDataPicker.SetDate(Element, e.Date);
                _command?.Execute(e.Date);
                _view.ClearFocus();
                _opened = false;
            }, date.Year, date.Month - 1, date.Day);
        }

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.ClearFocus();
            _opened = false;
        }

        void UpdateMaxDate()
        {
            if (_dialog != null)
            {
                _dialog.DatePicker.MaxDate = (long)AddDataPicker.GetMaxDate(Element)
                    .AddHours(23)
                    .AddMinutes(59)
                    .AddSeconds(59)
                    .ToUniversalTime()
                    .Subtract(DateTime.MinValue.AddYears(1969))
                    .TotalMilliseconds;
            }
        }

        void UpdateMinDate()
        {
            if (_dialog != null)
            {
                _dialog.DatePicker.MinDate = (long)AddDataPicker.GetMinDate(Element)
                    .ToUniversalTime()
                    .Subtract(DateTime.MinValue.AddYears(1969))
                    .TotalMilliseconds;
            }
        }

        void UpdateCommand()
        {
            _command = AddDataPicker.GetCommand(Element);
        }

        void UpdateTapCommand()
        {
            _tapCommand = AddDataPicker.GetTapCommand(Element);
        }
    }
}