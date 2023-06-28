using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using CHSBackOffice.CustomControls;
using CHSBackOffice.iOS.Rendereres;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ResolutionGroupName("CHSBackOffice.CustomControls")]
[assembly:ExportEffect(typeof(AddDatePickerPlatformEffect),nameof(AddDataPicker))]
namespace CHSBackOffice.iOS.Rendereres
{
    public class AddDatePickerPlatformEffect : PlatformEffect
    {
        UIView _view;
        UIDatePicker _picker;
        NoCaretField _entry;
        ICommand _command;
        ICommand _tapCommand;
        NSDate _preSelectedDate;

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            UpdateTapCommand();
            CreatePicker();
            UpdateDate();
            UpdateMaxDate();
            UpdateMinDate();
            UpdateCommand(); 
        }

        protected override void OnDetached()
        {
            _entry.RemoveFromSuperview();
            _entry.Dispose();
            _picker.Dispose();
            _preSelectedDate.Dispose();
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
            _entry.BottomAnchor.ConstraintEqualTo(_view.BottomAnchor).Active = true;
            _entry.RightAnchor.ConstraintEqualTo(_view.RightAnchor).Active = true;
            _entry.WidthAnchor.ConstraintEqualTo(_view.WidthAnchor).Active = true;
            _entry.HeightAnchor.ConstraintEqualTo(_view.HeightAnchor).Active = true;

            _view.UserInteractionEnabled = true;
            _view.SendSubviewToBack(_entry);

            _picker = new UIDatePicker { Mode = UIDatePickerMode.Date, TimeZone = new Foundation.NSTimeZone("UTC") };
            var todayText = AddDataPicker.GetTodayText(Element);

            var width = UIScreen.MainScreen.Bounds.Width;
            var toolbar = new UIToolbar(new CGRect(0, 0, (float)width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };

            var textColor = UIColor.FromRGB(58, 118, 240);
            var flexibleSpace = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, (o, e) => { });
            var cancelButton = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (o, e) => 
            {
                _entry.ResignFirstResponder();
                _picker.Date = _preSelectedDate;
            });
            cancelButton.SetTitleTextAttributes(new UITextAttributes {  TextColor = textColor }, UIControlState.Normal);

            var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace);
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, e) => 
            {
                _entry.ResignFirstResponder();
                DoneDate();
                _command?.Execute(_picker.Date.ToDateTime().Date);
            });
            doneButton.SetTitleTextAttributes(new UITextAttributes { TextColor = textColor }, UIControlState.Normal);

            if (!string.IsNullOrEmpty(todayText))
            {
                var labelButton = new UIBarButtonItem(todayText, UIBarButtonItemStyle.Plain, (o, e) =>
                {
                    SetToday();
                });
                labelButton.SetTitleTextAttributes(new UITextAttributes { TextColor = textColor }, UIControlState.Normal);
                var fixspacer = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = 20 };
                toolbar.SetItems(new[] { flexibleSpace, cancelButton, spacer, labelButton, fixspacer, doneButton }, false);
            }
            else
            {
                toolbar.SetItems(new[] { flexibleSpace, cancelButton, spacer, doneButton }, false);
            }

            _entry.InputView = _picker;
            _entry.InputAccessoryView = toolbar;
            _entry.EditingDidBegin += (o,e)=> 
            {
                _tapCommand?.Execute(null);
            };
            
            // Hide unnecessary Undo, Redo, Paste buttons and autoinsert toolbar above datepicker on IPad.
            _entry.InputAssistantItem.LeadingBarButtonGroups = Array.Empty<UIBarButtonItemGroup>();
            _entry.InputAssistantItem.TrailingBarButtonGroups = Array.Empty<UIBarButtonItemGroup>();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == AddDataPicker.DateProperty.PropertyName)
            {
                UpdateDate();
            }
            else if (e.PropertyName == AddDataPicker.CommandProperty.PropertyName)
            {
                UpdateCommand();
            }
            else if (e.PropertyName == AddDataPicker.TapCommandProperty.PropertyName)
            {
                UpdateTapCommand();
            }
            else if (e.PropertyName == AddDataPicker.MaxDateProperty.PropertyName)
            {
                UpdateMaxDate();
            }
            else if (e.PropertyName == AddDataPicker.MinDateProperty.PropertyName)
            {
                UpdateMinDate();
            }
        }

        void DoneDate()
        {
            var date = _picker.Date.ToDateTime().Date;
            AddDataPicker.SetDate(Element, date);
            _preSelectedDate = _picker.Date;
        }

        void SetToday()
        {
            if (_picker.MinimumDate.ToDateTime() <= DateTime.Today && _picker.MaximumDate.ToDateTime() >= DateTime.Today)
            {
                _picker.SetDate(DateTime.Today.ToNSDate(), true);
            }
        }

        void UpdateDate()
        {
            var date = AddDataPicker.GetDate(Element).ToNSDate();
            _picker.SetDate(date, false);
            _preSelectedDate = date;
        }

        void UpdateCommand()
        {
            _command = AddDataPicker.GetCommand(Element);
        }

        void UpdateTapCommand()
        {
            _tapCommand = AddDataPicker.GetTapCommand(Element);
        }

        void UpdateMaxDate()
        {
            _picker.MaximumDate = AddDataPicker.GetMaxDate(Element).ToNSDate();
        }

        void UpdateMinDate()
        {
            _picker.MinimumDate = AddDataPicker.GetMinDate(Element).ToNSDate();
        }

    }

    internal class NoCaretField : UITextField
    {
        public NoCaretField() : base(new CGRect()) { }
        public override CGRect GetCaretRectForPosition(UITextPosition position)
        {
            return new CGRect();
        }
    }
}