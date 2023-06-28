using System.Linq;

namespace CHSBackOffice.Support.ApexChart.DataMappers.ToCategories
{
    public class StringMultyArrayToCategoriesMapper : IDataToCategoriesMapper
    {
        readonly string[][] _array;

        #region ".CTOR"

        public StringMultyArrayToCategoriesMapper(string[][] array)
        {
            _array = array;
        }

        #endregion

        #region "IDataToSeriesMapper implementation"

        public string[] Map()
        {
            return _array.Select(s => s[0]).ToArray();
        }

        #endregion
    }
}
