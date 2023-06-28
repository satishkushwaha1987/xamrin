using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class RoundedCornerView : Grid
    {
        #region Bindable Properties
        public Color FillColor
        {
            get { return (Color)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        public static readonly BindableProperty FillColorProperty = BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(RoundedCornerView), Color.White);

        public double RoundedCornerRadius
        {
            get { return (double)GetValue(RoundedCornerRadiusProperty); }
            set { SetValue(RoundedCornerRadiusProperty, value); }
        }

        public static readonly BindableProperty RoundedCornerRadiusProperty = BindableProperty.Create(nameof(RoundedCornerRadius), typeof(double), typeof(RoundedCornerView), 3.0);

        public bool MakeCircle
        {
            get { return (bool)GetValue(MakeCircleProperty); }
            set { SetValue(MakeCircleProperty, value); }
        }

        public static readonly BindableProperty MakeCircleProperty = BindableProperty.Create(nameof(MakeCircle), typeof(bool), typeof(RoundedCornerView), false);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(RoundedCornerView), Color.Transparent);

        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }

        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(RoundedCornerView), 1);
        #endregion
    }
}
