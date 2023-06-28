using CHSBackOffice.Support;
using CHSBackOffice.Support.Interfaces;
using Plugin.CurrentActivity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace CHSBackOffice.Droid.InterfacesImplementation
{
    class SetToolbarColorImpl : ISetToolbarColor
    {
        private static Android.Support.V7.Widget.Toolbar GetToolbar() => (CrossCurrentActivity.Current?.Activity as MainActivity)?.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);

        public void SetToolbarColor(Color color)
        {
            try
            {
                if (color != null)
                {
                    var activity = (CrossCurrentActivity.Current?.Activity as MainActivity);
                    activity.Window.SetStatusBarColor(color.ToAndroid());

                    var tBar = GetToolbar();
                    if (tBar != null)
                        tBar.SetBackgroundColor(color.ToAndroid());
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }
    }
}