using CHSBackOffice.CustomControls;
using CHSBackOffice.Models.Popup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CHSBackOffice.Support
{
    class Commands
    {
        private static Dictionary<string, object> _popupResult;

        public static Commands Instance = new Commands();

        public View View { get; set; }

        internal static void ShowMenu()
        {
            MainMenuData.Instance.MenuIsActive = true;
        }

        internal Task<Dictionary<string, object>> ShowPopup(PopupParameters parameters)
        {
            return Task.Run(() =>
            {
                PopupData.Instance.PopupParameters = parameters;
                if (!parameters.CanChangeValues)
                    foreach (var row in PopupData.Instance.PopupParameters.Rows)
                        row.ReadOnly = true;
                if (View is UseDetailPopup)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() => (View as UseDetailPopup)?.DetailPressed());
                }
                if (View is UserAddPopup)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() => (View as UserAddPopup)?.DetailPressed());
                }
                PopupData.Instance.PopupIsActive = true;
                while (PopupData.Instance.PopupIsActive)
                    Task.Delay(10);
                return _popupResult;
            });
        
        }

        internal void HidePopup(bool submitted)
        {
            if (View is UseDetailPopup)
            {
                (View as UseDetailPopup)?.Hide();
            }
            if (View is UserAddPopup)
            {
                (View as UserAddPopup)?.Hide();
            }
            else
            {
                _popupResult = submitted ? PopupData.Instance.PopupParameters.Rows.ToDictionary(k => k.Key, v => v.Value) : null;
                PopupData.Instance.PopupIsActive = false;
            }
        }
    }
}
