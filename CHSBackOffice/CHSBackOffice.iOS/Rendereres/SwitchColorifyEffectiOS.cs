using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CHSBackOffice.CustomControls;
using CHSBackOffice.iOS.Rendereres;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(SwitchColorifyEffectiOS), nameof(SwitchColorifyEffect))]
namespace CHSBackOffice.iOS.Rendereres
{
    public class SwitchColorifyEffectiOS : PlatformEffect
    {
        protected override void OnAttached()
        {
            var effect = (SwitchColorifyEffect)Element.Effects.FirstOrDefault(e => e is SwitchColorifyEffect);

            if (Control != null && Control is UISwitch && effect != null)
            {
                var swt = (UISwitch)Control;
                ProcessControl(swt, effect);
            }
        }

        protected override void OnDetached()
        {

        }


        void ProcessControl(UISwitch swt, SwitchColorifyEffect effect)
        {
            if (swt.Enabled)
                swt.OnTintColor = effect.SwitchOnThumbColor.ToUIColor();
            else
                swt.OnTintColor = effect.SwitchOffThumbColor.ToUIColor();
        }
    }
}