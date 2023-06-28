using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class SwipeContainer: ContentView
    {
        #region Bindable Properties
        public ICommand SwipeCommand
        {
            get { return (ICommand)GetValue(SwipeCommandProperty); }
            set { SetValue(SwipeCommandProperty, value); }
        }

        public static BindableProperty SwipeCommandProperty = BindableProperty.Create(nameof(SwipeCommand), typeof(ICommand), typeof(SwipeContainer), null);

        public ICommand SizePreparedCommand
        {
            get { return (ICommand)GetValue(SizePreparedCommandProperty); }
            set { SetValue(SizePreparedCommandProperty, value); }
        }

        public static BindableProperty SizePreparedCommandProperty = BindableProperty.Create(nameof(SizePreparedCommand), typeof(ICommand), typeof(SwipeContainer), null);

        public SwipeContainer()
        {
            GestureRecognizers.Add(GetSwipeGestureRecognizer(SwipeDirection.Left));
            GestureRecognizers.Add(GetSwipeGestureRecognizer(SwipeDirection.Right));
            GestureRecognizers.Add(GetSwipeGestureRecognizer(SwipeDirection.Down));
            GestureRecognizers.Add(GetSwipeGestureRecognizer(SwipeDirection.Up));
        }

        SwipeGestureRecognizer GetSwipeGestureRecognizer(SwipeDirection direction)
        {
            var swipe = new SwipeGestureRecognizer { Direction = direction };
            swipe.Swiped += OnSwipe;
            return swipe;
        }

        private void OnSwipe(object sender,SwipedEventArgs e)
        {
            SwipeCommand?.Execute(e.Direction.ToString());
            if (e.Direction == SwipeDirection.Right)
            {

            }

            if (e.Direction == SwipeDirection.Left)
            {


            }
        }

        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            SizePreparedCommand?.Execute(null);
            return base.OnSizeRequest(widthConstraint, heightConstraint);
        }

        #endregion
    }
}
