using Acr.UserDialogs;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Input;

namespace CHSBackOffice.ViewModels
{
    public class TransactionsDetailsFrameViewModel : BindableBase
    {
        private int _currentTransactionNumber;
        private int CurrentTransactionNumber
        {
            get => _currentTransactionNumber;
            set
            {
                _currentTransactionNumber = value;
                RaisePropertyChanged(nameof(TransactionNumberText));
            }
        }

        private int _totalTransactionsCount;
        private int TotalTransactionsCount
        {
            get => _totalTransactionsCount;
            set
            {
                _totalTransactionsCount = value;
                RaisePropertyChanged(nameof(TransactionNumberText));
            }
        }

        public string TransactionNumberText => $"Record # {_currentTransactionNumber} of {_totalTransactionsCount} Records";

        public bool IsFailed => SelectedTransaction == null ? false : Support.TransactionsCache.IsFailed(SelectedTransaction);
        public bool ShowResolveArea => SelectedTransaction == null ? false  : IsFailed || Support.TransactionsCache.IsResolved(SelectedTransaction);

        private string _disputeComment;
        public string DisputeComment
        {
            get => _disputeComment;
            set => SetProperty(ref _disputeComment, value);
        }

        public string DisputeDate => 
            SelectedTransaction == null ? 
            "" : 
            SelectedTransaction.DisputeDate.ToDateTimeStr();

        private ICHSServiceAgent _serviceAgent;

        public CHBackOffice.ApiServices.ChsProxy.PatronTransaction SelectedTransaction { get; set; }

        public ICommand GoToTransactionCommand => new DelegateCommand<string>(ShowTransaction);
        public ICommand ResolveDisputeCommand => new DelegateCommand(ResolveDispute);
        public TransactionsDetailsFrameViewModel(CHBackOffice.ApiServices.ChsProxy.PatronTransaction transaction, int number, int total)
        {
            SelectedTransaction = transaction;
            CurrentTransactionNumber = number + 1;
            TotalTransactionsCount = total;
            DisputeComment = transaction.Dispute;
            RaisePropertyChanged(nameof(IsFailed));
            RaisePropertyChanged(nameof(ShowResolveArea));
        }

        async void ResolveDispute()
        {
            try
            {
                if (string.IsNullOrEmpty(DisputeComment))
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = "Add comment please"
                    });
                    return;
                }

                CommonViewObjects.Instance.IsLoadingVisible = true;
                if (_serviceAgent == null) _serviceAgent = IocContainer.CHSServiceAgent;
                var res = await _serviceAgent.ResolveDispute(StateInfoService.SessionId, SelectedTransaction.Id, DisputeComment);
                SelectedTransaction = await TransactionsCache.Instance.GetTransaction(CurrentTransactionNumber - 1);
                RaisePropertyChanged(nameof(IsFailed));
                RaisePropertyChanged(nameof(ShowResolveArea));
                RaisePropertyChanged(nameof(DisputeDate));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            finally
            {
                CommonViewObjects.Instance.IsLoadingVisible = false;
            }
            
        }


        private void ShowTransaction(string tag)
        {
            App.SharedEventAggregator.GetEvent<Events.ShowTransaction>().Publish(tag);
            
        }
    }
}
