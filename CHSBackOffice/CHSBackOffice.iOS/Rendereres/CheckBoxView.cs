﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace CHSBackOffice.iOS.Rendereres
{
    [Register("CheckBoxView")]
    public class CheckBoxView : UIButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxView"/> class.
        /// </summary>
        public CheckBoxView()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxView"/> class.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        public CheckBoxView(CGRect bounds) : base(bounds)
        {
            Initialize();
        }

        /// <summary>
        /// Sets the checked title.
        /// </summary>
        /// <value>The checked title.</value>
        public string CheckedTitle
        {
            set
            {
                SetTitle(value, UIControlState.Selected);
            }
        }

        /// <summary>
        /// Sets the unchecked title.
        /// </summary>
        /// <value>The unchecked title.</value>
        public string UncheckedTitle
        {
            set
            {
                SetTitle(value, UIControlState.Normal);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CheckBoxView"/> is checked.
        /// </summary>
        /// <value><c>true</c> if checked; otherwise, <c>false</c>.</value>
        public bool Checked
        {
            set { Selected = value; }
            get { return Selected; }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize()
        {
            AdjustEdgeInsets();
            ApplyStyle();

            TouchUpInside += (sender, args) => Selected = !Selected;
            // set default color, because type is not UIButtonType.System 
            SetTitleColor(UIColor.DarkTextColor, UIControlState.Normal);
            SetTitleColor(UIColor.DarkTextColor, UIControlState.Selected);
            
        }

        /// <summary>
        /// Adjusts the edge insets.
        /// </summary>
        void AdjustEdgeInsets()
        {
            const float Inset = 8f;

            HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
            ImageEdgeInsets = new UIEdgeInsets(0f, Inset, 0f, 0f);
            TitleEdgeInsets = new UIEdgeInsets(0f, Inset * 2, 0f, 0f);
        }

        /// <summary>
        /// Applies the style.
        /// </summary>
        void ApplyStyle()
        {
            var originalChecked = UIImage.FromBundle("Images/Checkbox/checked_checkbox.png");
            var tintedChecked = originalChecked.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            SetImage(tintedChecked, UIControlState.Selected);
            var originalUnChecked = UIImage.FromBundle("Images/Checkbox/unchecked_checkbox.png");
            var tintedUnChecked = originalUnChecked.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            SetImage(tintedUnChecked, UIControlState.Normal);
            this.TintColor = UIColor.White;
        }
    }
}