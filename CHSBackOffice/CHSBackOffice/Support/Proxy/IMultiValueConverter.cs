using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CHSBackOffice.Support.Proxy
{
    public interface IMultiValueConverter
    {
        object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
    }
}
