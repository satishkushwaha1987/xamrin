using CHSBackOffice.Converters;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class DetailPopup : Frame, INotifyPropertyChanged
    {
        #region Fields
        PopupLayout _popupLayout;
        RoundedCornerView gridContainer;
        Grid topPanel;
        Label titleContent;
        Label buttonContent;
        ExtendedStackLayout extendedStack;
        Frame frameContainer;
        bool isBindingContextSetFirst = true;
        #endregion

        #region Command
        public ICommand DetailPressedCommand => new Command(DetailPressed);
        #endregion

        #region INotifyPropertyChanged
        internal void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Bindable Property

        public object Context
        {
            set
            {
                ParentContext = value;
            }
        }

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        public static readonly BindableProperty ParentContextProperty =
           BindableProperty.Create("ParentContext", typeof(object), typeof(DetailPopup), null, propertyChanged: OnParentContextPropertyChanged);

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as DetailPopup)._popupLayout = newValue as PopupLayout;
            }
        }
        #endregion

        #region .CTOR
        public DetailPopup()
        {
            try
            {
                
                CornerRadius = 8;
                Padding = 0;
                BackgroundColor = Color.Transparent;
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                this.BindingContextChanged += DetailPopup_BindingContextChanged;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }
        #endregion

        #region Methods
        public void DetailPressed()
        {
            try
            {
                BindingContext = Support.PopupData.Instance.PopupParameters;
                NotifyPropertyChanged(nameof(this.BindingContext));
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        public void DetailPopup_BindingContextChanged(object sender, EventArgs e)
        {
            bool inprogress = false;
            if (!inprogress)
            {
                inprogress = true;
                if (isBindingContextSetFirst)
                {
                    SetupPopup();
                    isBindingContextSetFirst = false;
                }
                ShowPopup();
            }
        }

        private void ShowPopup()
        {
            try
            {
                if (_popupLayout != null && !_popupLayout.IsPopupActive)
                {
                    if (BindingContext != null && BindingContext is CHSBackOffice.Models.Popup.PopupParameters)
                    {
                        topPanel.BackgroundColor = (BindingContext as CHSBackOffice.Models.Popup.PopupParameters).TitleBackground;
                        titleContent.Text = (BindingContext as CHSBackOffice.Models.Popup.PopupParameters).Title;
                        titleContent.TextColor = (BindingContext as CHSBackOffice.Models.Popup.PopupParameters).TitleTextColor;
                        extendedStack.ItemsSource = (BindingContext as CHSBackOffice.Models.Popup.PopupParameters).Rows;

                        _popupLayout.ShowPopup(frameContainer);
                        _popupLayout.InputTransparent = false;
                        _popupLayout.ParentsView.BackgroundColor = Color.Black;
                        _popupLayout.ParentsView.Opacity = 0.3;
                        _popupLayout.DisableTap();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void Hide()
        {
            _popupLayout.InputTransparent = true;
            _popupLayout.DismissPopup();

            _popupLayout.ParentsView.BackgroundColor = Color.Transparent;
            _popupLayout.EnableTap();
        }

        private void SetupPopup()
        {
            try
            {
                #region Popover

                #region Frame
                frameContainer = new Frame();
                frameContainer.HeightRequest = Device.Idiom == TargetIdiom.Phone ? 290 : 320;
                frameContainer.WidthRequest = Device.Idiom == TargetIdiom.Phone ? 320 : 450;
                frameContainer.Padding = new Thickness(0);
                frameContainer.Margin = new Thickness(0);
                frameContainer.HasShadow = false;
                frameContainer.BorderColor = Color.Transparent;
                frameContainer.OutlineColor = Color.Transparent;
                frameContainer.BackgroundColor = Color.Transparent;
                frameContainer.CornerRadius = 5;
                frameContainer.IsClippedToBounds = true;
                #endregion

                #region Grid
                gridContainer = new RoundedCornerView();
                gridContainer.RoundedCornerRadius = 5;
                gridContainer.RowSpacing = 0;
                gridContainer.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                gridContainer.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                #endregion

                frameContainer.Content = gridContainer;

                #region Top Panel
                topPanel = new Grid();
                topPanel.Padding = new Thickness(0, 0, 5, 0);
                topPanel.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                topPanel.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                #endregion

                #region Title
                titleContent = new Label();
                titleContent.HorizontalTextAlignment = TextAlignment.Center;
                titleContent.VerticalTextAlignment = TextAlignment.Center;
                titleContent.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
                titleContent.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);
                titleContent.FontSize = StaticResourceManager.GetFontSize(AppFontSize.Small);
                #endregion

                #region Button
                Grid buttonContainer = new Grid();
                buttonContainer.WidthRequest = 25;
                buttonContainer.Padding = new Thickness(0, 5, 5, 5);
                buttonContainer.HorizontalOptions = new LayoutOptions(LayoutAlignment.End, true);
                buttonContainer.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
                var tapGestureRecognizer = new TapGestureRecognizer((v, o) =>
                {
                    Hide();
                });

                buttonContainer.GestureRecognizers.Add(tapGestureRecognizer);
                buttonContent = new Label();
                buttonContent.Text = "X";
                buttonContent.HorizontalOptions = new LayoutOptions(LayoutAlignment.End, true);
                buttonContent.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
                buttonContent.SetBinding(Label.TextColorProperty, new Binding("TitleTextColor", source: BindingContext));
                buttonContainer.Children.Add(buttonContent, 0, 0);
                #endregion

                topPanel.Children.Add(titleContent, 0, 0);
                topPanel.Children.Add(buttonContainer, 0, 0);

                gridContainer.Children.Add(topPanel, 0, 0);

                #region Row Template
                var rowTemplate = new DataTemplate(() =>
                {
                    Grid cellContainer = new Grid();
                    cellContainer.ColumnSpacing = 5;
                    cellContainer.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                    cellContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Device.Idiom == TargetIdiom.Phone ? 2 : 1, GridUnitType.Star) });
                    cellContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Device.Idiom == TargetIdiom.Phone ? 3 : 2, GridUnitType.Star) });

                    Label title = new Label();
                    title.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);
                    title.FontSize = StaticResourceManager.GetFontSize(AppFontSize.ExtraSmall);
                    title.SetBinding(Label.TextProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.Title));
                    title.SetBinding(Label.TextColorProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.TitleTextColor));
                    title.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                    title.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
                    title.HorizontalTextAlignment = TextAlignment.Start;

                    cellContainer.Children.Add(title, 0, 0);
                    #region Other
                    Switch @switch = new Switch();
                    @switch.SetBinding(Switch.IsVisibleProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsSwitch));
                    @switch.SetBinding(Switch.IsEnabledProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsEnabled));
                    @switch.SetBinding(Switch.IsToggledProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.BoolValue));
                    @switch.HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, true);
                    @switch.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);

                    cellContainer.Children.Add(@switch, 1, 0);

                    AbsoluteLayout datePickerContainer = new AbsoluteLayout();
                    datePickerContainer.SetBinding(AbsoluteLayout.IsVisibleProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsDatePicker));
                    datePickerContainer.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);

                    ExtendedLabel day = new ExtendedLabel();
                    day.VerticalTextAlignment = TextAlignment.End;
                    day.SetBinding(Label.TextColorProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.ValueTextColor));
                    day.FontSize = StaticResourceManager.GetFontSize(AppFontSize.ExtraSmall);
                    day.SetBinding(ExtendedLabel.TextProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.DateOfValue),
                        converter: new DateConverter(), stringFormat: "{0:dd/MM/yy}");
                    day.SetBinding(AddDataPicker.OnProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsEnabled));
                    day.SetBinding(AddDataPicker.DateProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.DateOfValue),
                        mode: BindingMode.TwoWay, converter: new DateConverter());
                    day.FontSize = StaticResourceManager.GetFontSize(AppFontSize.ExtraSmall);
                    day.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);

                    AbsoluteLayout.SetLayoutBounds(day, new Rectangle(0, .5, .7, .7));
                    AbsoluteLayout.SetLayoutFlags(day, AbsoluteLayoutFlags.All);
                    datePickerContainer.Children.Add(day);

                    ExtendedLabel time = new ExtendedLabel();
                    time.VerticalTextAlignment = TextAlignment.End;
                    time.FontSize = StaticResourceManager.GetFontSize(AppFontSize.ExtraSmall);
                    time.SetBinding(Label.TextColorProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.ValueTextColor));
                    time.SetBinding(Label.TextProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.TimeOfValue), stringFormat: "{0:HH:mm:ss tt}");
                    time.SetBinding(AddTimePicker.OnProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsEnabled));
                    time.SetBinding(AddTimePicker.TimeProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.TimeOfValue),
                        mode: BindingMode.TwoWay, converter: new TimeConverter());
                    time.FontSize = StaticResourceManager.GetFontSize(AppFontSize.ExtraSmall);
                    time.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);

                    AbsoluteLayout.SetLayoutBounds(time, Device.Idiom == TargetIdiom.Phone ? new Rectangle(1.5, .5, .7, .7) : new Rectangle(.9, .5, .7, .7));
                    AbsoluteLayout.SetLayoutFlags(time, AbsoluteLayoutFlags.All);
                    datePickerContainer.Children.Add(time);

                    cellContainer.Children.Add(datePickerContainer, 1, 0);

                    Entry entry = new Entry();
                    entry.BackgroundColor = Color.AliceBlue;
                    entry.SetBinding(Entry.IsVisibleProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsEntry));
                    entry.SetBinding(Entry.KeyboardProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.InputKeyboard));
                    entry.SetBinding(Entry.IsPasswordProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsPassword));
                    entry.SetBinding(Entry.TextProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.TextValue));
                    entry.SetBinding(Entry.PlaceholderProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.Placeholder));
                    entry.SetBinding(Entry.TextColorProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.ValueTextColor));
                    entry.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                    entry.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
                    entry.HorizontalTextAlignment = TextAlignment.Start;
                    entry.FontSize = StaticResourceManager.GetFontSize(AppFontSize.ExtraSmall);
                    entry.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);

                    cellContainer.Children.Add(entry, 1, 0);

                    Picker picker = new Picker();
                    picker.SetBinding(Picker.IsVisibleProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsSelectable));
                    picker.SetBinding(Picker.ItemsSourceProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.AllowedValues));
                    picker.SetBinding(Picker.SelectedItemProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.ObjectValue));
                    picker.SetBinding(Picker.TextColorProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.ValueTextColor));
                    picker.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                    picker.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
                    picker.FontSize = StaticResourceManager.GetFontSize(AppFontSize.ExtraSmall);
                    picker.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);

                    cellContainer.Children.Add(picker, 1, 0);

                    Label label = new Label();
                    label.SetBinding(Label.IsVisibleProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsLabel));
                    label.SetBinding(Label.TextProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.TextValue));
                    label.SetBinding(Label.TextColorProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.ValueTextColor));
                    label.SetBinding(Label.LineBreakModeProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.TextLineBreakMode));
                    label.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                    label.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
                    label.HorizontalTextAlignment = TextAlignment.Start;
                    label.FontSize = StaticResourceManager.GetFontSize(AppFontSize.ExtraSmall);
                    label.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);

                    cellContainer.Children.Add(label, 1, 0);
                    #endregion
                    return cellContainer;
                });

                #endregion

                #region Rows Container
                extendedStack = new ExtendedStackLayout();
                extendedStack.ZebraStyle = false;
                extendedStack.Tappable = false;
                extendedStack.Spacing = 5;
                extendedStack.BackgroundColor = Color.White;
                extendedStack.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                extendedStack.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                extendedStack.Orientation = StackOrientation.Vertical;
                extendedStack.Padding = new Thickness(10);
                extendedStack.ItemTemplate = rowTemplate;
                #endregion

                gridContainer.Children.Add(extendedStack, 0, 1);
                #endregion
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }
        #endregion
    }
}
