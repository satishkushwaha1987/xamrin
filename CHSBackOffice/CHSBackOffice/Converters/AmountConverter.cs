
using CHSBackOffice.Support;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class AmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var val = (long)value;
                return 1.0 * val / 100;
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
