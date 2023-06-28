using Acr.UserDialogs;
using CHBackOffice.ApiServices.Interfaces;
using CHBackOffice.Service;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Database;
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
    internal class EmployeesPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region Fields
        const string AllMachinesFilterKey = "All";

        #region Private Fields
        private CHBackOffice.ApiServices.ChsProxy.Employee userEditable;
        private CHBackOffice.ApiServices.ChsProxy.Employee[] users;
        private CHBackOffice.ApiServices.ChsProxy.EmployeeGroup[] groups;
        private List<Func<CHBackOffice.ApiServices.ChsProxy.Employee, PopupParameters>> PopupSources = new List<Func<CHBackOffice.ApiServices.ChsProxy.Employee, PopupParameters>>
        {
            UpdateUserDetailsParams,
            AddEmployeeParams
        };

        #endregion
        #endregion

        #region "PUBLIC PROPS"

        public static Dictionary<OperationResult, string> Messages { get; set; } = new Dictionary<OperationResult, string>
        {
            { OperationResult.Success, Resources.Resource.PasswordChanged },
            { OperationResult.Invalid, Resources.Resource.InvalidCurrentPassword }
        };
        public object UserDetailPopup { get; set; }
        public object AddPopup { get; set; }
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
        private ObservableCollection<CHBackOffice.ApiServices.ChsProxy.Employee> _users;
        public ObservableCollection<CHBackOffice.ApiServices.ChsProxy.Employee> Users
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

        private Models.Popup.PopupParameters _currentAddParameters;
        public Models.Popup.PopupParameters CurrentAddParameters
        {
            get => _currentAddParameters;
            set
            {
                SetProperty(ref _currentAddParameters, value);
            }
        }

        #endregion

        #endregion

        #region ".CTOR"

        public EmployeesPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            Width = DeviceDisplay.MainDisplayInfo.WidthInDp();
            AddToolbarItemCommand("filterToolbarButton", new Action<CustomControls.ToolbarButton>(FilterPressed));
            AddToolbarItemCommand("addToolbarButton", new Action<CustomControls.ToolbarButton>(AddPressed));

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
            users = await _serviceAgent.GetEmployees(StateInfoService.SessionId);
            if (users != null && users.Length > 0)
            {
                await Task.Delay(1000);
                IsNoDataLabelVisible = false;
                if (SelectedFilterItem == null || SelectedFilterItem.Key == "All")
                    Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.Employee>(users);
                else
                    Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.Employee>(
                        users.Where(x => x.Status.ToString() == SelectedFilterItem.Key));
                if (Users == null || Users.Count() == 0)
                    IsNoDataLabelVisible = true;

            }
            else
                IsNoDataLabelVisible = true;
            groups = await _serviceAgent.GetEmployeeGroups(StateInfoService.SessionId);
            PopoverData.Instance.Groups = GetGroups(groups);
        }

        #endregion

        #region "COMMANDS"

        public ICommand SearchCommand => new Xamarin.Forms.Command(SearchExecute);
        public ICommand ConfirmCommand => new Xamarin.Forms.Command(ConfirmExecute);
        public ICommand SubmitCommand => new Xamarin.Forms.Command(SubmitExecute);
        public ICommand HideAdditionalWindowsCommand => new Command(() =>
        {
            AdditionalWindowsVisible = false;
            FilterGridVisible = false;
        });
        public DelegateCommand<CHBackOffice.ApiServices.ChsProxy.Employee> ItemTappedCommand => new DelegateCommand<CHBackOffice.ApiServices.ChsProxy.Employee>(ItemTappedExecute);

        #region "COMMAND HANDLERS"

        void SearchExecute()
        {
            try
            {
                Func<CHBackOffice.ApiServices.ChsProxy.Employee, bool> predicate = user =>
                string.IsNullOrEmpty(SearchText) ||
                user.Id.ToUpper().Contains(SearchText.ToUpper()) ||
                user.FirstName.ToUpper().Contains(SearchText.ToUpper());

                if (users != null)
                    Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.Employee>(
                        users.AsParallel().Where(predicate).OrderBy(x => x.FirstName));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        async void ItemTappedExecute(CHBackOffice.ApiServices.ChsProxy.Employee user)
        {
            try
            {
                if (!StateInfoService.UserPermissions.Employee.UpdateEmployee)
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
                CurrentItemParameters = PopupSources[0](user);
                Commands.Instance.View = UserDetailPopup as UseDetailPopup;
                await Commands.Instance.ShowPopup(CurrentItemParameters);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        async void ConfirmExecute()
        {
            try
            {
                var userId = CurrentItemParameters.Rows[0].TextValue;
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
                                                       CHBackOffice.ApiServices.ChsProxy.UserType.Employee,
                                                       userId,
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
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        async void SubmitExecute()
        {
            try
            {
                if (!StateInfoService.UserPermissions.Employee.AddBOUser)
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Title = "Sorry !",
                        Message = "You don`t have permissions to access"
                    });
                    Commands.Instance.HidePopup(false);
                    return;
                }

                var userId = CurrentAddParameters.Rows[0].TextValue;
                var firstName = CurrentAddParameters.Rows[1].TextValue;
                var lastName = CurrentAddParameters.Rows[2].TextValue;
                var password = CurrentAddParameters.Rows[3].TextValue;
                var selectedGroup = CurrentAddParameters.Rows[4].PickerSelectedItem?.Key;
                var cardNumber = CurrentAddParameters.Rows[5].TextValue;
                if (string.IsNullOrEmpty(userId))
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = "Please enter User ID."
                    });
                    return;
                }
                if (string.IsNullOrEmpty(firstName))
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = "Please enter First Name"
                    });
                    return;
                }
                if (string.IsNullOrEmpty(lastName))
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = "Please enter Last Name"
                    });
                    return;
                }
                if (!Validate("NewSopOrEmplPassword", password))
                {
                    ValidationResult();
                    return;
                }
                if (string.IsNullOrEmpty(selectedGroup))
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = "Please select Group"
                    });
                    return;
                }
                if (string.IsNullOrEmpty(cardNumber))
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = "Please enter Card Number"
                    });
                    return;
                }
                bool confirm = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    OkText = Resources.Resource.Yes,
                    CancelText = Resources.Resource.No,
                    Message = "Are you sure, you want to add new user?"
                });
                if (confirm)
                {
                    var result = await _serviceAgent.AddEmployee(StateInfoService.SessionId, userId, firstName, lastName, selectedGroup, password, cardNumber);
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = "User added"
                    });
                    Commands.Instance.HidePopup(false);
                    Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.Employee>(result);
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void FilterPressed(CustomControls.ToolbarButton button)
        {
            AdditionalWindowsVisible = true;
            FilterGridVisible = true;
        }

        async void AddPressed(CustomControls.ToolbarButton button)
        {
            try
            {
                Commands.Instance.View = AddPopup as UserAddPopup;
                CurrentAddParameters = PopupSources[1](new CHBackOffice.ApiServices.ChsProxy.Employee());
                await Commands.Instance.ShowPopup(CurrentAddParameters);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #endregion

        #region "PRIVATE METHODS"

        ObservableCollection<PopoverItem> GetGroups(CHBackOffice.ApiServices.ChsProxy.EmployeeGroup[] groups)
        {
            try
            {
                if (groups != null)
                    return new ObservableCollection<PopoverItem>(groups.Select(x => new PopoverItem
                    {
                        Key = x.Name.ToString(),
                        Value = x.Name.ToString(),
                        Selected = false
                    })
                    .Distinct(new PopoverItemEqualityComparer())
                    .OrderBy(x => x.Key));
                return null;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        void FilterData()
        {
            try
            {
                if (SelectedFilterItem != null && SelectedFilterItem.Key != "All")
                {
                    Func<CHBackOffice.ApiServices.ChsProxy.Employee, bool> predicate = user =>
                  user.Status.ToString().Equals(SelectedFilterItem.Key);
                    if (users != null)
                        Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.Employee>(
                            users.AsParallel().Where(predicate).OrderBy(x => x.FirstName));

                }
                else if (users != null)
                    Users = new ObservableCollection<CHBackOffice.ApiServices.ChsProxy.Employee>(
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

        private static PopupParameters UpdateUserDetailsParams(CHBackOffice.ApiServices.ChsProxy.Employee userItem)
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
                            Value = userItem.Group,
                            ReadOnly = true,
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

        private static PopupParameters AddEmployeeParams(CHBackOffice.ApiServices.ChsProxy.Employee userItem)
        {
            try
            {
                return new PopupParameters
                {
                    Title = "Add New User",
                    TitleBackground = Color.FromHex("#e2920a"),
                    CanChangeValues = true,
                    Rows = new List<PopupRow>
                    {
                        new PopupRow
                        {
                            Title = "User ID",
                            Value = string.Empty,
                            ReadOnly = false,
                            Placeholder = "Enter User ID"
                        },
                        new PopupRow
                        {
                            Title = "First Name",
                            Value = string.Empty,
                            ReadOnly = false,
                            Placeholder = "Enter First Name"
                        },
                        new PopupRow
                        {
                            Title = "Last Name",
                            Value = string.Empty,
                            ReadOnly = false,
                            Placeholder = "Enter Last Name"
                        },
                        new PopupRow
                        {
                            Title = "Password",
                            Value = string.Empty,
                            IsPassword = true,
                            ReadOnly = false,
                            Placeholder = "Enter Password",
                            InputKeyboard = Keyboard.Numeric
                        },
                        new PopupRow
                        {
                            Title = "User Group",
                            IsPicker = true,
                            ReadOnly = true,
                            PickerItemSource = PopoverData.Instance.Groups,
                            Placeholder = "Select Group"
                        },
                        new PopupRow
                        {
                            Title = "Card Number",
                            Value = string.Empty,
                            ReadOnly = false,
                            Placeholder = "Enter Card Number"
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
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            
            return;
        }
        #endregion

        #endregion
    }
}
