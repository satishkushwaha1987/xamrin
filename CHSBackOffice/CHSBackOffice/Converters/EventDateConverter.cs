
using CHSBackOffice.Support;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class EventDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var val = (string)value;
                return DateTimeExtensions.DateTimeStrToNewFormat(val, "MM/dd/yyyy hh:mm tt");
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
