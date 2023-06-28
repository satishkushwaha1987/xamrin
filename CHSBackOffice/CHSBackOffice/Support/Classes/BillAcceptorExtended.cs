using CHBackOffice.ApiServices.ChsProxy;
using System;
using Xamarin.Forms;

namespace CHSBackOffice.Support.Classes
{
    public class BillAcceptorExtended : ListViewItemBase<ArrayOfAcceptorAcceptor>
    {
        public string Id => BaseObject.Id;
        public string BatchNumber => BaseObject.BatchNumber;
        public long BillsCount => BaseObject.BillsCount;
        public long TicketCount => BaseObject.TicketCount;
        public long TotalCount => BillsCount + TicketCount;
        public long Capacity => BaseObject.Capacity;

        public double RemainingPercent => TotalCount * 1.0 / BaseObject.Capacity;

        public Tuple<float, float, Func<float, float, Color>> DataSet
        {
            get 
            {
                return new Tuple<float, float, Func<float, float, Color>>
                    (TotalCount, BaseObject.Capacity, ColorFunc);
            }
        }

        public Func<float, float, Color> ColorFunc = (count, capacity) => { return Color.FromHex("#5C9ECD"); };
    }
}
