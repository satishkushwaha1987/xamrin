using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwitchTap : ContentView
    {
        #region Bindable Property
        public bool IsToggled
        {
            get { return (bool)GetValue(IsToggledProperty); }
            set { SetValue(IsToggledProperty, value); }
        }

        public static readonly BindableProperty IsToggledProperty =
            BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(SwitchTap), false, propertyChanged: OnToggledChanged);

        private static void OnToggledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var root = bindable as SwitchTap;
            root?.UpdateColors();
            root?.Toggled?.Invoke(root, new ToggledEventArgs((bool)newValue));
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SwitchTap), Color.White, propertyChanged: OnColorChanged);

        private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var root = bindable as SwitchTap;
            root?.UpdateColors();
        }

        public Color MainBackgroundColor
        {
            get { return (Color)GetValue(MainBackgroundColorProperty); }
            set { SetValue(MainBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty MainBackgroundColorProperty =
            BindableProperty.Create(nameof(MainBackgroundColor), typeof(Color), typeof(SwitchTap), Color.FromHex("#6CC435"), propertyChanged: OnColorChanged);

        public Color ActiveBackgroundColor
        {
            get { return (Color)GetValue(ActiveBackgroundColorProperty); }
            set { SetValue(ActiveBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty ActiveBackgroundColorProperty =
            BindableProperty.Create(nameof(ActiveBackgroundColor), typeof(Color), typeof(SwitchTap), Color.Transparent, propertyChanged: OnColorChanged);

        public Color InactiveBackgroundColor
        {
            get { return (Color)GetValue(InactiveBackgroundColorProperty); }
            set { SetValue(InactiveBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty InactiveBackgroundColorProperty =
            BindableProperty.Create(nameof(InactiveBackgroundColor), typeof(Color), typeof(SwitchTap), Color.FromHex("#66D8EBC5"), propertyChanged: OnColorChanged);


        public string RightText
        {
            get { return (string)GetValue(RightTextProperty); }
            set { SetValue(RightTextProperty, value); }
        }

        public static readonly BindableProperty RightTextProperty =
            BindableProperty.Create(nameof(RightText), typeof(string), typeof(SwitchTap), string.Empty, propertyChanged: OnRightTextChanged);

        private static void OnRightTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var root = bindable as SwitchTap;
            root.RightLabel.Text = (string)newValue;
        }

        public string LeftText
        {
            get { return (string)GetValue(LeftTextProperty); }
            set { SetValue(LeftTextProperty, value); }
        }

        public static readonly BindableProperty LeftTextProperty =
            BindableProperty.Create(nameof(LeftText), typeof(string), typeof(SwitchTap), string.Empty, propertyChanged: OnLeftTextChanged);

        private static void OnLeftTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var root = bindable as SwitchTap;
            root.LeftLabel.Text = (string)newValue;
        }
        #endregion

        #region .CTOR
        public SwitchTap()
        {
            try
            {
                InitializeComponent();
                ExtendedNaviPage.OnOrientartionChanged += ExtendedNaviPage_OnOrientartionChanged;
                UpdateColors();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region Methods
        private void OnLeftTap(object sender, EventArgs e)
        {
            IsToggled = true;
        }

        private void OnRightTap(object sender, EventArgs e)
        {
            IsToggled = false;
        }

        private void UpdateColors()
        {
            LeftLabel.TextColor = TextColor;
            RightLabel.TextColor = TextColor;
            if (IsToggled)
            {
                Container.BackgroundColor = MainBackgroundColor;
                LeftInnerContainer.BackgroundColor = ActiveBackgroundColor;
                RightInnerContainer.BackgroundColor = InactiveBackgroundColor;
            }
            else
            {
                Container.BackgroundColor = MainBackgroundColor;
                LeftInnerContainer.BackgroundColor = InactiveBackgroundColor;
                RightInnerContainer.BackgroundColor = ActiveBackgroundColor;
            }
        }

        #endregion

        #region "Event Handling"
        public event EventHandler<ToggledEventArgs> Toggled;

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            if (e.Direction == SwipeDirection.Left && !IsToggled)
                IsToggled = true;
            else if (e.Direction == SwipeDirection.Right && IsToggled)
                IsToggled = false;
        }

        private void ExtendedNaviPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
        }

        #endregion
    }
}