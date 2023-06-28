using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CHSBackOffice.CustomControls;
using CHSBackOffice.iOS.Rendereres;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(AddTimePickerPlatformEffect), nameof(AddTimePicker))]
namespace CHSBackOffice.iOS.Rendereres
{
    public class AddTimePickerPlatformEffect : PlatformEffect
    {
        UIView _view;
        UIDatePicker _picker;
        NoCaretField _entry;
        UILabel _title;
        ICommand _command;
        ICommand _tapCommand;
        NSDate _preSelectedDate;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            CreatePicker();
            UpdateTime();
            UpdateTitle();
            UpdateCommand();
            UpdateTapCommand();
        }

        protected override void OnDetached()
        {
            _entry.RemoveFromSuperview();
            _entry.Dispose();
            _title.Dispose();
            _picker.Dispose();
            _preSelectedDate.Dispose();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == AddTimePicker.TimeProperty.PropertyName)
            {
                UpdateTime();
            }
            else if (e.PropertyName == AddTimePicker.TitleProperty.PropertyName)
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

        void CreatePicker()
        {
            _entry = new NoCaretField();
            _entry.BorderStyle = UITextBorderStyle.None;
            _entry.BackgroundColor = UIColor.Clear;
            _view.AddSubview(_entry);

            _entry.TranslatesAutoresizingMaskIntoConstraints = false;

            _entry.TopAnchor.ConstraintEqualTo(_view.TopAnchor).Active = true;
            _entry.LeftAnchor.ConstraintEqualTo(_view.LeftAnchor).Active = true;
            _entry.RightAnchor.ConstraintEqualTo(_view.RightAnchor).Active = true;
            _entry.BottomAnchor.ConstraintEqualTo(_view.BottomAnchor).Active = true;
            _entry.WidthAnchor.ConstraintEqualTo(_view.WidthAnchor).Active = true;
            _entry.HeightAnchor.ConstraintEqualTo(_view.HeightAnchor).Active = true;

            _view.UserInteractionEnabled = true;
            _view.SendSubviewToBack(_entry);

            _picker = new UIDatePicker { Mode = UIDatePickerMode.Time, TimeZone = new Foundation.NSTimeZone("UTC") };
            _title = new UILabel();

            var width = UIScreen.MainScreen.Bounds.Width;
            var toolbar = new UIToolbar(new CGRect(0, 0, (float)width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };

            var textColor = UIColor.FromRGB(58, 118, 240);
            var flexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, (o, e) => { });
            var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, e) => 
            {
                _entry.ResignFirstResponder();
                _picker.Date = _preSelectedDate;
            });
            cancelButton.SetTitleTextAttributes(new UITextAttributes { TextColor = textColor }, UIControlState.Normal);

            //var labelButton = new UIBarButtonItem(_title);
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, e) => 
            {
                _entry.ResignFirstResponder();
                DoneTime();
                _command?.Execute(_picker.Date.ToDateTime() - new DateTime(1, 1, 1));
            });
            doneButton.SetTitleTextAttributes(new UITextAttributes { TextColor = textColor }, UIControlState.Normal);

            toolbar.SetItems(new[] { flexibleSpace, cancelButton, doneButton }, false);
            _entry.InputView = _picker;
            _entry.InputAccessoryView = toolbar;
            _entry.EditingDidBegin += (o, e) =>
            {
                _tapCommand?.Execute(null);
            };
        }

        void DoneTime()
        {
            var time = _picker.Date.ToDateTime() - new DateTime(1, 1, 1);
            AddTimePicker.SetTime(Element, time);
            _preSelectedDate = _picker.Date;
        }

        void UpdateTime()
        {
            var time = AddTimePicker.GetTime(Element);
            _picker.Date = new DateTime(1, 1, 1).Add(time).ToNSDate();
            _preSelectedDate = _picker.Date;
        }

        void UpdateTitle()
        {
            _title.Text = AddTimePicker.GetTitle(Element);
            _title.SizeToFit();
        }

        void UpdateCommand()
        {
            _command = AddTimePicker.GetCommand(Element);
        }

        void UpdateTapCommand()
        {
            _tapCommand = AddDataPicker.GetTapCommand(Element);
        }
    }
}