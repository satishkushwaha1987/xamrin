using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using Xamarin.Forms;
using CHSBackOffice.Behaviors;
using CHSBackOffice.Extensions;
using CHSBackOffice.Support;

namespace CHSBackOffice.CustomControls
{
    public class WrapLayout : Layout<View>
    {
        #region Properties
        bool waitingForBindingContext = false;
        public event LayoutItemAddedEventHandler ItemCreated;
        Dictionary<Size, LayoutData> layoutDataCache = new Dictionary<Size, LayoutData>();
        #endregion

        #region Commands
        ICommand TappedCommand => new Xamarin.Forms.Command((item) =>
        {
            if (ItemTappedCommand != null)
                if (ItemTappedCommand.CanExecute(item))
                    ItemTappedCommand.Execute(item);
            if (DetailCommand != null)
                if (DetailCommand.CanExecute(item))
                    DetailCommand.Execute(item);
        });
        #endregion

        #region "Bindable Properties"

        #region "ColumnSpacing"


        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(
            "ColumnSpacing",
            typeof(double),
            typeof(WrapLayout),
            5.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((WrapLayout)bindable).InvalidateLayout();
            });

        public double ColumnSpacing
        {
            set { SetValue(ColumnSpacingProperty, value); }
            get { return (double)GetValue(ColumnSpacingProperty); }
        }
        #endregion

