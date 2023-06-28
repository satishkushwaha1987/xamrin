using System;

namespace CHSBackOffice.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateTimeStr(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return String.Empty;

            return dateTime.Value.ToDateTimeStr();
        }

        public static string ToDateTimeStr(this DateTime dateTime)
        {
            if (dateTime < DateTime.UtcNow.AddYears(-50))
                return String.Empty;

            return dateTime.ToString("MM/dd/yyyy hh:mm:ss tt");
        }
    }
}
