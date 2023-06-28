using CHSBackOffice.Database;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CHSBackOffice.Support
{
    class TransactionsCache
    {
        internal static TransactionsCache Instance = new TransactionsCache();
        internal int PageSize = 30;

        const int AutoLoadInterval = 5;

        Semaphore _refreshSemaphore;
        Semaphore _loadMoreSemaphore;
        static List<string> _failStatuses = new List<string> { "Failed", "Partial Dispense" };

        int _currentOffset;
        

        string _unitId;
        bool _isDispute;

        string _filterCriteria;
        DateTime _filterStartDate;
        DateTime _filterFinishDate;

        private List<CHBackOffice.ApiServices.ChsProxy.PatronTransaction> _cache;

        internal int TransationsCount;

        public TransactionsCache()
        {
            _cache = new List<CHBackOffice.ApiServices.ChsProxy.PatronTransaction>();
            _loadMoreSemaphore = new Semaphore(1, 1);
            _refreshSemaphore = new Semaphore(1, 1);
            App.SharedEventAggregator.GetEvent<Events.LocationChanged>().Subscribe(ClearCache);
        }

        internal void ClearCache()
        {
            _cache.Clear();
            _currentOffset = 1;

            _unitId = "";
            _isDispute = false;

            _filterCriteria = "";
            _filterFinishDate = DateTime.MinValue;
            _filterStartDate = DateTime.MinValue;

            TransationsCount = 0;
        }

        internal async Task<CHBackOffice.ApiServices.ChsProxy.PatronTransaction[]> LoadMore()
        {
            _loadMoreSemaphore.WaitOne();
            try
            {
                var result = await LoadTransactions(_currentOffset, PageSize);

                await RefreshTransactionsCount();
                if (_cache.Count == 0 && TransationsCount != 0)
                {
                    _cache.AddRange(new CHBackOffice.ApiServices.ChsProxy.PatronTransaction[TransationsCount]);

                    if ((_cache.Count > 0) && (_currentOffset + result.Length < _cache.Count) && (_cache[_cache.Count - 1] == null))
                        await RefreshIfNeeded(_cache.Count - 1);
                }

                for (int i = 0; i < result.Length; i++)
                    _cache[_currentOffset - 1 + i] = result[i];

                _currentOffset += result.Length;

                

                return result;
            }
            catch (Exception ex)
            {
                Support.ExceptionProcessor.ProcessException(ex);
                return null; 
            }
            finally
            {
                _loadMoreSemaphore.Release();
            }
        }

        async Task<CHBackOffice.ApiServices.ChsProxy.PatronTransaction[]> LoadTransactions(int offset, int count)
        {
            CHBackOffice.ApiServices.ChsProxy.PatronTransaction[] result = new CHBackOffice.ApiServices.ChsProxy.PatronTransaction[0];
            try
            {
                if (String.IsNullOrEmpty(_unitId))
                {
                    if (String.IsNullOrEmpty(_filterCriteria) && _filterStartDate == DateTime.MinValue && _filterFinishDate == DateTime.MinValue)
                        result = await IocContainer.CHSServiceAgent.GetTransactions(StateInfoService.SessionId, _isDispute, offset, count);
                    else
                        result = await IocContainer.CHSServiceAgent.FindTransactions(StateInfoService.SessionId, _isDispute, _filterStartDate, _filterFinishDate, _filterCriteria, offset, count);
                }
                else
                    result = await IocContainer.CHSServiceAgent.GetMachineTransactions(StateInfoService.SessionId, _unitId, offset, count);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }


            return result;
        }

        async Task RefreshTransactionsCount()
        {
            if (TransationsCount == 0)
            {
                string result = "";
                if (String.IsNullOrEmpty(_unitId))
                {
                    if (String.IsNullOrEmpty(_filterCriteria) && _filterStartDate == DateTime.MinValue && _filterFinishDate == DateTime.MinValue)
                    {
                        if (_isDispute)
                            result = await IocContainer.CHSServiceAgent.GetNumberOfDisputeTransactions(StateInfoService.SessionId);
                        else
                            result = await IocContainer.CHSServiceAgent.GetNumberOfTransactions(StateInfoService.SessionId, "");
                    }
                    else
                        result = await IocContainer.CHSServiceAgent.GetNumberOfFilteredTransactions(StateInfoService.SessionId, _isDispute, _filterStartDate, _filterFinishDate, _filterCriteria);
                }
                else
                    result = await IocContainer.CHSServiceAgent.GetNumberOfMachineTransactions(StateInfoService.SessionId, _unitId);

                Int32.TryParse(result, out TransationsCount);
            }
                
        }

        internal void SetMachineId(string unitId)
        {
            _unitId = unitId;
        }

        internal void SetDispute(bool value)
        {
            _isDispute = value;
        }

        internal void SetFilterParams(DateTime startDate, DateTime finishDate, string criteria)
        {
            _filterStartDate = startDate;
            _filterFinishDate = finishDate;
            _filterCriteria = criteria;

            if (_filterStartDate == _filterFinishDate && _filterStartDate != DateTime.MinValue)
            {
                _filterStartDate = startDate.Date;
                _filterFinishDate = finishDate.Date.AddDays(1).AddSeconds(-1);
            }
        }

        internal async Task<CHBackOffice.ApiServices.ChsProxy.PatronTransaction> GetTransaction(int index)
        {
            if (_cache.Count <= index) return null;
            await RefreshIfNeeded(index);

            for (int i = Math.Max(index - AutoLoadInterval, 0); i < Math.Min(index + AutoLoadInterval, _cache.Count); i++)
                if (_cache[i] == null)
                {
                    int startIndex = index - AutoLoadInterval;
                    Task.Run(async () =>
                    {
                        var result = await LoadTransactions(startIndex + 1, AutoLoadInterval * 2);
                        for (int j = 0; j < result.Length; j++)
                            _cache[startIndex + j] = result[j];
                    });
                    break;
                }

            return _cache[index];
        }

        async Task RefreshIfNeeded(int index)
        {
            try
            {
                if (_cache.Count > index)
                {
                    if (_cache[index] == null || IsFailed(_cache[index]))//Failed, not resolved
                    {
                        var loaded = (await LoadTransactions(index + 1, 1));
                        if (loaded != null && loaded.Length > 0)
                            _cache[index] = loaded[0];
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        internal static bool IsFailed(CHBackOffice.ApiServices.ChsProxy.PatronTransaction transaction)
        {
            return _failStatuses.Contains(transaction.Status) && String.IsNullOrEmpty(transaction.Dispute) && (!transaction.Type.Equals("Bank Withdrawal", StringComparison.InvariantCultureIgnoreCase));
        }

        internal static bool IsResolved(CHBackOffice.ApiServices.ChsProxy.PatronTransaction transaction)
        {
            return (_failStatuses.Contains(transaction.Status) && !String.IsNullOrEmpty(transaction.Dispute));
        }
    }
}
