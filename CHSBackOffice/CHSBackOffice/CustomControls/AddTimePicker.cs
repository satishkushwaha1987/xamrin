using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class AddTimePicker : RoutingEffect
    {
        public AddTimePicker() : base("CHSBackOffice.CustomControls." + nameof(AddTimePicker))
        {

        }

        public static readonly BindableProperty OnProperty = BindableProperty.CreateAttached(propertyName: "On",
                                                                                             returnType: typeof(bool),
                                                                                             declaringType: typeof(AddTimePicker),
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
                view.Effects.Add(Effect.Resolve("CHSBackOffice.CustomControls." + nameof(AddTimePicker)));
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is AddTimePicker);
                if (toRemove != null)
                {
                    view.Effects.Remove(toRemove);
                }
            }
        }

        public static readonly BindableProperty TimeProperty = BindableProperty.CreateAttached(propertyName: "Time",
                                                                                               returnType: typeof(TimeSpan),
                                                                                               declaringType: typeof(AddTimePicker),
                                                                                               defaultValue: default(TimeSpan),
                                                                                               defaultBindingMode: BindingMode.TwoWay);

        public static void SetTime(BindableObject view, TimeSpan value)
        {
            view.SetValue(TimeProperty, value);
        }

        public static TimeSpan GetTime(BindableObject view)
        {
            return (TimeSpan)view.GetValue(TimeProperty);
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.CreateAttached(propertyName: "Title",
                                                                                                returnType: typeof(string),
                                                                                                declaringType: typeof(AddTimePicker),
                                                                                                defaultValue: default(string));

        public static void SetTitle(BindableObject view, string value)
        {
            view.SetValue(TitleProperty, value);
        }

        public static string GetTitle(BindableObject view)
        {
            return (string)view.GetValue(TitleProperty);
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached(propertyName: "Command",
                                                                                                  returnType: typeof(ICommand),
                                                                                                  declaringType: typeof(AddTimePicker),
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
                                                                                          declaringType: typeof(AddTimePicker),
                                                                                          defaultValue: default(ICommand));

        public static void SetTapCommand(BindableObject view, ICommand value)
        {
            view.SetValue(TapCommandProperty, value);
        }

        public static ICommand GetTapCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(TapCommandProperty);
        }

    }
}
