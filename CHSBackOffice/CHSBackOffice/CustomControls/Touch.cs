using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class Touch : RoutingEffect
    {
        public Touch() : base("CHSBackOffice.CustomControls." + nameof(Touch)) { }
        public bool Capture { set; get; }
        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }
        public event TouchActionEventHandler TouchAction;

    }

    public delegate void TouchActionEventHandler(object sender, TouchActionEventArgs args);
    public class TouchActionEventArgs : EventArgs
    {
        public TouchActionEventArgs(long id, TouchActionType type, Point location, bool isInContact)
        {
            Id = id;
            Type = type;
            Location = location;
            IsInContact = isInContact;
        }

        public long Id { private set; get; }
        public TouchActionType Type { private set; get; }
        public Point Location { private set; get; }
        public bool IsInContact { private set; get; }
    }

    public enum TouchActionType
    {
        Entered,
        Pressed,
        Moved,
        Released,
        Exited,
        Cancelled
    }


}
