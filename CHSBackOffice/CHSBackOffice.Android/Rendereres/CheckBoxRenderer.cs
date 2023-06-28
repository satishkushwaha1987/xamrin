using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Views;
using CHSBackOffice.Support;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CheckBox = CHSBackOffice.CustomControls.CheckBox;
using CheckBoxRenderer = CHSBackOffice.Droid.Rendereres.CheckBoxRenderer;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]
namespace CHSBackOffice.Droid.Rendereres
{
    public class CheckBoxRenderer : ViewRenderer<CheckBox, Android.Widget.CheckBox>
    {
        private ColorStateList defaultTextColor;

        public CheckBoxRenderer(Context context) : base(context)
        {
        }

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                //var checkBox = new Android.Widget.CheckBox(this.Context);
                LayoutInflater inflater = LayoutInflater.From(this.Context);
                var frame = inflater.Inflate(Resource.Layout.CheckBox, ViewGroup, false);
                var checkBox = frame.FindViewById<Android.Widget.CheckBox>(Resource.Id.checkbox);

                checkBox.CheckedChange += CheckBoxCheckedChange;

                defaultTextColor = checkBox.TextColors;
                this.SetNativeControl(checkBox);
            }

            Control.Text = e.NewElement.Text;
            Control.Checked = e.NewElement.Checked;
            UpdateTextColor();

            if (e.NewElement.FontSize > 0)
            {
                Control.TextSize = (float)e.NewElement.FontSize;
            }

            if (!string.IsNullOrEmpty(e.NewElement.FontName))
            {
                Control.Typeface = TrySetFont(e.NewElement.FontName);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Text = Element.Text;
                    Control.Checked = Element.Checked;
                    break;
                case "TextColor":
                    UpdateTextColor();
                    break;
                case "FontName":
                    if (!string.IsNullOrEmpty(Element.FontName))
                    {
                        Control.Typeface = TrySetFont(Element.FontName);
                    }
                    break;
                case "FontSize":
                    if (Element.FontSize > 0)
                    {
                        Control.TextSize = (float)Element.FontSize;
                    }
                    break;
                case "CheckedText":
                case "UncheckedText":
                    Control.Text = Element.Text;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
                    break;
            }
        }

        /// <summary>
        /// CheckBoxes the checked change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Android.Widget.CompoundButton.CheckedChangeEventArgs"/> instance containing the event data.</param>
        void CheckBoxCheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
        {
            this.Element.Checked = e.IsChecked;
        }

        /// <summary>
        /// Tries the set font.
        /// </summary>
        /// <param name="fontName">Name of the font.</param>
        /// <returns>Typeface.</returns>
        private Typeface TrySetFont(string fontName)
        {
            Typeface tf = Typeface.Default;
            try
            {
                tf = Typeface.CreateFromAsset(Context.Assets, fontName);
                return tf;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                try
                {
                    tf = Typeface.CreateFromFile(fontName);
                    return tf;
                }
                catch (Exception ex1)
                {
                    ExceptionProcessor.ProcessException(ex1);
                    return Typeface.Default;
                }
            }
        }

        /// <summary>
        /// Updates the color of the text
        /// </summary>
        private void UpdateTextColor()
        {
            if (Control == null || Element == null)
                return;

            if (Element.TextColor == Xamarin.Forms.Color.Default)
                Control.SetTextColor(defaultTextColor);
            else
                Control.SetTextColor(Element.TextColor.ToAndroid());
        }
    }
}