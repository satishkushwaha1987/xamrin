using CHBackOffice.ApiServices.ChsProxy;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Models;
using CHSBackOffice.Support;
using CHSBackOffice.Support.Classes;
using CHSBackOffice.Support.StaticResources;
using CHSBackOffice.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    internal class MachineStatusSkiaPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region "PRIVATE FIELDS"

        private KioskExtended[] _kiosks;
        private bool _updateMachineStatusesThreadActive = true;
        private static Thread _refreshMachinesThread;
        private static bool _isPageActive;


        #endregion

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
        private ObservableCollection<KioskExtended> _machineStatuses;
        public ObservableCollection<KioskExtended> MachineStatuses
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

        #region ".CTOR"

        public MachineStatusSkiaPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
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

        public DelegateCommand<KioskExtended> MachineTappedCommand => new DelegateCommand<KioskExtended>(MachineTapped);
        public ICommand SearchCommand => new Xamarin.Forms.Command(SearchExecute);
        public ICommand AppearingCommand => new Xamarin.Forms.Command(AppearingExecute);
        public ICommand DisappearingCommand => new Xamarin.Forms.Command(DisappearingExecute);

        #endregion

        #region "COMMAND HANDLERS"

        private void MachineTapped(KioskExtended machine)
        {
            CommonViewObjects.Instance.CurrentUnit = machine.BaseObject;
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

        void SearchExecute()
        {
            try
            {
                Func<KioskExtended, bool> predicate = kiosk =>
                string.IsNullOrEmpty(SearchText)
                || kiosk.BaseObject.Id.ToUpper().Contains(SearchText.ToUpper());
                if (_kiosks != null)
                    MachineStatuses = new ObservableCollection<KioskExtended>(
                        _kiosks.Where(predicate));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region Methods
        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            var kiosks = await _serviceAgent.GetMachines(StateInfoService.SessionId);
            var details = await _serviceAgent.GetKioskStatus(StateInfoService.SessionId);

            if (details == null)
                details = new Kiosk[0];

            if (kiosks == null)
                kiosks = new Kiosk[0];

            List<Tuple<string, int>> _machinesForUpdate = new List<Tuple<string, int>>();
            for (int i = 0; i < kiosks.Length; i++)
            {
                var detailsInfo = details.FirstOrDefault(kiosk => kiosk.Id == kiosks[i].Id);
                if (detailsInfo != null)
                    kiosks[i] = detailsInfo;
                else
                    _machinesForUpdate.Add(new Tuple<string, int>(kiosks[i].Id, i));

                CommonViewObjects.MachinesIdNeedsToRefresh.Add(kiosks[i].Id);

            }

            foreach (var t in _machinesForUpdate)
            {
                try
                {
                    var kioskDetails = await _serviceAgent.GetKioskDetails(StateInfoService.SessionId, t.Item1);
                    Kiosk.FillFromDetails(ref kiosks[t.Item2], kioskDetails);
                }
                catch
                {
                    if (CommonViewObjects.MachinesIdNeedsToRefresh.Contains(t.Item1))
                        CommonViewObjects.MachinesIdNeedsToRefresh.Remove(t.Item1);
                }
            }

            var listOfMachines = new List<KioskExtended>();

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => 
            {
                foreach (var k in kiosks)
                    listOfMachines.Add(new KioskExtended { BaseObject = k });

                _kiosks = listOfMachines.ToArray();
                MachineStatuses = new ObservableCollection<KioskExtended>(listOfMachines);
            });
        }

        async void RefreshNeedsMachines()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
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



                        var index = _machineStatuses.IndexOf(_machineStatuses.FirstOrDefault(m => m.BaseObject.Id == t));

                        //autoincrement for test
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
                        MachineStatuses[index].BaseObject.BillAcceptor = details.BillAcceptor;
                        MachineStatuses[index].BaseObject.CashDispenser = details.CashDispenser;
                        MachineStatuses[index].BaseObject.CoinHopper = details.CoinHopper;
                        MachineStatuses[index].BaseObject.Events = details.Events;
                        MachineStatuses[index].BaseObject.Group = details.Group;
                        MachineStatuses[index].BaseObject.Id = details.Id;
                        MachineStatuses[index].BaseObject.IpAddress = details.IpAddress;
                        MachineStatuses[index].BaseObject.Keyword1 = details.Keyword1;
                        MachineStatuses[index].BaseObject.Keyword2 = details.Keyword2;
                        MachineStatuses[index].BaseObject.KioskType = details.KioskType;
                        MachineStatuses[index].BaseObject.LastCommunication = details.LastCommunication;
                        MachineStatuses[index].BaseObject.Location = details.Location;
                        MachineStatuses[index].BaseObject.MachineGroup = details.MachineGroup;
                        MachineStatuses[index].BaseObject.ParameterCollection = details.ParameterCollection;
                        MachineStatuses[index].BaseObject.ParameterCount = details.ParameterCount;
                        if (details.Property == null)
                            MachineStatuses[index].BaseObject.Property = null;
                        else
                        {
                            if (MachineStatuses[index].BaseObject.Property == null)
                                MachineStatuses[index].BaseObject.Property = new KioskProperty();
                            MachineStatuses[index].BaseObject.Property.Id = details.Property.Id;
                            MachineStatuses[index].BaseObject.Property.Name = details.Property.Name;
                        }
                        MachineStatuses[index].BaseObject.RecyclerCassettes = details.RecyclerCassettes;
                        MachineStatuses[index].BaseObject.SerialNumber = details.SerialNumber;
                        MachineStatuses[index].BaseObject.State = details.State;
                        MachineStatuses[index].BaseObject.Status = details.Status;
                        MachineStatuses[index].BaseObject.TicketNumbers = details.TicketNumbers;
                        MachineStatuses[index].BaseObject.Version = details.Version;
                        MachineStatuses[index].BaseObject.Versions = details.Versions;



                        // });

                        await Task.Delay(10);
                    }
                    //lastVal+=10;

                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    Debug.WriteLine(elapsedMs.ToString());
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
                //if (CommonViewObjects.MachinesIdNeedsToRefresh.Count >= count)
                //    CommonViewObjects.MachinesIdNeedsToRefresh.RemoveRange(0, count);
            }
        }
        #endregion

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

        #endregion

    }
}
