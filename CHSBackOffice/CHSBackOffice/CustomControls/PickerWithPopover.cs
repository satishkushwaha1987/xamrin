using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using static CHSBackOffice.CustomControls.PopupLayout;

namespace CHSBackOffice.CustomControls
{
    public class PickerWithPopover : Frame
    {
        #region Fileds

        Label _pickerImage;
        PopupLayout _popupLayout;
        ListView popupView;
        ViewCell viewCell;

        protected RoundedCornerView _contentGrid;
        protected Label _pickerText;
        protected RelativeLayout upIconContainer;
        protected Grid popoverContainer;
        protected Frame frameContainer;
        protected Image upIcon;

        public double _Width { set; get; }

        double _height;
        public double _Height 
        {
            set => _height = value; 
            get 
            {
                return 
                    DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape 
                    ? (Device.Idiom == TargetIdiom.Phone ? _height / 2 : _height * 2 / 3 )
                    : _height; 
            }
        }

        static double _DefaultWidth => (Device.Idiom == TargetIdiom.Phone ? 200 : 250);
        static double _DefaultHeight => 
            DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape 
            ? Device.Idiom == TargetIdiom.Phone ? 150 : 250 
            : Device.Idiom == TargetIdiom.Phone ? 250 : 350;

        #endregion

        #region Events

        public event EventHandler<PopupIsActiveArgs> RaisePopupIsActive;

        protected virtual void OnRaisePopupIsActiveEvent(PopupIsActiveArgs e)
        {
            RaisePopupIsActive?.Invoke(this, e);
        }

        #endregion

        #region Properties
        public Models.Popup.PopoverItem OldSelectedItem { get; set; }
        public IEnumerable<Models.Popup.PopoverItem> OldSelectedItems { get; set; }
        #endregion

        #region Bindable Property

