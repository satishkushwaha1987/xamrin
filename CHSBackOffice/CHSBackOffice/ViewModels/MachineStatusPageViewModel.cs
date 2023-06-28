using CHBackOffice.ApiServices.ChsProxy;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Events;
using CHSBackOffice.Models;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using CHSBackOffice.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    internal class MachineStatusPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region "PUBLIC PROPS"

        public string ViewTypeText => Constants.CHSIcons.TopMenu;
        public string SortText => SortDesc ? Constants.CHSIcons.SortDesc : Constants.CHSIcons.SortAsc;

        #endregion

        #region "BINDABLE PROPS"

        #region "LegendColumn1"

        private List<MachineStatusLegendItem> _legendColumn1;
        public List<MachineStatusLegendItem> LegendColumn1
        {
            get => _legendColumn1;
            set => SetProperty(ref _legendColumn1, value);
        }

        #endregion

        #region "LegendColumn2"

        private List<MachineStatusLegendItem> _legendColumn2;
        public List<MachineStatusLegendItem> LegendColumn2
        {
            get => _legendColumn2;
            set => SetProperty(ref _legendColumn2, value);
        }

        #endregion

        #region "LegendColumn3"

        private List<MachineStatusLegendItem> _legendColumn3;
        public List<MachineStatusLegendItem> LegendColumn3
        {
            get => _legendColumn3;
            set => SetProperty(ref _legendColumn3, value);
        }

        #endregion

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

        #region "MachineStatuses"

        private ObservableCollection<Kiosk> _machineStatuses;
        public ObservableCollection<Kiosk> MachineStatuses
        {
            get => _machineStatuses;
            set
            {
                SetProperty(ref _machineStatuses, value);
                RaisePropertyChanged(nameof(MachineStatuses));
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

        #region "PRIVATE FIELDS"

        private Kiosk[] _kiosks;
        private bool _updateMachineStatusesThreadActive = true;
        private static Thread _refreshMachinesThread;
        private static bool _isPageActive;


        #endregion

        #region ".CTOR"

        public MachineStatusPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            #region "LegendColumn1 Init"

            if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                LegendColumn1 = new List<MachineStatusLegendItem>
                {
                    new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusNormal),
                        Caption = "Normal(Inservice/Online)"
                    },
                    new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusOffline),
                        Caption = "Offline"
                    },
                };
            else
                LegendColumn1 = new List<MachineStatusLegendItem>
                {
                    new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusNormal),
                        Caption = "Inservice/Online/Normal"
                    },
                    new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusWarning),
                        Caption = "Inservice/Online/Warning"
                    },
                    new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusSOP),
                        Caption = "SOP"
                    }
                };

            #endregion

            #region "LegendColumn2 Init"

            if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                LegendColumn2 = new List<MachineStatusLegendItem>
                {
                    new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusWarning),
                        Caption = "Warning(Inservice/Online)"
                    },
                new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusSOP),
                        Caption = "SOP"
                    }
                };
            else
                LegendColumn2 = new List<MachineStatusLegendItem>
                {
                    new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusCritical),
                        Caption = "Inservice/Online"
                    },
                    new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusOffline),
                        Caption = "Offline"
                    },
                    new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusOOS),
                        Caption = "OOS/Others"
                    }
                };

            #endregion

            #region "LegendColumn3 Init"

            if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                LegendColumn3 = new List<MachineStatusLegendItem>
                {
                    new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusCritical),
                        Caption = "Critical(Inservice/Online)"
                    },
                    new MachineStatusLegendItem
                    {
                        BoxColor = StaticResourceManager.GetColor(AppColor.StatusOOS),
                        Caption = "OOS/Others"
                    }
                };

            #endregion

            AddToolbarItemCommand("but1", new Action<CustomControls.ToolbarButton>(SearchPressed));

            //StartUpdateMachineStatusesThread();

            CommonViewObjects.MachinesIdNeedsToRefresh = new List<string>();
            if (Settings.AutoRefresh.BoolValue && _refreshMachinesThread == null)
            {
                _refreshMachinesThread = new Thread(RefreshNeedsMachines) { IsBackground = true };
                _refreshMachinesThread.Start();
            }


            SafeRefreshDataAsync();
        }

        #endregion

        #region "COMMANDS"

        public DelegateCommand<Kiosk> MachineTappedCommand => new DelegateCommand<Kiosk>(MachineTapped);
        public ICommand SearchCommand => new Xamarin.Forms.Command(SearchExecute);
        public ICommand AppearingCommand => new Xamarin.Forms.Command(AppearingExecute);
        public ICommand DisappearingCommand => new Xamarin.Forms.Command(DisappearingExecute);

        #region "COMMAND HANDLERS"

        private void MachineTapped(Kiosk machine)
        {
            CommonViewObjects.Instance.CurrentUnit = machine;
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                Services.Navigation.NavigateToDetailPageAsync(typeof(ATMInfoPage));
            else
                Services.Navigation.NavigateDetailPage(typeof(ATMInfoPage));
        }

        void SearchPressed(CustomControls.ToolbarButton button)
        {
            button?.Command?.Execute(null);
        }

        public virtual void AppearingExecute()
        {
            _updateMachineStatusesThreadActive = true;
            _isPageActive = Settings.AutoRefresh.BoolValue;
        }

        public virtual void DisappearingExecute()
        {
            _updateMachineStatusesThreadActive = false;
            _isPageActive = false;
        }

        #endregion

        #endregion

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                MachineStatuses = null;
            });

            _kiosks = await _serviceAgent.GetMachines(StateInfoService.SessionId);
            var _details = await _serviceAgent.GetKioskStatus(StateInfoService.SessionId);

            if (_details == null)
                _details = new Kiosk[0];

            if (_kiosks == null)
                _kiosks = new Kiosk[0];

            List<Tuple<string, int>> _machinesForUpdate = new List<Tuple<string, int>>();
            for (int i = 0; i < _kiosks.Length; i++)
            {
                var detailsInfo = _details.FirstOrDefault(kiosk => kiosk.Id == _kiosks[i].Id);
                if (detailsInfo != null)
                    _kiosks[i] = detailsInfo;
                else
                    _machinesForUpdate.Add(new Tuple<string, int>(_kiosks[i].Id, i));

                CommonViewObjects.MachinesIdNeedsToRefresh.Add(_kiosks[i].Id);

            }

            foreach (var t in _machinesForUpdate)
            {
                try
                {
                    var kioskDetails = await _serviceAgent.GetKioskDetails(StateInfoService.SessionId, t.Item1);
                    Kiosk.FillFromDetails(ref _kiosks[t.Item2], kioskDetails);
                }
                catch
                {
                    if (CommonViewObjects.MachinesIdNeedsToRefresh.Contains(t.Item1))
                        CommonViewObjects.MachinesIdNeedsToRefresh.Remove(t.Item1);
                }
            }


            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                MachineStatuses = new ObservableCollection<Kiosk>(_kiosks);
            });
        }
        
        async void RefreshNeedsMachines()
        {
            //int lastVal = 0;
            while (true)
            {
                await Task.Delay(Support.Constants.AutoRefreshInterval);
                if (Support.CommonViewObjects.Instance.IsLoadingVisible || _machineStatuses == null || !_isPageActive) continue;
                
                try
                {
                    
                    int count = CommonViewObjects.MachinesIdNeedsToRefresh.Count;
                    foreach (var t in CommonViewObjects.MachinesIdNeedsToRefresh)
                    {
                        var details = await _serviceAgent.GetKioskDetails(StateInfoService.SessionId, t);

                        

                        var index = _machineStatuses.IndexOf(_machineStatuses.FirstOrDefault(m => m.Id == t));

                        //if (index % 2 == 0 && details.BillAcceptor != null && details.BillAcceptor.Length > 0)
                        //{
                        //    details.BillAcceptor[0].BillsCount += lastVal;
                        //    details.BillAcceptor[0].TicketCount += lastVal;
                        //    details.BillAcceptor[0].Count = details.BillAcceptor[0].BillsCount + details.BillAcceptor[0].TicketCount;
                        //}

                        //Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        //{
                        //Kiosk.FillFromDetails(ref MachineStatuses[index], kioskDetails);
                        //don't work :( :(
                        MachineStatuses[index].BillAcceptor = details.BillAcceptor;
                            MachineStatuses[index].CashDispenser = details.CashDispenser;
                            MachineStatuses[index].CoinHopper = details.CoinHopper;
                            MachineStatuses[index].Events = details.Events;
                            MachineStatuses[index].Group = details.Group;
                            MachineStatuses[index].Id = details.Id;
                            MachineStatuses[index].IpAddress = details.IpAddress;
                            MachineStatuses[index].Keyword1 = details.Keyword1;
                            MachineStatuses[index].Keyword2 = details.Keyword2;
                            MachineStatuses[index].KioskType = details.KioskType;
                            MachineStatuses[index].LastCommunication = details.LastCommunication;
                            MachineStatuses[index].Location = details.Location;
                            MachineStatuses[index].MachineGroup = details.MachineGroup;
                            MachineStatuses[index].ParameterCollection = details.ParameterCollection;
                            MachineStatuses[index].ParameterCount = details.ParameterCount;
                            if (details.Property == null)
                                MachineStatuses[index].Property = null;
                            else
                            {
                                if (MachineStatuses[index].Property == null)
                                    MachineStatuses[index].Property = new KioskProperty();
                                MachineStatuses[index].Property.Id = details.Property.Id;
                                MachineStatuses[index].Property.Name = details.Property.Name;
                            }
                            MachineStatuses[index].RecyclerCassettes = details.RecyclerCassettes;
                            MachineStatuses[index].SerialNumber = details.SerialNumber;
                            MachineStatuses[index].State = details.State;
                            MachineStatuses[index].Status = details.Status;
                            MachineStatuses[index].TicketNumbers = details.TicketNumbers;
                            MachineStatuses[index].Version = details.Version;
                            MachineStatuses[index].Versions = details.Versions;



                       // });

                        await Task.Delay(10);
                    }
                    //lastVal+=10;
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
                //if (CommonViewObjects.MachinesIdNeedsToRefresh.Count >= count)
                //    CommonViewObjects.MachinesIdNeedsToRefresh.RemoveRange(0, count);
            }
        }


        #region "PRIVATE METHODS"

        private void StartUpdateMachineStatusesThread()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (_updateMachineStatusesThreadActive)
                        SafeRefreshDataSync();

                    await Task.Delay(Constants.IntervalTime);
                }
            });
        }

        void SearchExecute()
        {
            try
            {
                Func<Kiosk, bool> predicate = kiosk =>
                string.IsNullOrEmpty(SearchText)
                || kiosk.Id.ToUpper().Contains(SearchText.ToUpper());
                if (_kiosks != null)
                    MachineStatuses = new ObservableCollection<Kiosk>(
                        _kiosks.Where(predicate));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        
        #endregion
    }
}
