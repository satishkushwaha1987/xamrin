using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Events;
using CHSBackOffice.Models.ApexCharts.Common;
using Prism.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    internal abstract class ReportPageViewModel<TypeOfReport> : ExtendedNaviPageViewModelBase where TypeOfReport : ReportItem
    {
        protected ConcurrentDictionary<string, TypeOfReport> ReportCache;

        public ReportPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            NeedToRefreshDataOrientationChanged = true;

            ReportCache = new ConcurrentDictionary<string, TypeOfReport>();

            StateCollection = GetStateCollection();

            SelectedGroupKey = StateCollection.First().Key;
        }

        internal abstract Task<TypeOfReport> GetReport(string state, DisplayOrientation displayOrientation);

        internal abstract List<KeyValuePair<string, string>> GetStateCollection();

        internal virtual void AfterRefreshDataActions(TypeOfReport report)
        { }

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            //App.SharedEventAggregator.GetEvent<ReportClearChart>().Publish(GetType());

            try
            {
                if (isLocationChanged)
                    ReportCache = new ConcurrentDictionary<string, TypeOfReport>();

                TypeOfReport report;

                if (ReportCache.ContainsKey(SelectedGroupKey))
                    report = ReportCache[SelectedGroupKey];
                else
                {
                    report = await GetReport(SelectedGroupKey, CurrentDisplayOrientation);
                    ReportCache.TryAdd(SelectedGroupKey, report);
                }

                App.SharedEventAggregator.GetEvent<ReportDetailsReady>().Publish(new Support.Classes.ReportData { ColumnsCount = report.ColumnCount, ReportHtml = report.Html });

                AfterRefreshDataActions(report);
            }
            catch (Exception ex)
            {
                App.SharedEventAggregator.GetEvent<ReportClearChart>().Publish(GetType());
                Support.ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion


        #region Commands

        public ICommand GroupSelectedCommand => new DelegateCommand<object>(OnGroupSelected);
        public ICommand AppearingCommand => new Command(SafeRefreshDataAsync);

        #endregion

        #region "PRIVATE METHODS"

        private void OnGroupSelected(object selectedGroup)
        {
            var pair = (KeyValuePair<string, string>)selectedGroup;
            SelectedGroupKey = pair.Key;
            SafeRefreshDataAsync();
        }

        #endregion


        #region "PUBLIC PROPERTIES"

        private IEnumerable<KeyValuePair<string, string>> _stateCollection;
        public IEnumerable<KeyValuePair<string, string>> StateCollection
        {
            get => _stateCollection;
            set
            {
                SetProperty(ref _stateCollection, value);
            }
        }

        private string _selectedGroupKey;
        public string SelectedGroupKey
        {
            get => _selectedGroupKey;
            set => SetProperty(ref _selectedGroupKey, value);
        }

        #endregion
    }
}
