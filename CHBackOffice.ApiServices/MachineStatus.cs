
using System.Collections.Generic;

namespace CHBackOffice.ApiServices
{
    public class MachineStatus
    {
        public string Number { get; set; }
        public MachineState State { get; set; }
        public List<ItemCash> CashDis { get; set; }
    }

    public class ItemCash
    {
        public ItemCash(double percent)
        {
            Percent = percent;
        }
        public double Percent { get; set; }
    }
}
