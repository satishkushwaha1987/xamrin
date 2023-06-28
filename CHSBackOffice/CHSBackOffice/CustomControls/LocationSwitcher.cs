using CHSBackOffice.Database;
using CHSBackOffice.Events;
using CHSBackOffice.Extensions;
using CHSBackOffice.Models.Popup;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class LocationSwitcher : Grid
    {
        #region "PRIVATE PROPS"

        readonly Grid _grayMask;
        readonly Grid _popup;
        readonly double _popupWidth;

        #endregion

        #region "BINDABLE PROPERTIES"

        #region "PopupIsVisible"

        public bool PopupIsVisible
        {
            get { return (bool)GetValue(PopupIsVisibleProperty); }
            set { SetValue(PopupIsVisibleProperty, value); }
        }

        public static readonly BindableProperty PopupIsVisibleProperty = BindableProperty.Create(
            nameof(PopupIsVisible), 
            typeof(bool), 
            typeof(LocationSwitcher),
            false,
            propertyChanged: OnPopupIsVisiblePropertyChanged
        );

        private static void OnPopupIsVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                if (newValue != oldValue && newValue != null)
                {
                    var that = (bindable as LocationSwitcher);
                    var popupIsVisible = (bool)newValue;
                    if (popupIsVisible)
                        that.ShowPopup();
                    else
                        that.HidePopup();
                }
            }
            catch (System.Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region "OnLocationChangedCommand"

        public ICommand OnLocationChangedCommand
        {
            get { return (ICommand)GetValue(OnLocationChangedCommandProperty); }
            set { SetValue(OnLocationChangedCommandProperty, value); }
        }

        public static readonly BindableProperty OnLocationChangedCommandProperty =
            BindableProperty.Create(nameof(OnLocationChangedCommand), typeof(ICommand), typeof(SearchPanel), null);

        #endregion

        #region "SelectedItem"

        private PopoverItem _selectedLocation;
        public PopoverItem SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                try
                {
                    AllowedLocations.ForEach(l => l.Selected = false);
                    string oldKey = _selectedLocation == null ? "" : _selectedLocation.Key;
                    string newKey = value == null ? "" : value.Key;
                    _selectedLocation = value;
                    if (_selectedLocation != null) _selectedLocation.Selected = true;

                    OnPropertyChanged(nameof(SelectedLocation));
                    if (oldKey != newKey)
                        OnLocationChanged(_selectedLocation);
                }
                catch (System.Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
        }

        #endregion

        #region "AllowedLocations"

        private IEnumerable<PopoverItem> _allowedLocations;
        public IEnumerable<PopoverItem> AllowedLocations
        {
            get => _allowedLocations;
            set
            {
                _allowedLocations = value;
                OnPropertyChanged(nameof(AllowedLocations));
            }
        }

        #endregion

        #endregion

        #region ".CTOR"

        public LocationSwitcher()
        {
            try
            {
                if (StateInfoService.ThereAreMultipleLocations)
                {
                    #region "GrayMask"

                    _grayMask = new Grid
                    {
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
                        VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
                        BackgroundColor = Color.Black,
                        Opacity = 0.2
                    };

                    _grayMask.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = TapOnGrayMaskCommand
                    });

                    Children.Add(_grayMask);

                    #endregion

                    #region "Popup"

                    _popupWidth = Device.Idiom == TargetIdiom.Phone ? 160 : 200;

                    _popup = new Grid
                    {
                        WidthRequest = _popupWidth,
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.End, false),
                        VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
                        BackgroundColor = Color.White,
                        Opacity = 0
                    };

                    #region "LocationList"

                    var locationList = new ListView
                    {
                        HasUnevenRows = true,
                        BackgroundColor = Color.White
                    };

                    locationList.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(AllowedLocations), BindingMode.OneWay, source: this));
                    locationList.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SelectedLocation), BindingMode.TwoWay, source: this));
                    locationList.ItemTemplate = new DataTemplate(GetLocationListViewItemTemplate);
                    locationList.SeparatorVisibility = SeparatorVisibility.None;

                    _popup.Children.Add(locationList);

                    Children.Add(_popup);

                    #endregion

                    #endregion

                    #region "Set Init Data"

                    AllowedLocations = StateInfoService.AllowedLocations.Select(l => new PopoverItem
                    {
                        Key = l.Id,
                        Value = l.Name,
                        Selected = l.Id == StateInfoService.CurentLocationId
                    }).OrderBy(l => l.Key).ToArray();

                    #endregion

                    App.SharedEventAggregator.GetEvent<LocationChanged>().Subscribe(OnLocationChangedEvent);
                }

                IsVisible = false;
            }
            catch (System.Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region "COMMANDS"

        private ICommand TapOnGrayMaskCommand => new Command(TapOnGrayMask);

        #region "COMMAND HANDLERS"

        private void TapOnGrayMask()
        {
            PopupIsVisible = false;
        }

        #endregion

        #endregion

        #region "PRIVATE METHODS"

        private void OnLocationChangedEvent()
        {
            try
            {
               foreach(var t in AllowedLocations)
                t.Selected = (t.Key == StateInfoService.CurentLocationId);
                _selectedLocation = AllowedLocations.FirstOrDefault(l => l.Selected);
                OnPropertyChanged(nameof(AllowedLocations));
                OnPropertyChanged(nameof(SelectedLocation));
            }
            catch (System.Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void OnLocationChanged(PopoverItem currentLocation)
        {
            PopupIsVisible = false;
            if (OnLocationChangedCommand != null)
                OnLocationChangedCommand.Execute(currentLocation.Key);
        }

        private void ShowPopup()
        {
            Task.Run(async () =>
            {
                Device.BeginInvokeOnMainThread(() => {
                    IsVisible = true;
                });

                var popupWidth = _popup.Width > 0 ? _popup.Width : _popupWidth;
                await _popup.TranslateTo(popupWidth, _popup.Y, 0);
                await _popup.FadeTo(1, 0);
                await _popup.TranslateTo(0, _popup.Y, 250, Easing.CubicIn);
            });
        }

        private void HidePopup()
        {
            Task.Run(async () =>
            {
                await _popup.TranslateTo(_popup.Width, _popup.Y, 150, Easing.CubicOut);

                Device.BeginInvokeOnMainThread(() => {
                    IsVisible = false;
                });
            });
        }

        private Cell GetLocationListViewItemTemplate()
        {
            ViewCell cell = new ViewCell();

            #region "LabelContainer"

            Grid labelContainer = new Grid()
            {
                HeightRequest = Device.Idiom == TargetIdiom.Phone ? 25 : 35,
                ColumnSpacing = 0
            };
            labelContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            labelContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });

            #endregion

            #region "NameLabel"

            Label nameLabel = new Label
            {
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Start, true),
                VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true),
                FontSize = StaticResourceManager.GetFontSize(AppFontSize.Small)
            };
            nameLabel.SetBinding(Label.TextProperty, "Value");
            labelContainer.Children.Add(nameLabel, 0, 0);

            #endregion

            #region "SelectedImage"

            Label selectedImage = new Label
            {
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, true),
                VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true),
                FontFamily = StaticResourceManager.GetFontName(AppFont.ChsIcons),
                Text = Constants.CHSIcons.SelectedIcon,
                TextColor = Constants.CHSIconColors.SelectedIconColor
            };
            selectedImage.SetBinding(Label.IsVisibleProperty, "Selected");
            labelContainer.Children.Add(selectedImage, 1, 0);

            #endregion

            #region "Underline"

            BoxView underline0 = new BoxView
            {
                BackgroundColor = Color.LightGray,
                HeightRequest = 1,
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
                VerticalOptions = new LayoutOptions(LayoutAlignment.End, false)
            };
            BoxView underline1 = new BoxView
            {
                BackgroundColor = Color.LightGray,
                HeightRequest = 1,
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
                VerticalOptions = new LayoutOptions(LayoutAlignment.End, false)
            };

            labelContainer.Children.Add(underline0, 0, 0);
            labelContainer.Children.Add(underline1, 1, 0);

            #endregion

            cell.View = labelContainer;

            return cell;
        }

        #endregion
    }
}
