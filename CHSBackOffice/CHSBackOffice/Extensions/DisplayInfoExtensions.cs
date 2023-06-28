using System;
using Xamarin.Essentials;

namespace CHSBackOffice.Extensions
{
    public static class DisplayInfoExtensions
    {
        public static double WidthInDp(this DisplayInfo displayInfo)
        {
            return Math.Round(displayInfo.Width / displayInfo.Density);
        }

        public static double HeightInDp(this DisplayInfo displayInfo)
        {
            return Math.Round(displayInfo.Height / displayInfo.Density);
        }
    }
}
