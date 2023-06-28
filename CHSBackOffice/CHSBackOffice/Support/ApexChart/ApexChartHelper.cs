using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace CHSBackOffice.Support.ApexChart
{
    public sealed class ApexChartHelper
    {
        public static string[] WeekDays => new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        public static int GetOptimalColumnCount(DisplayOrientation displayOrientation)
        {

            if (Xamarin.Forms.Device.Idiom == Xamarin.Forms.TargetIdiom.Phone)
                return displayOrientation == DisplayOrientation.Portrait ? 6 : 10;

            if (displayOrientation == DisplayOrientation.Portrait)
                return 10;

            return 14;
        }

        public static string GetColumnWidthByColumnCount(int columnCount, DisplayOrientation displayOrientation)
        {
            if (columnCount == 0)
                return null;

            var optimalColumnCount = GetOptimalColumnCount(displayOrientation);
            if (columnCount >= optimalColumnCount)
                return null;

            return $"{Math.Floor((decimal)(100/optimalColumnCount-2)*columnCount)}%";
        }

        public static string GetLabelsFontSize()
        {
            if (Xamarin.Forms.Device.Idiom == Xamarin.Forms.TargetIdiom.Phone)
                return "9px";

            return "12px";
        }

        public static string GetMonthAsStr(string monthNumber, DateTime lastMonthPrevYear)
        {
            return lastMonthPrevYear.AddMonths(Convert.ToInt32(monthNumber)).ToString("MMM yyyy",
                System.Globalization.CultureInfo.GetCultureInfo("en-us"));
        }


        public static string[] GetYearLabels(DateTime utcNow)
        {
            var yearLabels = new List<string>();
            for (int i = 11; i >= 0; i--)
            {
                yearLabels.Add(utcNow.AddMonths(-i).ToString("MMM yyyy",
                       System.Globalization.CultureInfo.GetCultureInfo("en-us")));
            }
            return yearLabels.ToArray();
        }

        public static string[] GetCurrentYearLabels(DateTime utcNow)
        {
            var date = new DateTime(utcNow.Year, 1, 1);
            var yearLabels = new List<string>();
            while (date <= utcNow.Date)
            {
                yearLabels.Add(date.ToString("MMM yyyy",
                       System.Globalization.CultureInfo.GetCultureInfo("en-us")));
                date.AddMonths(1);
            }
            return yearLabels.ToArray();
        }

        public static string[] GetMonthLabels(DateTime utcNow)
        {
            var monthLabels = new List<string>();
            var curDate = utcNow.AddDays(-1);
            var monthAgoDate = curDate.AddMonths(-1);
            while (curDate >= monthAgoDate)
            {
                monthLabels.Add(curDate.ToString("MMM dd", System.Globalization.CultureInfo.GetCultureInfo("en-us")));
                curDate = curDate.AddDays(-1);
            }
            monthLabels.Reverse();

            return monthLabels.ToArray();
        }

        public static string[] GetWeekLabels()
        {
            return new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        }

        public static string[] GetDayLabels()
        {
            var hours = new List<string>();
            for (int i = 0; i < 24; i++)
            {
                hours.Add(i.ToString());
            }
            return hours.ToArray();
        }
    }
}
