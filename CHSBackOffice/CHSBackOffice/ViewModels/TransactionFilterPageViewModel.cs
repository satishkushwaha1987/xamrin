
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
    public class TransactionFilterPageViewModel : BindableBase
    {
        #region "BINDABLE PROPS"

        private ObservableCollection<Models.Popup.PopoverItem> _machineIDs;
        public ObservableCollection<Models.Popup.PopoverItem> MachineIDs
        {
            get => _machineIDs;
            set => SetProperty(ref _machineIDs, value);
        }

        private Models.Popup.PopoverItem _machineIdSelected;
        public Models.Popup.PopoverItem MachineIdSelected
        {
            get => _machineIdSelected;
            set
            {
                SetProperty(ref _machineIdSelected, value);

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
        }

        private int _checkedIndex;
        public int CheckedIndex
        {
            get => _checkedIndex;
            set => SetProperty(ref _checkedIndex, value);
        }

        private DateTime _startDateTime;
        public DateTime StartDateTime
        {
            get => _startDateTime;
            set => SetProperty(ref _startDateTime, value);
        }

        private DateTime _endDateTime;
        public DateTime EndDateTime
        {
            get => _endDateTime;
            set => SetProperty(ref _endDateTime, value);
        }

        private string _transactionId;
        public string TransactionId
        {
            get => _transactionId;
            set => SetProperty(ref _transactionId, value);
        }

        private string _sequenceId;
        public string SequenceId
        {
            get => _sequenceId;
            set => SetProperty(ref _sequenceId, value);
        }

        private ObservableCollection<Models.Popup.PopoverItem> _transactionTypes;
        public ObservableCollection<Models.Popup.PopoverItem> TransactionTypes
        {
            get => _transactionTypes;
            set => SetProperty(ref _transactionTypes, value);
        }

        private Models.Popup.PopoverItem _transactionTypeSelected;
        public Models.Popup.PopoverItem TransactionTypeSelected
        {
            get => _transactionTypeSelected;
            set
            {
                try
                {
                    SetProperty(ref _transactionTypeSelected, value);

                    if (_transactionTypeSelected != null && _transactionTypeSelected.Selected == true)
                    {
                        _transactionTypeSelected.Selected = false;
                        var source = PopoverData.Instance.TransactionTypes.ToList().Find(x => x.Equals(value));
                        source.Selected = false;
                    }
                    else if (_transactionTypeSelected != null && _transactionTypeSelected.Selected == false)
                    {
                        _transactionTypeSelected.Selected = true;
                        var source = PopoverData.Instance.TransactionTypes.ToList().Find(x => x.Equals(value));
                        source.Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
        }

        private ObservableCollection<Models.Popup.PopoverItem> _transactionStatuses;
        public ObservableCollection<Models.Popup.PopoverItem> TransactionStatuses
        {
            get => _transactionStatuses;
            set => SetProperty(ref _transactionStatuses, value);
        }

        private Models.Popup.PopoverItem _transactionStatusSelected;
        public Models.Popup.PopoverItem TransactionStatusSelected
        {
            get => _transactionStatusSelected;
            set
            {
                try
                {
                    SetProperty(ref _transactionStatusSelected, value);

                    if (_transactionStatusSelected != null && _transactionStatusSelected.Selected == true)
                    {
                        _transactionStatusSelected.Selected = false;
                        var source = PopoverData.Instance.TransactionStatuses.ToList().Find(x => x.Equals(value));
                        source.Selected = false;
                    }
                    else if (_transactionStatusSelected != null && _transactionStatusSelected.Selected == false)
                    {
                        _transactionStatusSelected.Selected = true;
                        var source = PopoverData.Instance.TransactionStatuses.ToList().Find(x => x.Equals(value));
                        source.Selected = true;
                    }
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
        }

        #endregion

        #region ".CTOR"

        public TransactionFilterPageViewModel()
        {
            MachineIDs = PopoverData.Instance.MachineIDs;
            TransactionTypes = PopoverData.Instance.TransactionTypes;
            TransactionStatuses = PopoverData.Instance.TransactionStatuses;
            TransactionId = PopoverData.Instance.TransactionIdSelected;
            SequenceId = PopoverData.Instance.SequenceIdSelected;
            CheckedIndex = PopoverData.Instance.CheckedIndexRadioButtonGroup;

        }

        #endregion

        #region "COMMANDS"

        public ICommand SubmitCommand => new Command(SubmitPressed);
        public ICommand ResetCommand => new Command(ResetPressed);

        #region "COMMAND HANDLERS"

        void SubmitPressed()
        {
            PopoverData.Instance.TransactionIdSelected = TransactionId;
            PopoverData.Instance.SequenceIdSelected = SequenceId;
            PopoverData.Instance.FilterAction = true;
            PopoverData.Instance.StartDateTimeSelected = StartDateTime;
            PopoverData.Instance.EndDateTimeSelected = EndDateTime;
            PopoverData.Instance.CheckedIndexRadioButtonGroup = CheckedIndex;

            Services.Navigation.GoBack();
        }

        void ResetPressed()
        {
            try
            {
                PopoverData.Instance.FilterAction = true;
                PopoverData.Instance.MachineIDs.Where(x => x.Selected == true).ForEach((item) => { item.Selected = false; });
                PopoverData.Instance.TransactionTypes.Where(x => x.Selected == true).ForEach((item) => { item.Selected = false; });
                PopoverData.Instance.TransactionStatuses.Where(x => x.Selected == true).ForEach((item) => { item.Selected = false; });
                PopoverData.Instance.TransactionIdSelected = string.Empty;
                PopoverData.Instance.SequenceIdSelected = string.Empty;
                PopoverData.Instance.CheckedIndexRadioButtonGroup = 2;
                PopoverData.Instance.StartDateTimeSelected = DateTime.MinValue;
                PopoverData.Instance.EndDateTimeSelected = DateTime.MinValue;

                Services.Navigation.GoBack();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #endregion

     
    }
}
