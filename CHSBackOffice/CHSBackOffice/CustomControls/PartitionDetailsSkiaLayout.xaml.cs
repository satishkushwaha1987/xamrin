using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartitionDetailsSkiaLayout : ContentView
    {
        #region .CTOR
        public PartitionDetailsSkiaLayout()
        {
            InitializeComponent();
        }
        #endregion

        #region DataSource
        public Tuple<float, float, Func<float, float, Color>> DataSource
        {
            get { return (Tuple<float, float, Func<float, float, Color>>)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly BindableProperty DataSourceProperty = BindableProperty.Create(
            nameof(DataSource), typeof(Tuple<float, float, Func<float, float, Color>>), typeof(PartitionDetailsSkiaLayout), default(Tuple<float, float, Func<float, float, Color>>), propertyChanged: OnDataSourceChanged);

        private static void OnDataSourceChanged(BindableObject bindable, object oldValue, object newValue)
        { 
        }
        #endregion
    }
}