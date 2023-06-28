using Xamarin.Essentials;
using Xamarin.Forms;

namespace CHSBackOffice.CustomControls
{
    public class PickerWithPopoverBottom : PickerWithPopover
    {
        public PickerWithPopoverBottom()
        {
            ExtendedNaviPage.OnOrientartionChanged += ExtendedNaviPage_OnOrientartionChanged;
            InitPopover();
        }

        #region "Events Handling"
        private void ExtendedNaviPage_OnOrientartionChanged(object sender, PageOrientationEventsArgs e)
        {
            DemensionsPopover();
        }

        #endregion

        #region Methods
        public override void InitPopover()
        {
            base.InitPopover();
            #region Popover
            popoverContainer = new Grid();
            DemensionsPopover();
            popoverContainer.RowSpacing = 0;
            popoverContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(16) });
            popoverContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            upIconContainer = new RelativeLayout();
            upIcon = new Image();
            upIcon.Source = "PopupPickerArrowTop.png"; // "UpIcon.png";
            upIcon.Aspect = Aspect.AspectFit;
            upIconContainer.Children.Add(upIcon,
                xConstraint: Constraint.RelativeToParent((parent) => { return parent.Width * 0.92; }),
                widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width * 0.05; }),
                heightConstraint: Constraint.RelativeToParent((parent) => { return parent.Height; }));

            popoverContainer.Children.Add(frameContainer, 0, 1);
            popoverContainer.Children.Add(upIcon, 0, 0);


            #endregion

            #region Content

            _contentGrid.Children.Clear();
            _contentGrid.Children.Add(_pickerText, 0, 0);
            Grid.SetColumnSpan(_pickerText, 2);
            #endregion
        }

        #endregion
    }
}
