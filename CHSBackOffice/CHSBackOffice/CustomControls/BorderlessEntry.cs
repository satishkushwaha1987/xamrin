using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class BorderlessEntry : Entry
    {
        #region "PROPS"

        public static readonly BindableProperty BorderlessProperty =
           BindableProperty.Create(nameof(Borderless), typeof(bool), typeof(BorderlessEntry), true);

        public bool Borderless
        {
            get { return (bool)GetValue(BorderlessProperty); }
            set { SetValue(BorderlessProperty, value); }
        }

        public static readonly BindableProperty LineColorProperty =
         BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(BorderlessEntry), Color.White);


        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public static readonly BindableProperty RoundedStyleProperty =
            BindableProperty.Create(nameof(RoundedStyle), typeof(RoundedEntryStyle), typeof(BorderlessEntry), RoundedEntryStyle.White);

        public RoundedEntryStyle RoundedStyle
        {
            get { return (RoundedEntryStyle)GetValue(RoundedStyleProperty); }
            set { SetValue(RoundedStyleProperty, value); }
        }
        
        #endregion
    }

    public enum RoundedEntryStyle
    {
        None,
        Oppacity,
        White
    }
}
