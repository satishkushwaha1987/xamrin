using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class DateConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = DateTime.Now;
            string val = value?.ToString();
            if (val == null)
                return date;
            DateTime.TryParse(val, out date);
            return date;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
