
using CHSBackOffice.Events;
using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class PopupLayout : ContentView, ICloneable
    {
        #region Fields
        private double _width;
        private double _height;
        private View content;
        private View popup;
        private static Grid _opacitygrid;
        private readonly RelativeLayout layout;
        private double _scrollY = 0;
        #endregion

        #region Properties

        private Page _parentPage;
        internal Page ParentPage
        {
            get => _parentPage;
            set
            {
                if (_parentPage != value)
                {
                    _parentPage = value;
                }
            }
        }

        private View _parentsView;
        internal View ParentsView
        {
            get => _parentsView;
            set
            {
                if (_parentsView != value)
                {
                    _parentsView = value;
                }
            }
        }

        public View presenter { get; set; }

        public bool IsTappable { get; set; }

        public bool IsPopupActive
        {
            get;
            set;
        }

        public bool HasInnerPopover
        {
            get;
            set;
        }

        #endregion

        #region .CTOR

        private PopupLayout()
        {
            base.Content = this.layout = new RelativeLayout();

            App.SharedEventAggregator.GetEvent<ScrollYChanged>().Subscribe(OnScrollYChanged);
        }

        private void OnScrollYChanged(double scrollY)
        {
            _scrollY = scrollY;
        }

        #endregion

        #region Bindable Properties

        #region Content property

        public new static BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(PopupLayout), null, propertyChanged: OnContentChanged);

        private void SetContent(View view)
        {
            if (view != null)
                layout.Children.Remove(view);
            content = view;
            if (content != null)
                layout.Children.Add(content, () => Bounds);
        }

        private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obj = bindable as PopupLayout;
            obj.SetContent(newValue as View);
        }

        public new View Content
        {
            get
            {
                return GetValue(ContentProperty) as View;
            }
            set
            {
                SetValue(ContentProperty, value);
            }
        }
        #endregion

        #endregion

        #region Static Members

        static PopupLayout _popupLayout;

        public static View AddToPage(ContentPage page)
        {
            _popupLayout = new PopupLayout();
            var mainContainer = page.Content;
            _opacitygrid = new Grid();
           
            if (mainContainer != null)
            {
                if (mainContainer is Grid)
                {
                    
                    _opacitygrid.HorizontalOptions = LayoutOptions.FillAndExpand;
                    _opacitygrid.VerticalOptions = LayoutOptions.FillAndExpand;
                    _opacitygrid.InputTransparent = true;
                    TapGestureRecognizer tap = new TapGestureRecognizer((v,o)=> 
                    {
                        _popupLayout.DismissPopup();
                        _opacitygrid.InputTransparent = true;
                        if (_popupLayout.ParentsView != null & !_popupLayout.HasInnerPopover)
                        {
                            _popupLayout.ParentsView.BackgroundColor = Color.Transparent;
                        }
                    });
                    _opacitygrid.GestureRecognizers.Add(tap);

                    var mainGrid = mainContainer as Grid;
                    mainGrid.Children.Add(_popupLayout, 0, 0);

                    Device.BeginInvokeOnMainThread(()=> 
                    {
                        var gridContent = new Grid();
                        gridContent.HorizontalOptions = LayoutOptions.FillAndExpand;
                        gridContent.VerticalOptions = LayoutOptions.FillAndExpand;

                        gridContent.Children.Add(mainGrid.Children[0], 0, 0);
                        gridContent.Children.Add(_opacitygrid, 0, 0);

                        _popupLayout.Content = gridContent;
                        if(!_popupLayout.IsTappable)
                            _popupLayout.InputTransparent = true;
                    });
                }
            }
            _popupLayout.ParentsView = _opacitygrid;
            _popupLayout.ParentPage = page;
            return _popupLayout;
        }

        #endregion

        #region Methods

        public void ShowPopup(View popupView, bool centered, double pX, double pY)
        {
            if (centered)
            {
                this.ShowPopup(
               popupView,
               Constraint.RelativeToParent(p => (this.Width - this.popup.WidthRequest) / 2 - pX),
               Constraint.RelativeToParent(p => (this.Height - this.popup.HeightRequest) / 2 - pY)
               );
            }
            else
            {
                this.ShowPopup(
               popupView,
               Constraint.RelativeToParent(p => pX),
               Constraint.RelativeToParent(p => pY)
               );
            }
        }

        public void ShowPopup(View popupView)
        {
            this.ShowPopup(
                popupView,
                Constraint.RelativeToParent(p => (this.Width - this.popup.WidthRequest) / 2),
                Constraint.RelativeToParent(p => (this.Height - this.popup.HeightRequest) / 2)
                );
        }

        public void ShowPopup(View popupView, double factor, double width, double height)
        {
            try
            {
                DismissPopup();
                this.popup = popupView;

                //if (this.content != null)
                //    this.content.InputTransparent = true;
                _opacitygrid.InputTransparent = false;
                this.layout.Children.Add(this.popup,
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Width * factor - width / 2;//coordinate X
                    }),
                    Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Height * factor - height / 2;//coordinate Y
                    }),
                    Constraint.Constant(width),//width
                    Constraint.Constant(height)//height
                    );
                this.layout.ForceLayout();
                
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        public void ShowPopup(View popupView, Constraint xConstraint, Constraint yConstraint, Constraint widthConstraint = null, Constraint heightConstraint = null)
        {
            try
            {
                DismissPopup();
                this.popup = popupView;

                //if (this.content != null)
                //    this.content.InputTransparent = true;
                _opacitygrid.InputTransparent = false;

                this.layout.Children.Add(this.popup, xConstraint, yConstraint, widthConstraint, heightConstraint);

                this.layout.ForceLayout();
                
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        public void ShowPopup(View popupView, View presenter, PopupLocation location, float paddingX = 0, float paddingY = 0)
        {
            DismissPopup();
            this.popup = popupView;

            Constraint constraintX = null, constraintY = null;
            var coordinates = GetScreenCoordinates(presenter);
            switch (location)
            {
                case PopupLocation.Bottom:
                    constraintX = Constraint.RelativeToParent(parent => coordinates.X + (presenter.Width - this.popup.WidthRequest) / 2);
                    constraintY = Constraint.RelativeToParent(parent => parent.Y + coordinates.Y + presenter.Height + paddingY);
                    break;
                case PopupLocation.Top:
                    constraintX = Constraint.RelativeToParent(parent => coordinates.X + (presenter.Width - this.popup.WidthRequest) / 2);
                    constraintY = Constraint.RelativeToParent(parent => parent.Y + coordinates.Y - this.popup.HeightRequest - paddingY);
                    break;

            }

            this.ShowPopup(popupView, constraintX, constraintY);
        }

        public void ShowPopup(View popupView, VisualElement presenter, PopupLocation location)
        {
            DismissPopup();
            this.popup = popupView;

            Constraint constraintX = null, constraintY = null;
            var coordinates = GetScreenCoordinates(presenter);
            switch (location)
            {
                case PopupLocation.Bottom:
                    constraintX = Constraint.RelativeToParent(parent => coordinates.X + (presenter.Width - this.popup.WidthRequest) / 2);
                    constraintY = Constraint.RelativeToParent(parent => coordinates.Y + presenter.Height);
                    break;
                case PopupLocation.Top:
                    constraintX = Constraint.RelativeToParent(parent => coordinates.X + (presenter.Width - this.popup.WidthRequest) / 2);
                    constraintY = Constraint.RelativeToParent(parent => coordinates.Y - this.popup.HeightRequest - _scrollY);
                    break;

            }

            this.ShowPopup(popupView, constraintX, constraintY);
        }

        public (double X, double Y) GetScreenCoordinates(VisualElement view)
        {
            double screenCoordinateX = view.X;
            double screenCoordinateY = view.Y;
            // Get the view's parent (if it has one...)
            if (view.Parent.GetType() != typeof(App))
            {
                VisualElement parent = (VisualElement)view.Parent;


                // Loop through all parents
                while (parent != null)
                {
                    // Add in the coordinates of the parent with respect to ITS parent
                    screenCoordinateX += parent.X;
                    screenCoordinateY += parent.Y;

                    // If the parent of this parent isn't the app itself, get the parent's parent.
                    var type = parent.GetType();
                    if (parent.GetType() == typeof(NavigationPage))
                        parent = null;
                    else
                        parent = (VisualElement)parent.Parent;
                }
            }
            return (screenCoordinateX, screenCoordinateY);
        }

        public void DismissPopup()
        {
            try
            {
                PopupData.Instance.PopupIsActive = false;
                if (this.popup != null)
                {
                    this.layout.Children.Remove(this.popup);
                    this.popup = null;
                }

                this.layout.InputTransparent = false;

                if (this.content != null)
                {
                    this.content.InputTransparent = false;
                }
                _opacitygrid.InputTransparent = true;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        public void EnableTap()
        {
            _opacitygrid.InputTransparent = false;
        }
        public void DisableTap()
        {
            _opacitygrid.InputTransparent = true;
        }

        public PopupLayout Clone()
        {
            return (PopupLayout)this.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        public enum PopupLocation
        {
            Top,
            Bottom
        }

    }

    public class PopupIsActiveArgs : EventArgs
    {
        public bool IsActive { get; set; }
    }
}