        #region RowSpacing
        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(
            "RowSpacing",
            typeof(double),
            typeof(WrapLayout),
            5.0,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((WrapLayout)bindable).InvalidateLayout();
            });

        public double RowSpacing
        {
            set { SetValue(RowSpacingProperty, value); }
            get { return (double)GetValue(RowSpacingProperty); }
        }

        #endregion

        #region ItemSource

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<object>), typeof(WrapLayout), defaultValue: null, defaultBindingMode: BindingMode.OneWay, propertyChanged: OnItemsChanged);

        private static void OnItemsChanged(BindableObject bindable, object oldVal, object newVal)
        {
            IEnumerable<object> oldValue = oldVal as IEnumerable<object>;
            IEnumerable<object> newValue = newVal as IEnumerable<object>;

            var control = bindable as WrapLayout;
            var oldObservableCollection = oldValue as INotifyCollectionChanged;
            if (oldObservableCollection != null)
            {
                oldObservableCollection.CollectionChanged -= control.OnItemsSourceCollectionChanged;
            }

            if (control.BindingContext == null)
            {
                control.waitingForBindingContext = true;
                return;
            }

            control.waitingForBindingContext = false;
            var newObservableCollection = newValue as INotifyCollectionChanged;

            if (newObservableCollection != null)
            {
                newObservableCollection.CollectionChanged += control.OnItemsSourceCollectionChanged;
            }

            try
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => control.Children.Clear());
                if (newValue != null)
                {
                    foreach (var item in newValue)
                    {
                        var view = control.CreateChildViewFor(item);
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() => control.Children.Add(view));
                        control.OnItemCreated(view);
                    }
                }
                control.UpdateChildrenLayout();
                control.InvalidateLayout();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }
        #endregion

        #region "ItemTemplate"

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(WrapLayout), defaultValue: null, defaultBindingMode: BindingMode.OneWay);

        #endregion

        #region "ItemTappedAnimationType"

        public TappedAnimationType ItemTappedAnimationType
        {
            get => (TappedAnimationType)GetValue(ItemTappedAnimationTypeProperty);
            set => SetValue(ItemTappedAnimationTypeProperty, value);
        }

        public static readonly BindableProperty ItemTappedAnimationTypeProperty = BindableProperty.Create(
                                                             propertyName: nameof(ItemTappedAnimationType),
                                                             returnType: typeof(TappedAnimationType),
                                                             declaringType: typeof(WrapLayout),
                                                             defaultValue: TappedAnimationType.None);

        #endregion

        #region "ItemTappedCommand"

        public ICommand ItemTappedCommand
        {
            get => (ICommand)GetValue(ItemTappedCommandProperty);
            set => base.SetValue(ItemTappedCommandProperty, value);
        }


        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.Create(
                                                            propertyName: "ItemTappedCommand",
                                                            returnType: typeof(ICommand),
                                                            declaringType: typeof(WrapLayout),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.OneWay,
                                                            propertyChanged: OnItemTappedCommandChanged);

        static void OnItemTappedCommandChanged(object sender, object oldVal, object newVal)
        {
            WrapLayout s = (WrapLayout)sender;
            if (s.ItemsSource != null)
                OnItemsChanged(s, null, s.ItemsSource);
        }

        #endregion

        #region "DetailCommand"

        public ICommand DetailCommand
        {
            get => (ICommand)GetValue(DetailCommandProperty);
            set => base.SetValue(DetailCommandProperty, value);
        }

        public static readonly BindableProperty DetailCommandProperty = BindableProperty.Create(
                                                            propertyName: "TappedCommand",
                                                            returnType: typeof(ICommand),
                                                            declaringType: typeof(WrapLayout),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.OneWay,

                                                            propertyChanged: OnDetailCommandChanged);

        static void OnDetailCommandChanged(object sender, object oldVal, object newVal)
        {
            WrapLayout s = (WrapLayout)sender;
            if (s.ItemsSource != null)
                OnItemsChanged(s, null, s.ItemsSource);
        }

        #endregion

        #endregion

        #region Methods
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            LayoutData layoutData = GetLayoutData(widthConstraint, heightConstraint);
            if (layoutData.VisibleChildCount == 0)
            {
                return new SizeRequest();
            }

            Size totalSize = new Size(layoutData.CellSize.Width * layoutData.Columns + ColumnSpacing * (layoutData.Columns - 1),
                                      layoutData.CellSize.Height * layoutData.Rows + RowSpacing * (layoutData.Rows - 1));
            return new SizeRequest(totalSize);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            LayoutData layoutData = GetLayoutData(width, height);

            if (layoutData.VisibleChildCount == 0)
            {
                return;
            }

            double xChild = x;
            double yChild = y;
            int row = 0;
            int column = 0;

            foreach (View child in Children)
            {
                if (!child.IsVisible)
                {
                    continue;
                }

                LayoutChildIntoBoundingRegion(child, new Rectangle(new Point(xChild, yChild), layoutData.CellSize));

                if (++column == layoutData.Columns)
                {
                    column = 0;
                    row++;
                    xChild = x;
                    yChild += RowSpacing + layoutData.CellSize.Height;
                }
                else
                {
                    xChild += ColumnSpacing + layoutData.CellSize.Width;
                }
            }
        }

        protected override void InvalidateLayout()
        {
            base.InvalidateLayout();

            // Discard all layout information for children added or removed.
            layoutDataCache.Clear();
        }

        protected override void OnChildMeasureInvalidated()
        {
            base.OnChildMeasureInvalidated();

            // Discard all layout information for child size changed.
            layoutDataCache.Clear();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null && waitingForBindingContext && ItemsSource != null)
            {
                OnItemsChanged(this, null, ItemsSource);
            }
        }

        protected virtual void OnItemCreated(View view)
        {
            if (this.ItemCreated != null)
            {
                ItemCreated.Invoke(this, new LayoutItemAddedEventArgs(view, view.BindingContext));
            }
        }

        #region "Private Methods"

        private LayoutData GetLayoutData(double width, double height)
        {
            Size size = new Size(width, height);

            // Check if cached information is available.
            if (layoutDataCache.ContainsKey(size))
            {
                return layoutDataCache[size];
            }

            int visibleChildCount = 0;
            Size maxChildSize = new Size();
            int rows = 0;
            int columns = 0;
            LayoutData layoutData = new LayoutData();

            // Enumerate through all the children.
            foreach (View child in Children)
            {
                // Skip invisible children.
                if (!child.IsVisible)
                    continue;

                // Count the visible children.
                visibleChildCount++;

                // Get the child's requested size.
                SizeRequest childSizeRequest = child.Measure(Double.PositiveInfinity, Double.PositiveInfinity);

                // Accumulate the maximum child size.
                maxChildSize.Width = Math.Max(maxChildSize.Width, childSizeRequest.Request.Width);
                maxChildSize.Height = Math.Max(maxChildSize.Height, childSizeRequest.Request.Height);
            }

            if (visibleChildCount != 0)
            {
                // Calculate the number of rows and columns.
                if (Double.IsPositiveInfinity(width))
                {
                    columns = visibleChildCount;
                    rows = 1;
                }
                else
                {
                    columns = (int)((width + ColumnSpacing) / (maxChildSize.Width + ColumnSpacing));
                    columns = Math.Max(1, columns);
                    rows = (visibleChildCount + columns - 1) / columns;
                }

                // Now maximize the cell size based on the layout size.
                Size cellSize = new Size();

                if (Double.IsPositiveInfinity(width))
                {
                    cellSize.Width = maxChildSize.Width;
                }
                else
                {
                    cellSize.Width = (width - ColumnSpacing * (columns - 1)) / columns;
                }

                if (Double.IsPositiveInfinity(height))
                {
                    cellSize.Height = maxChildSize.Height;
                }
                else
                {
                    cellSize.Height = (height - RowSpacing * (rows - 1)) / rows;
                }

                layoutData = new LayoutData(visibleChildCount, cellSize, rows, columns);
            }

            layoutDataCache.Add(size, layoutData);
            return layoutData;
        }
        
        private View CreateChildViewFor(object item)
        {
            try
            {
                this.ItemTemplate.SetValue(BindableObject.BindingContextProperty, item);
                View view;
                if (this.ItemTemplate is DataTemplateSelector)
                {
                    var dts = (DataTemplateSelector)this.ItemTemplate;
                    view = (View)dts.SelectTemplate(item, null).CreateContent();
                }
                else
                {
                    view = (View)this.ItemTemplate.CreateContent();
                }

                var viewTappedButtonBehavior = new ViewTappedButtonBehavior
                {
                    AnimationType = ItemTappedAnimationType,
                    CommandParameter = item,
                    Command = TappedCommand
                };
                view.Behaviors.Add(viewTappedButtonBehavior);

                return view;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return null;
            }
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                var invalidate = false;
                List<View> createdViews = new List<View>();
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    var list = sender as System.Collections.IEnumerable;

                    this.Children.SyncList(
                        list,
                        (item) =>
                        {
                            var view = this.CreateChildViewFor(item);
                            createdViews.Add(view);
                            return view;
                        },
                        (item, view) => view.BindingContext == item,
                        null);
                    foreach (View view in createdViews)
                    {
                        OnItemCreated(view);
                    }
                    invalidate = true;
                }
                if (e.OldItems != null)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() => this.Children.RemoveAt(e.OldStartingIndex));
                    invalidate = true;
                }
                if (e.NewItems != null)
                {
                    for (var i = 0; i < e.NewItems.Count; ++i)
                    {
                        var item = e.NewItems[i];
                        var view = this.CreateChildViewFor(item);
                        int index = i + e.NewStartingIndex;
                        if (view != null)
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => this.Children.Insert(index, view));
                            OnItemCreated(view);
                        }
                    }

                    invalidate = true;
                }

                if (invalidate)
                {
                    this.UpdateChildrenLayout();
                    this.InvalidateLayout();
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        public void Measure(double width, double height)
        {
            OnMeasure(width, height);
        }
        #endregion

    }

    struct LayoutData
    {
        public int VisibleChildCount { get; private set; }

        public Size CellSize { get; private set; }

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public LayoutData(int visibleChildCount, Size cellSize, int rows, int columns)
        {
            VisibleChildCount = visibleChildCount;
            CellSize = cellSize;
            Rows = rows;
            Columns = columns;
        }
    }
}
