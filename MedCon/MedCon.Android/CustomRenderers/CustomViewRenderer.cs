using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using MedCon.CustomControls;
using MedCon.Droid.CustomRenderers;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using Android.Util;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomView), typeof(CustomViewRenderer))]
namespace MedCon.Droid.CustomRenderers
{
   public class CustomViewRenderer : VisualElementRenderer<CustomView>
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            //HandlePropertyChanged (sender, e);
            BorderRendererVisual.UpdateBackground(Element, this.ViewGroup);
        }


        protected override void OnElementChanged(ElementChangedEventArgs<CustomView> e)
        {
            base.OnElementChanged(e);
            BorderRendererVisual.UpdateBackground(Element, this.ViewGroup);
        }

        /*void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Content")
			{
				BorderRendererVisual.UpdateBackground (Element, this.ViewGroup);
			}
		}*/

        protected override void DispatchDraw(Android.Graphics.Canvas canvas)
        {
            if (Element.IsClippedToBorder)
            {
                canvas.Save(SaveFlags.Clip);
                BorderRendererVisual.SetClipPath(this, canvas);
                base.DispatchDraw(canvas);
                canvas.Restore();
            }
            else
            {
                base.DispatchDraw(canvas);
            }
        }

    }
}