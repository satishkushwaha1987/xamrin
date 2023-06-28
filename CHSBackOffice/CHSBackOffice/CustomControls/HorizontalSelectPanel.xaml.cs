using CHSBackOffice.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HorizontalSelectPanel : ContentView
    {
        public HorizontalSelectPanel()
        {
            InitializeComponent();
        }

        #region Bindable commands

        public ICommand ItemTappedCommand
        {
            get { return (ICommand)GetValue(ItemTappedCommandProperty); }
            set { SetValue(ItemTappedCommandProperty, value); }
        }

        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.Create(
            nameof(ItemTappedCommand), typeof(ICommand), typeof(HorizontalSelectPanel), default(ICommand));


        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        public static readonly BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(
            nameof(ItemSelectedCommand), typeof(ICommand), typeof(HorizontalSelectPanel), default(ICommand));

        #endregion

        

        public ICommand InternalItemTappedCommand => new Command<object>(OnItemTapped);

        private void OnItemTapped(object item)
        {
            var selected = (KeyValuePair<string, string>)item;

            if (ItemTappedCommand != null && ItemTappedCommand.CanExecute(selected))
                ItemTappedCommand.Execute(item);

            ItemTapped?.Invoke(this, new ItemTappedEventArgs { Item = selected });
        }

        private void OnItemSelected(object item)
        {
            if (ItemSelectedCommand != null && ItemTappedCommand.CanExecute(item))
                ItemSelectedCommand.Execute(item);

            ItemSelected?.Invoke(this, new SelectedItemEventArgs { SelectedItem = item });
        }

        private void RefreshItems()
        {
            container.RefreshItems();
        }

        public event EventHandler<ItemTappedEventArgs> ItemTapped;
        public event EventHandler<SelectedItemEventArgs> ItemSelected;

        public class ItemTappedEventArgs : EventArgs
        {
            public object Item { get; set; }
        }

        public class SelectedItemEventArgs : EventArgs
        {
            public object SelectedItem { get; set; }
        }

        #region Bindable Property
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(HorizontalSelectPanel), null, BindingMode.OneWay, propertyChanged: OnItemSourceChaged);

        private static void OnItemSourceChaged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var element = bindable as HorizontalSelectPanel;
                var source = element.ItemsSource;
                var key = element.SelectedKey;
                if (source != null)
                {
                    foreach (var pair in source)
                    {
                        if (((KeyValuePair<string, string>)pair).Key == key)
                        {
                            element.OnItemSelected(pair);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        public string SelectedKey
        {

            get { return (string)GetValue(SelectedKeyProperty); }
            set { SetValue(SelectedKeyProperty, value); }
        }

        public static readonly BindableProperty SelectedKeyProperty = BindableProperty.Create(nameof(SelectedKey), typeof(string), typeof(HorizontalSelectPanel), string.Empty, BindingMode.OneWay,propertyChanged:OnSelectedKeyChanged);

        private static void OnSelectedKeyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var element = bindable as HorizontalSelectPanel;            
            element?.RefreshItems();
        }
        #endregion
    }
}