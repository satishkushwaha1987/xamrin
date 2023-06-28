using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Support.Interfaces
{
    public interface ILocationFetcher
    {
        System.Drawing.PointF GetCoordinates(global::Xamarin.Forms.VisualElement view);
    }
}
