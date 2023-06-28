using CHBackOffice.ApiServices.ChsProxy;

namespace CHSBackOffice.Support.Classes
{
    public class TransactionsExtended : ListViewItemBase<PatronTransaction>
    {

        public string Type => BaseObject.Type;
        public string SequenceId => BaseObject.SequenceId;
        public string UnitId => BaseObject.KioskId;
        public string TransactionId => BaseObject.Id;
        public string Status => BaseObject.Status;
        public string AmountRequested => "$" + (BaseObject.AmountRequested * 1.0 / 100).ToString("0.00");
        public string AmountDispensed => "$" + (BaseObject.AmountDispensed * 1.0 / 100).ToString("0.00");
    }
}
