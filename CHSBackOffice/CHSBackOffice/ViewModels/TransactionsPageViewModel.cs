using CHBackOffice.ApiServices.ChsProxy;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Database;
using CHSBackOffice.Models.Popup;
using CHSBackOffice.Support;
using CHSBackOffice.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CHSBackOffice.ViewModels
{
    internal class TransactionsPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region "PUBLIC PROPS"

        public string TransactionsLeftTabCaption => "DISPUTES";
        public string TransactionsRightTabCaption => "ALL";
        public string AllTransactionsLoadingText => "Retrieving All Transactions...";
        public string DisputesTransactionsLoadingText => "Retrieving Disputes Transactions...";
        public PopoverItem AllMachine => new PopoverItem { Key = "All Machines", Value = "All Machines", Selected = false };
        public string ViewTypeText => Constants.CHSIcons.TopMenu;
        public bool AllTabSelected { get; set; } = false;
        public string SortText => SortDesc ? Constants.CHSIcons.SortDesc : Constants.CHSIcons.SortAsc;
        #endregion

        #region "BINDABLE PROPS"

        #region "SortDesc"

        private bool _sortDesc;
        public bool SortDesc
        {
            get => _sortDesc;
            set
            {
                SetProperty(ref _sortDesc, value);
                RaisePropertyChanged(nameof(SortText));
            }
        }

        #endregion

        #region "Items"

        private ObservableCollection<CHBackOffice.ApiServices.ChsProxy.PatronTransaction> _items;
        public ObservableCollection<CHBackOffice.ApiServices.ChsProxy.PatronTransaction> Items
        {
            get => _items;
            set
            {
                SetProperty(ref _items, value);
                RaisePropertyChanged(nameof(Items));
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
                RaisePropertyChanged(nameof(SearchText));
                SearchCommand?.Execute(null);
            }
        }

        #endregion

        #endregion

        #region "PRIVATE PROPS"

        private PatronTransaction[] transactions;
        private Kiosk[] machineList;
        private TransactionType[] transactionTypes;
        private TransactionStatus[] transactionStatuses;
        private string criteria;

        bool ReturningFromFilter = false;
        bool State { get; set; }
        bool CriteriaEmptyCondition
        {
            get
            {
                try
                {
                    var machineIds = PopoverData.Instance.MachineIDs.Where(x => x.Selected == true);
                    var transactionTypes = PopoverData.Instance.TransactionTypes.Where(x => x.Selected == true);
                    var transactionStatuses = PopoverData.Instance.TransactionStatuses.Where(x => x.Selected == true);

                    return (machineIds.Count() == 0
                        || machineIds.Where(x => x.Selected == true).Contains(AllMachine))
                        && string.IsNullOrEmpty(PopoverData.Instance.TransactionIdSelected)
                        && string.IsNullOrEmpty(PopoverData.Instance.SequenceIdSelected)
                        && transactionTypes.Count() == 0
                        && transactionStatuses.Count() == 0;
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                    return false;
                }
            } 
        }

        #endregion
        
        #region ".CTOR"

        public TransactionsPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            TransactionsCache.Instance.ClearCache();

            PopoverData.Instance.FilterAction = false;

            AddToolbarItemCommand("but2", FilterPressed);
        }

        #endregion

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            transactions = await TransactionsCache.Instance.LoadMore();// _serviceAgent.GetTransactions(StateInfoService.SessionId, !State, CurrentOffset, TotalCount);
            Items = new ObservableCollection<PatronTransaction>(transactions);

            machineList = await _serviceAgent.GetMachineList(StateInfoService.SessionId);
            transactionTypes = await _serviceAgent.GetTransactionTypes();
            transactionStatuses = await _serviceAgent.GetTransactionStatus();
            PopoverData.Instance.MachineIDs = GetMachineIDs(machineList);
            PopoverData.Instance.MachineIDs.Insert(0, AllMachine);
            PopoverData.Instance.TransactionTypes = GetTransactionTypes(transactionTypes);
            PopoverData.Instance.TransactionStatuses = GetTransactionStatuses(transactionStatuses);
        }

        #endregion

        #region "COMMANDS"

        public ICommand LoadMoreCommand => new Xamarin.Forms.Command(LoadMoreExecute);
        public ICommand SearchCommand => new Xamarin.Forms.Command(SearchExecute);
        public ICommand SearchStartedCommand => new Xamarin.Forms.Command(SearchStartedExecute);
        public ICommand SearchEndedCommand => new Xamarin.Forms.Command(SearchEndedExecute);

        public DelegateCommand<object> SwitchCollectionCommand => new DelegateCommand<object>(SwitchCollectionCommandExecute);
        public DelegateCommand<PatronTransaction> SwitchedToCollectionItemTappedCommand => new DelegateCommand<PatronTransaction>(TransactionTapExecute);

        public ICommand AppearingCommand => new Xamarin.Forms.Command(AppearingExecute);

        #region "COMMANDS HANDLER"

        async void LoadMoreExecute()
        {
            try
            {
                if (!CHSBackOffice.Support.PopoverData.Instance.LoadMoreAction)
                    return;

                CommonViewObjects.Instance.IsLoadingVisible = true;
                PatronTransaction[] nextRecords = null;
                if (PopoverData.Instance.FilterAction)
                {
                    TransactionsCache.Instance.SetFilterParams(
                        PopoverData.Instance.StartDateTimeSelected,
                        PopoverData.Instance.EndDateTimeSelected,
                        CriteriaEmptyCondition ? string.Empty : CreateCriteria());
                }
                else
                    TransactionsCache.Instance.SetFilterParams(DateTime.MinValue, DateTime.MinValue, String.Empty);

                TransactionsCache.Instance.SetDispute(!State);
                nextRecords = await TransactionsCache.Instance.LoadMore();

                //if (!PopoverData.Instance.FilterAction)
                //{
                //    nextRecords = await
                //        _serviceAgent.GetTransactions(StateInfoService.SessionId, !State, CurrentOffset, TotalCount);
                //}
                //else
                //{
                //    criteria = CriteriaEmptyCondition ? string.Empty : CreateCriteria();
                //    nextRecords = await _serviceAgent.FindTransactions(StateInfoService.SessionId, !State,
                //                                                                    PopoverData.Instance.StartDateTimeSelected,
                //                                                                    PopoverData.Instance.EndDateTimeSelected,
                //                                                                    criteria,
                //                                                                    CurrentOffset,
                //                                                                    TotalCount);
                //}

                foreach (var record in nextRecords)
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() => Items.Add(record));
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => RaisePropertyChanged(nameof(Items)));
                var prevRecords = transactions;
                if (nextRecords != null || nextRecords.Count() != 0)
                    transactions = prevRecords?.Concat(nextRecords).ToArray();

            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            CommonViewObjects.Instance.IsLoadingVisible = false;
        }

        async void SearchExecute()
        {
            await Task.Run(() =>
            {
                if (transactions == null)
                    return;

                Func<PatronTransaction, bool> predicate = transaction =>
                    string.IsNullOrEmpty(SearchText) ||
                    transaction.KioskId.ToUpper().Contains(SearchText.ToUpper()) ||
                    transaction.SequenceId.ToUpper().Contains(SearchText.ToUpper()) ||
                    transaction.Id.ToUpper().Contains(SearchText.ToUpper()) ||
                    transaction.Status.ToUpper().Contains(SearchText.ToUpper());

                Items = new ObservableCollection<PatronTransaction>(
                        transactions.AsParallel().Where(predicate).ToArray());
            });
        }

        void SearchStartedExecute()
        {
            CHSBackOffice.Support.PopoverData.Instance.LoadMoreAction = false;
        }

        void SearchEndedExecute()
        {
            CHSBackOffice.Support.PopoverData.Instance.LoadMoreAction = true;
        }

        void SwitchCollectionCommandExecute(object arg)
        {
            if (State != (bool)arg)
                TransactionsCache.Instance.ClearCache();

            State = (bool)arg;
            TransactionsCache.Instance.SetDispute(!State);

            SafeRefreshDataAsync();
        }

        void TransactionTapExecute(PatronTransaction item)
        {
            CommonViewObjects.Instance.SelectedTransactionNumber = Items.IndexOf(item);
            Services.Navigation.NavigateDetailPage(typeof(TransactionsCarouselPage));
            //ItemBroker<PatronTransaction>.Instance.Bind(item);
            //ItemBroker<bool>.Instance.Bind(State);
            //var index = transactions.ToList().FindIndex(t => t.Id.Equals(item.Id));
            //ItemBroker<int>.Instance.Bind(index + 1);
            //ItemBroker<List<PatronTransaction>>.Instance.Bind(transactions.ToList());
            //if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            //    Services.Navigation.NavigateDetailPage(typeof(TransactionDetailsPage));
            //else
            //    Services.Navigation.NavigateDetailPage(typeof(TransactionDetailsPageIos));
        }

        async void AppearingExecute()
        {
            if (PopoverData.Instance.FilterAction && ReturningFromFilter)
                await FindData();
            ReturningFromFilter = false;
        }

        void FilterPressed(ToolbarButton button)
        {
            ReturningFromFilter = true;
            Services.Navigation.NavigateDetailPage(typeof(TransactionFilterPage));
        }

        #endregion

        #endregion

        #region "PRIVATE METHODS"

        private ObservableCollection<PopoverItem> GetMachineIDs(Kiosk[] machineList)
        {
            try
            {
                return new ObservableCollection<PopoverItem>(machineList.Select(x => new PopoverItem
                {
                    Key = x.Id,
                    Value = x.Id,
                    Selected = false
                }
                )
                .Distinct(new PopoverItemEqualityComparer())
                .OrderBy(x => x.Key)
            );
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        private ObservableCollection<PopoverItem> GetTransactionTypes(TransactionType[] transactionTypes)
        {
            try
            {
                return new ObservableCollection<PopoverItem>(transactionTypes.Select(x => new PopoverItem
                {
                    Key = x.Description,
                    Value = x.Description,
                    Selected = false
                }
                )
                .Distinct(new PopoverItemEqualityComparer())
                .OrderBy(x => x.Key)
            );
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        private ObservableCollection<PopoverItem> GetTransactionStatuses(TransactionStatus[] transactionTypes)
        {
            try
            {
                return new ObservableCollection<PopoverItem>(transactionStatuses.Select(x => new PopoverItem
                {
                    Key = x.Description,
                    Value = x.Description,
                    Selected = false
                }
                )
                .Distinct(new PopoverItemEqualityComparer())
                .OrderBy(x => x.Key)
            );
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        async Task FindData()
        {
            try
            {
                CommonViewObjects.Instance.IsLoadingVisible = true;
                criteria = CriteriaEmptyCondition ? string.Empty : CreateCriteria();
                TransactionsCache.Instance.ClearCache();
                TransactionsCache.Instance.SetDispute(!State);
                TransactionsCache.Instance.SetFilterParams(PopoverData.Instance.StartDateTimeSelected,
                    PopoverData.Instance.EndDateTimeSelected,
                    criteria);

                transactions = await TransactionsCache.Instance.LoadMore();
                    //await _serviceAgent.FindTransactions(StateInfoService.SessionId, !State, PopoverData.Instance.StartDateTimeSelected, PopoverData.Instance.EndDateTimeSelected, criteria, CurrentOffset, TotalCount);
                Items = null;
                Items = new ObservableCollection<PatronTransaction>(transactions);
                CommonViewObjects.Instance.IsLoadingVisible = false;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        string CreateCriteria()
        {
            try
            {
                var criteriaSB = new StringBuilder();
                var machineIds = PopoverData.Instance.MachineIDs.Where(x => x.Selected == true);
                var transactionTypes = PopoverData.Instance.TransactionTypes.Where(x => x.Selected == true);
                var transactionStatuses = PopoverData.Instance.TransactionStatuses.Where(x => x.Selected == true);
                if (machineIds.Count() != 0 && !machineIds.Contains(AllMachine))
                {
                    criteriaSB.Append($"UnitId:");
                    foreach (var item in machineIds)
                    {
                        criteriaSB.Append($"{ item.Value},");
                    }
                    criteriaSB.Append($";");
                }
                if (!string.IsNullOrEmpty(PopoverData.Instance.TransactionIdSelected))
                    criteriaSB.Append($"TransactionId:{PopoverData.Instance.TransactionIdSelected};");
                if (!string.IsNullOrEmpty(PopoverData.Instance.SequenceIdSelected))
                    criteriaSB.Append($"SequenceId:{PopoverData.Instance.SequenceIdSelected};");
                if (transactionTypes.Count() > 0)
                {
                    criteriaSB.Append($"Type:");
                    foreach (var item in transactionTypes)
                    {
                        criteriaSB.Append($"{item.Key},");
                    }
                    criteriaSB.Append($";");
                }
                if (transactionStatuses.Count() > 0)
                {
                    criteriaSB.Append($"Status:");
                    foreach (var item in transactionStatuses)
                    {
                        criteriaSB.Append($"{item.Key},");
                    }
                    criteriaSB.Append($";");
                }
                return criteriaSB.ToString();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return string.Empty;
            }
        }

        #endregion

    }
}
