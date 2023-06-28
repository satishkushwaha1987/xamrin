using CHSBackOffice.Database;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using CHSBackOffice.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtendedNaviPage : ContentPage, INotifyPropertyChanged
    {
        #region "Private Fields"

        LoadingView _loadingView;
        LocationSwitcher _locationSwitcher;
        NoInternetConnectionView _noInternetView;
        bool _additionalControlsAdded;

        #endregion

        #region "PUBLIC PROPS"

        public static string ChangeLocationBtnName => "changeLocationBtn";
        public static string SearchBtnName => "searchBtn";

        public static event EventHandler<PageOrientationEventsArgs> OnOrientartionChanged = (e, a) => { };

        public static event EventHandler<PageDemensionEventsArgs> OnDemensionChanged = (e, a) => { };

        public static event EventHandler OnKeyboardShow = (e, a) => { };

        public static event EventHandler OnKeyboardHide = (e, a) => { };

        public static PageOrientation CurrentOrientation { get; private set; }

        private bool _isNetworkDependent;
        public bool IsNetworkDependent
        {
            get => _isNetworkDependent;
            set
            {
                _isNetworkDependent = value;
                NetworkChanged();
            }
        }

        private double _width;
        public double WidthExcludeMenu
        {
            get => _width;
            set
            {
                _width = value;
                RaisePropertyChanged(nameof(WidthExcludeMenu));
            }
        }

        private double _height;
        public double HeightExcludeNavBar
        {
            get => _height;
            set
            {
                _height = value;
                RaisePropertyChanged(nameof(HeightExcludeNavBar));
            }
        }


        #endregion

        #region "BINDABLE PROPS"

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

        #region DisappearingCommandProperty

        public ICommand DisappearingCommand
        {
            get { return (ICommand)GetValue(DisappearingCommandProperty); }
            set { SetValue(DisappearingCommandProperty, value); }
        }

        public static readonly BindableProperty DisappearingCommandProperty = BindableProperty.Create(
            nameof(DisappearingCommand), typeof(ICommand), typeof(ExtendedNaviPage), default(ICommand));

        #endregion

        #region AppearingCommandProperty

        public ICommand AppearingCommand
        {
            get { return (ICommand)GetValue(AppearingCommandProperty); }
            set { SetValue(AppearingCommandProperty, value); }
        }

        public static readonly BindableProperty AppearingCommandProperty = BindableProperty.Create(
            nameof(AppearingCommand), typeof(ICommand), typeof(ExtendedNaviPage), default(ICommand));

        #endregion

        #region NaviBarBackgroundColorProperty

        public static readonly BindableProperty NaviBarBackgroundColorProperty = BindableProperty.Create(
                                                            propertyName: "NaviBarBackgroundColor",
                                                            returnType: typeof(Color),
                                                            declaringType: typeof(ExtendedNaviPage),
                                                            defaultValue: Color.FromHex("#30A9D6"),
                                                            defaultBindingMode: BindingMode.OneWay,
                                                            propertyChanged: NaviBarBackgroundColorChanged);

        public Color NaviBarBackgroundColor
        {
            get => (Color)GetValue(NaviBarBackgroundColorProperty);
            set => base.SetValue(NaviBarBackgroundColorProperty, value);
        }

        static void NaviBarBackgroundColorChanged(object sender, object oldValue, object newValue)
        {
            ((ExtendedNaviPage)sender).RaisePropertyChanged(nameof(NaviBarBackgroundColor));
            //CommonViewObjects.TimeoutAlertConfig.BackgroundColor = (System.Drawing.Color)((Color)newValue);
        }

        #endregion

        #region Title

        #region TitleProperty

        public new static readonly BindableProperty TitleProperty = BindableProperty.Create(
                                                            propertyName: "Title",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(ExtendedNaviPage),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.OneWay,
                                                            propertyChanged: PageTitleChanged);

        public new string Title
        {
            get => (string)GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }

        static void PageTitleChanged(object sender, object oldValue, object newValue)
        {
            ((ExtendedNaviPage)sender).RaisePropertyChanged(nameof(Title));
        }

        #endregion

        #region HasTitleIconProperty

        public bool HasTitleIcon
        {
            get { return (bool)GetValue(HasTitleIconProperty); }
            set { SetValue(HasTitleIconProperty, value); }
        }

        public static readonly BindableProperty HasTitleIconProperty = BindableProperty.Create(
            nameof(HasTitleIcon), typeof(bool), typeof(ExtendedNaviPage), false, BindingMode.OneWay, propertyChanged: HasTitleIconChanged);

        static void HasTitleIconChanged(object sender, object oldValue, object newValue)
        {
            ((ExtendedNaviPage)sender).RaisePropertyChanged(nameof(HasTitleIcon));
        }

        #endregion

        #region TitleIconProperty

        public static readonly BindableProperty TitleIconProperty = BindableProperty.Create(
                                                            propertyName: "TitleIcon",
                                                            returnType: typeof(string),
                                                            declaringType: typeof(ExtendedNaviPage),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.OneWay,
                                                            propertyChanged: TitleIconChanged);

        public string TitleIcon
        {
            get => (string)GetValue(TitleIconProperty);
            set => base.SetValue(TitleIconProperty, value);
        }

        static void TitleIconChanged(object sender, object oldValue, object newValue)
        {
            ((ExtendedNaviPage)sender).RaisePropertyChanged(nameof(TitleIcon));
        }
        #endregion

        #region TitleTextColorProperty

        public Color TitleTextColor
        {
            get => (Color)GetValue(TitleTextColorProperty);
            set => SetValue(TitleTextColorProperty, value);
        }

        public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(
                                                            propertyName: nameof(TitleTextColor),
                                                            returnType: typeof(Color),
                                                            declaringType: typeof(ExtendedNaviPage),
                                                            defaultValue: Color.White,
                                                            defaultBindingMode: BindingMode.OneWay,
                                                            propertyChanged: PageTitleTextColorChanged);

        static void PageTitleTextColorChanged(object sender, object oldValue, object newValue)
        {
            ((ExtendedNaviPage)sender).RaisePropertyChanged(nameof(TitleTextColor));
        }

        #endregion

        #endregion

        #region "LeftButton"

        private ToolbarButton _leftButton;
        public ToolbarButton LeftButton
        {
            get => _leftButton;
            set
            {
                SetProperty(ref _leftButton, value);
                HasLeftButton = value != null;
            }
        }

        #endregion

        #region "LeftButtonPressedCommand"

        public ICommand LeftButtonPressedCommand
        {
            get => (ICommand)GetValue(LeftButtonPressedCommandProperty);
            set => base.SetValue(LeftButtonPressedCommandProperty, value);
        }

        public static readonly BindableProperty LeftButtonPressedCommandProperty = BindableProperty.Create(
                                                        propertyName: nameof(LeftButtonPressedCommand),
                                                        returnType: typeof(ICommand),
                                                        declaringType: typeof(ExtendedNaviPage),
                                                        defaultValue: null,
                                                        defaultBindingMode: BindingMode.OneWay,
                                                        propertyChanged: LeftButtonPressedCommandChanged
                                                        );

        static void LeftButtonPressedCommandChanged(object sender, object oldValue, object newValue)
        {
            ((ExtendedNaviPage)sender).RaisePropertyChanged(nameof(LeftButtonPressedCommand));
        }

        #region "HasLeftButton"

        private bool _hasLeftButton;
        public bool HasLeftButton
        {
            get => _hasLeftButton;
            set => SetProperty(ref _hasLeftButton, value);
        }


        #endregion

        #endregion

        #region "ToolbarButtons"

        public ObservableCollection<ToolbarButton> ToolbarButtons
        {
            get;
            set;
        }

        #endregion

        #region "TitleFont"

        private string _titleFont = StaticResourceManager.GetFontName(AppFont.KlavikaCHLight);
        public string TitleFont
        {
            get => _titleFont;
            set => SetProperty(ref _titleFont, value);
        }

        #endregion

        #region "TitleFontSize"

        private double _titleFontSize;
        public double TitleFontSize
        {
            get => _titleFontSize;
            set => SetProperty(ref _titleFontSize, value);
        }

        #endregion

        #region "ToolbarFontSize"

        private int _toolbarFontSize;
        public int ToolbarFontSize
        {
            get => _toolbarFontSize;
            set => SetProperty(ref _toolbarFontSize, value);
        }

        #endregion

        #region "ChangeLocationToolbarButtonIsVisibleProperty"

        public bool ChangeLocationToolbarButtonIsVisible
        {
            get => (bool)GetValue(ChangeLocationToolbarButtonIsVisibleProperty);
            set => SetValue(ChangeLocationToolbarButtonIsVisibleProperty, value);
        }

        public static readonly BindableProperty ChangeLocationToolbarButtonIsVisibleProperty = BindableProperty.Create(
                                                            propertyName: nameof(ChangeLocationToolbarButtonIsVisible),
                                                            returnType: typeof(bool),
                                                            declaringType: typeof(ExtendedNaviPage),
                                                            defaultValue: false,
                                                            defaultBindingMode: BindingMode.OneWay,
                                                            propertyChanged: ChangeLocationToolbarButtonIsVisibleChanged);

        static void ChangeLocationToolbarButtonIsVisibleChanged(object sender, object oldValue, object newValue)
        {
            if (StateInfoService.ThereAreMultipleLocations)
            {
                if ((bool)newValue == true)
                    ((ExtendedNaviPage)sender).AddChangeLocationToolbarButton();
                else
                    ((ExtendedNaviPage)sender).RemoveChangeLocationToolbarButton();
            }
        }

        #endregion

        #region Search

        #region SearchPanelEnabledProperty

        public bool SearchPanelEnabled
        {
            get => (bool)GetValue(SearchPanelEnabledProperty);
            set => SetValue(SearchPanelEnabledProperty, value);
        }

        public static readonly BindableProperty SearchPanelEnabledProperty = BindableProperty.Create(
                                                            propertyName: nameof(SearchPanelEnabled),
                                                            returnType: typeof(bool),
                                                            declaringType: typeof(ExtendedNaviPage),
                                                            defaultValue: false,
                                                            defaultBindingMode: BindingMode.OneWay,
                                                            propertyChanged: SearchPanelEnabledPropertyChanged);

        static void SearchPanelEnabledPropertyChanged(object sender, object oldValue, object newValue)
        {
            ((ExtendedNaviPage)sender).CheckSearchToolbarButton((bool)newValue);
        }

        #endregion

        #region SearchText

        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);

        }

        public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(
                                                            propertyName: nameof(SearchText),
                                                            returnType: typeof(string),
                                                            declaringType: typeof(ExtendedNaviPage),
                                                            defaultValue: "",
                                                            defaultBindingMode: BindingMode.TwoWay);

        #endregion

        #region SearchStartedCommandProperty

        public ICommand SearchStartedCommand
        {
            get { return (ICommand)GetValue(SearchStartedCommandProperty); }
            set { SetValue(SearchStartedCommandProperty, value); }
        }

        public static readonly BindableProperty SearchStartedCommandProperty = BindableProperty.Create(
            nameof(SearchStartedCommand), typeof(ICommand), typeof(ExtendedNaviPage), default(ICommand));

        #endregion

        #region SearchEndedCommandProperty

        public ICommand SearchEndedCommand
        {
            get { return (ICommand)GetValue(SearchEndedCommandProperty); }
            set { SetValue(SearchEndedCommandProperty, value); }
        }

        public static readonly BindableProperty SearchEndedCommandProperty = BindableProperty.Create(
            nameof(SearchEndedCommand), typeof(ICommand), typeof(ExtendedNaviPage), default(ICommand));

        #endregion

        #endregion

        #endregion

        #region .CTOR

        public ExtendedNaviPage()
        {
            try
            {
                InitializeComponent();

                //CommonViewObjects.Instance.IsLoadingVisible = false;

                Init();

                BackgroundColor = Color.White;

                SearchGrid.IsVisible = false;
                SearchGrid.PropertyChanged += SearchPanelEntry_PropertyChanged;

                TitleFontSize = StaticResourceManager.GetFontSize(AppFontSize.Large);
                TitleFont = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);

                ToolbarFontSize = 35;
                var a = Width;
                var b = Height;

                ToolbarButtons = new ObservableCollection<ToolbarButton>();
                ToolbarButtons.CollectionChanged += ((sender, e) => RaisePropertyChanged(nameof(ToolbarButtons)));

                var keyboardService = IocContainer.KeyboardService;
                if (keyboardService != null)
                {
                    keyboardService.Hide += KeyboardService_Hide;
                    keyboardService.Show += KeyboardService_Show;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }

        }

        #endregion

        #region Additional Views (Loading Network, PopupLayout, Location)

        void AddAdditionalWindows()
        {
            if (!_additionalControlsAdded)
            {
                try
                {
                    AddContentToSeparateGrid();
                    AddNetworkFrame();
                    AddWaitingFrame();

                    _additionalControlsAdded = true;

                    CheckAndAddLocationPanel();
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
        }

        void AddContentToSeparateGrid()
        {
            var allContent = this.Content;
            var mainGrid = new Grid()
            {
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
                VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
                InputTransparent = false
            };

            mainGrid.Children.Add(allContent);
            this.Content = mainGrid;
            allContent.Parent = mainGrid;
        }

        void AddWaitingFrame()
        {
            _loadingView = new LoadingView { HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true), VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true) };
            _loadingView.SetBinding(LoadingView.IsVisibleProperty, new Binding(nameof(CommonViewObjects.IsLoadingVisible), BindingMode.OneWay, source: CommonViewObjects.Instance));
            (this.Content as Grid).Children.Add(_loadingView);
        }

        void CheckAndAddLocationPanel()
        {
            if (_additionalControlsAdded && ChangeLocationToolbarButtonIsVisible && _locationSwitcher == null && this.Content != null && this.Content is Grid)
            {
                _locationSwitcher = new LocationSwitcher();
                _locationSwitcher.SetBinding(LocationSwitcher.PopupIsVisibleProperty, new Binding(nameof(ExtendedNaviPageViewModelBase.LocationSwitcherVisible), BindingMode.TwoWay));
                _locationSwitcher.SetBinding(LocationSwitcher.OnLocationChangedCommandProperty, new Binding(nameof(ExtendedNaviPageViewModelBase.OnLocationChangedCommand), BindingMode.TwoWay));
                (this.Content as Grid).Children.Add(_locationSwitcher);
            }
        }

        #region Network 

        void AddNetworkFrame()
        {
            _noInternetView = new NoInternetConnectionView { HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true), VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true) };
            _noInternetView.IsVisible = false;
            (this.Content as Grid).Children.Add(_noInternetView);
            NetworkChanged();
        }

        async void NetworkChanged()
        {
            if (IsNetworkDependent && _noInternetView != null && Network.Current != null)
            {
                bool isNetwrokEnabledAndServerAvailable = Network.Current.IsConnected && await CheckConnectionToServer();

                if (isNetwrokEnabledAndServerAvailable && _noInternetView.IsVisible)//Connection state changed to Connected, maybe data on page is empty
                    App.SharedEventAggregator.GetEvent<Events.NeedToRefreshPageData>().Publish();

                Device.BeginInvokeOnMainThread(() => _noInternetView.IsVisible = !isNetwrokEnabledAndServerAvailable);

                if (Network.Current.IsConnected && !isNetwrokEnabledAndServerAvailable)
                    Task.Run(async () =>
                    {
                        await Task.Delay(5000);
                        App.SharedEventAggregator.GetEvent<Events.NetworkStateChanged>().Publish();
                    });
            }
        }

        async Task<bool> CheckConnectionToServer()
        {
            //If we havent't session ID server can be active? I think no.
            //if (!StateInfoService.HasActiveSession)
            //    return true;

            try
            {
                var serverConnectionResult = await IocContainer.CHSServiceAgent.TestConnection();
                return serverConnectionResult;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region "PAGE EVENTS"

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (this.BindingContext is ExtendedNaviPageViewModelBase)
                (this.BindingContext as ExtendedNaviPageViewModelBase).OnPageDisappearing();

            if (DisappearingCommand != null)
                DisappearingCommand.Execute(null);

            App.SharedEventAggregator.GetEvent<Events.NetworkStateChanged>().Unsubscribe(NetworkChanged);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IocContainer.SetToolbarColor != null)
                IocContainer.SetToolbarColor.SetToolbarColor(NaviBarBackgroundColor);

            if (this.BindingContext is ExtendedNaviPageViewModelBase)
                (this.BindingContext as ExtendedNaviPageViewModelBase).OnPageAppearing();

            if (AppearingCommand != null)
                AppearingCommand.Execute(null);

            App.SharedEventAggregator.GetEvent<Events.NetworkStateChanged>().Subscribe(NetworkChanged);
        }

        private void SearchPanelEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(VisualElement.IsVisible))
            {
                SearchText = "";
                SearchPanelEntry.Text = "";
                if (SearchGrid.IsVisible)
                    SearchPanelEntry.Focus();

                if (SearchGrid.IsVisible)
                {
                    if (SearchStartedCommand != null && SearchStartedCommand.CanExecute(null))
                        SearchStartedCommand?.Execute(null);
                }
                else
                {
                    if (SearchEndedCommand != null && SearchEndedCommand.CanExecute(null))
                        SearchEndedCommand?.Execute(null);
                    SearchPanelEntry.Unfocus();
                }

            }
        }

        private void KeyboardService_Hide(object sender, Support.Interfaces.SoftwareKeyboardEventArgs args)
        {
            if (_width == -1 && _height == -1)
            {
                return;
            }
            OnKeyboardHide?.Invoke(this, EventArgs.Empty);
        }

        private void KeyboardService_Show(object sender, Support.Interfaces.SoftwareKeyboardEventArgs args)
        {
            if (_width == -1 && _height == -1)
            {
                return;
            }
            OnKeyboardShow?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            AddAdditionalWindows();
        }

        #endregion

        #region Protected Methods

        protected override void OnSizeAllocated(double width, double height)
        {
            var oldWidth = _width;
            const double sizenotallocated = -1;

            base.OnSizeAllocated(width, height);
            if (Equals(_width, width) && Equals(_height, height)) return;

            WidthExcludeMenu = width;
            HeightExcludeNavBar = height;
            OnDemensionChanged?.Invoke(this, new PageDemensionEventsArgs { DemensionWidth = WidthExcludeMenu, DemensionHeight = HeightExcludeNavBar });
            if (Equals(oldWidth, sizenotallocated)) return;

            if (!Equals(width, oldWidth))
            {
                UpdateDemension();
            }
        }

        #endregion

        #region "PRIVATE METHODS"

        private void UpdateDemension()
        {
            if ((_width == -1 && _height == -1))
            {
                return;
            }
            CurrentOrientation = (_width < _height) ? PageOrientation.Vertical : PageOrientation.Horizontal;
            OnOrientartionChanged?.Invoke(this, new PageOrientationEventsArgs(CurrentOrientation));
        }

        public void RaiseOnOrientationChanged()
        {
            OnOrientartionChanged?.Invoke(this, new PageOrientationEventsArgs(CurrentOrientation));
        }

        private void AddChangeLocationToolbarButton()
        {
            try
            {
                if (!ToolbarButtons.Any(b => b.Name == ChangeLocationBtnName) && this.BindingContext is ExtendedNaviPageViewModelBase)
                {
                    ToolbarButtons.Insert(0, new ToolbarButton
                    {
                        Name = ChangeLocationBtnName,
                        FontName = StaticResourceManager.GetFontName(AppFont.ChsIcons),
                        Text = Constants.CHSIcons.LocationSwitcher,
                        FontSize = StaticResourceManager.GetFontSize(AppFontSize.CHSIconSize)
                    });
                    (this.BindingContext as ExtendedNaviPageViewModelBase).NeedToRefreshDataOnLocationChanged = true;
                    CheckAndAddLocationPanel();
                    RaisePropertyChanged(nameof(ToolbarButtons));
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void RemoveChangeLocationToolbarButton()
        {
            try
            {
                var changeLocationBtn = ToolbarButtons.FirstOrDefault(b => b.Name == ChangeLocationBtnName);
                if (changeLocationBtn != null)
                {
                    ToolbarButtons.Remove(changeLocationBtn);
                    RaisePropertyChanged(nameof(ToolbarButtons));
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void CheckSearchToolbarButton(bool needs)
        {
            try
            {
                if (this.BindingContext is ExtendedNaviPageViewModelBase)
                {
                    SearchGrid.SetBinding(VisualElement.IsVisibleProperty, new Binding(nameof(ExtendedNaviPageViewModelBase.SearchIsVisible), source: this.BindingContext));
                    TitleGrid.SetBinding(VisualElement.IsVisibleProperty, new Binding(nameof(ExtendedNaviPageViewModelBase.TitleIsVisible), source: this.BindingContext));

                    var searchBtn = ToolbarButtons.FirstOrDefault(b => b.Name == SearchBtnName);
                    if (needs && searchBtn == null)
                    {
                        int btnIndex = 0;
                        if (ToolbarButtons.Any(b => b.Name == ChangeLocationBtnName))
                            btnIndex = 1;
                        ToolbarButtons.Insert(btnIndex, new ToolbarButton
                        {
                            Name = SearchBtnName,
                            FontName = StaticResourceManager.GetFontName(AppFont.ChsIcons),
                            Text = Constants.CHSIcons.Search,
                            FontSize = StaticResourceManager.GetFontSize(AppFontSize.CHSIconSearchSize)
                        });
                    }

                    if (!needs && searchBtn != null)
                        ToolbarButtons.Remove(searchBtn);

                    RaisePropertyChanged(nameof(ToolbarButtons));
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void Init()
        {
            _width = this.Width;
            _height = this.Height;
        }

        #endregion
    }

    public class PageDemensionEventsArgs : EventArgs
    {
        public double DemensionWidth { get; set; }

        public double DemensionHeight { get; set; }
    }

    public class PageOrientationEventsArgs : EventArgs
    {
        public PageOrientation Orientation { get; set; }

        public PageOrientationEventsArgs(PageOrientation orientation)
        {
            Orientation = orientation;
        }

    }

    public enum PageOrientation
    {
        Horizontal = 0,
        Vertical = 1,
    }
}
