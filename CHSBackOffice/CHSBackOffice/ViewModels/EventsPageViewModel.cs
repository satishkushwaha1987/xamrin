using CHBackOffice.ApiServices.ChsProxy;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.Database;
using CHSBackOffice.Models.Popup;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using CHSBackOffice.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CHSBackOffice.ViewModels
{
    internal class EventsPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region Fields
        private static BaseMatcher<CHBackOffice.ApiServices.ChsProxy.SeverityType, string> severityTypeToColorMatcher;
        #endregion

        #region "BINDABLE PROPS"

        #region "Events"

        private ObservableCollection<Event> _events;
        public ObservableCollection<Event> Events
        {
            get => _events;
            set
            {
                SetProperty(ref _events, value);
                RaisePropertyChanged(nameof(Events));
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
                RaisePropertyChanged(nameof(IsNoDataLabelVisible));
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

        private Event[] events;
        private Event[] filteredEvents;
        private readonly List<Func<Event, PopupParameters>> PopupSorces = new List<Func<Event, PopupParameters>>
        {
            UpdateEventDetailsParams
        };

        #endregion

        #region ".CTOR"

        public EventsPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            AddToolbarItemCommand("but1", new Action<CustomControls.ToolbarButton>(CommandPressed));
            AddToolbarItemCommand("but2", new Action<CustomControls.ToolbarButton>(FilterPressed));
            severityTypeToColorMatcher = FactoryMatcher.GetSeverityTypeToColorMatcher();
            SafeRefreshDataAsync();
        }

        #endregion

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            Events = null;
            IsNoDataLabelVisible = false;

            events = await _serviceAgent.GetAllEvents(StateInfoService.SessionId);
            if (events != null && events.Length > 0)
            {
                await Task.Delay(1000);

                Events = new ObservableCollection<Event>(events);

                PopoverData.Instance.MachineIDs = GetMachineIDs(events);
                PopoverData.Instance.EventDescriptions = GetEventDescriptions(events);
                PopoverData.Instance.MachineStatuses = GetMachineStatuses(events);
                PopoverData.Instance.EventSeverites = GetEventSeverites(events);
                PopoverData.Instance.MachineStates = GetMachineStates(events);
                PopoverData.Instance.MachineTypes = GetMachineTypes(events);
            }
            else
                IsNoDataLabelVisible = true;
        }

        #endregion

        #region "COMMANDS"

        public DelegateCommand<Event> EventTappedCommand => new DelegateCommand<Event>(EventTapped);
        public ICommand SearchCommand => new Xamarin.Forms.Command(SearchExecute);

        public ICommand AppearingCommand => new Xamarin.Forms.Command(AppearingExecute);

        #region "COMMAND HANDLERS"

        void EventTapped(Event eventItem)
        {
            PopupData.Instance.PopupParameters = PopupSorces[0](eventItem);
        }

        void SearchExecute()
        {
            try
            {
                if (filteredEvents == null && events != null)
                    filteredEvents = events.ToArray();

                if (filteredEvents != null)
                    Events = new ObservableCollection<Event>(
                        filteredEvents.Where(
                            e =>
                             string.IsNullOrEmpty(SearchText) ||
                             e.Unit.Id.ToUpper().Contains(SearchText.ToUpper()) ||
                             e.Description.ToUpper().Contains(SearchText.ToUpper())
                            ));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void AppearingExecute()
        {
            FilterData();
        }

        void CommandPressed(CustomControls.ToolbarButton button)
        {
            button?.Command?.Execute(null);
        }

        void FilterPressed(CustomControls.ToolbarButton button)
        {
            Services.Navigation.NavigateDetailPage(typeof(EventsFilterPage));
        }

        #endregion

        #endregion

        #region "PRIVATE METHODS"

        #region "Get PopoverData.Instance Parameters"

        private ObservableCollection<PopoverItem> GetMachineIDs(Event[] events)
        {
            try
            {
                return new ObservableCollection<PopoverItem>(events.Select(x => new PopoverItem
                {
                    Key = x.Unit.Id,
                    Value = x.Unit.Id,
                    Selected = false
                })
                .Distinct(new PopoverItemEqualityComparer())
                .OrderBy(x => x.Key));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
            
        }

        private ObservableCollection<PopoverItem> GetEventDescriptions(Event[] events)
        {
            try
            {
                return new ObservableCollection<PopoverItem>(events.Select(x => new PopoverItem
                {
                    Key = x.Description,
                    Value = x.Description,
                    Selected = false
                })
                .Distinct(new PopoverItemEqualityComparer())
                .OrderBy(x => x.Key));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null; 
            }
            
        }

        private ObservableCollection<PopoverItem> GetMachineStatuses(Event[] events)
        {
            try
            {
                return new ObservableCollection<PopoverItem>(events.Select(x => new PopoverItem
                {
                    Key = x.Unit.Status.ToString(),
                    Value = x.Unit.Status.ToString(),
                    Selected = false
                })
                .Distinct(new PopoverItemEqualityComparer())
                .OrderBy(x => x.Key));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        private ObservableCollection<PopoverItem> GetEventSeverites(Event[] events)
        {
            try
            {
                return new ObservableCollection<PopoverItem>(events.Select(x => new PopoverItem
                {
                    Key = x.Severity.ToString(),
                    Value = x.Severity.ToString(),
                    Selected = false
                })
                .Distinct(new PopoverItemEqualityComparer())
                .OrderBy(x => x.Key));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        private ObservableCollection<PopoverItem> GetMachineStates(Event[] events)
        {
            try
            {
                return new ObservableCollection<PopoverItem>(events.Select(x => new PopoverItem
                {
                    Key = x.Unit.State.ToString(),
                    Value = x.Unit.State.ToString(),
                    Selected = false
                })
                .Distinct(new PopoverItemEqualityComparer())
                .OrderBy(x => x.Key));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        private ObservableCollection<PopoverItem> GetMachineTypes(Event[] events)
        {
            try
            {
                return new ObservableCollection<PopoverItem>(events.Select(x => new PopoverItem
                {
                    Key = x.Unit.KioskType.ToString(),
                    Value = x.Unit.KioskType.ToString(),
                    Selected = false
                })
                .Distinct(new PopoverItemEqualityComparer())
                .OrderBy(x => x.Key));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        #endregion

        void FilterData()
        {
            try
            {
                if (events == null)
                    return;

                Expression<Func<Event, bool>> predicate = null;

                #region "Build Predicate"

                #region  "build selected MachineIDs predicate"

                if (PopoverData.Instance.MachineIDs != null)
                {
                    var selected = PopoverData.Instance.MachineIDs.Where(x => x.Selected == true);
                    if (selected.Count() > 0)
                    {
                        Expression<Func<Event, bool>> predicate2 = null;
                        foreach (var item in selected)
                        {
                            if (predicate2 != null)
                            {
                                var predicate3 = predicate2;
                                predicate2 = @event =>
                                    @event.Unit.Id.ToUpper().Contains(item.Key.ToUpper());
                                predicate2 = PredicateBuilder.Or(predicate2, predicate3);
                            }
                            else
                            {
                                predicate2 = @event =>
                                       @event.Unit.Id.ToUpper().Contains(item.Key.ToUpper());

                            }
                        }
                        if (predicate != null)
                        {

                            predicate = PredicateBuilder.And(predicate, predicate2);
                        }
                        else
                        {
                            predicate = predicate2;

                        }
                    }
                }

                #endregion

                #region  "build selected EventDescriptions predicate"

                if (PopoverData.Instance.EventDescriptions != null)
                {
                    var selected = PopoverData.Instance.EventDescriptions.Where(x => x.Selected == true);
                    if (selected.Count() > 0)
                    {
                        Expression<Func<Event, bool>> predicate2 = null;
                        foreach (var item in selected)
                        {
                            if (predicate2 != null)
                            {
                                var predicate3 = predicate2;
                                predicate2 = @event =>
                                    @event.Description.ToUpper().Contains(item.Key.ToUpper());
                                predicate2 = PredicateBuilder.Or(predicate2, predicate3);
                            }
                            else
                            {
                                predicate2 = @event =>
                                       @event.Description.ToUpper().Contains(item.Key.ToUpper());

                            }
                        }

                        if (predicate != null)
                        {

                            predicate = PredicateBuilder.And(predicate, predicate2);
                        }
                        else
                        {
                            predicate = predicate2;

                        }
                    }
                }

                #endregion

                #region "build selected MachineStatuses predicate"

                if (PopoverData.Instance.MachineStatuses != null)
                {
                    var selected = PopoverData.Instance.MachineStatuses.Where(x => x.Selected == true);
                    if (selected.Count() > 0)
                    {
                        Expression<Func<Event, bool>> predicate2 = null;
                        foreach (var item in selected)
                        {
                            if (predicate2 != null)
                            {
                                var predicate3 = predicate2;
                                predicate2 = @event =>
                                    @event.Unit.Status.ToString().ToUpper().Contains(item.Key.ToUpper());
                                predicate2 = PredicateBuilder.Or(predicate2, predicate3);
                            }
                            else
                            {
                                predicate2 = @event =>
                                       @event.Unit.Status.ToString().ToUpper().Contains(item.Key.ToUpper());

                            }
                        }

                        if (predicate != null)
                        {

                            predicate = PredicateBuilder.And(predicate, predicate2);
                        }
                        else
                        {
                            predicate = predicate2;

                        }
                    }
                }

                #endregion

                #region "build selected EventSeverites predicate"

                if (PopoverData.Instance.EventSeverites != null)
                {
                    var selected = PopoverData.Instance.EventSeverites.Where(x => x.Selected == true);
                    if (selected.Count() > 0)
                    {
                        Expression<Func<Event, bool>> predicate2 = null;
                        foreach (var item in selected)
                        {
                            if (predicate2 != null)
                            {
                                var predicate3 = predicate2;
                                predicate2 = @event =>
                                    @event.Severity.ToString().ToUpper().Contains(item.Key.ToUpper());
                                predicate2 = PredicateBuilder.Or(predicate2, predicate3);
                            }
                            else
                            {
                                predicate2 = @event =>
                                       @event.Severity.ToString().ToUpper().Contains(item.Key.ToUpper());

                            }
                        }

                        if (predicate != null)
                        {

                            predicate = PredicateBuilder.And(predicate, predicate2);
                        }
                        else
                        {
                            predicate = predicate2;

                        }
                    }
                }

                #endregion

                #region "build selected MachineStates predicate"

                if (PopoverData.Instance.MachineStates != null)
                {
                    var selected = PopoverData.Instance.MachineStates.Where(x => x.Selected == true);
                    if (selected.Count() > 0)
                    {
                        Expression<Func<Event, bool>> predicate2 = null;
                        foreach (var item in selected)
                        {
                            if (predicate2 != null)
                            {
                                var predicate3 = predicate2;
                                predicate2 = @event =>
                                    @event.Unit.State.ToString().ToUpper().Contains(item.Key.ToUpper());
                                predicate2 = PredicateBuilder.Or(predicate2, predicate3);
                            }
                            else
                            {
                                predicate2 = @event =>
                                       @event.Unit.State.ToString().ToUpper().Contains(item.Key.ToUpper());

                            }
                        }

                        if (predicate != null)
                        {

                            predicate = PredicateBuilder.And(predicate, predicate2);
                        }
                        else
                        {
                            predicate = predicate2;

                        }
                    }
                }

                #endregion

                #region "build selected MachineTypes predicate"

                if (PopoverData.Instance.MachineTypes != null)
                {
                    var selected = PopoverData.Instance.MachineTypes.Where(x => x.Selected == true);
                    if (selected.Count() > 0)
                    {
                        Expression<Func<Event, bool>> predicate2 = null;
                        foreach (var item in selected)
                        {
                            if (predicate2 != null)
                            {
                                var predicate3 = predicate2;
                                predicate2 = @event =>
                                    @event.Unit.KioskType.ToString().ToUpper().Contains(item.Key.ToUpper());
                                predicate2 = PredicateBuilder.Or(predicate2, predicate3);
                            }
                            else
                            {
                                predicate2 = @event =>
                                       @event.Unit.KioskType.ToString().ToUpper().Contains(item.Key.ToUpper());

                            }
                        }

                        if (predicate != null)
                        {

                            predicate = PredicateBuilder.And(predicate, predicate2);
                        }
                        else
                        {
                            predicate = predicate2;

                        }
                    }
                }

                #endregion

                #endregion

                if (predicate != null)
                {
                    filteredEvents = events.Where(predicate.Compile()).ToArray();
                }
                else
                {
                    filteredEvents = events;
                }
                Events = new ObservableCollection<Event>(filteredEvents);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private static PopupParameters UpdateEventDetailsParams(Event eventItem)
        {
            try
            {
                return new PopupParameters
                {
                    Title = "Event Details",
                    Status = severityTypeToColorMatcher.ToValue(eventItem.Severity),
                    TitleBackground = StaticResourceManager.GetColor(severityTypeToColorMatcher.ToValue(eventItem.Severity)),
                    CanChangeValues = false,
                    Rows = new List<PopupRow>
                    {
                        new PopupRow
                        {
                            Title = Resources.Resource.Unit,
                            Value = eventItem.Unit.Id,
                            ReadOnly = true
                        },
                        new PopupRow
                        {
                            Title = Resources.Resource.ErrorCode,
                            Value = eventItem.ErrorCode,
                            ReadOnly = true
                        },
                        new PopupRow
                        {
                            Title = Resources.Resource.Status,
                            Value = eventItem.Unit.Status,
                            ReadOnly = true
                        },
                        new PopupRow
                        {
                            Title = Resources.Resource.EventDescription,
                            Value = eventItem.Description,
                            ReadOnly = true,
                            TextLineBreakMode = Xamarin.Forms.LineBreakMode.WordWrap
                        },
                        new PopupRow
                        {
                            Title = Resources.Resource.EventTimestamp,
                            Value = DateTimeExtensions.DateTimeStrToNewFormat(eventItem.EventDate,"MM/dd/yyyy hh:mm tt"),
                            ReadOnly = true
                        },
                        new PopupRow
                        {
                            Title = Resources.Resource.Definition,
                            Value = eventItem.Definition,
                            ReadOnly = true
                        },
                        new PopupRow
                        {
                            Title = Resources.Resource.Comments,
                            Value = eventItem.Comments,
                            ReadOnly = true
                        },
                        new PopupRow
                        {
                            Title = Resources.Resource.CauseOfError,
                            Value = eventItem.Cause,
                            ReadOnly = true,
                            TextLineBreakMode = Xamarin.Forms.LineBreakMode.WordWrap
                        },
                        new PopupRow
                        {
                            Title = Resources.Resource.Service,
                            Value = eventItem.Service,
                            ReadOnly = true
                        },
                        new PopupRow
                        {
                            Title = Resources.Resource.EmailNotyfyStatus,
                            Value = eventItem.NotificationStatus,
                            ReadOnly = true
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        #endregion
    }
}
