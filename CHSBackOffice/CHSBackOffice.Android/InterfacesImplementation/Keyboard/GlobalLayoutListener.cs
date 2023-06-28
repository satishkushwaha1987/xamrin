using System;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Interfaces;
using Plugin.CurrentActivity;

namespace CHSBackOffice.Droid.InterfacesImplementation.Keyboard
{
    internal class GlobalLayoutListener : Java.Lang.Object,ViewTreeObserver.IOnGlobalLayoutListener
    {
        private static InputMethodManager _inputManager;
        private readonly SoftwareKeyboardService _softwareKeyboardService;

        private static void ObtainInputManager()
        {
            try 
            {
                _inputManager = (InputMethodManager)(CrossCurrentActivity.Current?.Activity as MainActivity)
                .GetSystemService(Context.InputMethodService);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        public GlobalLayoutListener(SoftwareKeyboardService softwareKeyboardService)
        {
            _softwareKeyboardService = softwareKeyboardService;
            ObtainInputManager();
        }

        public void OnGlobalLayout()
        {
            if (_inputManager == null)
                return;
            if (_inputManager.Handle == IntPtr.Zero)
            {
                ObtainInputManager();
            }
            if (_inputManager.IsAcceptingText)
            {
                _softwareKeyboardService.InvokeKeyboardShow(new SoftwareKeyboardEventArgs(true));
            }
            else
            {
                _softwareKeyboardService.InvokeKeyboardHide(new SoftwareKeyboardEventArgs(false));
            }
        }

        public new void Dispose()
        {
            try 
            {
                _inputManager.Dispose();
                _inputManager = null;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }
    }
}