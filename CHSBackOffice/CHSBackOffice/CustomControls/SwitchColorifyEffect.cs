
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class SwitchColorifyEffect : RoutingEffect
    {
        public SwitchColorifyEffect() : base("CHSBackOffice.CustomControls." + nameof(SwitchColorifyEffect))
        {
        }

        public Color SwitchOffColor { set; get; }

        public Color SwitchOnColor { set; get; }

        public Color SwitchOffThumbColor { set; get; }

        public Color SwitchOnThumbColor { set; get; }
    }
}
