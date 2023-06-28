using System;

namespace CHSBackOffice.Support
{
    public static class DateTimeExtensions
    {
        public static string DateTimeStrToNewFormat(string dateString, string format)
        {
            if (String.IsNullOrWhiteSpace(format))
                return dateString;

            var dateTimeArr = dateString.Trim().Split(' ');
            if (dateTimeArr.Length == 2)
            {
                var dateArr = dateTimeArr[0].Split(new char[] { '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
                var timeArr = dateTimeArr[1].Split(':');

                var day = int.Parse(dateArr[0]);
                var month = int.Parse(dateArr[1]);
                var year = int.Parse($"{20}{dateArr[2]}");

                var hour = int.Parse(timeArr[0]);
                var minute = int.Parse(timeArr[1]);
                var second = int.Parse(timeArr[2]);

                var dateTime = new DateTime(year, month, day, hour, minute, second);

                return dateTime.ToString(format);
            }

            return dateString;
        }
    }
}
