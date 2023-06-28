using Android.Content;
using Android.Runtime;
using CHSBackOffice.CustomControls;
using CHSBackOffice.Droid.InterfacesImplementation;
using CHSBackOffice.Droid.Rendereres;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Page), typeof(CommonPageRenderer))]
namespace CHSBackOffice.Droid.Rendereres
{
    [Preserve(AllMembers = true)]
    class CommonPageRenderer : PageRenderer
    {
        SetToolbarColorImpl _setToolbarColorImpl = new SetToolbarColorImpl();

        static Color EmptyColor => Color.FromHex("#013122");

        Color naviBarColor = EmptyColor;

        

        public CommonPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            
            if (e.NewElement is ExtendedNaviPage)
                naviBarColor = (e.NewElement as ExtendedNaviPage).NaviBarBackgroundColor;

            SetToolbarColor();
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            SetToolbarColor();
        }

        void SetToolbarColor()
        {
            if (naviBarColor != EmptyColor && naviBarColor != null)
                _setToolbarColorImpl.SetToolbarColor(naviBarColor);
        }

    }
}