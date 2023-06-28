using CHSBackOffice.iOS.Extensions;
using CHSBackOffice.CustomControls;
using CHSBackOffice.iOS.Rendereres;
using CHSBackOffice.Support;
using Foundation;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Page), typeof(CommonPageRenderer))]
namespace CHSBackOffice.iOS.Rendereres
{
    [Preserve(AllMembers = true)]
    class CommonPageRenderer : PageRenderer
    {
        const string menuTitle = "\ue62b ";
        const string menuWord = "menu";
        const string chsFontName = "chs-icons";
        const int chsFontSize = 35;
        Color menuIconColor = Color.White;

        Color naviBarColor;

        NSObject _keyboardShowObserver;
        NSObject _keyboardHideObserver;
        NSObject _winAppearObserver;
        NSObject _firstResponderObserver;

        private UIView activeView;
        private bool _pageWasShiftedUp;
        private double _activeViewBottom;

        private bool IsKeyboardShown
        {
            get => _isKeyboardShown;
            set
            {
                _isKeyboardShown = value;
            }
        }
        internal bool _isKeyboardShown;

        float _lastWidth = 0;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement is ExtendedNaviPage)
                naviBarColor = (e.NewElement as ExtendedNaviPage).NaviBarBackgroundColor;

            
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            //SetFont(chsFontName, chsFontSize, menuIconColor);
            SetNavigationBarColor();

            var element = Element as ContentPage;

            if (element != null)
            {
                var contentScrollView = element.Content as ScrollView;

                if (contentScrollView != null)
                    return;

                if (_winAppearObserver == null)
                    _winAppearObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidBecomeActiveNotification, WinBecomeActive);

                RegisterForFirstResponderNotification();
                RegisterForKeyboardNotifications();
            }
        }


        void SetNavigationBarColor()
        {
            if (NavigationController != null && NavigationController.NavigationBar != null && naviBarColor != null)
            {
                NavigationController.View.BackgroundColor = naviBarColor.ToUIColor();
                NavigationController.NavigationBar.BarTintColor = naviBarColor.ToUIColor();
                NavigationController.NavigationBar.BackgroundColor = naviBarColor.ToUIColor();
            }
        }

        void SetFont(string name, float size, Color foregroundColor)
        {
            var TitleAttr = new UITextAttributes
            {
                TextColor = foregroundColor.ToUIColor(),
                Font = UIFont.FromName(name, size),
            };
            
            if (NavigationController != null && NavigationController.TopViewController != null && NavigationController.TopViewController.NavigationItem != null)
            {
                
                var naviItem = this.NavigationController.TopViewController.NavigationItem;
                if (naviItem.LeftBarButtonItem != null && String.Compare(naviItem.LeftBarButtonItem.Title, menuWord, true) == 0)
                {
                    naviItem.LeftBarButtonItem.Title = menuTitle;
                    naviItem.LeftBarButtonItem.SetTitleTextAttributes(TitleAttr, UIControlState.Normal);
                    naviItem.LeftBarButtonItem.SetTitleTextAttributes(TitleAttr, UIControlState.Selected);
                }
            }
        }

        private void WinBecomeActive(NSNotification a_notification)
        {

        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            UnregisterForFirstResponderNotification();
            UnregisterForKeyboardNotifications();
        }

        void RegisterForFirstResponderNotification()
        {
            try
            {
                if (_firstResponderObserver == null)
                    _firstResponderObserver = NSNotificationCenter.DefaultCenter.AddObserver(UITextField.TextDidBeginEditingNotification, OnFirstResponder);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void UnregisterForFirstResponderNotification()
        {
            try
            {
                if (_firstResponderObserver != null)
                {
                    NSNotificationCenter.DefaultCenter.RemoveObserver(_firstResponderObserver);
                    _firstResponderObserver.Dispose();
                    _firstResponderObserver = null;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }

        }

        void RegisterForKeyboardNotifications()
        {
            try
            {
                if (_keyboardShowObserver == null)
                    _keyboardShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardShow);
                if (_keyboardHideObserver == null)
                    _keyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardHide);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void UnregisterForKeyboardNotifications()
        {
            try
            {
                IsKeyboardShown = false;
                if (_keyboardShowObserver != null)
                {
                    NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardShowObserver);
                    _keyboardShowObserver.Dispose();
                    _keyboardShowObserver = null;
                }

                if (_keyboardHideObserver != null)
                {
                    NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardHideObserver);
                    _keyboardHideObserver.Dispose();
                    _keyboardHideObserver = null;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        protected virtual void OnFirstResponder(NSNotification notification)
        {
            try
            {
                activeView = notification.Object as UIView;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        protected virtual void OnKeyboardShow(NSNotification notification)
        {
            try
            {
                if (!IsViewLoaded || _isKeyboardShown)
                    return;

                _isKeyboardShown = true;
                var activeView = View.FindFirstResponder();

                if (activeView == null)
                    return;

                var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
                var isOverlapping = activeView.IsKeyboardOverlapping(View, keyboardFrame);

                if (!isOverlapping)
                    return;

                if (isOverlapping)
                {
                    _activeViewBottom = activeView.GetViewRelativeBottom(View);
                    ShiftPageUp(keyboardFrame.Height, _activeViewBottom);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void OnKeyboardHide(NSNotification notification)
        {
            try
            {
                if (!IsViewLoaded)
                    return;

                _isKeyboardShown = false;
                var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);

                if (_pageWasShiftedUp)
                {
                    ShiftPageDown(keyboardFrame.Height, _activeViewBottom);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void ShiftPageUp(nfloat keyboardHeight, double activeViewBottom)
        {
            try
            {
                var pageFrame = Element.Bounds;

                var newY = pageFrame.Y + CalculateShiftByAmount(pageFrame.Height, keyboardHeight, activeViewBottom);

                Element.LayoutTo(new Rectangle(pageFrame.X, newY,
                    pageFrame.Width, pageFrame.Height));

                _pageWasShiftedUp = true;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void ShiftPageDown(nfloat keyboardHeight, double activeViewBottom)
        {
            try
            {
                var pageFrame = Element.Bounds;

                var newY = pageFrame.Y - CalculateShiftByAmount(pageFrame.Height, keyboardHeight, activeViewBottom);

                Element.LayoutTo(new Rectangle(pageFrame.X, newY,
                    pageFrame.Width, pageFrame.Height));

                _pageWasShiftedUp = false;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private double CalculateShiftByAmount(double pageHeight, nfloat keyboardHeight, double activeViewBottom)
        {
            return (pageHeight - activeViewBottom) - keyboardHeight;
        }
    }
}