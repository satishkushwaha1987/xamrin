using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;
using CHSBackOffice.Behaviors;
using CHSBackOffice.Support;

namespace CHSBackOffice.CustomControls
{
    public class ExtendedStackLayout : StackLayout, IEquatable<ExtendedStackLayout>
    {
        public bool ZebraStyle { get; set; }

        public DataTemplate ItemTemplate { get; set; }

        public string id { get; set; }

        #region "BINDABLE PROPS"

        #region "IndexOf"

        public static readonly BindableProperty IndexOfProperty = BindableProperty.Create(
                                                             propertyName: "IndexOf",
                                                             returnType: typeof(int),
                                                             declaringType: typeof(ExtendedStackLayout),
                                                             defaultValue: 0,
                                                             defaultBindingMode: BindingMode.OneWay,
                                                             propertyChanged: null);

        public int IndexOf
        {
            get => (int)GetValue(IndexOfProperty);
            set => base.SetValue(IndexOfProperty, value);
        }

        #endregion

        #region "Tappable"

        public bool Tappable
        {
            get => (bool)GetValue(TappableProperty);
            set => base.SetValue(TappableProperty, value);
        }

        public static readonly BindableProperty TappableProperty = BindableProperty.Create(
                                                             propertyName: "Tappable",
                                                             returnType: typeof(bool),
                                                             declaringType: typeof(ExtendedStackLayout),
                                                             defaultValue: true);

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

        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.Create(
                                                            propertyName: "ItemTappedCommand",
                                                            returnType: typeof(ICommand),
                                                            declaringType: typeof(ExtendedStackLayout),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.OneWay,

                                                            propertyChanged: null);
        public ICommand ItemTappedCommand
        {
            get => (ICommand)GetValue(ItemTappedCommandProperty);
            set => base.SetValue(ItemTappedCommandProperty, value);
        }

        #endregion

        #region "ItemsSource"

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => base.SetValue(ItemsSourceProperty, value);
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
                                                            propertyName: "ItemsSource",
                                                            returnType: typeof(IEnumerable),
                                                            declaringType: typeof(ExtendedStackLayout),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.OneWay,
                                                            propertyChanged: ItemsSourceChanged);

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ExtendedStackLayout)bindable).RefreshItems();
        }

        #endregion

        #endregion

        #region "PRIVATE METHODS"

        internal void RefreshItems()
        {
            try
            {
                if ((ItemsSource != null) && (ItemTemplate != null))
                {
                    Children.Clear();
                    int count = 0;
                    int curNumber = 0;
                    foreach (var item in ItemsSource)
                    {
                        try
                        {
                            IndexOf = count;
                            var cont = ItemTemplate.CreateContent();
                            var view = cont as View;

                            if (curNumber % 2 == 0 && ZebraStyle)
                                view.BackgroundColor = new Color(view.BackgroundColor.R / 1.3, view.BackgroundColor.G / 1.3, view.BackgroundColor.B / 1.3);

                            if (view is BindableObject bindableObject)
                                bindableObject.BindingContext = item;

                            if (Tappable)
                            {
                                var viewTappedButtonBehavior = new ViewTappedButtonBehavior
                                {
                                    AnimationType = ItemTappedAnimationType,
                                    CommandParameter = item
                                };
                                viewTappedButtonBehavior.SetBinding(ViewTappedButtonBehavior.CommandProperty, new Binding(nameof(ItemTappedCommand), source: this));
                                view.Behaviors.Add(viewTappedButtonBehavior);
                            }

                            Children.Add(view);
                            count++;
                        }
                        catch (Exception ex)
                        {
                            ExceptionProcessor.ProcessException(ex);
                        }
                        curNumber++;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ExtendedStackLayout);
        }
        public bool Equals(ExtendedStackLayout other)
        {
            return this.Id.Equals(other?.Id);
        }

        #endregion
    }
}
