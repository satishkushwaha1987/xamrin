using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class ActiveFloatDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try 
            {
                var val = (DateTime)value;
                var str = val.ToString();
                return DateTimeExtensions.DateTimeStrToNewFormat(str, "MM/dd/yyyy hh:mm tt");
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
