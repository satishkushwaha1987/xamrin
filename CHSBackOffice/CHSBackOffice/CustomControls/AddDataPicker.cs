using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class AddDataPicker : RoutingEffect
    {
        public AddDataPicker() : base("CHSBackOffice.CustomControls." + nameof(AddDataPicker)) { }
        public static readonly BindableProperty OnProperty = BindableProperty.CreateAttached(propertyName: "On",
                                                                                             returnType: typeof(bool),
                                                                                             declaringType: typeof(AddDataPicker),
                                                                                             defaultValue: false,
                                                                                             propertyChanged: OnOffChanged);

        public static void SetOn(BindableObject view, bool value)
        {
            view.SetValue(OnProperty, value);
        }

        public static bool GetOn(BindableObject view)
        {
            return (bool)view.GetValue(OnProperty);
        }

        private static void OnOffChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Xamarin.Forms.View;
            if (view == null)
                return;
            if ((bool)newValue)
            {
                view.Effects.Add(Effect.Resolve("CHSBackOffice.CustomControls." + nameof(AddDataPicker)));
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is AddDataPicker);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }

        public static readonly BindableProperty DateProperty = BindableProperty.CreateAttached(propertyName: "Date",
                                                                                               returnType: typeof(DateTime),
                                                                                               declaringType: typeof(AddDataPicker),
                                                                                               defaultValue: default(DateTime),
                                                                                               defaultBindingMode: BindingMode.TwoWay,
                                                                                               propertyChanged:OnDatePropertyChanged);

        private static void OnDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
        }

        public static void SetDate(BindableObject view, DateTime value)
        {
            view.SetValue(DateProperty, value);
        }

        public static DateTime GetDate(BindableObject view)
        {
            return (DateTime)view.GetValue(DateProperty);
        }

        public static readonly BindableProperty MaxDateProperty = BindableProperty.CreateAttached(propertyName: "MaxDate",
                                                                                                  returnType: typeof(DateTime),
                                                                                                  declaringType: typeof(AddDataPicker),
                                                                                                  defaultValue: new DateTime(2100, 12, 31));

        public static void SetMaxDate(BindableObject view, DateTime value)
        {
            view.SetValue(MaxDateProperty, value);
        }

        public static DateTime GetMaxDate(BindableObject view)
        {
            return (DateTime)view.GetValue(MaxDateProperty);
        }

        public static readonly BindableProperty MinDateProperty = BindableProperty.CreateAttached(propertyName: "MinDate",
                                                                                                  returnType: typeof(DateTime),
                                                                                                  declaringType: typeof(AddDataPicker),
                                                                                                  defaultValue: new DateTime(1900, 1, 1));
        public static void SetMinDate(BindableObject view, DateTime value)
        {
            view.SetValue(MinDateProperty, value);
        }

        public static DateTime GetMinDate(BindableObject view)
        {
            return (DateTime)view.GetValue(MinDateProperty);
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached(propertyName: "Command",
                                                                                                  returnType: typeof(ICommand),
                                                                                                  declaringType: typeof(AddDataPicker),
                                                                                                  defaultValue: default(ICommand));

        public static void SetCommand(BindableObject view, ICommand value)
        {
            view.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(CommandProperty);
        }

        public static readonly BindableProperty TapCommandProperty = BindableProperty.CreateAttached(propertyName: "TapCommand",
                                                                                          returnType: typeof(ICommand),
                                                                                          declaringType: typeof(AddDataPicker),
                                                                                          defaultValue: default(ICommand));

        public static void SetTapCommand(BindableObject view, ICommand value)
        {
            view.SetValue(TapCommandProperty, value);
        }

        public static ICommand GetTapCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(TapCommandProperty);
        }

        public static readonly BindableProperty TodayTextProperty = BindableProperty.CreateAttached(propertyName: "TodayText",
                                                                                                    returnType: typeof(string),
                                                                                                    declaringType: typeof(AddDataPicker),
                                                                                                    defaultValue: default(string));

        public static void SetTodayText(BindableObject view, DateTime value)
        {
            view.SetValue(TodayTextProperty, value);
        }

        public static string GetTodayText(BindableObject view)
        {
            return (string)view.GetValue(TodayTextProperty);
        }

        
    }
}
