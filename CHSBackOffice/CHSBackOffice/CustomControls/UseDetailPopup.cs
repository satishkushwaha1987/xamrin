using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class UseDetailPopup : Frame
    {
        #region Fields
        PopupLayout _popupLayout;
        RoundedCornerView popoverContainer;
        Grid topPanel;
        Label titleContent;
        Label buttonContent;

        protected Frame roundedContainer;
        protected ExtendedStackLayout extendedStack;
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
           BindableProperty.Create("ParentContext", typeof(object), typeof(UseDetailPopup), null, propertyChanged: OnParentContextPropertyChanged);

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as UseDetailPopup)._popupLayout = newValue as PopupLayout;
            }
        }
        #endregion

        #region CTOR
        public UseDetailPopup()
        {
            CornerRadius = 5;
            Padding = 0;
            BackgroundColor = Color.Transparent;
            HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
            VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
        }
        #endregion

        #region Methods
        public void Hide()
        {
            _popupLayout.InputTransparent = true;
            _popupLayout.DismissPopup();

            _popupLayout.ParentsView.BackgroundColor = Color.Transparent;
            _popupLayout.EnableTap();
        }

        public virtual void InitialPopup()
        {
            try
            {
                BindingContext = Support.PopupData.Instance.PopupParameters;

                #region Popover

                #region Frame
                roundedContainer = new Frame();
                roundedContainer.HeightRequest = Device.Idiom == TargetIdiom.Phone ? 200 : 250;
                roundedContainer.WidthRequest = Device.Idiom == TargetIdiom.Phone ? 220 : 450;
                roundedContainer.Padding = new Thickness(0);
                roundedContainer.Margin = new Thickness(0);
                roundedContainer.HasShadow = false;
                roundedContainer.BorderColor = Color.Transparent;
                roundedContainer.OutlineColor = Color.Transparent;
                roundedContainer.BackgroundColor = Color.Transparent;
                roundedContainer.CornerRadius = 5;
                roundedContainer.IsClippedToBounds = true;

                #endregion

                #region Grid
                popoverContainer = new RoundedCornerView();
                popoverContainer.RoundedCornerRadius = 8;
                popoverContainer.RowSpacing = 0;
                popoverContainer.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                popoverContainer.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                popoverContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                popoverContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                popoverContainer.SetBinding(Grid.BackgroundColorProperty, new Binding("TitleBackground", source: BindingContext));
                popoverContainer.SetBinding(RoundedCornerView.FillColorProperty, new Binding("TitleBackground", source: BindingContext));
                roundedContainer.Content = popoverContainer;
                #endregion

                #region Top Panel
                topPanel = new Grid();
                topPanel.Padding = new Thickness(0, 0, 5, 0);
                topPanel.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                topPanel.VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                topPanel.SetBinding(Grid.BackgroundColorProperty, new Binding("TitleBackground", source: BindingContext));
                #endregion

                #region Title
                titleContent = new Label();
                titleContent.HorizontalTextAlignment = TextAlignment.Center;
                titleContent.VerticalTextAlignment = TextAlignment.Center;
                titleContent.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
                titleContent.SetBinding(Label.TextProperty, new Binding("Title", source: BindingContext));
                titleContent.SetBinding(Label.TextColorProperty, new Binding("TitleTextColor", source: BindingContext));
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

                popoverContainer.Children.Add(topPanel, 0, 0);

                #region Row Template
                var popupViewTemplate = new DataTemplate(() =>
                {
                    Grid cellContainer = new Grid();
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
                    BorderlessEntry entry = new BorderlessEntry();
                    entry.RoundedStyle = RoundedEntryStyle.White;
                    entry.PlaceholderColor = Color.LightGray;
                    entry.HeightRequest = Device.Idiom == TargetIdiom.Phone ? 20 : 30;
                    entry.SetBinding(Entry.PlaceholderProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.Placeholder));
                    entry.SetBinding(Entry.IsEnabledProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsEnabled));
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

                    Label label = new Label();
                    label.SetBinding(Label.IsVisibleProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsLabel));
                    label.SetBinding(Label.TextProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.TextValue));
                    label.SetBinding(Label.TextColorProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.ValueTextColor));
                    label.LineBreakMode = LineBreakMode.TailTruncation;
                    label.HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);
                    label.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
                    label.HorizontalTextAlignment = TextAlignment.Start;
                    label.FontSize = StaticResourceManager.GetFontSize(AppFontSize.ExtraSmall);
                    label.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);

                    cellContainer.Children.Add(label, 1, 0);


                    Button btn = new Button();
                    btn.BorderRadius = 8;
                    btn.HeightRequest = Device.Idiom == TargetIdiom.Phone ? 20 : 30;
                    btn.WidthRequest = 150;
                    btn.HorizontalOptions = LayoutOptions.Start;
                    btn.BackgroundColor = Color.FromHex("#e2920a");
                    btn.Text = "Submit";
                    btn.TextColor = Color.Black;
                    btn.SetBinding(Button.IsVisibleProperty, nameof(CHSBackOffice.Models.Popup.PopupRow.IsButton));
                    btn.SetBinding(Button.CommandProperty, new Binding("ConfirmCommand", source: Parent.BindingContext));
                    btn.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);
                    btn.FontSize = StaticResourceManager.GetFontSize(AppFontSize.ExtraSmall);

                    cellContainer.Children.Add(btn, 1, 0);

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
                extendedStack.ItemTemplate = popupViewTemplate;
                extendedStack.ItemsSource = (BindingContext as CHSBackOffice.Models.Popup.PopupParameters)?.Rows;
                #endregion

                popoverContainer.Children.Add(extendedStack, 0, 1);

                #endregion
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        public void DetailPressed()
        {
            if (!_popupLayout.IsPopupActive)
            {
                InitialPopup();
                _popupLayout.ShowPopup(roundedContainer);
                _popupLayout.InputTransparent = false;

                _popupLayout.ParentsView.BackgroundColor = Color.Black;
                _popupLayout.ParentsView.Opacity = 0.3;
                _popupLayout.DisableTap();
            }
        }
        #endregion
    }
}
