using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Droid.Rendereres;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(SwitchColorifyEffectDroid),nameof(SwitchColorifyEffect))]
namespace CHSBackOffice.Droid.Rendereres
{
    public class SwitchColorifyEffectDroid : PlatformEffect
    {
        protected override void OnAttached()
        {
            var effect = (SwitchColorifyEffect)Element.Effects.FirstOrDefault(e => e is SwitchColorifyEffect);

            if (Control != null && Control is SwitchCompat && effect != null)
            {
                var swt = (SwitchCompat)Control;
                ProcessControl(swt, effect);
            }
        }

        protected override void OnDetached()
        {

        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            var effect = (SwitchColorifyEffect)Element.Effects.FirstOrDefault(e => e is SwitchColorifyEffect);

            if (Control != null && Control is SwitchCompat && args.PropertyName == "IsToggled" && effect != null)
            {
                var swt = (SwitchCompat)Control;
                ProcessControl(swt, effect);
            }
        }

        void ProcessControl(SwitchCompat swt, SwitchColorifyEffect effect)
        {
            if (swt.Enabled)
            {
                if (swt.Checked)
                {
                    swt.TrackDrawable.SetColorFilter(effect.SwitchOnColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                    swt.ThumbDrawable.SetColorFilter(effect.SwitchOnThumbColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
                else
                {
                    swt.ThumbDrawable.SetColorFilter(effect.SwitchOffThumbColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
            }
            else
            {
                if (swt.Checked)
                {
                    swt.TrackDrawable.SetColorFilter(effect.SwitchOnColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                    swt.ThumbDrawable.SetColorFilter(effect.SwitchOffThumbColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
                else
                {
                    swt.ThumbDrawable.SetColorFilter(effect.SwitchOffThumbColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
            }
        }
    }
}