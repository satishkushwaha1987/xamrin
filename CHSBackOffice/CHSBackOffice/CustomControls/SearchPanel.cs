using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class SearchPanel : Grid
    {
        #region Fields
        PopupLayout _popupLayout;
        BorderlessEntry _searchEntry;
        static double _popupLayoutHeight = Device.Idiom == TargetIdiom.Tablet ? 64 : 56;
        Grid popoverContainer;
        #endregion

        #region "Bindable Properties"

        #region "ParentContext"

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        public static readonly BindableProperty ParentContextProperty =
            BindableProperty.Create("ParentContext", typeof(object), typeof(SearchPanel), null, propertyChanged: OnParentContextPropertyChanged);

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                var that = bindable as SearchPanel;
                that._popupLayout = newValue as PopupLayout;
            }
        }

        #endregion

        #region "BackgroundColor"

        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly new BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SearchPanel), Color.Transparent, propertyChanged: OnBackgroundColorPropertyChanged);

        private static void OnBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as SearchPanel).popoverContainer.BackgroundColor = (Color)newValue;
            }
        }

        #endregion

        #region "Text"

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(SearchPanel), string.Empty);


        public ICommand SearchTextChangeCommand
        {
            get { return (ICommand)GetValue(SearchTextChangeCommandProperty); }
            set { SetValue(SearchTextChangeCommandProperty, value); }
        }

        public static readonly BindableProperty SearchTextChangeCommandProperty =
            BindableProperty.Create(nameof(SearchTextChangeCommand), typeof(ICommand), typeof(SearchPanel), null);

        #endregion

        #endregion

        #region ".CTOR"

        public SearchPanel()
        {
            BackgroundColor = Color.Transparent;
            
            #region Popover

            popoverContainer = new Grid();
            popoverContainer.HeightRequest = _popupLayoutHeight;
            popoverContainer.WidthRequest = DeviceDisplay.MainDisplayInfo.WidthInDp();
            popoverContainer.RowSpacing = 0;
            popoverContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            popoverContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            popoverContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            popoverContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            popoverContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            popoverContainer.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
            popoverContainer.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);

            var iphonePaddingGrid = new Grid();
            iphonePaddingGrid.BackgroundColor = Color.Transparent;
            iphonePaddingGrid.HeightRequest = Device.RuntimePlatform == Device.iOS ? 10 : 0;
            Grid.SetColumnSpan(iphonePaddingGrid, 3);
            popoverContainer.Children.Add(iphonePaddingGrid, 0, 0);

            var leftButtonContainer = new Grid();
            leftButtonContainer.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
            leftButtonContainer.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
            leftButtonContainer.Padding = new Thickness(20, 0, 0, 0);
            var leftButtonGestureRecognizer = new TapGestureRecognizer((v, o) => 
            {
                if (_popupLayout.IsPopupActive)
                {
                    var page = _popupLayout.ParentPage as ExtendedNaviPage;
                    NavigationPage.SetHasNavigationBar(page, true);
                    SlideСontentUp(page);
                    _popupLayout.InputTransparent = true;
                    _popupLayout.DismissPopup();
                    _popupLayout.BackgroundColor = Color.Transparent;
                    _searchEntry.Text = string.Empty;

                }
            });
            leftButtonContainer.GestureRecognizers.Add(leftButtonGestureRecognizer);

            var leftbuttonText = new Label();
            leftbuttonText.Text = Constants.CHSIcons.ArrowLeft;
            leftbuttonText.TextColor = Color.White;
            leftbuttonText.FontFamily = StaticResourceManager.GetFontName(AppFont.ChsIcons);
            leftbuttonText.FontSize = StaticResourceManager.GetFontSize(AppFontSize.Large);
            leftbuttonText.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
            leftbuttonText.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);

            leftButtonContainer.Children.Add(leftbuttonText, 0, 0);
            popoverContainer.Children.Add(leftButtonContainer, 0, 1);

            _searchEntry = new BorderlessEntry();
            _searchEntry.HeightRequest = Device.Idiom == TargetIdiom.Phone ? 25 : 40;
            _searchEntry.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
            _searchEntry.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
            _searchEntry.Placeholder = CHSBackOffice.Resources.Resource.Search;
            _searchEntry.PlaceholderColor = Color.White;
            _searchEntry.FontSize = StaticResourceManager.GetFontSize(AppFontSize.Large);
            _searchEntry.TextChanged += (o, a) => 
            {
                Text = a.NewTextValue;
                SearchTextChangeCommand?.Execute(null);
            };
            _searchEntry.RoundedStyle = RoundedEntryStyle.None;
            popoverContainer.Children.Add(_searchEntry, 1, 1);

            var rightButtonContainer = new Grid();
            rightButtonContainer.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
            rightButtonContainer.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
            rightButtonContainer.Padding = new Thickness(0, 0, 20, 0);
            var rightButtonTapGestureRecognizer = new TapGestureRecognizer((v, o) => 
            {
                if (_popupLayout.IsPopupActive)
                {
                    var page = _popupLayout.ParentPage as ExtendedNaviPage;
                    NavigationPage.SetHasNavigationBar(page, true);
                    SlideСontentUp(page);
                    _popupLayout.InputTransparent = true;
                    _popupLayout.DismissPopup();
                    _popupLayout.BackgroundColor = Color.Transparent;
                    _searchEntry.Text = string.Empty;

                }
            });
            rightButtonContainer.GestureRecognizers.Add(rightButtonTapGestureRecognizer);

            var rightbuttonText = new Label();
            rightbuttonText.Text = Constants.CHSIcons.Cancel;
            rightbuttonText.TextColor = Color.White;
            rightbuttonText.FontFamily = StaticResourceManager.GetFontName(AppFont.ChsIcons);
            rightbuttonText.FontSize = StaticResourceManager.GetFontSize(AppFontSize.CHSIconSize);
            rightbuttonText.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
            rightbuttonText.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);

            rightButtonContainer.Children.Add(rightbuttonText, 0, 0);
            popoverContainer.Children.Add(rightButtonContainer, 2, 1);

            #endregion
        }

        #endregion

        #region "COMMANDS"

        public ICommand SearchCommand => new Command(SearchPressed);

        #region "COMMAND HANDLERS"

        void SearchPressed()
        {
            if (!_popupLayout.IsPopupActive)
            {
                var page = _popupLayout.ParentPage as ExtendedNaviPage;
                NavigationPage.SetHasNavigationBar(page,false);
                SlideСontentDown(page);
                _popupLayout.Margin = new Thickness(0, -_popupLayoutHeight, 0, 0);
                _popupLayout.InputTransparent = false;
                _popupLayout.ShowPopup(popoverContainer, false ,0, 0);
                _popupLayout.BackgroundColor = Color.FromRgba(0, 0, 0, 0.1);
            }
        }

        #endregion

        #endregion

        #region "PRIVATE METHODS"

        public void SlideСontentDown(ExtendedNaviPage page)
        {
            if (page.Content is Grid pageContent)
                pageContent.Padding = new Thickness(0, _popupLayoutHeight, 0, 0);
        }

        public void SlideСontentUp(ExtendedNaviPage page)
        {
            if (page.Content is Grid pageContent)
                pageContent.Padding = new Thickness(0, 0, 0, 0);
        }

        #endregion
    }
}
