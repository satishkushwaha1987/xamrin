using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class HalfValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var val = (double)value;
                return val / 2;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
