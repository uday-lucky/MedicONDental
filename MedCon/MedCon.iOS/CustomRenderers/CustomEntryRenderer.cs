using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using MedCon.CustomControls;
using MedCon.iOS.CustomRenderers;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MedCon.iOS.CustomRenderers
{
   public class CustomEntryRenderer: EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // do whatever you want to the UITextField here!
                Control.TextAlignment = UITextAlignment.Left;

               
               
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.BorderColor = Color.FromHex("#ffffff").ToCGColor();
                Control.Layer.BorderWidth = 1f;
                Control.TextColor = Color.Black.ToUIColor();
                Control.Font = UIFont.FromName("proximanova-light-webfont", 15);
               // Control.Frame = new CGRect(x: 5, y: 0, width: 20, height: 100);
            }
        }
    }
}