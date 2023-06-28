using CHBackOffice.ApiServices.ChsProxy;
using System;
using Xamarin.Forms;

namespace CHSBackOffice.Support.Classes
{
    public class DispenserElementExtended : ListViewItemBase<ArrayOfDispenserDispenser>
    {
        #region Fields
        const string CurrencyLabel = "$";
        #endregion

        #region Public Properties
        public string Id => BaseObject.Id;
        public string Currency => BaseObject.Currency;
        public string DenomString => CurrencyLabel + (BaseObject.Denom * 1.0 / 100).ToString();
        public string RejectedValue => CurrencyLabel + ((BaseObject.RejectedCount * BaseObject.Denom * 1.0) / 100).ToString();
        public string RejectedUnit => BaseObject.RejectedCount.ToString();

        public string DispensedValue => CurrencyLabel + ((BaseObject.DispensedCount * BaseObject.Denom * 1.0) / 100).ToString();
        public string DispensedUnit => BaseObject.DispensedCount.ToString();

        public string RemainingValue => CurrencyLabel + ((BaseObject.Count * BaseObject.Denom * 1.0) / 100).ToString();
        public string RemainingUnit => BaseObject.Count.ToString();

        public double RemainingPercent => BaseObject.Count * 1.0 / BaseObject.Capacity;

        public Tuple<float, float, Func<float, float, Color>> DataSet
        {
            get 
            {
                return new Tuple<float, float, Func<float, float, Color>>
                    (BaseObject.Count, BaseObject.Capacity, ColorFunc);
            }
        }

        public Func<float, float, Color> ColorFunc = (count, capacity) => { return Color.FromHex("#5C9ECD"); };
        #endregion

    }
}
