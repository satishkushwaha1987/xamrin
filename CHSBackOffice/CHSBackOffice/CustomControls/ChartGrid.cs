
using CHSBackOffice.CustomControls.CustomCells;
using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using CHSBackOffice.Support.StaticResources;
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class ChartGrid : FastCell, INotifyPropertyChanged
    {
        #region "Private Members"
        internal BoxView ValueBox;
        #endregion

        #region .CTOR
        public ChartGrid()
        {
            try 
            {
                PrepareCell();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region Bindable Properties

        #region INotifyPropertyChanged
        public new event PropertyChangedEventHandler PropertyChanged;

        public delegate void ChangePropertyByBindingContext<T>(ChartGrid view, T item);

        internal void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Context
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
           BindableProperty.Create("ParentContext", typeof(object), typeof(ChartGrid), null, propertyChanged: OnParentContextPropertyChanged);

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as ChartGrid).ParentContext = newValue;
                (bindable as ChartGrid).PropertyChanged?.Invoke((bindable as ChartGrid), new PropertyChangedEventArgs(nameof(ParentContext)));
            }
        }

        #endregion

        #region OuterColor
        public Color OuterColor
        {
            get { return (Color)GetValue(OuterColorProperty); }
            set { SetValue(OuterColorProperty, value); }
        }

        public static readonly BindableProperty OuterColorProperty = BindableProperty.Create(nameof(OuterColor), typeof(Color), typeof(ChartGrid), Color.White, propertyChanged: OuterColorChanged);

        private static void OuterColorChanged(object bindable, object oldValue, object newValue)
        {
            (bindable as ChartGrid).RaisePropertyChanged(nameof(OuterColor));
        }

        #endregion

        #region InnerColor

        public Color InnerColor
        {
            get { return (Color)GetValue(InnerColorProperty); }
            set { SetValue(InnerColorProperty, value); }
        }

        public static readonly BindableProperty InnerColorProperty = BindableProperty.Create(nameof(InnerColor), typeof(Color), typeof(ChartGrid), Color.White, propertyChanged: InnerColorChanged);

        private static void InnerColorChanged(object bindable, object oldValue, object newValue)
        {
            (bindable as ChartGrid).RaisePropertyChanged(nameof(InnerColor));
        }

        #endregion

        #region IsDynamicColor
        public bool IsDynamicColor
        {
            get { return (bool)GetValue(IsDynamicColorProperty); }
            set { SetValue(IsDynamicColorProperty, value); }
        }

        public static readonly BindableProperty IsDynamicColorProperty = BindableProperty.Create(nameof(IsDynamicColor), typeof(bool), typeof(ChartGrid), false, propertyChanged: IsDynamicColorChanged);

        private static void IsDynamicColorChanged(object bindable, object oldValue, object newValue)
        {
            (bindable as ChartGrid).RaisePropertyChanged(nameof(IsDynamicColor));
        }

        #endregion

        #region BorderColor
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ChartGrid), Color.Transparent, propertyChanged: BorderColorChanged);

        private static void BorderColorChanged(object bindable, object oldValue, object newValue)
        {
            (bindable as ChartGrid).RaisePropertyChanged(nameof(BorderColor));
        }
        #endregion

        #region BorderWidth
        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }

        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(ChartGrid), 0, propertyChanged: BorderWidthChanged);

        private static void BorderWidthChanged(object bindable, object oldValue, object newValue)
        {
            (bindable as ChartGrid).RaisePropertyChanged(nameof(BorderWidth));
        }
        #endregion

        #region PercentValue
        public double PercentValue
        {
            get { return (double)GetValue(PercentValueProperty); }
            set { SetValue(PercentValueProperty, value); }
        }

        public static readonly BindableProperty PercentValueProperty = BindableProperty.Create(nameof(PercentValue), typeof(double), typeof(ChartGrid), (double)1, propertyChanged: OnPercentVlaueChanged);

        private static void OnPercentVlaueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var chartGrid = bindable as ChartGrid;
                if (chartGrid.ChartOrientation == StackOrientation.Horizontal)
                {
                    var width = chartGrid.WidthRequest;
                    if (newValue != null)
                    {
                        if (((double)newValue > 0 && (double)newValue < 1 / width))
                        {
                            chartGrid.ValueBox.WidthRequest = 1.5;
                        }
                        else
                        {
                            chartGrid.ValueBox.WidthRequest = width * (double)newValue;// - 2 * (chartGrid.Content as RoundedCornerView).BorderWidth - 6;
                        }
                        chartGrid.RaisePropertyChanged(nameof(WidthRequest));
                    }
                }
                else
                {
                    var height = chartGrid.HeightRequest;
                    if (newValue != null)
                    {
                        if (((double)newValue > 0 && (double)newValue < 1 / height))
                        {
                            chartGrid.ValueBox.HeightRequest = 1;
                        }
                        else
                        {
                            chartGrid.ValueBox.HeightRequest = height * (double)newValue;// - 2 * (chartGrid.Content as RoundedCornerView).BorderWidth - 6;                            

                        }

                        chartGrid.RaisePropertyChanged(nameof(HeightRequest));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        #endregion

        #region ChartOrientation
        public StackOrientation ChartOrientation
        {
            get { return (StackOrientation)GetValue(ChartOrientationProperty); }
            set { SetValue(ChartOrientationProperty, value); }
        }
        public static readonly BindableProperty ChartOrientationProperty = BindableProperty.Create(nameof(ChartOrientation), typeof(StackOrientation), typeof(ChartGrid), StackOrientation.Vertical);

        #endregion

        #endregion

        #region Methods

        protected override void OnParentSet()
        {
            base.OnParentSet();
            CalculateSpace();   
        }

        private void MachineStateLayout_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            CalculateSpace();
        }

        private void CalculateSpace()
        {
            if (Parent != null)
            {
                var space = LayoutResizer.Instance.InitColumnSpace(
                                (Parent as View).WidthRequest,
                                Device.Idiom == TargetIdiom.Phone ? 7 : 10,
                                Device.Idiom == TargetIdiom.Phone ? 2 : 5);
                this.Margin = new Thickness(space, 0, 0, 0);
            }
        }

        #endregion

        #region FastCell
        protected override void InitializeCell()
        {
            ExtendedNaviPage.OnOrientartionChanged += MachineStateLayout_OnOrientartionChanged;
            ValueBox = new BoxView() { VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.Start, Margin = 0 };
            RoundedCornerView layout = new RoundedCornerView() { RoundedCornerRadius = 3, Padding = 0 };
            layout.SetBinding(Grid.HeightRequestProperty, new Binding(nameof(HeightRequest), source: this));
            layout.SetBinding(Grid.WidthRequestProperty, new Binding(nameof(WidthRequest), source: this));
            layout.SetBinding(RoundedCornerView.BorderColorProperty, new Binding(nameof(BorderColor), source: this));
            layout.SetBinding(RoundedCornerView.BorderWidthProperty, new Binding(nameof(BorderWidth), source: this));
            if (!IsDynamicColor)
            {
                ValueBox.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(InnerColor), source: this));
                layout.SetBinding(RoundedCornerView.FillColorProperty, new Binding(nameof(OuterColor), source: this));
                layout.SetBinding(Grid.BackgroundColorProperty, new Binding(nameof(OuterColor), source: this));
            }
            layout.Children.Add(ValueBox);
            Frame frame = new Frame
            {
                BackgroundColor = Color.Transparent,
                BorderColor = Color.Transparent,
                CornerRadius = 3,
                HasShadow = false,
                Padding = new Thickness(0),
                IsClippedToBounds = true
            };
            frame.Content = layout;
            Content = frame;
        }

        protected override void SetupCell(bool isRecycled)
        {
            if (IsDynamicColor)
            {
                var item = BindingContext;
                if (item is CHBackOffice.ApiServices.ChsProxy.ArrayOfDispenserDispenser)
                {
                    CashCoinDrawGraphs(item as CHBackOffice.ApiServices.ChsProxy.ArrayOfDispenserDispenser);
                }
                else if (item is CHBackOffice.ApiServices.ChsProxy.ArrayOfAcceptorAcceptor)
                {
                    BillDrawGraphs(item as CHBackOffice.ApiServices.ChsProxy.ArrayOfAcceptorAcceptor);
                }
            }
        }

        #region Draw Graphs
        private void CashCoinDrawGraphs(CHBackOffice.ApiServices.ChsProxy.ArrayOfDispenserDispenser cashOrCoinDisp)
        {
            OuterColor = StaticResourceManager.GetColor("ChartCapacity");
            if (cashOrCoinDisp.Count >= 100)
            {
                InnerColor = StaticResourceManager.GetColor("ChartNormal");
            }
            else if (cashOrCoinDisp.Count > 0 && cashOrCoinDisp.Count < 100)
            {
                InnerColor = StaticResourceManager.GetColor("ChartWarning");
            }
            else if (cashOrCoinDisp.Count == 0)
            {
                OuterColor = StaticResourceManager.GetColor("ChartCritical");
            }
            var percValue = (double)cashOrCoinDisp.Count * 1.0 / cashOrCoinDisp.Capacity;
            PercentValue = percValue;
        }


        private void BillDrawGraphs(CHBackOffice.ApiServices.ChsProxy.ArrayOfAcceptorAcceptor billAcceptor)
        {
            OuterColor = StaticResourceManager.GetColor("ChartCapacity");
            if (billAcceptor.Count < (billAcceptor.Capacity - 100))
            {
                InnerColor = StaticResourceManager.GetColor("ChartNormal");
            }
            else if (billAcceptor.Count >= (billAcceptor.Capacity - 100))
            {
                InnerColor = StaticResourceManager.GetColor("ChartWarning");
            }
            else if (billAcceptor.Count == billAcceptor.Capacity)
            {
                InnerColor = StaticResourceManager.GetColor("ChartCritical");
            }
            var percValue = (double)billAcceptor.Count * 1.0 / billAcceptor.Capacity;
            PercentValue = percValue;
        }

        #endregion

        #endregion
    }
}
