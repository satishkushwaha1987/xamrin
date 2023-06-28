
using Android.App;
using Android.Text.Format;
using Android.Widget;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Droid.Rendereres;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportEffect(typeof(AddTimePickerPlatformEffect),nameof(AddTimePicker))]
namespace CHSBackOffice.Droid.Rendereres
{
    public class AddTimePickerPlatformEffect : PlatformEffect
    {
        Android.Views.View _view { get; set; }
        TimePickerDialog _dialog;
        ICommand _command;
        ICommand _tapCommand;
        string _title;

        protected override void OnAttached()
        {
            _view = Control ?? Container;
            _view.Clickable = true;
            _view.Touch += OnTouchView;
            //_view.SetOnTouchListener(new MyOnTouchListener());
            var effect = (Element.Effects.FirstOrDefault(e => e is Touch));
            if (effect is Touch)
                (effect as Touch).TouchAction += OnShowDialog;

            UpdateTitle();
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

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == AddTimePicker.TitleProperty.PropertyName)
            {
                UpdateTitle();
            }
            else if (e.PropertyName == AddTimePicker.CommandProperty.PropertyName)
            {
                UpdateCommand();
            }
            else if (e.PropertyName == AddDataPicker.TapCommandProperty.PropertyName)
            {
                UpdateTapCommand();
            }
        }


        void OnTouchView(object sender, Android.Views.View.TouchEventArgs e)
        {
            if (e.Event.Action == Android.Views.MotionEventActions.Move)
            {
                return;
            }
            _tapCommand?.Execute(null);
            CreateDialog();
        }

        void OnShowDialog(object sender, TouchActionEventArgs e)
        {
            if (e.Type == TouchActionType.Pressed)
            {
                CreateDialog();
            }
        }

        void CreateDialog()
        {
            var time = AddTimePicker.GetTime(Element);
            if (_dialog == null)
            {
                bool is24HourFormat = DateFormat.Is24HourFormat(_view.Context);
                _dialog = new TimePickerDialog(_view.Context, TimeSelected, time.Hours, time.Minutes, is24HourFormat);

                var title = new TextView(_view.Context);
                if (!string.IsNullOrEmpty(_title))
                {
                    title.Gravity = Android.Views.GravityFlags.Center;
                    title.SetPadding(10, 10, 10, 10);
                    title.Text = _title;
                    _dialog.SetCustomTitle(title);
                }

                _dialog.SetCanceledOnTouchOutside(true);

                _dialog.DismissEvent += (s, e) => 
                {
                    title.Dispose();
                    _dialog.Dispose();
                    _dialog = null;
                };
                _dialog.Show();
            }
        }

        void TimeSelected(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            var time = new TimeSpan(e.HourOfDay, e.Minute, 0);
            AddTimePicker.SetTime(Element, time);
            _command?.Execute(time);
        }

        void UpdateTitle()
        {
            _title = AddTimePicker.GetTitle(Element);
        }

        void UpdateCommand()
        {
            _command = AddTimePicker.GetCommand(Element);
        }

        void UpdateTapCommand()
        {
            _tapCommand = AddTimePicker.GetTapCommand(Element);
        }

    }
}