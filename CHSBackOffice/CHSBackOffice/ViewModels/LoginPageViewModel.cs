using Acr.UserDialogs;
using CHBackOffice.ApiServices.Interfaces;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Database;
using CHSBackOffice.Support;
using CHSBackOffice.Views;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    internal class LoginPageViewModel : ExtendedNaviPageViewModelBase
    {
        #region "PRIVATE FIELDS"

        #endregion

        #region "PUBLIC PROPS"

        public string SettingButtonText => Constants.CHSIcons.Settings;
        public string BackgroundIcon => Device.Idiom == TargetIdiom.Phone ?
            Constants.CHSImages.BgImagePhone : CurrentDisplayOrientation == DisplayOrientation.Landscape ?
                                                                            Constants.CHSImages.BgImageLandscapeTablet :
                                                                            Constants.CHSImages.BgImagePortraitTablet;
        public string LogoIcon => Constants.CHSImages.CHSIconTablet;
        public string CompanyName => Resources.Resource.BackOffice;
        public string UserLoginPlaceholder => Resources.Resource.UserName;
        public string UserPasswordPlaceholder => Resources.Resource.UserPassword;
        public string RememberMeText => Resources.Resource.RememberMe;
        public string ButtonText => Resources.Resource.SignIn;

        #endregion

        #region "BINDABLE PROPS"

        private string _userLogin;
        public string UserLogin
        {
            get => _userLogin;
            set => SetProperty(ref _userLogin, value);
        }

        private string _userPassword;
        public string UserPassword
        {
            get => _userPassword;
            set => SetProperty(ref _userPassword, value);
        }

        private bool _rememberMe;
        public bool RememberMe
        {
            get => _rememberMe;
            set => SetProperty(ref _rememberMe, value);
        }

        #endregion

        #region ".CTOR"

        public LoginPageViewModel(ICHSServiceAgent serviceAgent) : base(serviceAgent)
        {
            try
            {
                //For change background image for tablets
                NeedToRefreshDataOrientationChanged = true;

                LoadingIndicatorColor = Color.White;

                RememberMe = Settings.RememberMe.Value;

                if (StateInfoService.RememberMeEnabled && StateInfoService.HasUserWithRememberme)
                {
                    UserLogin = StateInfoService.CurrentUser?.UserName;
                    UserPassword = StateInfoService.CurrentUser?.Password;
                }

                AddToolbarItemCommand("but1", new Action<ToolbarButton>((b) => { Services.Navigation.NavigateToPage(typeof(BackOfficeHostAddressPage)); }));

                if (!String.IsNullOrEmpty(StateInfoService.SessionId))
                {
                    _serviceAgent.EndSession(StateInfoService.SessionId);
                    StateInfoService.CleanCurrentSession();
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region "COMMANDS"

        public ICommand LoginButtonCommand => new Command(LoginButtonPressed);
        public ICommand LeftButtonPressedCommand => new Command(LeftButtonPressed);

        #region "COMMAND HANDLERS"

        async void LoginButtonPressed()
        {
            try
            {
                if (!await CheckObjectAndShowError(UserLogin, Resources.Resource.PleaseEnterLogin, false) ||
                    !await CheckObjectAndShowError(UserPassword, Resources.Resource.PleaseEnterPassword, false)) return;

                CommonViewObjects.Instance.IsLoadingVisible = true;

                var sessionInfo = await _serviceAgent.GetSession(UserLogin, UserPassword);
                
                if (!await CheckObjectAndShowError(sessionInfo, Resources.Resource.UnknownError, false) ||
                    !await CheckObjectAndShowError(sessionInfo.Error, sessionInfo.Error, true)) return;

                var user = Repository.SaveUser(UserLogin, RememberMe ? UserPassword : string.Empty, RememberMe);
                Settings.RememberMe.Value = RememberMe;

                StateInfoService.CurrentUser = user;

                var userProfile = await _serviceAgent.GetUserProfile(sessionInfo.SessionId);
                if (userProfile == null)
                    return;

                StateInfoService.SetUserProfileData(userProfile);
                if (!StateInfoService.UserCanStartApp)
                    return;

                StateInfoService.SessionId = sessionInfo.SessionId;
                var startPageType = string.IsNullOrEmpty(StateInfoService.CurrentUser.StartPage) ? typeof(Views.DashboardPage) : Type.GetType("CHSBackOffice.Views." + StateInfoService.CurrentUser.StartPage);
                if (!PagesPermissions.CheckRights(startPageType))
                    startPageType = PagesPermissions.GetFirstAllowedPage();

                if (Device.RuntimePlatform == DevicePlatform.Android.ToString())
                {
                    CommonViewObjects.Instance.IsLoadingVisible = false;
                    await Task.Delay(100);
                }

                await Services.Navigation.OpenMainMenuPage(startPageType);
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
            }
            finally
            {
                CommonViewObjects.Instance.IsLoadingVisible = false;
            }
        }

        async void LeftButtonPressed()
        {
            if (!string.IsNullOrEmpty(StateInfoService.SessionId))
                await _serviceAgent.EndSession(StateInfoService.SessionId);
            IocContainer.CloseBehavior.Close();
        }



        #endregion

        #endregion

        #region Methods

        bool IsString(object val)
        {
            try
            {
                var a = (string)val;
                return true;
            }
            catch
            {
                return false;
            }
        }

        async Task<bool> CheckObjectAndShowError(object ObjectForCheck, string ErrorMessage, bool mustBeEmpty)
        {
            bool result = mustBeEmpty && (ObjectForCheck == null || (IsString(ObjectForCheck) && string.IsNullOrWhiteSpace(ObjectForCheck as string)));

            if (!result)
                result = !mustBeEmpty && ObjectForCheck != null && (!IsString(ObjectForCheck) || (!string.IsNullOrWhiteSpace(ObjectForCheck as string)));

            if (!result && !String.IsNullOrWhiteSpace(ErrorMessage))
                await UserDialogs.Instance.AlertAsync(new AlertConfig { Message = ErrorMessage });

            return result;
        }

        internal override async Task RefreshData(bool isLocationChanged = false)
        {
            RaisePropertyChanged(nameof(BackgroundIcon));
            //Dummy implementation
            await Task.Delay(1);
        }

        #endregion
    }
}
