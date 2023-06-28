namespace CHSBackOffice.Extensions
{
    public static class DoubleExtensions
    {
        public static string ToMoney(this double value)
        {
            return value.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-us"));
        }
    }
}
