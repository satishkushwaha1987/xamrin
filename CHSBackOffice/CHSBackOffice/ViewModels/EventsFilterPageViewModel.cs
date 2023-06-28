using CHSBackOffice.Support;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CHSBackOffice.ViewModels
{
    public class EventsFilterPageViewModel: BindableBase
    {
        public ICommand SubmitCommand => new Command(SubmitPressed);
        public ICommand ResetCommand => new Command(ResetPressed);

        private ObservableCollection<Models.Popup.PopoverItem> _machineIDs;
        public ObservableCollection<Models.Popup.PopoverItem> MachineIDs
        {
            get => _machineIDs;
            set
            {
                SetProperty(ref _machineIDs, value);
                RaisePropertyChanged(nameof(MachineIDs));
            }
        }

        private Models.Popup.PopoverItem _machineIdSelected;
        public Models.Popup.PopoverItem MachineIdSelected
        {
            get => _machineIdSelected;
            set
            {
                try
                {
                    SetProperty(ref _machineIdSelected, value);
                    RaisePropertyChanged(nameof(MachineIdSelected));

                    if (_machineIdSelected != null && _machineIdSelected.Selected == true)
                    {
                        _machineIdSelected.Selected = false;
                        var source = PopoverData.Instance.MachineIDs.ToList().Find(x => x.Equals(value));
                        source.Selected = false;
                    }
                    else if (_machineIdSelected != null && _machineIdSelected.Selected == false)
                    {
                        _machineIdSelected.Selected = true;
                        var source = PopoverData.Instance.MachineIDs.ToList().Find(x => x.Equals(value));
                        source.Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _eventDescriptions;
        public ObservableCollection<Models.Popup.PopoverItem> EventDescriptions
        {
            get => _eventDescriptions;
            set
            {
                SetProperty(ref _eventDescriptions, value);
                RaisePropertyChanged(nameof(EventDescriptions));
            }
        }

        private Models.Popup.PopoverItem _eventDescriptionSelected;
        public Models.Popup.PopoverItem EventDescriptionSelected
        {
            get => _eventDescriptionSelected;
            set
            {
                try
                {
                    SetProperty(ref _eventDescriptionSelected, value);
                    RaisePropertyChanged(nameof(EventDescriptionSelected));

                    if (_eventDescriptionSelected != null && _eventDescriptionSelected.Selected == true)
                    {
                        _eventDescriptionSelected.Selected = false;
                        var source = PopoverData.Instance.EventDescriptions.ToList().Find(x => x.Equals(value));
                        source.Selected = false;
                    }
                    else if (_eventDescriptionSelected != null && _eventDescriptionSelected.Selected == false)
                    {
                        _eventDescriptionSelected.Selected = true;
                        var source = PopoverData.Instance.EventDescriptions.ToList().Find(x => x.Equals(value));
                        source.Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _machineStatuses;
        public ObservableCollection<Models.Popup.PopoverItem> MachineStatuses
        {
            get => _machineStatuses;
            set
            {
                SetProperty(ref _machineStatuses, value);
                RaisePropertyChanged(nameof(MachineStatuses));
            }
        }

        private Models.Popup.PopoverItem _machineStatusSelected;
        public Models.Popup.PopoverItem MachineStatusSelected
        {
            get => _machineStatusSelected;
            set
            {
                try
                {
                    SetProperty(ref _machineStatusSelected, value);
                    RaisePropertyChanged(nameof(MachineStatusSelected));

                    if (_machineStatusSelected != null && _machineStatusSelected.Selected == true)
                    {
                        _machineStatusSelected.Selected = false;
                        var source = PopoverData.Instance.MachineStatuses.ToList().Find(x => x.Equals(value));
                        source.Selected = false;
                    }
                    else if (_machineStatusSelected != null && _machineStatusSelected.Selected == false)
                    {
                        _machineStatusSelected.Selected = true;
                        var source = PopoverData.Instance.MachineStatuses.ToList().Find(x => x.Equals(value));
                        source.Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _eventSeverites;
        public ObservableCollection<Models.Popup.PopoverItem> EventSeverites
        {
            get => _eventSeverites;
            set
            {
                SetProperty(ref _eventSeverites, value);
                RaisePropertyChanged(nameof(EventSeverites));
            }
        }

        private Models.Popup.PopoverItem _eventSeveritySelected;
        public Models.Popup.PopoverItem EventSeveritySelected
        {
            get => _eventSeveritySelected;
            set
            {
                try
                {
                    SetProperty(ref _eventSeveritySelected, value);
                    RaisePropertyChanged(nameof(EventSeveritySelected));

                    if (_eventSeveritySelected != null && _eventSeveritySelected.Selected == true)
                    {
                        _eventSeveritySelected.Selected = false;
                        var source = PopoverData.Instance.EventSeverites.ToList().Find(x => x.Equals(value));
                        source.Selected = false;
                    }
                    else if (_eventSeveritySelected != null && _eventSeveritySelected.Selected == false)
                    {
                        _eventSeveritySelected.Selected = true;
                        var source = PopoverData.Instance.EventSeverites.ToList().Find(x => x.Equals(value));
                        source.Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _machineStates;
        public ObservableCollection<Models.Popup.PopoverItem> MachineStates
        {
            get => _machineStates;
            set
            {
                SetProperty(ref _machineStates, value);
                RaisePropertyChanged(nameof(MachineStates));
            }
        }

        private Models.Popup.PopoverItem _machineStateSelected;
        public Models.Popup.PopoverItem MachineStateSelected
        {
            get => _machineStateSelected;
            set
            {
                try
                {
                    SetProperty(ref _machineStateSelected, value);
                    RaisePropertyChanged(nameof(MachineStateSelected));

                    if (_machineStateSelected != null && _machineStateSelected.Selected == true)
                    {
                        _machineStateSelected.Selected = false;
                        var source = PopoverData.Instance.MachineStates.ToList().Find(x => x.Equals(value));
                        source.Selected = false;
                    }
                    else if (_machineStateSelected != null && _machineStateSelected.Selected == false)
                    {
                        _machineStateSelected.Selected = true;
                        var source = PopoverData.Instance.MachineStates.ToList().Find(x => x.Equals(value));
                        source.Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                } 
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _machineTypes;
        public ObservableCollection<Models.Popup.PopoverItem> MachineTypes
        {
            get => _machineTypes;
            set
            {
                SetProperty(ref _machineTypes, value);
                RaisePropertyChanged(nameof(MachineTypes));
            }
        }

        private Models.Popup.PopoverItem _machineTypeSelected;
        public Models.Popup.PopoverItem MachineTypeSelected
        {
            get => _machineTypeSelected;
            set
            {
                try
                {
                    SetProperty(ref _machineTypeSelected, value);
                    RaisePropertyChanged(nameof(MachineTypeSelected));

                    if (_machineTypeSelected != null && _machineTypeSelected.Selected == true)
                    {
                        _machineTypeSelected.Selected = false;
                        var source = PopoverData.Instance.MachineTypes.ToList().Find(x => x.Equals(value));
                        source.Selected = false;
                    }
                    else if (_machineTypeSelected != null && _machineTypeSelected.Selected == false)
                    {
                        _machineTypeSelected.Selected = true;
                        var source = PopoverData.Instance.MachineTypes.ToList().Find(x => x.Equals(value));
                        source.Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
        }

        public EventsFilterPageViewModel()
        {
            Init();
        }

        void Init()
        {
            MachineIDs = PopoverData.Instance.MachineIDs;
            EventDescriptions = PopoverData.Instance.EventDescriptions;           
            MachineStatuses = PopoverData.Instance.MachineStatuses;       
            EventSeverites = PopoverData.Instance.EventSeverites;      
            MachineStates = PopoverData.Instance.MachineStates;
            MachineTypes = PopoverData.Instance.MachineTypes;
        }

        void ResetPressed()
        {
            try
            {
                PopoverData.Instance.MachineIDs.Where(x => x.Selected == true).ForEach((item) => { item.Selected = false; });
                PopoverData.Instance.EventDescriptions.Where(x => x.Selected == true).ForEach((item) => { item.Selected = false; });
                PopoverData.Instance.MachineStatuses.Where(x => x.Selected == true).ForEach((item) => { item.Selected = false; });
                PopoverData.Instance.EventSeverites.Where(x => x.Selected == true).ForEach((item) => { item.Selected = false; });
                PopoverData.Instance.MachineStates.Where(x => x.Selected == true).ForEach((item) => { item.Selected = false; });
                PopoverData.Instance.MachineTypes.Where(x => x.Selected == true).ForEach((item) => { item.Selected = false; });
                Services.Navigation.GoBack();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void SubmitPressed()
        {
            Services.Navigation.GoBack();
        }
    }
}
