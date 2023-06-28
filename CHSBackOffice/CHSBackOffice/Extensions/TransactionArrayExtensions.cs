using CHBackOffice.ApiServices.ChsProxy;
using System.Linq;

namespace CHSBackOffice.Extensions
{
    internal static class TransactionArrayExtensions
    {
        public static Transaction[] GetWithMinShift(this Transaction[] transactions)
        {
            return transactions
                .OrderBy(_ => _.Shift)
                .GroupBy(_ => _.Time)
                .Select(g => g.First())
                .ToArray();
        }
    }
}
