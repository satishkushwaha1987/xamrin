using Xamarin.Forms;

namespace CHSBackOffice.Support
{
    internal class Constants
    {
        internal static int IntervalTime = 300000;
        internal static int AutoRefreshInterval = 8000;
        internal static int SoapTimeoutSec = 60;
        
        internal static class CHSImages
        {
            internal static string CHSIcon = "chs.png";
            internal static string CHSIconTablet = "CHS_logo_New.png";
            internal static string BgImagePhone = "backgrnd.png";
            internal static string BgImageLandscapeTablet = "BgLandscape.jpg";
            internal static string BgImagePortraitTablet = "BgPortrait.jpg";
        }

        internal static class CHSIcons
        {
            #region "MainMenuIcons"

            internal static string UserSiluet = "\ue99b";
            internal static string MachineStatus = "\ue7f7";
            internal static string Events = "\ue972";
            internal static string DoorOpenEvents = "\ue6f8";
            internal static string Dashboard = "\ue6d5";
            internal static string CashOnHand = "\ue6b9";
            internal static string CashUtilization = "\ue92d";
            internal static string AllTransactions = "\ue795";
            internal static string TransactionByType = "\ue850";
            internal static string MachineAvalabitity = "\ue7f4";
            internal static string RemoteControl = "\ue726";
            internal static string ActiveFloats = "\ue6b6";
            internal static string SystemParameters = "\ue73d";
            internal static string Transactions = "\ue70f";
            internal static string EmployeeManagment = "\ue62d";
            internal static string Users = "\ue64f";
            internal static string SOPUsers = "\ue6bb";
            internal static string Employees = "\ue650";
            internal static string Settings = "\ue717";
            internal static string Logout = "\ue662";

            #endregion

            #region "NavigationBarButtons"

            internal static string Close = "\ue662";
            internal static string TopMenu = "\ue6d8";
            internal static string ArrowLeft = "\ue956";
            internal static string LocationSwitcher = "\ue6d8";
            internal static string Search = "\ue651";
            internal static string Filter = "\ue7c0";
            internal static string Add = "\ue7b3";
            internal static string Gear = "\ue717";
            internal static string Remote = "\ue726";
            internal static string Cancel = "\ue7ba";

            #endregion

            #region "Content"

            internal static string SortAsc = "\ue7c0";
            internal static string SortDesc = "\ue7d2";
            internal static string ArrowRight = "\ue6a7";
            internal static string PickerImage = "\ue625";
            internal static string SelectedIcon = "\ue6d9";
            internal static string Calendar = "\ue6db";

            #endregion
        }

        internal static class CHSIconColors
        {
            #region "Main Menu"

            internal static Color MachineStatus = Color.FromHex("#40BCEF");
            internal static Color Events = Color.FromHex("#EA7563");
            internal static Color DoorOpenEvents = Color.FromHex("#FCD04B");
            internal static Color Dashboard = Color.FromHex("#626262");
            internal static Color CashOnHand = Color.FromHex("#4EB1AE");
            internal static Color CashUtilization = Color.FromHex("#4EB1AE");
            internal static Color AllTransactions = Color.FromHex("#4EB1AE");
            internal static Color TransactionByType = Color.FromHex("#4EB1AE");
            internal static Color MachineAvalabitity = Color.FromHex("#4EB1AE");
            internal static Color RemoteControl = Color.FromHex("#866EC3");
            internal static Color ActiveFloats = Color.FromHex("#2588CA");
            internal static Color SystemParameters = Color.FromHex("#7B4240");
            internal static Color Transactions = Color.FromHex("#86C857");
            internal static Color EmployeeManagment = Color.FromHex("#E9A32E");
            internal static Color Users = Color.FromHex("#E9A32E");
            internal static Color SOPUsers = Color.FromHex("#E9A32E");
            internal static Color Employees = Color.FromHex("#E9A32E");
            internal static Color Settings = Color.FromHex("#C4B79B");
            internal static Color Logout = Color.FromHex("#C4B79B");

            #endregion

            internal static Color Search = Color.FromHex("#FFFFFF");
            internal static Color TopMenu = Color.FromHex("#FFFFFF");
            internal static Color UserSiluet = Color.FromHex("#FFFFFF");
            internal static Color DashBoardColor = Color.FromHex("#3fa978");
            internal static Color PickerImageColor = Color.Black;
            internal static Color SelectedIconColor = Color.FromHex("#235dba");
        }
    }
}
