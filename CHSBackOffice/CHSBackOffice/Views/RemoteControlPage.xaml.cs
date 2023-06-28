using CHSBackOffice.CustomControls;
using CHSBackOffice.Support;
using CHSBackOffice.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace CHSBackOffice.Views
{
    public partial class RemoteControlPage : ExtendedNaviPage
    {
        #region "PUBLIC PROPS"

        public double RemoteCommandButtonsHeight { set; get; }

        #endregion

        #region ".CTOR"

        public RemoteControlPage()
        {
            try
            {
                InitializeComponent();
                RemoteCommandButtonsHeight = 40;
                ((RemoteControlPageViewModel)BindingContext).PropertyChanged += OnContextPropertyChanged;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region "EVENT HANDLERS"

        private void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == nameof(RemoteControlPageViewModel.IsRemoteCommandButtonsVisible))
                {
                    RemoteControlPageViewModel context = sender as RemoteControlPageViewModel;
                    if (context.IsRemoteCommandButtonsVisible)
                        ShowRemoteCommandButtons();
                    else
                        HideRemoteCommandButtons();
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region "PRIVATE METHODS"

        private void ShowRemoteCommandButtons()
        {
            remoteActionButtons.IsVisible = true;
            new Animation
            {
                { 0, 1, new Animation(v => remoteActionButtons.Opacity = v, 0, 1, Easing.CubicIn)},
                { 0, 1, new Animation(v => remoteActionButtons.HeightRequest = v, 0, RemoteCommandButtonsHeight, Easing.CubicOut)},
                { 0, 1, new Animation(v => remoteActionButtons.Padding = v, 0, 10, Easing.CubicOut)}
            }.Commit(this, nameof(ShowRemoteCommandButtons), 10, 300);
        }

        private void HideRemoteCommandButtons()
        {
            new Animation
            {
                { 0, 0.7, new Animation(v => remoteActionButtons.Opacity = v, 1, 0, Easing.CubicIn)},
                { 0, 1, new Animation(v => remoteActionButtons.HeightRequest = v, RemoteCommandButtonsHeight, 0, Easing.CubicOut)},
                { 0, 1, new Animation(v => remoteActionButtons.Padding = v, 10, 0, Easing.CubicIn)}
            }.Commit(this, nameof(HideRemoteCommandButtons), 10, 300, finished: (v, c) => remoteActionButtons.IsVisible = false);
        }

        #endregion
    }
}
