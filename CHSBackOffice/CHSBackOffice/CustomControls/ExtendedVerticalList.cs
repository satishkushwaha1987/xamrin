using CHSBackOffice.CustomControls.CustomCells;
using CHSBackOffice.Support;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class ExtendedVerticalList : ListView
    {
        #region Fields
        static int count = 0;
        const int ItemNumberForLoadNewData = 3;
        #endregion

        #region Bindable Properties

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommand), typeof(ICommand), typeof(ExtendedVerticalList), null);

        public IDataTemplateSelector TemplateSelector
        {
            get { return (IDataTemplateSelector)GetValue(TemplateSelectorProperty); }
            set { SetValue(TemplateSelectorProperty, value); }
        }

        public static readonly BindableProperty TemplateSelectorProperty = BindableProperty.Create(nameof(TemplateSelector), typeof(IDataTemplateSelector), typeof(ExtendedVerticalList), null);

        public Color OddBackgroundColor
        {
            get { return (Color)GetValue(OddBackgroundColorProperty); }
            set { SetValue(OddBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty OddBackgroundColorProperty = BindableProperty.Create(nameof(OddBackgroundColor), typeof(Color), typeof(ExtendedVerticalList), Color.FromHex("#F4F4F4"));
        
        public Color EvenBackgroundColor
        {
            get { return (Color)GetValue(EvenBackgroundColorProperty); }
            set { SetValue(EvenBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty EvenBackgroundColorProperty = BindableProperty.Create(nameof(EvenBackgroundColor), typeof(Color), typeof(ExtendedVerticalList), Color.White);

        #endregion

        #region CTOR
        public ExtendedVerticalList()
        {
            TemplateSelector = new EvenOddDataTemplateSelector();
            ItemTemplate = new DataTemplate(GetTemplate);
            ItemAppearing += OnItemAppearing;
            SeparatorVisibility = SeparatorVisibility.None;
        }

        #endregion

        #region  Methods
        public void ItemAppear(object item, IList items)
        {
            Task.Run(() =>
            {
                try
                {
                    if (items.Count <= ItemNumberForLoadNewData || items[items.Count - ItemNumberForLoadNewData] != item)
                        return;
                    //Device.BeginInvokeOnMainThread(() => IsRefreshing = true);
                    LoadMoreCommand?.Execute(item);
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
                finally
                {
                    //Device.BeginInvokeOnMainThread(() => IsRefreshing = false);
                }
            });
        }

        private void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            ItemAppear(e.Item, ItemsSource as IList);
        }

        private Cell GetTemplate()
        {
            var content = new CustomViewCell();
            content.BindingContextChanged += OnBindingContextChanged;
            return content;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            try
            {
                IsRefreshing = false;
                var cell = (ViewCell)sender;
                if (TemplateSelector != null)
                {
                    if (count % 2 == 0)
                    {
                        var template = TemplateSelector.EvenTemplate;
                        var view = ((View)template.CreateContent());
                        view.SetBinding(View.BackgroundColorProperty, new Binding(nameof(EvenBackgroundColor), source: this));
                        var viewCell = new CustomViewCell() { View = view };
                        viewCell.AddTapListener();
                        viewCell.TappedCell += OnTappedViewCell;
                        cell.View = viewCell.View;
                        cell.ForceUpdateSize();
                    }
                    else
                    {
                        var template = TemplateSelector.OddTemplate;
                        var view = ((View)template.CreateContent());
                        view.SetBinding(View.BackgroundColorProperty, new Binding(nameof(OddBackgroundColor), source: this));
                        var viewCell = new CustomViewCell() { View = view };
                        viewCell.AddTapListener();
                        viewCell.TappedCell += OnTappedViewCell;
                        cell.View = viewCell.View;
                        cell.ForceUpdateSize();
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        private void OnTappedViewCell(object sender, TappedEventArgs e)
        {
            Tapped.Invoke(this, e);
        }

        #endregion

        public event EventHandler<TappedEventArgs> Tapped;
    }

    public class CustomViewCell : ViewCell, ITapCallBack
    {
        public CustomViewCell()
        {
        }

        public void AddTapListener()
        {
            TapListener tapListener = new TapListener(View, this);
        }

        public void OnTap(View view)
        {
            TappedCell.Invoke(this, new TappedEventArgs(view.BindingContext));
        }

        public event EventHandler<TappedEventArgs> TappedCell;
    }

    public class EvenOddDataTemplateSelector : IDataTemplateSelector
    {
        public DataTemplate OddTemplate { get; set; } = Device.Idiom == TargetIdiom.Phone?
            new DataTemplate(typeof(TransactionLayoutPhone)):
            new DataTemplate(typeof(TransactionLayoutTablet));
        public DataTemplate EvenTemplate { get; set; } = Device.Idiom == TargetIdiom.Phone ? 
            new DataTemplate(typeof(TransactionLayoutPhone)):
            new DataTemplate(typeof(TransactionLayoutTablet));

    }

    public interface IDataTemplateSelector
    {
        DataTemplate OddTemplate { get; set; }
        DataTemplate EvenTemplate { get; set; }
    }

}
