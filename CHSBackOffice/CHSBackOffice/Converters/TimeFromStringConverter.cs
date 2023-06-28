using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class TimeFromStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                TimeSpan time = DateTime.Now.TimeOfDay;
                string val = value?.ToString();
                if (val == null)
                    return time;
                TimeSpan.TryParse(val, out time);
                return time;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return DateTime.Now.TimeOfDay;
            }            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var time = (TimeSpan)value;
                string res = time.ToString();
                return res;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return DateTime.Now.TimeOfDay.ToString();
            }
        }
    }
}
