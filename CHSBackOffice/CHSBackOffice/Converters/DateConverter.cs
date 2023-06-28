using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class DateConverter : IValueConverter
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
            var date = (DateTime)value;
            string res = date.ToString("dd/MM/yy");
            return res;
        }
    }
}
