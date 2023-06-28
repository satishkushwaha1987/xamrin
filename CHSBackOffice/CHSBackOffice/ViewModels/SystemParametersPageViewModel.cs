using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Database;
using CHSBackOffice.Models.Popup;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Classes;
using CHSBackOffice.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    internal class SystemParametersPageViewModel : ExtendedNaviPageViewModelBase
    {
        public class ParamClass : ListViewItemBase<CHBackOffice.ApiServices.ChsProxy.Parameter1>
        {
            public string Category => BaseObject.GroupId;
            public string Key => BaseObject.Id;
            public string Value => BaseObject.Value;
            public string Description => BaseObject.Description;
        }

        const string AllGroupsFilterKey = "All";
        const string FilterToolbarButtonName = "filterToolbarButton";
        const string AddToolbarButtonName = "addToolbarButton";

        bool _needToRefreshWhenAppearing;

        #region "BINDABLE PROPS"

        public DataTable ParametersData => CreateDataTableFromListOfParams(_filteredAndSearchedParams);


        #region SearchText

        private string _SearchText;
        public string SearchText
        {
            get => _SearchText;
            set
            {
                SetProperty(ref _SearchText, value);
                Search();

                RaisePropertyChanged(nameof(ParametersData));
            }

        }

        #endregion


        #region FilterIsVisible

        private bool _FilterIsVisible;
        public bool FilterIsVisible
        {
            get => _FilterIsVisible;
            set => SetProperty(ref _FilterIsVisible, value);

        }

        #endregion


        #region FilterItems

        private List<PopoverItem> _FilterItems;
        public List<PopoverItem> FilterItems
        {
            get => _FilterItems;
            set => SetProperty(ref _FilterItems, value);

        }

        #endregion

        #region SelectedFilterItem

        private PopoverItem _SelectedFilterItem;
        public PopoverItem SelectedFilterItem
        {
            get => _SelectedFilterItem;
            set
            {
                if (_SelectedFilterItem != null)
                    _SelectedFilterItem.Selected = false;

                SetProperty(ref _SelectedFilterItem, value);

                if (_SelectedFilterItem != null)
                {
                    _SelectedFilterItem.Selected = true;

                    FilterAndSearchData();
                }

                HideAdditionalWindowsCommand?.Execute(null);
            }

        }

        #endregion


        #endregion

        #region Fields

        private List<CHBackOffice.ApiServices.ChsProxy.ParameterGroup> _allParams;
        private List<ParamClass> _filteredParams;
        private List<ParamClass> _filteredAndSearchedParams;
        private List<string> _columns;

        private Semaphore _searchDataSemaphore;
        private Semaphore _filterDataSemaphore;

        #endregion

        #region ".CTOR"

        public SystemParametersPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            _searchDataSemaphore = new Semaphore(1, 1);
            _filterDataSemaphore = new Semaphore(1, 1);

            AddToolbarItemCommand(FilterToolbarButtonName, new Action<CustomControls.ToolbarButton>(ToolbarButtonPressed));
            AddToolbarItemCommand(AddToolbarButtonName, new Action<CustomControls.ToolbarButton>(ToolbarButtonPressed));

            _needToRefreshWhenAppearing = false;

            SafeRefreshDataAsync();
            
        }

        #endregion

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            var paramsFromServer = await _serviceAgent.GetParameters(StateInfoService.SessionId);
            _allParams = new List<CHBackOffice.ApiServices.ChsProxy.ParameterGroup>(paramsFromServer.Where(p => p != null));
            var allParameterNames = await _serviceAgent.GetParameterGroupNames(StateInfoService.SessionId);
            FilterItems = new List<PopoverItem> { new PopoverItem { Key = AllGroupsFilterKey, Value = AllGroupsFilterKey } };

            var allowedParameterGroups = new List<PopoverItem>();

            foreach (var t in allParameterNames)
            {
                FilterItems.Add(new PopoverItem { Key = t, Value = t });
                allowedParameterGroups.Add(new PopoverItem { Key = t, Value = t });
            }
            RaisePropertyChanged(nameof(FilterItems));

            if (_SelectedFilterItem == null)
                _SelectedFilterItem = FilterItems[0];

            RaisePropertyChanged(nameof(SelectedFilterItem));

            CommonViewObjects.Instance.AllowedParameterGroups = allowedParameterGroups;
            CommonViewObjects.Instance.ExistingParamNames = new List<string>();
            foreach (var t in paramsFromServer)
                foreach (var p in t.Collection)
                    CommonViewObjects.Instance.ExistingParamNames.Add(p.Id.ToLower());

            FilterAndSearchData();
            _needToRefreshWhenAppearing = false;
        }

        #endregion

        #region "COMMANDS"

        public ICommand SettingTappedCommand => new Command<int>(SettingTapped);

        public ICommand AppearingCommand => new Command(AppearingExecute);

        public ICommand HideAdditionalWindowsCommand => new Command(() =>{ FilterIsVisible = false; });

        #region "COMMAND HANDLERS"

        void ToolbarButtonPressed(ToolbarButton button)
        {
            switch (button.Name)
            {
                case FilterToolbarButtonName:
                    FilterIsVisible = true;
                    break;
                case AddToolbarButtonName:
                    GoToParameterPage();
                    break;
            }
        }

        void GoToParameterPage(CHBackOffice.ApiServices.ChsProxy.Parameter1 parameter = null)
        {
            try
            {
                Support.CommonViewObjects.Instance.CurrentSystemParameter = parameter;

                if (parameter != null)
                    foreach (var t in CommonViewObjects.Instance.AllowedParameterGroups)
                        t.Selected = t.Key == parameter.GroupId;

                _needToRefreshWhenAppearing = true;
                Services.Navigation.NavigateDetailPage(typeof(SystemParameterPage));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void SettingTapped(int rowNumber)
        {
            GoToParameterPage(_filteredAndSearchedParams[rowNumber].BaseObject);
        }

        void AppearingExecute()
        {
            if (_needToRefreshWhenAppearing)
                SafeRefreshDataAsync();
        }

        #endregion

        #endregion

        #region "PRIVATE METHODS"

        void FilterAndSearchData()
        {
            Filter();
            Search();

            RaisePropertyChanged(nameof(ParametersData));
        }

        void Filter()
        {
            try
            {
                _filterDataSemaphore.WaitOne();

                _filteredParams = new List<ParamClass>();
                int number = 0;

                foreach (var p in _allParams.Where(p => (SelectedFilterItem == null || SelectedFilterItem.Key == AllGroupsFilterKey || SelectedFilterItem.Key == p.Name)))
                    foreach (var t in p.Collection.Where(s => s != null))
                        _filteredParams.Add(new ParamClass { BaseObject = t, Number = number++ });

                _filteredAndSearchedParams = _filteredParams;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            finally
            {
                _filterDataSemaphore.Release();
            }

        }

        void Search()
        {
            try
            {
                _searchDataSemaphore.WaitOne();

                if (String.IsNullOrEmpty(SearchText?.Trim()))
                    _filteredAndSearchedParams = _filteredParams;
                else
                {
                    string pattern = SearchText.ToLower().Trim();
                    _filteredAndSearchedParams = _filteredParams.Where(p => p.Description.ToLower().Contains(pattern) || p.Key.ToLower().Contains(pattern)).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            finally
            {
                _searchDataSemaphore.Release();
            }
        }

        DataTable CreateDataTableFromListOfParams(IList<ParamClass> list)
        {
            var parametersData = new DataTable();

            try
            {

                if (_columns == null)
                    _columns = new List<string>(typeof(ParamClass).GetProperties(
                        System.Reflection.BindingFlags.Public
                        | System.Reflection.BindingFlags.Instance
                        | System.Reflection.BindingFlags.DeclaredOnly).Select(p => p.Name));

                foreach (var c in _columns)
                    parametersData.Columns.Add(c);
              
                if (list != null)
                    foreach (var t in list)
                        parametersData.Rows.Add(t.Category, t.Key, t.Value, t.Description);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            return parametersData;
        }

        #endregion
    }
}
