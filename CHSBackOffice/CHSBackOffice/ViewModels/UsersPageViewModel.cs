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
    internal class UsersPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region Fields

        const string AllMachinesFilterKey = "All";

        #region "Private Fields"
        private CHBackOffice.ApiServices.ChsProxy.User userEditable;
        private CHBackOffice.ApiServices.ChsProxy.User[] users;
        private readonly List<Func<CHBackOffice.ApiServices.ChsProxy.User, PopupParameters>> PopupSorces = new List<Func<CHBackOffice.ApiServices.ChsProxy.User, PopupParameters>>
        {
            UpdateUserDetailsParams
        };
        #endregion

        #endregion

        #region "PUBLIC PROPS"

        public string LeftButtonText => Constants.CHSIcons.ArrowLeft;
        public static Dictionary<OperationResult, string> Messages { get; set; } = new Dictionary<OperationResult, string>
        {
            { OperationResult.Success, Resources.Resource.PasswordChanged },
            { OperationResult.Invalid, Resources.Resource.InvalidCurrentPassword }
        };

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

        #region FilterGridVisible

        private bool _FilterGridVisible;
        public bool FilterGridVisible
        {
            get => _FilterGridVisible;
            set => SetProperty(ref _FilterGridVisible, value);

        }

        #endregion

        #region AdditionalWindowsVisible

        private bool _AdditionalWindowsVisible;
        public bool AdditionalWindowsVisible
        {
            get => _AdditionalWindowsVisible;
            set => SetProperty(ref _AdditionalWindowsVisible, value);
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
        private ObservableCollection<CHBackOffice.ApiServices.ChsProxy.User> _users;
        public ObservableCollection<CHBackOffice.ApiServices.ChsProxy.User> Users
        {
            get => _users;
            set
            {
                SetProperty(ref _users, value);
            }
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

                    FilterData();
                }

                HideAdditionalWindowsCommand?.Execute(null);
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

        #region ".CTOR"

        public UsersPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            Width = DeviceDisplay.MainDisplayInfo.WidthInDp();
            AddToolbarItemCommand("filterToolbarButton", new Action<CustomControls.ToolbarButton>(FilterPressed));
            SafeRefreshDataAsync();
            FilterItems = new List<PopoverItem>
            {
                new PopoverItem { Key = AllMachinesFilterKey, Value = "All" },
                new PopoverItem { Key = CHBackOffice.ApiServices.ChsProxy.Status.Disabled.ToString(), Value = "Locked"},
                new PopoverItem { Key = CHBackOffice.ApiServices.ChsProxy.Status.Enabled.ToString(), Value = "Unlocked" },
            };

            SelectedFilterItem = FilterItems[0];
        }

        #endregion

        #region Refresh data implementation

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            Users = null;
            IsNoDataLabelVisible = false;
            users = await _serviceAgent.GetBOUsers(StateInfoService.SessionId);
            if (users != null && users.Length > 0)
            {
                await Task.Delay(1000);
                IsNoDataLabelVisible = false;
                if (SelectedFilterItem == null || SelectedFilterItem.Key == "All")
                    Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.User>(users);
                else
                    Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.User>(
                        users.Where(x => x.Status.ToString() == SelectedFilterItem.Key));

                if (Users == null || Users.Count() == 0)
                    IsNoDataLabelVisible = true;
            }
            else
                IsNoDataLabelVisible = true;
        }

        #endregion

        #region "COMMANDS"

        public ICommand SearchCommand => new Xamarin.Forms.Command(SearchExecute);
        public ICommand ConfirmCommand => new Xamarin.Forms.Command(ConfirmExecute);
        public ICommand HideAdditionalWindowsCommand => new Command(() =>
        {
            AdditionalWindowsVisible = false;
            FilterGridVisible = false;
        });

        public DelegateCommand<CHBackOffice.ApiServices.ChsProxy.User> ItemTappedCommand => new DelegateCommand<CHBackOffice.ApiServices.ChsProxy.User>(ItemTappedExecute);

        #region "COMMAND HANDLERS"

        void SearchExecute()
        {
            try
            {
                Func<CHBackOffice.ApiServices.ChsProxy.User, bool> predicate = user =>
                string.IsNullOrEmpty(SearchText) ||
                user.Id.ToUpper().Contains(SearchText.ToUpper()) ||
                user.FirstName.ToUpper().Contains(SearchText.ToUpper()) ||
                user.LastName.ToUpper().Contains(SearchText.ToUpper());

                if (users != null)
                    Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.User>(
                        users.AsParallel().Where(predicate).OrderBy(x => x.FirstName));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        async void ItemTappedExecute(CHBackOffice.ApiServices.ChsProxy.User user)
        {
            if (!StateInfoService.UserPermissions.Employee.UpdateBOUser)
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
            try
            {
                string password = CurrentItemParameters.Rows[2].TextValue;
                string confirmPassword = CurrentItemParameters.Rows[3].TextValue;
                if (!Validate("NewPassword", password))
                {
                    ValidationResult();
                    return;
                }
                if (!Validate("Confirm Password", confirmPassword))
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
                                                       CHBackOffice.ApiServices.ChsProxy.UserType.BackOffice,
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
            catch(Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void FilterPressed(CustomControls.ToolbarButton button)
        {
            AdditionalWindowsVisible = true;
            FilterGridVisible = true;
        }

        #endregion

        #endregion

        #region "PRIVATE METHODS"

        void FilterData()
        {
            try
            {
                if (SelectedFilterItem != null && SelectedFilterItem.Key != "All")
                {
                    Func<CHBackOffice.ApiServices.ChsProxy.User, bool> predicate = user =>
                  user.Status.ToString().Equals(SelectedFilterItem.Key);
                    if (users != null)
                        Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.User>(
                            users.AsParallel().Where(predicate).OrderBy(x => x.FirstName));
                }
                else if (users != null)
                    Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.User>(
                                users);

                if (Users == null || Users.Count == 0)
                    IsNoDataLabelVisible = true;
                else
                    IsNoDataLabelVisible = false;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

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
                            Placeholder = "Enter Password"
                        },

                        new PopupRow
                        {
                            Title = "Re Enter to confirm",
                            Value = string.Empty,
                            ReadOnly = false,
                            IsPassword = true,
                            Placeholder = "Re-Enter to confirm"
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

        #region Validation

        bool Validate(string propName, string propValue)
        {
            try
            {
                ValidationHelper.ValidationResult.FailureList.Clear();
                if ("NewPassword".Equals(propName))
                {
                    var failure = ValidationHelper.ValidateProperty(propName, propValue);
                    if (failure != null)
                    {
                        ValidationHelper.ValidationResult.FailureList.Add(failure);
                    }
                }
                if ("Confirm Password".Equals(propName))
                {
                    var failure = ValidationHelper.ValidateProperty(propName, propValue);
                    if (failure != null)
                    {
                        ValidationHelper.ValidationResult.FailureList.Add(failure);
                    }
                }
                if (ValidationHelper.ValidationResult.FailureList.Count == 0)
                    return true;
                return false;
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

        #endregion
    }
}
