using CHSBackOffice.Models.ApexCharts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CHSBackOffice.Support.ApexChart.DataMappers.ToSeries
{
    public class StringMultyArrayToSeriesMapper : IDataToSeriesMapper
    {
        readonly string[][] _array;

        #region ".CTOR"

        public StringMultyArrayToSeriesMapper(string[][] array)
        {
            _array = array;
        }

        #endregion

        #region "IDataToSeriesMapper implementation"

        public ApexChartConfigSeries[] Map()
        {
            return new ApexChartConfigSeries[]
            {
                new ApexChartConfigSeries
                {
                    Name = "Value",
                    Data = GetData().ToArray()
                }
            };
        }

        #endregion

        #region "PRIVATE METHODS"

        private IEnumerable<object> GetData()
        {
            return _array
                .Select(s => Double.Parse(s[1], CultureInfo.GetCultureInfo("en-US")))
                .Cast<object>();
        }

        #endregion
    }
}
