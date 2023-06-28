using CHBackOffice.ApiServices.ChsProxy;
using System.Collections.Generic;

namespace CHSBackOffice.Support.Classes
{
    public class TransactionDetailsClass
    {
        public PatronTransaction Transaction = new PatronTransaction();
        public List<CHBackOffice.ApiServices.ChsProxy.PatronTransaction> ParentTransactionsList { get; set; }
        public int RecordNumber { get; set; }
        public int RecordsCount { get; set; }
        public string Id => Transaction.Id;
        public string Code => Transaction.Type;
        public string StartDate => Transaction.StartDate.ToString("yyyy-MM-dd HH:mm:ss");
        public string EndDate => Transaction.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
        public string UnitId => Transaction.KioskId;
        public string SequenceId => Transaction.SequenceId;
        public string Patron => Transaction.Patron;
        public string RequestedAmount => "$" + Transaction.AmountRequested.ToString("0.00");
        public string DispensedAmount => "$" + Transaction.AmountDispensed.ToString("0.00");
        public string Fee => "$" + Transaction.Fee.ToString("0.00");
        public string TotalAmount => "$" + (Transaction.AmountRequested + Transaction.Fee).ToString("0.00");
        public string HostDate => Transaction.HostDate.ToString("yyyy-MM-dd HH:mm:ss");
        public string HostResponse => Transaction.HostStatus;
        public string HostSequenseId => Transaction.HostSequenceId;
        public string Status => Transaction.Status;
    }
}
