using CHBackOffice.ApiServices.ChsProxy;
using CHSBackOffice.Support.StaticResources;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.Support.Classes
{
    public class ActiveFloatExtended : ListViewItemBase<ActiveFloat>
    {
        #region Fields
        private BaseMatcher<string, string> stringStatusToColorMatcher;
        #endregion

        #region Bindable Properties
        private Color colorState;
        public Color ColorState
        {
            get => colorState;
            set
            {
                colorState = value;
            }
        }

        private string lblEmplIdNameValue;
        public string LblEmplIdNameValue 
        {
            get => lblEmplIdNameValue;
            set
            {
                lblEmplIdNameValue = value;
            }
        }

        private string lbDateLastWithdrawal;
        public string LbDateLastWithdrawal
        {
            get => lbDateLastWithdrawal;
            set 
            {
                lbDateLastWithdrawal = value;
            }
        }

        public string TotalAmountWithdrawalDuringShift => BaseObject.TotalAmountWithdrawalDuringShift;
        public string TotalDepositAfterLastWithdrawal => BaseObject.TotalDepositAfterLastWithdrawal;
        public string AmountDeposit => BaseObject.AmountDeposit;
        public string Shift => BaseObject.Shift;

        #endregion

        #region .CTOR
        public ActiveFloatExtended(ActiveFloat baseObject)
        {
            BaseObject = baseObject;
            stringStatusToColorMatcher = FactoryMatcher.GetActiveFloatToColorMatcher();
            ColorState = StaticResourceManager.GetColor(stringStatusToColorMatcher.ToValue(BaseObject.Status));
            LblEmplIdNameValue = string.Format(@"{0} / {1}", BaseObject.EmployeeID, BaseObject.Name);
            LbDateLastWithdrawal = DateTimeExtensions.DateTimeStrToNewFormat(baseObject.DateLastWithdrawal.ToString(), "MM/dd/yyyy hh:mm tt");
        }
        #endregion
    }
}
