
﻿using Acr.UserDialogs;
using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SwitchedToCollection : ContentView
    {
        #region Bindable Properties

        #region "RightText"

        public string RightText
        {
            get { return (string)GetValue(RightTextProperty); }
            set { SetValue(RightTextProperty, value); }
        }

        public static readonly BindableProperty RightTextProperty =
            BindableProperty.Create(nameof(RightText), typeof(string), typeof(SwitchedToCollection), string.Empty, propertyChanged: OnRightTextChanged);

        private static void OnRightTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var root = bindable as SwitchedToCollection;
            root.StateSwitch.RightText = (string)newValue;
        }

        #endregion

        #region "RightLoadingText"

        public string RightLoadingText
        {
            get { return (string)GetValue(RightLoadingTextProperty); }
            set { SetValue(RightLoadingTextProperty, value); }
        }

        public static readonly BindableProperty RightLoadingTextProperty =
            BindableProperty.Create(nameof(RightText), typeof(string), typeof(SwitchedToCollection), string.Empty);

        #endregion

        #region "LeftText"

        public string LeftText
        {
            get { return (string)GetValue(LeftTextProperty); }
            set { SetValue(LeftTextProperty, value); }
        }

        public static readonly BindableProperty LeftTextProperty =
            BindableProperty.Create(nameof(LeftText), typeof(string), typeof(SwitchedToCollection), string.Empty, propertyChanged: OnLeftTextChanged);

        private static void OnLeftTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var root = bindable as SwitchedToCollection;
            root.StateSwitch.LeftText = (string)newValue;
        }

        #endregion

        #region "LeftLoadingText"

        public string LeftLoadingText
        {
            get { return (string)GetValue(LeftLoadingTextProperty); }
            set { SetValue(LeftLoadingTextProperty, value); }
        }

        public static readonly BindableProperty LeftLoadingTextProperty =
            BindableProperty.Create(nameof(RightText), typeof(string), typeof(SwitchedToCollection), string.Empty);

        #endregion

        #region "State"

        public bool State
        {
            get { return (bool)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly BindableProperty StateProperty =
            BindableProperty.Create(nameof(State), typeof(bool), typeof(SwitchedToCollection), true, propertyChanged: OnStateChanged);

        private static void OnStateChanged(BindableObject bindable, object oldValue, object newVlaue)
        {
            var root = bindable as SwitchedToCollection;
            if (newVlaue != oldValue && root.SwitchCollectionCommand != null)
            {
                var state = (bool)newVlaue;
                root.SwitchCollectionCommand.Execute(root.State);
            }
        }

        #endregion

        #region "SwitchedToSource"

        public IEnumerable<object> SwitchedToSource
        {
            get { return (IEnumerable<object>)GetValue(SwitchedToSourceProperty); }
            set { SetValue(SwitchedToSourceProperty, value); }
        }

        public static readonly BindableProperty SwitchedToSourceProperty =
            BindableProperty.Create(nameof(SwitchedToSource), typeof(IEnumerable<object>), typeof(SwitchedToCollection), propertyChanged:OnSourceChanged);

        private static void OnSourceChanged(BindableObject bindable, object oldValue, object newVlaue)
        {
            var root = bindable as SwitchedToCollection;
            root.CollectionView.ItemsSource = root.SwitchedToSource;
        }

        #endregion

        #region "ItemTapSwitchedCollectionCommand"

        public ICommand ItemTapSwitchedCollectionCommand
        {
            get { return (ICommand)GetValue(ItemTapSwitchedCollectionCommandProperty); }
            set { SetValue(ItemTapSwitchedCollectionCommandProperty, value); }
        }

        public static readonly BindableProperty ItemTapSwitchedCollectionCommandProperty =
            BindableProperty.Create(nameof(ItemTapSwitchedCollectionCommand), typeof(ICommand), typeof(SwitchedToCollection), null);

        #endregion

        #region "LoadMoreCommand"

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommand), typeof(ICommand), typeof(SwitchedToCollection), null);

        #endregion

        #region "SwitchCollectionCommand"

        public ICommand SwitchCollectionCommand
        {
            get { return (ICommand)GetValue(SwitchCollectionCommandProperty); }
            set { SetValue(SwitchCollectionCommandProperty, value); }
        }

        public static readonly BindableProperty SwitchCollectionCommandProperty =
            BindableProperty.Create(nameof(SwitchCollectionCommand), typeof(ICommand), typeof(SwitchedToCollection), null);

        #endregion

        #region "ItemTemplateSwitchedCollection"

        public DataTemplate ItemTemplateSwitchedCollection
        {
            get { return (DataTemplate)GetValue(ItemTemplateSwitchedCollectionProperty); }
            set { SetValue(ItemTemplateSwitchedCollectionProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateSwitchedCollectionProperty =
            BindableProperty.Create(nameof(ItemTemplateSwitchedCollection), typeof(DataTemplate), typeof(SwitchedToCollection), null, propertyChanged: OnItemTemplateChanged);

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newVlaue)
        {
            var root = bindable as SwitchedToCollection;
            root.CollectionView.ItemTemplate = (DataTemplate)newVlaue;
        }

        #endregion

        #region "IsWaitingVisible"

        public bool IsWaitingVisible
        {
            get { return (bool)GetValue(IsWaitingVisibleProperty); }
            set { SetValue(IsWaitingVisibleProperty, value); }
        }

        public static readonly BindableProperty IsWaitingVisibleProperty = BindableProperty.Create(
            nameof(IsWaitingVisible), 
            typeof(bool), 
            typeof(SwitchedToCollection), 
            propertyChanged: OnIsWaitingVisibleChanged
        );

        private static void OnIsWaitingVisibleChanged(BindableObject bindable, object oldValue, object newVlaue)
        {
            var root = bindable as SwitchedToCollection;

            if ((bool)newVlaue == true)
                root.HasData.IsVisible = false;
            else
            {
                var collectionHasData = root.SwitchedToSource?.Any() ?? false;
                if (collectionHasData)
                    root.CollectionView.IsVisible = true;
                else
                    root.HasData.IsVisible = true;
            }
        }

        #endregion

        #endregion

        #region ".CTOR"

        public SwitchedToCollection()
        {
            try
            {
                InitializeComponent();
                Xamarin.Forms.Application.Current.On<iOS>().SetPanGestureRecognizerShouldRecognizeSimultaneously(true);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region "EVENT HANDLERS"

        private void OnToggledStateSwitch(object sender, ToggledEventArgs e)
        {
            State = !e.Value;
        }

        private void OnItemTappedCollectionView(object sender, TappedEventArgs e)
        {
            if (ItemTapSwitchedCollectionCommand != null)
            {
                ItemTapSwitchedCollectionCommand.Execute(e.Parameter);
            }
        }

        #endregion
    }
}