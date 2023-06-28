using CHBackOffice.ApiServices.ChsProxy;
using System;
using Xamarin.Forms;

namespace CHSBackOffice.Support.Classes
{
    public class RecyclerExtended : ListViewItemBase<Recycler>
    {
        #region Fields
        const string CurrencyLabel = "$";
        #endregion

        #region Public Properties
        public string Id => (Number+1).ToString();
        public string Currency => BaseObject.CurrencyInRecycler;
        public string DenomString => CurrencyLabel + (BaseObject.MediaDenominationValue * 1.0 / 100).ToString();
        public string RejectedValue => CurrencyLabel + ((BaseObject.MediaDenominationValue * BaseObject.MediaRejectedUnit * 1.0) / 100).ToString();
        public string RejectedUnit => BaseObject.MediaRejectedUnit.ToString();

        public string DispensedValue => CurrencyLabel + ((BaseObject.MediaDenominationValue * BaseObject.MediaDispensedUnit * 1.0) / 100).ToString();
        public string DispensedUnit => BaseObject.MediaDispensedUnit.ToString();

        public string RemainingValue => CurrencyLabel + ((BaseObject.MediaDenominationValue * BaseObject.MediaRemainingUnit * 1.0) / 100).ToString();
        public string RemainingUnit => BaseObject.MediaRemainingUnit.ToString();

        public double RemainingPercent => BaseObject.MediaRemainingUnit * 1.0 / BaseObject.Capacity;

        public Tuple<float, float, Func<float, float, Color>> DataSet
        {
            get
            {
                return new Tuple<float, float, Func<float, float, Color>>
                    (BaseObject.MediaRemainingUnit, BaseObject.Capacity, ColorFunc);
            }
        }

        public Func<float, float, Color> ColorFunc = (count, capacity) => { return Color.FromHex("#5C9ECD"); };
        #endregion
    }
}
