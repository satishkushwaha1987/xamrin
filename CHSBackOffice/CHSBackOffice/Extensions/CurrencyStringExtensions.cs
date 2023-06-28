using System.Globalization;

namespace CHSBackOffice.Extensions
{
    public static class CurrencyStringExtensions
    {
        public static double ToDouble(this string currencyString)
        {
            return double.Parse(currencyString, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
        }
    }
}
