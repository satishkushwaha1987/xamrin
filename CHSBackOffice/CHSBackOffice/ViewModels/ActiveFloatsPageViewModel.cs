using CHBackOffice.ApiServices.ChsProxy;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Events;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    internal class ActiveFloatsPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region "BINDABLE PROPS"

        #region "ActiveFloats"
        private ObservableCollection<ActiveFloatExtended> _activeFloats;
        public ObservableCollection<ActiveFloatExtended> ActiveFloats
        {
            get => _activeFloats;
            set
            {
                SetProperty(ref _activeFloats, value);
                RaisePropertyChanged(nameof(ActiveFloats));
            }
        }
        #endregion

        #region "IsNoDataLabelVisible"

        private bool _isNoDataLabelVisible;
        public bool IsNoDataLabelVisible
        {
            get => _isNoDataLabelVisible;
            set
            {
                SetProperty(ref _isNoDataLabelVisible, value);
            }
        }

        #endregion

        #region "SearchText"

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                SearchCommand?.Execute(null);
            }
        }
        #endregion

        #endregion

        #region "PRIVATE FIELDS"

        private ActiveFloat[] activeFloats;
        private List<ActiveFloatExtended> extendedActiveFloats;
        #endregion

        #region ".CTOR"

        public ActiveFloatsPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            SafeRefreshDataAsync();
        }

        #endregion

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            IsNoDataLabelVisible = false;

            ActiveFloats = null;
            activeFloats = await _serviceAgent.GetActiveFloats(StateInfoService.SessionId);
            //activeFloats = GetTestData();
            if (activeFloats != null && activeFloats.Length > 0)
            {
                extendedActiveFloats = new List<ActiveFloatExtended>();
                foreach (var obj in activeFloats)
                {
                    extendedActiveFloats.Add(new ActiveFloatExtended(obj));
                }
                ActiveFloats = new ObservableCollection<ActiveFloatExtended>(extendedActiveFloats);
                
            }
            else
                IsNoDataLabelVisible = true;
        }

        #endregion

        #region "COMMANDS"
        public ICommand AppearingCommand => new Xamarin.Forms.Command(AppearingExecute);
        public ICommand SearchCommand => new Xamarin.Forms.Command(SearchExecute);
        #endregion

        #region COMMAND HANDLERS
        void AppearingExecute()
        {
            
        }

        void SearchExecute()
        {
            try 
            {
                if (activeFloats != null)
                    ActiveFloats = new ObservableCollection<ActiveFloatExtended>(
                        extendedActiveFloats.Where(
                            e =>
                            string.IsNullOrEmpty(SearchText) ||
                            e.BaseObject.EmployeeID.ToUpper().Contains(SearchText.ToUpper()) ||
                            e.BaseObject.Name.ToUpper().Contains(SearchText.ToUpper()) ||
                            e.BaseObject.Shift.ToUpper().Contains(SearchText.ToUpper()) ||
                            e.LbDateLastWithdrawal.ToUpper().Contains(SearchText.ToUpper()) || 
                            e.BaseObject.TotalAmountWithdrawalDuringShift.ToUpper().Contains(SearchText.ToUpper()) ||
                            e.BaseObject.TotalDepositAfterLastWithdrawal.ToUpper().Contains(SearchText.ToUpper()) ||
                            e.BaseObject.AmountDeposit.ToUpper().Contains(SearchText.ToUpper()) ||
                            e.BaseObject.Status.ToUpper().Contains(SearchText.ToUpper())
                            ));

            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region Methods

        private ActiveFloat[] GetTestData()
        {
            return new ActiveFloat[]
            {
                new ActiveFloat
                {
                    Name = "Aasokan",
                    EmployeeID = "12345",
                    DateLastWithdrawal = new System.DateTime(2019,8,15),
                    AmountDeposit = "7000",
                    TotalAmountWithdrawalDuringShift = "4000",
                    TotalDepositAfterLastWithdrawal = "3000",
                    Shift = "500",
                    Status = "Normal"
                },
                new ActiveFloat
                {
                    Name = "Admin2",
                    EmployeeID = "56977",
                    DateLastWithdrawal = new System.DateTime(2019,5,25),
                    AmountDeposit = "8000",
                    TotalAmountWithdrawalDuringShift = "4000",
                    TotalDepositAfterLastWithdrawal = "4000",
                    Shift = "300",
                    Status = "InService"
                },
                new ActiveFloat
                {
                    Name = "Admin1",
                    EmployeeID = "89789",
                    DateLastWithdrawal = new System.DateTime(2019,1,10),
                    AmountDeposit = "2000",
                    TotalAmountWithdrawalDuringShift = "1000",
                    TotalDepositAfterLastWithdrawal = "1000",
                    Shift = "100",
                    Status = "Critical"
                },
                new ActiveFloat
                {
                    Name = "Admin3",
                    EmployeeID = "42357",
                    DateLastWithdrawal = new System.DateTime(2019,9,25),
                    AmountDeposit = "6000",
                    TotalAmountWithdrawalDuringShift = "3000",
                    TotalDepositAfterLastWithdrawal = "3000",
                    Shift = "400",
                    Status = "Normal"
                },
                new ActiveFloat
                {
                    Name = "Admin4",
                    EmployeeID = "9551",
                    DateLastWithdrawal = new System.DateTime(2019,8,15),
                    AmountDeposit = "7500",
                    TotalAmountWithdrawalDuringShift = "4500",
                    TotalDepositAfterLastWithdrawal = "3000",
                    Shift = "500",
                    Status = "InService"
                },
                new ActiveFloat
                {
                    Name = "Admin5",
                    EmployeeID = "8900",
                    DateLastWithdrawal = new System.DateTime(2019,4,30),
                    AmountDeposit = "8900",
                    TotalAmountWithdrawalDuringShift = "4000",
                    TotalDepositAfterLastWithdrawal = "4900",
                    Shift = "800",
                    Status = "Critical"
                },
                new ActiveFloat
                {
                    Name = "Admin6",
                    EmployeeID = "87878",
                    DateLastWithdrawal = new System.DateTime(2019,3,2),
                    AmountDeposit = "5500",
                    TotalAmountWithdrawalDuringShift = "2500",
                    TotalDepositAfterLastWithdrawal = "3000",
                    Shift = "300",
                    Status = "Normal"
                },
                new ActiveFloat
                {
                    Name = "Admin7",
                    EmployeeID = "48999",
                    DateLastWithdrawal = new System.DateTime(2019,6,23),
                    AmountDeposit = "4500",
                    TotalAmountWithdrawalDuringShift = "3000",
                    TotalDepositAfterLastWithdrawal = "1500",
                    Shift = "500",
                    Status = "InService"
                },
                new ActiveFloat
                {
                    Name = "Admin8",
                    EmployeeID = "55597",
                    DateLastWithdrawal = new System.DateTime(2019,4,3),
                    AmountDeposit = "8900",
                    TotalAmountWithdrawalDuringShift = "7000",
                    TotalDepositAfterLastWithdrawal = "1900",
                    Shift = "900",
                    Status = "Critcal"
                },
                new ActiveFloat
                {
                    Name = "Admin9",
                    EmployeeID = "566778",
                    DateLastWithdrawal = new System.DateTime(2019,8,1),
                    AmountDeposit = "4800",
                    TotalAmountWithdrawalDuringShift = "4000",
                    TotalDepositAfterLastWithdrawal = "800",
                    Shift = "500",
                    Status = "Normal"
                },
                new ActiveFloat
                {
                    Name = "Admin10",
                    EmployeeID = "45899",
                    DateLastWithdrawal = new System.DateTime(2019,3,18),
                    AmountDeposit = "8900",
                    TotalAmountWithdrawalDuringShift = "4000",
                    TotalDepositAfterLastWithdrawal = "4900",
                    Shift = "500",
                    Status = "InService"
                },
                new ActiveFloat
                {
                    Name = "Admin11",
                    EmployeeID = "89879",
                    DateLastWithdrawal = new System.DateTime(2019,8,15),
                    AmountDeposit = "8500",
                    TotalAmountWithdrawalDuringShift = "5500",
                    TotalDepositAfterLastWithdrawal = "3000",
                    Shift = "1000",
                    Status = "Critical"
                }
            };
        }
        #endregion
    }
}
