using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class GestureGrid : Grid
    {
        public event EventHandler SwipeLeft;
        public event EventHandler SwipeRight;

        public GestureGrid()
        {
            GestureRecognizers.Add(GetSwipeGestureRecognizer(SwipeDirection.Left));
            GestureRecognizers.Add(GetSwipeGestureRecognizer(SwipeDirection.Right));
        }

        SwipeGestureRecognizer GetSwipeGestureRecognizer(SwipeDirection direction)
        {
            var swipe = new SwipeGestureRecognizer { Direction = direction };
            swipe.Swiped += OnSwipe;
            return swipe;
        }

        private void OnSwipe(object sender, SwipedEventArgs e)
        {
            if (e.Direction == SwipeDirection.Right)
            {
                SwipeRight?.Invoke(this, null);
            }

            if (e.Direction == SwipeDirection.Left)
            {
                SwipeLeft?.Invoke(this, null);

            }
        }

        public void OnSwipeLeft() =>
            SwipeLeft?.Invoke(this, null);

        public void OnSwipeRight() =>
            SwipeRight?.Invoke(this, null);
    }
}
