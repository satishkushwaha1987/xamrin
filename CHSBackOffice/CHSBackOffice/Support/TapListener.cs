using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Support
{
    public class TapListener : PanGestureRecognizer
    {
        private ITapCallBack _tapCallBack;

        public TapListener(View view, ITapCallBack tapCallBack)
        {
            _tapCallBack = tapCallBack;
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnTapped;
            view.GestureRecognizers.Add(tapGesture);
        }

        private void OnTapped(object sender, EventArgs e)
        {
            View content = sender as View;
            _tapCallBack.OnTap(content);
        }
    }

    public interface ITapCallBack
    {
        void OnTap(View view);
    }
}
