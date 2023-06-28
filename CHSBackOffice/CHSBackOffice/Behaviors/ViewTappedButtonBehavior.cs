using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CHSBackOffice.Behaviors
{
    #region "TappedAnimationType"

    public enum TappedAnimationType
    {
        None,
        Fade,
        FlipHorizontal,
        FlipVertical,
        Rotate,
        ScaleInForce,
        ScaleIn,
        ScaleOut,
        Shake
    }

    #endregion

    public class ViewTappedButtonBehavior : Behavior<View>
    {
        bool _isAnimating = false;

        #region "BINDABLE PROPS"

        #region "AnimationType"

        public static readonly BindableProperty AnimationTypeProperty =
            BindableProperty.Create(nameof(AnimationType), typeof(TappedAnimationType), typeof(ViewTappedButtonBehavior), TappedAnimationType.Fade);

        public TappedAnimationType AnimationType
        {
            get { return (TappedAnimationType)GetValue(AnimationTypeProperty); }
            set { SetValue(AnimationTypeProperty, value); }
        }

        #endregion

        #region "Command"

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ViewTappedButtonBehavior), null, defaultBindingMode: BindingMode.TwoWay);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #region "CommandParameter"

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ViewTappedButtonBehavior), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        #endregion

        #endregion

        #region "PUBLIC PROPS"

        public View AssociatedObject { get; private set; }

        #endregion

        #region "EVENT HANDLERS"

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            bindable.BindingContextChanged += OnBindingContextChanged;

            if (bindable is Button myButton)
            {
                myButton.Clicked += View_Tapped;
            }
            else if (bindable is Switch mySwitch)
            {
                mySwitch.Toggled += View_Tapped;
            }
            else
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += View_Tapped;
                bindable.GestureRecognizers.Add(tapGestureRecognizer);
            }

            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;

            var exists = bindable.GestureRecognizers.FirstOrDefault() as TapGestureRecognizer;

            if (exists != null)
                exists.Tapped -= View_Tapped;

            base.OnDetachingFrom(bindable);
        }

        void View_Tapped(object sender, EventArgs e)
        {
            if (_isAnimating)
                return;

            _isAnimating = true;

            var view = (View)sender;

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    switch (AnimationType)
                    {
                        case TappedAnimationType.Fade:
                            await view.FadeTo(0.3, 150);
                            await view.FadeTo(1, 150);
                            break;
                        case TappedAnimationType.FlipHorizontal:
                            // Perform half of the flip
                            await view.RotateYTo(90, 200);
                            await view.RotateYTo(0, 200);
                            break;
                        case TappedAnimationType.FlipVertical:
                            // Perform half of the flip
                            await view.RotateXTo(90, 200);
                            await view.RotateXTo(0, 200);
                            break;
                        case TappedAnimationType.Rotate:
                            await view.RotateTo(360, 200, easing: Easing.Linear);
                            view.Rotation = 0;
                            break;
                        case TappedAnimationType.ScaleInForce:
                            await view.ScaleTo(0.95, 2, easing: Easing.Linear);
                            await view.ScaleTo(1, 5, easing: Easing.Linear);
                            await Task.Delay(10);
                            break;
                        case TappedAnimationType.ScaleIn:
                            await view.ScaleTo(0.95, 30, easing: Easing.Linear);
                            await view.ScaleTo(1, 50, easing: Easing.Linear);
                            await Task.Delay(20);
                            break;
                        case TappedAnimationType.ScaleOut:
                            await view.ScaleTo(1.05, 30, easing: Easing.Linear);
                            await view.ScaleTo(1, 50, easing: Easing.Linear);
                            await Task.Delay(20);
                            break;
                        case TappedAnimationType.Shake:
                            await view.TranslateTo(-15, 0, 50);
                            await view.TranslateTo(15, 0, 50);
                            await view.TranslateTo(-10, 0, 50);
                            await view.TranslateTo(10, 0, 50);
                            await view.TranslateTo(-5, 0, 50);
                            await view.TranslateTo(5, 0, 50);
                            view.TranslationX = 0;
                            break;
                        default:
                            break;
                    }
                }
                finally
                {
                    if (Command != null)
                    {
                        if (Command.CanExecute(CommandParameter))
                        {
                            Command.Execute(CommandParameter);
                        }
                    }

                    _isAnimating = false;
                }
            });
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }

        #endregion
    }
}
