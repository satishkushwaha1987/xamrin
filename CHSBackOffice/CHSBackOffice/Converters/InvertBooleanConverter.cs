using CHSBackOffice.Support;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var b = (bool?)value;
                return b.HasValue && !b.Value;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
