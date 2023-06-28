using Acr.UserDialogs;
using CHBackOffice.ApiServices.Interfaces;
using CHBackOffice.Service;
using CHSBackOffice.Database;
using CHSBackOffice.Events;
using CHSBackOffice.Extensions;
using CHSBackOffice.Models.Popup;
using CHSBackOffice.Support;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    internal class SOPUsersPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region "PUBLIC PROPS"
        private CHBackOffice.ApiServices.ChsProxy.SOPUser userEditable;
        private CHBackOffice.ApiServices.ChsProxy.SOPUser[] users;
        private readonly List<Func<CHBackOffice.ApiServices.ChsProxy.SOPUser, PopupParameters>> PopupSorces = new List<Func<CHBackOffice.ApiServices.ChsProxy.SOPUser, PopupParameters>>
        {
            UpdateUserDetailsParams
        };

        public static Dictionary<OperationResult, string> Messages { get; set; } = new Dictionary<OperationResult, string>
        {
            { OperationResult.Success, Resources.Resource.PasswordChanged },
            { OperationResult.Invalid, Resources.Resource.InvalidCurrentPassword }
        };
        #endregion

        #region ".CTOR"

        public SOPUsersPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            Width = DeviceDisplay.MainDisplayInfo.WidthInDp();
            SafeRefreshDataAsync();
        }

        #endregion

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            Users = null;
            IsNoDataLabelVisible = false;
            users = await _serviceAgent.GetSOPUsers(StateInfoService.SessionId);
            if (users != null && users.Length > 0)
                Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.SOPUser>(users);
            else
                IsNoDataLabelVisible = true;
        }

        #endregion

        #region "BINDABLE PROPS"

        #region "Width"
        private double _width;
        public double Width
        {
            get => _width;
            set
            {
                SetProperty(ref _width, value);
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

        #region Users
        private ObservableCollection<CHBackOffice.ApiServices.ChsProxy.SOPUser> _users;
        public ObservableCollection<CHBackOffice.ApiServices.ChsProxy.SOPUser> Users
        {
            get => _users;
            set
            {
                SetProperty(ref _users, value);
            }
        }
        #endregion

        #region Current Parameters
        private Models.Popup.PopupParameters _currentItemParameters;
        public Models.Popup.PopupParameters CurrentItemParameters
        {
            get => _currentItemParameters;
            set
            {
                SetProperty(ref _currentItemParameters, value);
            }
        }
        #endregion

        #endregion

        #region "COMMANDS"

        public ICommand SearchCommand => new Xamarin.Forms.Command(SearchExecute);
        public ICommand ConfirmCommand => new Xamarin.Forms.Command(ConfirmExecute);
        public DelegateCommand<CHBackOffice.ApiServices.ChsProxy.SOPUser> ItemTappedCommand => new DelegateCommand<CHBackOffice.ApiServices.ChsProxy.SOPUser>(ItemTappedExecute);

        #region "COMMAND HANDLERS"

        void SearchExecute()
        {
            Func<CHBackOffice.ApiServices.ChsProxy.SOPUser, bool> predicate = user =>
                string.IsNullOrEmpty(SearchText) ||
                user.Id.ToUpper().Contains(SearchText.ToUpper()) ||
                user.FirstName.ToUpper().Contains(SearchText.ToUpper()) ||
                user.LastName.ToUpper().Contains(SearchText.ToUpper());

            if (users != null)
                Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.SOPUser>(
                    users.AsParallel().Where(predicate).OrderBy(x => x.FirstName));
        }

        async void ItemTappedExecute(CHBackOffice.ApiServices.ChsProxy.SOPUser user)
        {
            if (!StateInfoService.UserPermissions.Employee.UpdateSOPUser)
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Title = "Sorry !",
                    Message = "You don`t have permissions to access"
                });
                Commands.Instance.HidePopup(false);
                return;
            }
            userEditable = user;
            CurrentItemParameters = PopupSorces[0](user);
            await Commands.Instance.ShowPopup(CurrentItemParameters);
        }

        async void ConfirmExecute()
        {
            string password = CurrentItemParameters.Rows[2].TextValue;
            string confirmPassword = CurrentItemParameters.Rows[3].TextValue;
            if (!Validate("NewSopOrEmplPassword", password))
            {
                ValidationResult();
                return;
            }
            
            if (!password.Equals(confirmPassword))
            {
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Message = Resources.Resource.PasswordNotMatch
                });
                return;
            }
            bool confirm = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                OkText = Resources.Resource.Yes,
                CancelText = Resources.Resource.No,
                Message = Resources.Resource.WantToChangePassword
            });
            if (confirm)
            {
                var str = await _serviceAgent.ChangePassword(StateInfoService.SessionId,
                                                   CHBackOffice.ApiServices.ChsProxy.UserType.ServicePanel,
                                                   userEditable.Id,
                                                   password,
                                                   confirmPassword,
                                                   false);
                Enum.TryParse(str, out OperationResult res);
                Messages.TryGetValue(res, out string Message);
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Message = Message
                });
                if (res == OperationResult.Success)
                    Commands.Instance.HidePopup(false);
            }
        }

        #endregion

        #endregion

        #region "PRIVATE METHODS"

        private static PopupParameters UpdateUserDetailsParams(CHBackOffice.ApiServices.ChsProxy.User userItem)
        {
            try
            {
                return new PopupParameters
                {
                    Title = "Update User Details",
                    TitleBackground = Color.FromHex("#e2920a"),
                    CanChangeValues = true,
                    Rows = new List<PopupRow>
                    {
                        new PopupRow
                        {
                            Title = "User ID",
                            Value = userItem.Id,
                            ReadOnly = true
                        },
                        new PopupRow
                        {
                            Title = "Name",
                            Value = userItem.FirstName +" " + userItem.LastName,
                            ReadOnly = true
                        },
                        new PopupRow
                        {
                            Title = "Password",
                            Value = string.Empty,
                            ReadOnly = false,
                            IsPassword = true,
                            Placeholder = "Enter Password",
                            InputKeyboard = Keyboard.Numeric
                        },

                        new PopupRow
                        {
                            Title = "Re Enter to confirm",
                            Value = string.Empty,
                            ReadOnly = false,
                            IsPassword = true,
                            Placeholder = "Re-Enter to confirm",
                            InputKeyboard = Keyboard.Numeric
                        },
                        new PopupRow
                        {
                            Title = "User Group",
                            Value = userItem.AuthorityGroup.Id,
                            ReadOnly = true
                        },
                        new PopupRow
                        {
                            IsButton = true,
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

        #region Validation

        bool Validate(string propName, string propValue)
        {
            try
            {
                ValidationHelper.ValidationResult.FailureList.Clear();
                var failure = ValidationHelper.ValidateProperty(propName, propValue);
                if (failure != null)
                    ValidationHelper.ValidationResult.FailureList.Add(failure);

                return ValidationHelper.ValidationResult.FailureList.Count == 0;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return false;
            }
        }

        async void ValidationResult()
        {
            try
            {
                string message = string.Empty;
                var errorList = ValidationHelper.ValidationResult.FailureList;
                if (errorList.Where(er => er.Reason == FailureCode.Empty).Any())
                {
                    var emptyPropertyList = errorList.Where(er => er.Reason == FailureCode.Empty);
                    foreach (var error in emptyPropertyList)
                    {
                        message += error.PropertyName + " ";
                    }
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = Resources.Resource.PleaseEnter + message
                    });
                    return;
                }
                var failure = ValidationHelper.ValidationResult.FailureList.FirstOrDefault();
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Message = failure.ErrorMessage
                });
                return;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }
        #endregion
    }
}
