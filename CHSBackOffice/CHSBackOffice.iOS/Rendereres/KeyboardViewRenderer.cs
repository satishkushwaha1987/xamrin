using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CHSBackOffice.CustomControls;
using CHSBackOffice.iOS.Extensions;
using CHSBackOffice.iOS.Rendereres;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Interfaces;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Prism.DryIoc;
using Prism.Ioc;

[assembly:ExportRenderer(typeof(KeyboardView),typeof(KeyboardViewRenderer))]
namespace CHSBackOffice.iOS.Rendereres
{
    [Preserve(AllMembers = true)]
    public class KeyboardViewRenderer : ViewRenderer
    {//I used UITableView for showing the menulist of secondary toolbar items.
        List<ToolbarItem> _secondaryItems;
        UITableView table;

        NSObject _keyboardShowObserver;
        NSObject _keyboardHideObserver;

        SoftwareKeyboardService _softwareKeyboardService;
        private static IContainerProvider Container
        {
            get => ((Prism.DryIoc.PrismApplication)App.Current).Container;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                RegisterForKeyboardNotifications();
            }

            if (e.OldElement != null)
            {
                UnregisterForKeyboardNotifications();
            }

            
        }

        void RegisterForKeyboardNotifications()
        {
            try 
            {
                _softwareKeyboardService = (SoftwareKeyboardService)Container.Resolve<ISoftwareKeyboardService>();
                if (_keyboardShowObserver == null)
                {
                    _keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
                }
                if (_keyboardHideObserver == null)
                {
                    _keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {
            try 
            {
                NSValue result = (NSValue)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
                CGSize keyboardSize = result.RectangleFValue.Size;
                if (Element != null)
                {
                    Element.Margin = new Thickness(0, 0, 0, keyboardSize.Height);
                }

                _softwareKeyboardService.InvokeKeyboardShow(new SoftwareKeyboardEventArgs(true));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                Element.Margin = new Thickness(0);
            }
            _softwareKeyboardService.InvokeKeyboardHide(new SoftwareKeyboardEventArgs(false));
        }

        void UnregisterForKeyboardNotifications()
        {
            try 
            {
                if (_keyboardShowObserver != null)
                {
                    _keyboardShowObserver.Dispose();
                    _keyboardShowObserver = null;
                }

                if (_keyboardHideObserver != null)
                {
                    _keyboardHideObserver.Dispose();
                    _keyboardHideObserver = null;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

    }
}