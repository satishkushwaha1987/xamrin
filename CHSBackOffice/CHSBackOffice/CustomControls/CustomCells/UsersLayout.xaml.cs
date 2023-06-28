using CHSBackOffice.Extensions;
using CHSBackOffice.Support;
using CHSBackOffice.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls.CustomCells
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UsersLayout : Frame, IParentContext
    {

        #region .CTOR
        public UsersLayout ()
		{
            try
            {
                InitializeComponent();
                ExtendedNaviPage.OnOrientartionChanged += ExtendedNaviPage_OnOrientartionChanged;

            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }
        #endregion

        #region Bindable Properties

        #region IParentContext
        public object Context
        {
            set
            {
                ParentContext = value;
            }
            get { return ParentContext; }
        }

        public object ParentContext
        {
            get { return GetValue(ParentContextProperty); }
            set { SetValue(ParentContextProperty, value); }
        }

        public static readonly BindableProperty ParentContextProperty =
           BindableProperty.Create("ParentContext", typeof(object), typeof(UsersLayout), null, propertyChanged: OnParentContextPropertyChanged);

        private static void OnParentContextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && newValue != null)
            {
                (bindable as UsersLayout).ParentContext = newValue;
            }
        }

        #endregion

        #endregion

        #region "Event Handling"
        private void ExtendedNaviPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            WidthRequest = Device.Idiom == TargetIdiom.Phone ? 150 : 350;
            HeightRequest = Device.Idiom == TargetIdiom.Phone ? 60 : 120;
        }
        #endregion
    }
}