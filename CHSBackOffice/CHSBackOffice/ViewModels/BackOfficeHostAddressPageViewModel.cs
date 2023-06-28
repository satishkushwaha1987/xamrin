using Acr.UserDialogs;
using CHBackOffice.ApiServices.Interfaces;
using CHBackOffice.Service;
using CHSBackOffice.Database;
using CHSBackOffice.Support;
using Prism.Mvvm;
using System;
using System.ServiceModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CHSBackOffice.ViewModels
{
    public class BackOfficeHostAddressPageViewModel : BindableBase
    {
        private ICHSServiceAgent _serviceAgent;
        public string IgnoreCertText => Resources.Resource.IgnoreCertText;
        public string PageTitle => StateInfoService.HasBackOfficeHostAddress ? Resources.Resource.SetHostURLString : string.Empty;
        public string ButtonText => StateInfoService.HasBackOfficeHostAddress ? Resources.Resource.SaveHostURL : Resources.Resource.Continue;
        public string InfoText => Resources.Resource.InfoText;
        public string CompanyName => Resources.Resource.BackOffice;
        public string BackgroundIcon => Device.Idiom == TargetIdiom.Phone ?
            Constants.CHSImages.BgImagePhone : DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape ?
                                                                            Constants.CHSImages.BgImageLandscapeTablet :
                                                                            Constants.CHSImages.BgImagePortraitTablet;
        public string LogoIcon => Constants.CHSImages.CHSIconTablet;
        public ICommand ButtonCommand => new Command(ButtonPressed);
        private string _serviceUrl;
        public string ServiceUrl
        {
            get => _serviceUrl;
            set
            {
                SetProperty(ref _serviceUrl, value);
            }
        }

        public bool IgnoreCert
        {
            get => Settings.IgnoreCertificate.BoolValue;
            set
            {
                Settings.IgnoreCertificate.Value = value;
                RaisePropertyChanged(nameof(IgnoreCert));
            }
        }

        public BackOfficeHostAddressPageViewModel(ICHSServiceAgent serviceAgent)
        {
            _serviceAgent = serviceAgent;
            if (StateInfoService.HasBackOfficeHostAddress)
                ServiceUrl = Settings.ServerAddress.Value;
        }

        async void ButtonPressed()
        {
            try
            {
                CommonViewObjects.Instance.IsLoadingVisible = true;
                if (string.IsNullOrWhiteSpace(ServiceUrl))
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = Resources.Resource.PleaseEnterURL
                    });
                    return;
                }

                if (!Uri.TryCreate(ServiceUrl, UriKind.Absolute, out Uri uri))
                {
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Message = Resources.Resource.UnsupportedURL
                    });
                    return;
                }

                _serviceAgent.SetSoapClient(uri.ToString());
                var res = await _serviceAgent.TestConnection();

                if (res)
                {
                    if (!StateInfoService.HasBackOfficeHostAddress)
                        Services.Navigation.SetMainPage(typeof(Views.LoginPage));
                    else
                    {
                        await UserDialogs.Instance.AlertAsync(new AlertConfig
                        {
                            Message = Resources.Resource.HostURLUpdated
                        });
                    }
                    Settings.ServerAddress.Value = uri.ToString();
                }
                else
                {
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
                CommonViewObjects.Instance.IsLoadingVisible = false;
            }

            if (!String.IsNullOrEmpty(Settings.ServerAddress.Value))
                _serviceAgent.SetSoapClient(Settings.ServerAddress.Value.ToString());
        }
    }
}
