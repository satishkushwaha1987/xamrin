
using CHSBackOffice.Support;
using Plugin.CurrentActivity;

namespace CHSBackOffice.Droid.InterfacesImplementation.Keyboard
{
    public class SoftwareKeyboardService : SoftwareKeyboardServiceBase
    {
        public SoftwareKeyboardService()
        {
            CheckListener();
        }

        private GlobalLayoutListener _globalLayoutListener;

        private void CheckListener()
        {
            if (_globalLayoutListener == null)
            {
                _globalLayoutListener = new GlobalLayoutListener(this);
                var activity = (CrossCurrentActivity.Current?.Activity as MainActivity);
                activity.Window.DecorView.ViewTreeObserver.AddOnGlobalLayoutListener(_globalLayoutListener);
            }
        }
    }
}