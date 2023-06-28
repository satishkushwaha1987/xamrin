using CHBackOffice.ApiServices.ChsProxy;
using CHSBackOffice.Behaviors;
using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using static CHSBackOffice.CustomControls.ChartGrid;

namespace CHSBackOffice.CustomControls
{
    public class FlowLayout : FlexLayout
    {
        bool waitingForBindingContext = false;
        public event LayoutItemAddedEventHandler ItemCreated;


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

        #region ".CTOR"

        public FlowLayout()
        {
        }

        #endregion

        #region "Bindable Properties"

        #region "ItemsSource"

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<object>), typeof(FlowLayout), defaultValue: null, defaultBindingMode: BindingMode.OneWay, propertyChanged: OnItemsChanged);

        private static void OnItemsChanged(BindableObject bindable, object oldVal, object newVal)
        {
            IEnumerable<object> oldValue = oldVal as IEnumerable<object>;
            IEnumerable<object> newValue = newVal as IEnumerable<object>;

            var control = bindable as FlowLayout;
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
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => { 
                control.Children.Clear();
                if (newValue != null)
                {
                    foreach (var item in newValue)
                    {
                        var view = control.CreateChildViewFor(item);
                        control.Children.Add(view);
                        control.OnItemCreated(view);
                    }
                }
                control.UpdateChildrenLayout();
                control.InvalidateLayout();
                });
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
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(FlowLayout), defaultValue: null, defaultBindingMode: BindingMode.OneWay);

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
                                                             declaringType: typeof(ExtendedStackLayout),
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
                                                            declaringType: typeof(FlowLayout),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.OneWay,
                                                            propertyChanged: OnItemTappedCommandChanged);

        static void OnItemTappedCommandChanged(object sender, object oldVal, object newVal)
        {
            FlowLayout s = (FlowLayout)sender;
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
                                                            declaringType: typeof(FlowLayout),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.OneWay,

                                                            propertyChanged: OnDetailCommandChanged);

        static void OnDetailCommandChanged(object sender, object oldVal, object newVal)
        {
            FlowLayout s = (FlowLayout)sender;
            if (s.ItemsSource != null)
                OnItemsChanged(s, null, s.ItemsSource);
        }

        #endregion

        #endregion

        #region "PROTECTED METHODS"

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

        #endregion

        #region "PRIVATE METHODS"

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
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => 
                { 
                                
                var invalidate = false;
                List<View> createdViews = new List<View>();
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    var list = sender as IEnumerable;

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
                    this.Children.RemoveAt(e.OldStartingIndex);
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
                            this.Children.Insert(index, view);
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
                
                });
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion


    }

    public class LayoutItemAddedEventArgs : EventArgs
    {
        private readonly View view;
        private readonly object model;

        public LayoutItemAddedEventArgs(View view, object model)
        {
            this.view = view;
            this.model = model;
        }

        public View View { get { return view; } }
        public object Model { get { return model; } }
    }

    public delegate void LayoutItemAddedEventHandler(object sender, LayoutItemAddedEventArgs args);
}
