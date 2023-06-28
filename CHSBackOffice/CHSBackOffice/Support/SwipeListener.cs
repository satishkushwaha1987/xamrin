using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Support
{
    public class SwipeListener : PanGestureRecognizer
    {
        private ISwipeCallBack _swipeCallBack;
        private double translatedX = 0, translatedY = 0;

        public SwipeListener(View view, ISwipeCallBack swipeCallBack)
        {
            _swipeCallBack = swipeCallBack;
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            view.GestureRecognizers.Add(panGesture);
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            View content = sender as View;
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    try
                    {
                        translatedX = e.TotalX;
                        translatedY = e.TotalY;

                        if (translatedX < 0 && Math.Abs(translatedX) > Math.Abs(translatedY))
                        {
                            _swipeCallBack.OnLeftSwipe(content);
                        }
                        else if (translatedX > 0 && translatedX > Math.Abs(translatedY))
                        {
                            _swipeCallBack.OnRightSwipe(content);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionProcessor.ProcessException(ex);
                    }
                    break;
            }
        }
    }

    public interface ISwipeCallBack
    {
        void OnLeftSwipe(View view);
        void OnRightSwipe(View view);
    }
}
