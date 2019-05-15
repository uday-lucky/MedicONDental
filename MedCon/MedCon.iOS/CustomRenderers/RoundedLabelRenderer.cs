using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MedCon.CustomControls;
using MedCon.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedLabel), typeof(CurvedCornersLabelRenderer))]
namespace MedCon.iOS.CustomRenderers
{
    public class CurvedCornersLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var _xfViewReference = (RoundedLabel)Element;

                // Radius for the curves
                this.Layer.CornerRadius = (float)_xfViewReference.CurvedCornerRadius;

                this.Layer.BackgroundColor = _xfViewReference.CurvedBackgroundColor.ToCGColor();
            }
        }
    }
}