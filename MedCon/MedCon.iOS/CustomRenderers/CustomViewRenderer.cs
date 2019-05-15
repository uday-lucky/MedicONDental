using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using MedCon.CustomControls;
using CoreAnimation;
using Xamarin.Forms.Platform.iOS;
using MedCon.iOS.CustomRenderers;
using System.ComponentModel;
using CoreGraphics;

[assembly: ExportRenderer(typeof(CustomView), typeof(CustomViewRenderer))]
namespace MedCon.iOS.CustomRenderers
{
   public class CustomViewRenderer : VisualElementRenderer<CustomView>
    {
        CALayer[] borderLayers = new CALayer[4];

        public CustomViewRenderer()
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {

                this.SetupLayer();

            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
                e.PropertyName == CustomView.StrokeProperty.PropertyName ||
                e.PropertyName == CustomView.StrokeThicknessProperty.PropertyName ||
                e.PropertyName == CustomView.CornerRadiusProperty.PropertyName ||
                e.PropertyName == CustomView.WidthProperty.PropertyName ||
                e.PropertyName == CustomView.HeightProperty.PropertyName)
            {
                this.ClipsToBounds = true;

                this.SetupLayer();
            }
        }

        private void SetupLayer()
        {

            if (Element == null || Element.Width <= 0 || Element.Height <= 0)
            {
                return;
            }

            Layer.CornerRadius = (nfloat)Element.CornerRadius;


            if (Element.BackgroundColor != Color.Default)
            {
                Layer.BackgroundColor = Element.BackgroundColor.ToCGColor();
            }
            else
            {
                Layer.BackgroundColor = UIColor.White.CGColor;
            }

            UpdateBorderLayer(BorderPosition.Left, (nfloat)Element.StrokeThickness.Left);
            UpdateBorderLayer(BorderPosition.Top, (nfloat)Element.StrokeThickness.Top);
            UpdateBorderLayer(BorderPosition.Right, (nfloat)Element.StrokeThickness.Right);
            UpdateBorderLayer(BorderPosition.Bottom, (nfloat)Element.StrokeThickness.Bottom);

            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
        }

        enum BorderPosition
        {
            Left,
            Top,
            Right,
            Bottom
        }

        void UpdateBorderLayer(BorderPosition borderPosition, nfloat thickness)
        {

            var borderLayer = borderLayers[(int)borderPosition];
            if (thickness <= 0)
            {
                if (borderLayer != null)
                {
                    borderLayer.RemoveFromSuperLayer();
                    borderLayers[(int)borderPosition] = null;
                }
            }
            else
            {
                if (borderLayer == null)
                {
                    borderLayer = new CALayer();
                    Layer.AddSublayer(borderLayer);
                    borderLayers[(int)borderPosition] = borderLayer;
                }

                switch (borderPosition)
                {
                    case BorderPosition.Left:
                        borderLayer.Frame = new CGRect(0, 0, thickness, (nfloat)Element.Height);
                        break;
                    case BorderPosition.Top:
                        borderLayer.Frame = new CGRect(0, 0, (nfloat)Element.Width, thickness);
                        break;
                    case BorderPosition.Right:
                        borderLayer.Frame = new CGRect((nfloat)Element.Width - thickness, 0, thickness, (nfloat)Element.Height);
                        break;
                    case BorderPosition.Bottom:
                        borderLayer.Frame = new CGRect(0, (nfloat)Element.Height - thickness, (nfloat)Element.Width, thickness);
                        break;
                }


                borderLayer.BackgroundColor = Element.Stroke.ToCGColor();
                borderLayer.CornerRadius = (nfloat)Element.CornerRadius;
                ClipsToBounds = true;
            }
        }
    }
}