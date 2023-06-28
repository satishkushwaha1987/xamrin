using System;
using System.Collections.Generic;
using System.Text;

namespace CHSBackOffice.Support.Interfaces
{
    public interface ISoftwareKeyboardService
    {
        event SoftwareKeyboardEventHandler Hide;

        event SoftwareKeyboardEventHandler Show;
    }

    public delegate void SoftwareKeyboardEventHandler(object sender, SoftwareKeyboardEventArgs args);

    public class SoftwareKeyboardEventArgs : EventArgs
    {
        public SoftwareKeyboardEventArgs(bool isVisible)
        {
            IsVisible = isVisible;
        }
        public bool IsVisible { get; private set; }
    }
}
