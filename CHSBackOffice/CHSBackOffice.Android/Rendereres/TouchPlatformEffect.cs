using Android.Views;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Droid.Rendereres;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportEffect(typeof(TouchPlatformEffect),nameof(Touch))]
namespace CHSBackOffice.Droid.Rendereres
{
    public class TouchPlatformEffect : PlatformEffect
    {
        Android.Views.View _view;
        Element _element;
        Touch _touch;
        bool capture;
        Func<double, double> fromPixels;
        int[] twoIntArray = new int[2];

        static Dictionary<Android.Views.View, TouchPlatformEffect> viewDictionary =
            new Dictionary<Android.Views.View, TouchPlatformEffect>();
        static Dictionary<int, TouchPlatformEffect> idToEffectDictionary =
            new Dictionary<int, TouchPlatformEffect>();

        protected override void OnAttached()
        {
            _view = Control ?? Container;

            Touch touch = (Touch)Element.Effects.FirstOrDefault(e => e is Touch);
            if (touch != null && _view != null)
            {
                viewDictionary.Add(_view, this);
                _element = Element;
                _touch = touch;
                fromPixels = _view.Context.FromPixels;
                _view.Touch += OnTouch;
            }
        }

        protected override void OnDetached()
        {
            if (viewDictionary.ContainsKey(_view))
            {
                viewDictionary.Remove(_view);
                _view.Touch -= OnTouch;
            }
        }

        private void OnTouch(object sender, Android.Views.View.TouchEventArgs e)
        {
            Android.Views.View senderView = sender as Android.Views.View;
            MotionEvent motionEvent = e.Event;

            int pointerIndex = motionEvent.ActionIndex;
            int id = motionEvent.GetPointerId(pointerIndex);

            senderView.GetLocationOnScreen(twoIntArray);
            Point screenPointerCoords = new Point(twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                 twoIntArray[1] + motionEvent.GetY(pointerIndex));

            switch (e.Event.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    FireEvent(this, id, TouchActionType.Pressed, screenPointerCoords, true);
                    idToEffectDictionary.Add(id, this);
                    capture = _touch.Capture;
                    break;
                case MotionEventActions.Move:
                    for (pointerIndex = 0; pointerIndex < motionEvent.PointerCount; pointerIndex++)
                    {
                        id = motionEvent.GetPointerId(pointerIndex);
                        if (capture)
                        {
                            senderView.GetLocationOnScreen(twoIntArray);
                            screenPointerCoords = new Point(twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                            twoIntArray[1] + motionEvent.GetY(pointerIndex));
                            FireEvent(this, id, TouchActionType.Moved, screenPointerCoords, true);
                        }
                        else
                        {
                            CheckForBoundaryHop(id, screenPointerCoords);
                            if (idToEffectDictionary[id] != null)
                            {
                                FireEvent(idToEffectDictionary[id], id, TouchActionType.Moved, screenPointerCoords, true);
                            }
                        }
                    }
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                    if (capture)
                    {
                        FireEvent(this, id, TouchActionType.Released, screenPointerCoords, false);
                    }
                    else
                    {
                        CheckForBoundaryHop(id, screenPointerCoords);
                        if (idToEffectDictionary[id] != null)
                        {
                            FireEvent(idToEffectDictionary[id], id, TouchActionType.Released, screenPointerCoords, false);
                        }
                    }
                    idToEffectDictionary.Remove(id);
                    break;
                case MotionEventActions.Cancel:
                    if (capture)
                    {
                        FireEvent(this, id, TouchActionType.Cancelled, screenPointerCoords, false);
                    }
                    else
                    {
                        if (idToEffectDictionary[id] != null)
                        {
                            FireEvent(idToEffectDictionary[id], id, TouchActionType.Cancelled, screenPointerCoords, false);
                        }
                    }
                    idToEffectDictionary.Remove(id);
                    break;
            }
        }

        void CheckForBoundaryHop(int id, Point pointerLocation)
        {
            TouchPlatformEffect touch = null;
            foreach (Android.Views.View view in viewDictionary.Keys)
            {
                try
                {
                    view.GetLocationOnScreen(twoIntArray);
                }
                catch
                {
                    continue;
                }
                Rectangle viewRect = new Rectangle(twoIntArray[0], twoIntArray[1], view.Width, view.Height);
                if (viewRect.Contains(pointerLocation))
                {
                    touch = viewDictionary[view];
                }
            }
            if (touch != idToEffectDictionary[id])
            {
                if (idToEffectDictionary[id] != null)
                {
                    FireEvent(idToEffectDictionary[id], id, TouchActionType.Exited, pointerLocation, true);
                }
                if (touch != null)
                {
                    FireEvent(touch, id, TouchActionType.Entered, pointerLocation, true);
                }
                idToEffectDictionary[id] = touch;
            }
        }

        void FireEvent(TouchPlatformEffect touch, int id, TouchActionType actionType, Point pointerLocation, bool isInContact)
        {
            Action<Element, TouchActionEventArgs> onTouchAction = touch._touch.OnTouchAction;

            touch._view.GetLocationOnScreen(twoIntArray);
            double x = pointerLocation.X - twoIntArray[0];
            double y = pointerLocation.Y - twoIntArray[1];
            Point point = new Point(fromPixels(x), fromPixels(y));

            onTouchAction(touch.Element, new TouchActionEventArgs(id, actionType, point, isInContact));

        }
    }
}