using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class HorizontalPanelExtentions
    {
        public static readonly BindableProperty SelectedKeyProperty = BindableProperty.Create("SelectedKey", typeof(string), typeof(HorizontalPanelExtentions), string.Empty);
        public static void SetSelectedKey(BindableObject bindable, string key)
        {
            bindable.SetValue(SelectedKeyProperty, key);
        }

        public static string GetSelectedKey(BindableObject bindable)
        {
            return (string)bindable.GetValue(SelectedKeyProperty);
        }
    }
}
