using CHSBackOffice.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CHSBackOffice.CustomControls.CustomCells
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TextValuePairLayout : Grid, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged(string nameOfProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameOfProperty));
        }

        void SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = "")
        {
            backingStore = value;

            RaisePropertyChanged(propertyName);
        }
        #endregion

        public TextValuePairLayout ()
		{
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
		}

        #region ValueProperty

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(
            nameof(Value), typeof(string), typeof(TextValuePairLayout), "", propertyChanged: ValuePropertyChanged);

        static void ValuePropertyChanged(object j, object oldVal, object newVal)
        {
            ((TextValuePairLayout)j).RaisePropertyChanged(nameof(Value));
        }

        #endregion

        #region KeyProperty

        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public static readonly BindableProperty KeyProperty = BindableProperty.Create(
            nameof(Key), typeof(string), typeof(TextValuePairLayout), "", propertyChanged: KeyPropertyChanged);

        static void KeyPropertyChanged(object j, object oldVal, object newVal)
        {
            ((TextValuePairLayout)j).RaisePropertyChanged(nameof(Key));
        }

        #endregion

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor), typeof(Color), typeof(TextValuePairLayout), Color.Black, propertyChanged: TextColorPropertyChanged);

        static void TextColorPropertyChanged(object j, object oldVal, object newVal)
        {
            ((TextValuePairLayout)j).RaisePropertyChanged(nameof(TextColor));
        }

        public string Separator
        {
            get { return (string)GetValue(SeparatorProperty); }
            set { SetValue(SeparatorProperty, value); }
        }

        public static readonly BindableProperty SeparatorProperty = BindableProperty.Create(
            nameof(Separator), typeof(string), typeof(TextValuePairLayout), "", propertyChanged: SeparatorPropertyChanged);

        static void SeparatorPropertyChanged(object j, object oldVal, object newVal)
        {
            ((TextValuePairLayout)j).RaisePropertyChanged(nameof(Separator));
        }
    }
}