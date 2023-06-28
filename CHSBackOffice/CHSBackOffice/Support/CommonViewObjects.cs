using Acr.UserDialogs;
using CHSBackOffice.Models.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CHSBackOffice.Support
{
    class CommonViewObjects : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged(string nameOfProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameOfProperty));
        }

        void SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "")
        {
            backingStore = value;

            RaisePropertyChanged(propertyName);
        }
        #endregion

        public class Resolver
        {
            public bool Mark { get; set; }
        }

        public class ResolverButton { }
        public static CommonViewObjects Instance = new CommonViewObjects();

        private string _userFirstName;
        public string UserFirstName
        {
            get => _userFirstName;
            set => SetProperty(ref _userFirstName, value);
        }

        private string _userLastName;
        public string UserLastName
        {
            get => _userLastName;
            set => SetProperty(ref _userLastName, value);
        }

        private CHBackOffice.ApiServices.ChsProxy.Kiosk _currentUnit;
        public CHBackOffice.ApiServices.ChsProxy.Kiosk CurrentUnit
        {
            get => _currentUnit;
            set => SetProperty(ref _currentUnit, value);
        }

        private Support.Classes.TransactionDetailsClass _currentTransaction;
        public Support.Classes.TransactionDetailsClass CurrentTransaction
        {
            get => _currentTransaction;
            set => SetProperty(ref _currentTransaction, value);
        }

        private CHBackOffice.ApiServices.ChsProxy.Parameter1 _currentSystemParameter;
        public CHBackOffice.ApiServices.ChsProxy.Parameter1 CurrentSystemParameter
        {
            get => _currentSystemParameter;
            set => SetProperty(ref _currentSystemParameter, value);
        }

        private IEnumerable<PopoverItem> _allowedParameterGroups;
        public IEnumerable<PopoverItem> AllowedParameterGroups
        {
            get => _allowedParameterGroups;
            set => SetProperty(ref _allowedParameterGroups, value);
        }

        public List<string> ExistingParamNames;

        private bool _isLoadingVisible;
        public bool IsLoadingVisible
        {
            get => _isLoadingVisible || IsPageCreating;
            set => SetProperty(ref _isLoadingVisible, value);
        }


        private bool _isPageCreating;
        public bool IsPageCreating
        {
            get => _isPageCreating;
            set
            { 
                SetProperty(ref _isPageCreating, value);
                RaisePropertyChanged(nameof(IsLoadingVisible));
            }
        }

        internal int SelectedTransactionNumber;

        internal static List<string> MachinesIdNeedsToRefresh;

        internal static DateTime LastTimeoutException = DateTime.MinValue;
        internal static ToastConfig TimeoutAlertConfig = new ToastConfig(Resources.Resource.TimeoutAlertText) { 
            MessageTextColor = System.Drawing.Color.White,
            BackgroundColor = System.Drawing.Color.FromArgb(230, 0, 0), // RED - #e60000
            Duration = new TimeSpan(0, 0, 6) 
        };
    }
}
