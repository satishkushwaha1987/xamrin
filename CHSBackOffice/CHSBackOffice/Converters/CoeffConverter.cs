using CHSBackOffice.Support;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class CoeffConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double val = (double)value;
                double coeff = Double.Parse((string)parameter);
                return val * coeff;
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
