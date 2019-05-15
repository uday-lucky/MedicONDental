using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using Xamarin.Forms;
using MedCon.iOS.CustomRenderers;
using MedCon.CustomControls;

[assembly:ExportRenderer(typeof(CustomFrame),typeof(CustomFrameRenderer))]
namespace MedCon.iOS.CustomRenderers
{
   public class CustomFrameRenderer:FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            SetupShadowLayer();
            base.Draw(rect);
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
           if(e.NewElement!=null)
            {

            }
        }

        void SetupShadowLayer()
        {
            
            Layer.CornerRadius = 2; // 5 Default
            if (Element.BackgroundColor == Xamarin.Forms.Color.Default)
            {
                Layer.BackgroundColor = UIColor.White.CGColor;
            }
            else
            {
                Layer.BackgroundColor = Element.BackgroundColor.ToCGColor();
            }

            Layer.ShadowRadius = 2; // 5 Default
            Layer.ShadowColor = UIColor.Black.CGColor;
            Layer.ShadowOpacity = 0.4f; // 0.8f Default
            Layer.ShadowOffset = new CGSize(0f, 2.5f);

            if (Element.OutlineColor == Xamarin.Forms.Color.Default)
            {
                Layer.BorderColor = UIColor.Clear.CGColor;
            }
            else
            {
                Layer.BorderColor = Element.OutlineColor.ToCGColor();
                Layer.BorderWidth = 1;
            }

            Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            Layer.ShouldRasterize = true;
        }
    }
}