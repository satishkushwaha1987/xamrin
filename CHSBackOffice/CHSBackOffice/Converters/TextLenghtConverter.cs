using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Converters
{
    public class TextLenghtConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            int textLenght;
            Int32.TryParse((string)parameter, out textLenght);
            if (!string.IsNullOrEmpty(text))
            {
                if (text.Length > textLenght)
                {
                    return text.Substring(0, textLenght) + "...";
                }
                return text;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
