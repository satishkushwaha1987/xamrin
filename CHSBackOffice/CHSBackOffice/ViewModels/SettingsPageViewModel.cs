using Acr.UserDialogs;
using CHBackOffice.ApiServices.Interfaces;
using CHBackOffice.Service;
using CHSBackOffice.Database;
using CHSBackOffice.Support;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    public class SettingsPageViewModel : BindableBase
	{
        private ICHSServiceAgent _serviceAgent;

        public ICommand ResetCommand => new Command(ResetButtonPressed);
        public ICommand ChangeCommand => new Command(ChangeButtonPressed);
        public ICommand SwitchCommand => new Command(SwitchPressed);
        public ICommand SaveCommand => new Command(SavePressed);
        public static Dictionary<OperationResult, string> Messages { get; set; } = new Dictionary<OperationResult, string>
        {
            { OperationResult.Success, Resources.Resource.PasswordChanged },
            { OperationResult.Invalid, Resources.Resource.InvalidCurrentPassword }
        };
        private static List<string> PropertyForValidation { get; set; } = new List<string>() { nameof(CurrentPassword), nameof(NewPassword), nameof(ConfirmPassword) };

        public SettingsPageViewModel(ICHSServiceAgent serviceAgent)
        {
            _serviceAgent = serviceAgent;
            PopoverData.Instance.InitMenu();
            BackOfficeHost = Settings.ServerAddress.Value;
            Items = PopoverData.Instance.SettingsPopoverItems;
            SelectedItem = (string.IsNullOrEmpty(StateInfoService.CurrentUser.StartPage) ?
                null :
                Items.Where(x => x.Value.Equals(StateInfoService.CurrentUser.StartPage)).FirstOrDefault());
        }

        public string IgnoreCertText => Resources.Resource.IgnoreCertText;
        public bool IgnoreCert
        {
            get => Settings.IgnoreCertificate.BoolValue;
            set
            {
                Settings.IgnoreCertificate.Value = value;
                RaisePropertyChanged(nameof(IgnoreCert));
            }
        }

        public string AutoRefreshText => Resources.Resource.AutoRefreshText;
        public bool AutoRefresh
        {
            get => Settings.AutoRefresh.BoolValue;
            set
            {
                Settings.AutoRefresh.Value = value;
                RaisePropertyChanged(nameof(AutoRefresh));
            }
        }


        

        private string _currentPassword;
        public string CurrentPassword
        {
            get => _currentPassword;
            set
            {
                SetProperty(ref _currentPassword, value);
                RaisePropertyChanged(nameof(CurrentPassword));
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                SetProperty(ref _newPassword, value);
                RaisePropertyChanged(nameof(NewPassword));
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetProperty(ref _confirmPassword, value);
                RaisePropertyChanged(nameof(ConfirmPassword));
            }
        }

        private string _backOfficeHost;
        public string BackOfficeHost
        {
            get => _backOfficeHost;
            set
            {
                SetProperty(ref _backOfficeHost, value);
                RaisePropertyChanged(nameof(BackOfficeHost));
            }
        }


        ObservableCollection<Models.Popup.PopoverItem> items;
        public ObservableCollection<Models.Popup.PopoverItem> Items
        {
            get { return items; }
            set
            {
                SetProperty(ref items, value);
                RaisePropertyChanged(nameof(Items));
            }
        }

        Models.Popup.PopoverItem _selectedItem;
        public Models.Popup.PopoverItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != null)
                {
                    _selectedItem.Selected = false;
                }

                SetProperty(ref _selectedItem, value);
                RaisePropertyChanged(nameof(SelectedItem));

                if (_selectedItem != null)
                {
                    _selectedItem.Selected = true;
                }
            }
        }

       
        void ResetButtonPressed()
        {
            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }

        async void ChangeButtonPressed()
        {
            try
            {
                string message = string.Empty;
                if (!Validate())
                {
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

                if (!NewPassword.Equals(ConfirmPassword))
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
                    var str = await _serviceAgent.ChangePassword(StateInfoService.SessionId, CHBackOffice.ApiServices.ChsProxy.UserType.BackOffice, StateInfoService.CurrentUser.UserName, CurrentPassword, NewPassword, true);
                    Enum.TryParse(str, out OperationResult res);
                    Messages.TryGetValue(res, out string Message);
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = Message
                    });
                    if (res == OperationResult.Success)
                        ResetButtonPressed();
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        async void SwitchPressed()
        {
            try
            {
                if (string.IsNullOrEmpty(BackOfficeHost))
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = Resources.Resource.PleaseEnterHostUrl
                    });

                    return;
                }

                if (!Uri.TryCreate(BackOfficeHost, UriKind.Absolute, out Uri uri))
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = Resources.Resource.UnsupportedURL
                    });
                    return;
                }

                bool confirm = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    OkText = Resources.Resource.Yes,
                    CancelText = Resources.Resource.No,
                    Title = Resources.Resource.AreYouSureSwitchUrl,
                    Message = Resources.Resource.SignedOut
                });
                if (confirm)
                {
                    Support.CommonViewObjects.Instance.IsLoadingVisible = true;
                    _serviceAgent.SetSoapClient(uri.ToString());
                    var res = await _serviceAgent.TestConnection();

                    if (res)
                    {
                        await UserDialogs.Instance.AlertAsync(new AlertConfig
                        {
                            Message = Resources.Resource.HostURLUpdated
                        });
                        Settings.ServerAddress.Value = uri.ToString();
                        Support.Services.Navigation.SetMainPage(typeof(Views.LoginPage));
                    }
                    else
                        await UserDialogs.Instance.AlertAsync(new AlertConfig
                        {
                            Message = Resources.Resource.EndpointNotFound
                        });
                }

            }
            catch (EndpointNotFoundException ex)
            {
                ExceptionProcessor.ProcessException(ex);
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Message = Resources.Resource.EndpointNotFound
                });
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                await UserDialogs.Instance.AlertAsync(new AlertConfig
                {
                    Message = Resources.Resource.UnknownError
                });
            }
            finally
            {
                Support.CommonViewObjects.Instance.IsLoadingVisible = false;
            }

            if (!String.IsNullOrEmpty(Settings.ServerAddress.Value))
                _serviceAgent.SetSoapClient(Settings.ServerAddress.Value.ToString());
        }

        async void SavePressed()
        {
            try
            {
                bool confirm = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    OkText = Resources.Resource.Yes,
                    CancelText = Resources.Resource.No,
                    Message = Resources.Resource.AreYouSureSetStartPage + SelectedItem.Key + Resources.Resource.AsStartPage
                });

                if (confirm)
                {
                    StateInfoService.ChangeStartPage(SelectedItem.Value);
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Title = Resources.Resource.Success,
                        Message = Resources.Resource.StartPageUpdated + '\'' + SelectedItem.Key + '\''
                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        protected internal bool Validate()
        {
            try
            {
                ValidationHelper.ValidationResult.FailureList.Clear();
                foreach (string prop in PropertyForValidation)
                {
                    var failure = ValidationHelper.ValidateProperty(prop, (string)GetType().GetProperty(prop).GetValue(this, null));
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
    }
}
