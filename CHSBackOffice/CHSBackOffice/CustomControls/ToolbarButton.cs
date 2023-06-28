using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class ToolbarButton : View, INotifyPropertyChanged
    {
        #region "PUBLIC PROPS"

        public string Name { get; set; }
        public string FontName { get; set; }
        public double FontSize { get; set; } = 35;

        #endregion

        #region "BINDABLE PROPS"

        #region "Command"

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command), typeof(ICommand), typeof(ToolbarButton), default(ICommand));

        #endregion

        #region "Text"

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text), typeof(string), typeof(ToolbarButton), "", propertyChanged: TextChanged);

        static void TextChanged(object o, object oldVal, object newVal)
        {
            ((ToolbarButton)o).RaisePropertyChanged(nameof(Text));
        }

        #endregion

        #region "IsButtonVisible"

        public bool IsButtonVisible
        {
            get { return (bool)GetValue(IsButtonVisibleProperty); }
            set { SetValue(IsButtonVisibleProperty, value); }
        }

        public static readonly BindableProperty IsButtonVisibleProperty = BindableProperty.Create(
            nameof(IsButtonVisible), typeof(bool), typeof(ToolbarButton), true, propertyChanged: VisibleChanged);

        static void VisibleChanged(object o, object oldVal, object newVal)
        {
            ((ToolbarButton)o).RaisePropertyChanged(nameof(IsButtonVisible));
        }

        #endregion

        #endregion

        #region "INTERNAL METHODS"

        internal void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        #endregion
    }
}
