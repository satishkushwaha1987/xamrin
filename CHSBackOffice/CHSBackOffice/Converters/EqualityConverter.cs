using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Proxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class EqualityConverter : CHSBackOffice.Support.Proxy.IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var horizontalStack = parameter as Binding;
                var container = horizontalStack.Source as ExtendedStackLayout;
                int indexOf = container.IndexOf;
                if (values[0] == null || values[1] == null)
                    return false;
                var list = values[0] as List<KeyValuePair<string, string>>;
                if (list == null && list.Count < indexOf)
                    return false;
                return list[indexOf].Key.Equals(values[1]);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return false;
            }
        }
    }
}
