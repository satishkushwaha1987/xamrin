using CHBackOffice.ApiServices.ChsProxy;
using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return null;
                var cashDisp = value as ArrayOfDispenserDispenser;
                double percent = (double)cashDisp.Count / cashDisp.Capacity;
                return percent;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return -1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
