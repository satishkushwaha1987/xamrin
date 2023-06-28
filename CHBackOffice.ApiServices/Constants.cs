using System;
using System.Collections.Generic;
using System.Text;

namespace CHBackOffice.ApiServices
{
    public class Constants
    {
        internal static event Action IsSoaupUrlChanged;
        private static string soaupUrl = "http://137.117.92.154/mobile/chs.asmx";
        public static string SoaupUrl
        {
            set
            {
                soaupUrl = value;
                IsSoaupUrlChanged?.Invoke();
            }
            get => soaupUrl;
        }
    }
}
