using System;
using System.Collections.Generic;
using System.Text;

namespace CHSBackOffice.Support.Interfaces
{
    public interface IOrientationHandler
    {
        void ForceLandscape();
        void ForcePortrait();
        void ForceUnspecified();
    }
}