        #region ParentContext
        public object Context
        {
            set => ParentContext = value;
        }

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        public static readonly BindableProperty ParentContextProperty =
           BindableProperty.Create("ParentContext", typeof(object), typeof(PickerWithPopover), null, propertyChanged: OnParentContextPropertyChanged);

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                (bindable as PickerWithPopover)._popupLayout = newValue as PopupLayout;
                (bindable as PickerWithPopover)._popupLayout.IsTappable = true;
            }
        }

        #endregion

        #region IsPopupActive
        public bool IsPopupActive
        {
            get { return (bool)GetValue(IsPopupActiveProperty); }
            set { SetValue(IsPopupActiveProperty, value); }
        }

        public static BindableProperty IsPopupActiveProperty = 
            BindableProperty.Create(nameof(IsPopupActive), typeof(bool), typeof(PickerWithPopover), false, propertyChanged: OnPopupIsActiveChanged);

        private static void OnPopupIsActiveChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {
                var obj = bindable as PickerWithPopover;
                obj.OnRaisePopupIsActiveEvent(new PopupIsActiveArgs() { IsActive = (bool)newValue });
            }
        }

        #endregion

        #region XOffset
        public double XOffset
        {
            get { return (double)GetValue(XOffsetProperty); }
            set { SetValue(XOffsetProperty, value); }
        }

        public static readonly BindableProperty XOffsetProperty =
            BindableProperty.Create(nameof(XOffset), typeof(double), typeof(PickerWithPopover), (double)0);
        #endregion

        #region YOffset
        public double YOffset
        {
            get { return (double)GetValue(YOffsetProperty); }
            set { SetValue(YOffsetProperty, value); }
        }

        public static readonly BindableProperty YOffsetProperty =
            BindableProperty.Create(nameof(YOffset), typeof(double), typeof(PickerWithPopover), (double)(Device.RuntimePlatform == Device.Android ? 50 : 70));

        #endregion

        #region Location
        public PopupLocation Location
        {
            get { return (PopupLocation)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        public static readonly BindableProperty LocationProperty =
            BindableProperty.Create("Location", typeof(PopupLocation), typeof(PickerWithPopover), PopupLocation.Top, propertyChanged:OnLocationChanged);

        private static void OnLocationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
        }
        #endregion

        #region VerticalPosition
        public LocationOption VerticalPosition
        {
            get { return (LocationOption)GetValue(VerticalPositionProperty); }
            set { SetValue(VerticalPositionProperty, value); }
        }

        public static readonly BindableProperty VerticalPositionProperty =
            BindableProperty.Create("VerticalPosition", typeof(LocationOption), typeof(PickerWithPopover), LocationOption.RelativeTo, propertyChanged: OnLocationChanged);
        #endregion

        #region IsIconVisible
        public bool IsIconVisible
        {
            get { return (bool)GetValue(IsIconVisibleProperty); }
            set { SetValue(IsIconVisibleProperty, value); }
        }

        public static readonly BindableProperty IsIconVisibleProperty =
            BindableProperty.Create(nameof(IsIconVisible), typeof(bool), typeof(PickerWithPopover), true);
        #endregion

        #region Placeholder
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create("Placeholder", typeof(string), typeof(PickerWithPopover), string.Empty,propertyChanged:OnPlaceholderChanged);

        private static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null && newValue != oldValue)
                (bindable as PickerWithPopover).RefreshCaptionString();
        }
        #endregion

        #region RoundedCornerRasius
        public double RoundedCornerRadius
        {
            get { return (double)GetValue(RoundedCornerRadiusProperty); }
            set { SetValue(RoundedCornerRadiusProperty, value); }
        }

        public static readonly BindableProperty RoundedCornerRadiusProperty = BindableProperty.Create(nameof(RoundedCornerRadius), typeof(double), typeof(PickerWithPopover), 8.0, propertyChanged: OnRoundedCornerRadiusChanged);

        private static void OnRoundedCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerWithPopover)._contentGrid.RoundedCornerRadius = (double)newValue;
            (bindable as PickerWithPopover).RoundedCornerRadius = (double)newValue;
        }
        #endregion

        #region SelectedItem
        public Models.Popup.PopoverItem SelectedItem
        {
            get { return (Models.Popup.PopoverItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(Models.Popup.PopoverItem), typeof(PickerWithPopover), null, propertyChanged: OnSelectedItemChanged);

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerWithPopover).RefreshCaptionString();
        }
        #endregion

        #region ItemsSource
        public IEnumerable<Models.Popup.PopoverItem> ItemsSource
        {
            get { return (IEnumerable<Models.Popup.PopoverItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource), 
                typeof(IEnumerable<Models.Popup.PopoverItem>), 
                typeof(PickerWithPopover), 
                defaultValue: null, 
                defaultBindingMode: BindingMode.OneWay, 
                propertyChanged: OnItemsChanged);

        private static void OnItemsChanged(BindableObject bindable, object oldVal, object newVal)
        {
            if (newVal != oldVal && newVal != null)
            {
                var newValEnum = newVal as IEnumerable<Models.Popup.PopoverItem>;
                var picker = bindable as PickerWithPopover;
                picker.popupView.ItemsSource = newValEnum;
                picker.RefreshCaptionString();
            }
        }

        #endregion

        #region IsMultiSelect
        public bool IsMultiSelect
        {
            get
            {
                return (bool)GetValue(IsMultiSelectProperty);
            }
            set
            {
                SetValue(IsMultiSelectProperty, value);
            }
        }

        public static readonly BindableProperty IsMultiSelectProperty =
            BindableProperty.Create(nameof(IsMultiSelect), typeof(bool), typeof(PickerWithPopover), false);
        #endregion

        #region IsBotomVisible
        public bool IsBottomVisible
        {
            get
            {
                return (bool)GetValue(IsBottomVisibleProperty);
            }
            set
            {
                SetValue(IsBottomVisibleProperty, value);
            }
        }

        public static readonly BindableProperty IsBottomVisibleProperty =
            BindableProperty.Create(nameof(IsBottomVisible), typeof(bool), typeof(PickerWithPopover), true, propertyChanged: IsBottomVisibleChanged);

        static void IsBottomVisibleChanged(object o, object oldVal, object newVal)
        {
            ((PickerWithPopover)o).OnPropertyChanged(nameof(IsBottomVisible));
        }
        #endregion

        #region BorderWidth
        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }

        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(PickerWithPopover), 0);
        #endregion

        #region Height
        public new double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        public static new readonly BindableProperty HeightProperty = BindableProperty.Create(nameof(Height), typeof(double), typeof(PickerWithPopover),
                                                                        _DefaultHeight, propertyChanged:OnHeightChanged);

        static void OnHeightChanged(object o, object oldValue, object newValue)
        {
            if (oldValue != newValue && (double)newValue != -1)
            {
                ((PickerWithPopover)o)._Height = (double)newValue;
                ((PickerWithPopover)o).DemensionsPopover();
            }
                
        }
        #endregion

        #region Width
        public new double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public static new readonly BindableProperty WidthProperty = BindableProperty.Create(nameof(Width), typeof(double), typeof(PickerWithPopover),
                                                                            _DefaultWidth,
                                                                            propertyChanged:OnWidthChanged);

        static void OnWidthChanged(object o, object oldValue, object newValue)
        {
            if (oldValue != newValue && (double)newValue != -1)
            {
                ((PickerWithPopover)o)._Width = (double)newValue;
                ((PickerWithPopover)o).DemensionsPopover();
            }
                
        }
        #endregion
        #endregion

        #region .CTOR

        public PickerWithPopover()
        {
            try
            {
                InitPopover();
                ExtendedNaviPage.OnOrientartionChanged += ExtendedNaviPage_OnOrientartionChanged;

            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        

        #region Methods

        public PickerWithPopover(double width = -1, double height = -1)
        {
            _Width = width;
            _Height = height;
            InitPopover();
        }

        public virtual void InitPopover()
        {
            Padding = new Thickness(0);
            Margin = new Thickness(0);
            IsClippedToBounds = true;
            HasShadow = false;
            BorderColor = Color.Transparent;
            OutlineColor = Color.Transparent;
            BackgroundColor = Color.Transparent;
            CornerRadius = 8;
            this.SetBinding(Frame.CornerRadiusProperty, new Binding(nameof(RoundedCornerRadius), source: this));

            #region Popover
            popoverContainer = new Grid();
            DemensionsPopover();
            popoverContainer.RowSpacing = 0;
            popoverContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            popoverContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(16) });

            upIconContainer = new RelativeLayout();
            upIcon = new Image();
            upIcon.Source = "PopupPickerArrowBottom.png";// "UpIcon.png";
            upIcon.Aspect = Aspect.AspectFit;
            upIconContainer.Children.Add(upIcon,
                xConstraint: Constraint.RelativeToParent((parent) => { return parent.Width * 0.92; }),
                widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width * 0.05; }),
                heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; }));

            upIcon.SetBinding(RelativeLayout.IsVisibleProperty, new Binding(nameof(IsBottomVisible), source: this));

            frameContainer = new Frame();
            frameContainer.Padding = new Thickness(0);
            frameContainer.Margin = new Thickness(0);
            frameContainer.HasShadow = false;
            frameContainer.BorderColor = Color.Transparent;
            frameContainer.OutlineColor = Color.Transparent;
            frameContainer.BackgroundColor = Color.Transparent;
            frameContainer.CornerRadius = 5;
            frameContainer.IsClippedToBounds = true;

            popoverContainer.Children.Add(frameContainer, 0, 0);
            popoverContainer.Children.Add(upIcon, 0, 1);

            RoundedCornerView gridContainer = new RoundedCornerView();
            gridContainer.BackgroundColor = Color.White;
            gridContainer.FillColor = Color.White;
            gridContainer.BorderWidth = 0;
            gridContainer.RoundedCornerRadius = 5;
            gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Device.Idiom == TargetIdiom.Phone ? 30 : 50) });
            gridContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            frameContainer.Content = gridContainer;

            Grid topContainer = new Grid();
            topContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            topContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            topContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            topContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            topContainer.RowDefinitions.Add(new RowDefinition { Height = 1 });
            topContainer.BackgroundColor = StaticResourceManager.GetColor(AppColor.PopoverTopPanelColor);

            BoxView line = new BoxView();
            line.HeightRequest = 1;
            line.BackgroundColor = StaticResourceManager.GetColor(AppColor.PopoverTopPanelLineColor);
            topContainer.Children.Add(line, 0, 1);
            Grid.SetColumnSpan(line, 3);

            Button cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.FontSize = StaticResourceManager.GetFontSize(AppFontSize.Small);
            cancelButton.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);
            cancelButton.TextColor = StaticResourceManager.GetColor(AppColor.PopoverTopPanelButtonColor);
            cancelButton.Margin = new Thickness(0, 5, 0, 0);
            cancelButton.BackgroundColor = Color.Transparent;
            cancelButton.Clicked += OnCancelButtonClicked;
            topContainer.Children.Add(cancelButton, 0, 0);

            Label titleLabel = new Label();
            titleLabel.Text = "Select";
            titleLabel.Margin = new Thickness(0, 5, 0, 0);
            titleLabel.FontSize = StaticResourceManager.GetFontSize(AppFontSize.Medium);
            titleLabel.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHMedium);
            titleLabel.HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, true);
            titleLabel.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
            topContainer.Children.Add(titleLabel, 1, 0);

            Button doneButton = new Button();
            doneButton.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
            doneButton.BackgroundColor = Color.Transparent;
            doneButton.TextColor = StaticResourceManager.GetColor(AppColor.PopoverTopPanelButtonColor);
            doneButton.Margin = new Thickness(0, 5, 0, 0);
            doneButton.Text = "Done";
            doneButton.FontSize = StaticResourceManager.GetFontSize(AppFontSize.Small);
            doneButton.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);
            doneButton.Clicked += OnDoneButtonClicked;
            topContainer.Children.Add(doneButton, 2, 0);


            var popupViewTemplate = new DataTemplate(GetTemplate);

            popupView = new ListView();
            popupView.HasUnevenRows = true;
            popupView.BackgroundColor = Color.White;

            popupView.SetBinding(ListView.SelectedItemProperty, new Binding(nameof(SelectedItem), BindingMode.TwoWay, source: this));
            popupView.ItemTemplate = popupViewTemplate;
            popupView.SeparatorColor = Color.Transparent;
            gridContainer.Children.Add(topContainer, 0, 0);
            gridContainer.Children.Add(popupView, 0, 1);

            #endregion

            #region Input
            _contentGrid = new RoundedCornerView
            {
                BorderWidth = 0,
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
                VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, true)
            };
            _contentGrid.SetBinding(RoundedCornerView.RoundedCornerRadiusProperty, new Binding(nameof(RoundedCornerRadius), source: this));
            
            _contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            _contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });

            _contentGrid.SetBinding(Grid.BackgroundColorProperty, new Binding(nameof(BackgroundColor), source: this));
            _contentGrid.SetBinding(RoundedCornerView.FillColorProperty, new Binding(nameof(BackgroundColor), source: this));
            _contentGrid.SetBinding(RoundedCornerView.BorderColorProperty, new Binding(nameof(BorderColor), source: this));
            _contentGrid.SetBinding(RoundedCornerView.BorderWidthProperty, new Binding(nameof(BorderWidth), source: this));

            _pickerText = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,
                VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true),
                FontSize = StaticResourceManager.GetFontSize(AppFontSize.ExtraSmall),
                FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular),
                BindingContext = SelectedItem
            };
            _pickerText.SetBinding(Label.TextProperty, new Binding(nameof(Placeholder), source: this));

            _pickerImage = new Label
            {
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, true),
                VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true),
                FontFamily = StaticResourceManager.GetFontName(AppFont.ChsIcons),
                Text = Constants.CHSIcons.PickerImage,
                TextColor = Constants.CHSIconColors.PickerImageColor
            };
            _pickerImage.SetBinding(Label.IsVisibleProperty, new Binding(nameof(IsIconVisible), source: this));

            _contentGrid.Children.Add(_pickerText, 0, 0);
            _contentGrid.Children.Add(_pickerImage, 1, 0);
            Content = _contentGrid;
            #endregion

            #region Tap
            TapGestureRecognizer tap = new TapGestureRecognizer((v, o) =>
            {
                if (_popupLayout.IsPopupActive)
                {
                    this.IsPopupActive = false;
                    _popupLayout.DismissPopup();
                }
                else
                {
                    this.IsPopupActive = true;
                    if (VerticalPosition == LocationOption.RelativeTo)
                    {
                        _popupLayout.ShowPopup(popoverContainer, this, Location);
                    }
                    else if (VerticalPosition == LocationOption.RelativeToParent)
                    {
                        var paddingTop = ((Xamarin.Forms.Layout<Xamarin.Forms.View>)Parent)?.Padding.Top;
                        _popupLayout.ShowPopup(popoverContainer, (VisualElement)Content, Location);
                    }
                    if (_popupLayout.ParentsView != null)
                    {
                        _popupLayout.ParentsView.BackgroundColor = Color.Black;
                        _popupLayout.ParentsView.Opacity = 0.3;
                    }
                }
            });
            Content.GestureRecognizers.Add(tap);
            #endregion
        }

        public virtual void DemensionsPopover()
        {
            popoverContainer.WidthRequest = _Width == 0 ? _DefaultWidth : _Width;
            popoverContainer.HeightRequest = _Height == 0 ? _DefaultHeight : _Height;
        }

        private Cell GetTemplate()
        {
            var content = new ViewCell();
            content.BindingContextChanged += OnBindingContextChanged;
            return content;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            var cell = (ViewCell)sender;
            Grid cellContainer = new Grid() { HeightRequest = Device.Idiom == TargetIdiom.Phone ? 25 : 35 };
            cellContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            cellContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
            cellContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            cellContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1) });

            var line = new BoxView();
            line.HeightRequest = 1;
            line.BackgroundColor = StaticResourceManager.GetColor(AppColor.PopoverTopPanelLineColor);

            var nameLabel = new Label();
            nameLabel.Margin = new Thickness(10, 0, 0, 0);
            nameLabel.HorizontalOptions = new LayoutOptions(LayoutAlignment.Start, true);
            nameLabel.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
            nameLabel.FontSize = StaticResourceManager.GetFontSize(AppFontSize.Small);
            nameLabel.FontFamily = StaticResourceManager.GetFontName(AppFont.KlavikaCHRegular);
            nameLabel.SetBinding(Label.TextProperty, "Key");

            var selectedImage = new Label();
            selectedImage.HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, true);
            selectedImage.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, true);
            selectedImage.FontFamily = StaticResourceManager.GetFontName(AppFont.ChsIcons);
            selectedImage.Text = Constants.CHSIcons.SelectedIcon;
            selectedImage.TextColor = Constants.CHSIconColors.SelectedIconColor;
            selectedImage.SetBinding(Label.IsVisibleProperty, "Selected");

            cellContainer.Children.Add(line, 0, 1);
            Grid.SetColumnSpan(line, 2);
            cellContainer.Children.Add(nameLabel, 0, 0);
            cellContainer.Children.Add(selectedImage, 1, 0);
            viewCell = new ViewCell { View = cellContainer };
            cell.View = viewCell.View;
            cell.ForceUpdateSize();
            cell.Tapped += ViewCell_Tapped;
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var cell = sender as ViewCell;
            cell.ForceUpdateSize();
        }

        protected void OnDoneButtonClicked(object sender, EventArgs e)
        {
            if (IsMultiSelect)
                OldSelectedItems = ItemsSource?.Where(x => x.Selected == true).ToList();
            else
                OldSelectedItem = SelectedItem;
            
            this.IsPopupActive = false;
            RefreshCaptionString();
            DismissPopover();
        }

        string GetCaptionString(IEnumerable<Models.Popup.PopoverItem> list, Models.Popup.PopoverItem selectedItem)
        {
            string selectedString = Placeholder;
            if (IsMultiSelect && list != null && list.Count() > 0)
            {
                selectedString = string.Empty;
                foreach (var item in list)
                {
                    if (selectedString.IsNullOrEmpty())
                        selectedString = item.Key;
                    else
                        selectedString += string.Format(", {0}", item.Key);
                }
            }

            if (!IsMultiSelect && selectedItem != null)
                selectedString = selectedItem.Key;

            return selectedString;
        }

        void RefreshCaptionString()
        {
            _pickerText.Text = GetCaptionString(ItemsSource?.Where(i => i.Selected), SelectedItem);
        }

        //void ResetCaptionString()
        //{
        //    _pickerText.Text = GetCaptionString(OldSelectedItems, OldSelectedItem);
        //}

        #region "Events Handling"

        private void ExtendedNaviPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            DismissPopover();
            DemensionsPopover();
        }

        protected void OnCancelButtonClicked(object sender, EventArgs e)
        {
            this.IsPopupActive = false;
            if (IsMultiSelect)
            {
                if (OldSelectedItems != null)
                    this.ItemsSource.ForEach(x => x.Selected = OldSelectedItems.Contains(x));
                else
                    this.ItemsSource.ForEach(x => x.Selected = false);
            }
            else
            {
                SelectedItem = OldSelectedItem;
                if (SelectedItem != null)
                    this.ItemsSource.ForEach(x => x.Selected = x.Equals(SelectedItem) ? true : false);
                else
                    this.ItemsSource.ForEach(x => x.Selected = false);
            }

            RefreshCaptionString();
            DismissPopover();
        }

        public void DismissPopover()
        {
            _popupLayout.DismissPopup();
            if (_popupLayout.ParentsView != null & !_popupLayout.HasInnerPopover)
                _popupLayout.ParentsView.BackgroundColor = Color.Transparent;
        }

        #endregion

        #endregion

        public enum LocationOption
        {
            RelativeTo,
            RelativeToParent
        }
    }
}
