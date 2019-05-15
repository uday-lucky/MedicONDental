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
using MedCon.Droid.CustomRenderers;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Android.Content.Res;
using MedCon.CustomControls;
using Android.Graphics.Drawables;

[assembly:ExportRenderer(typeof(CustomEntry),typeof(CustomEntryRenderer))]
namespace MedCon.Droid.CustomRenderers
{
   public class CustomEntryRenderer:EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            //if (Control == null || e.NewElement == null) return;

            //if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            //    Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.White);
            //else
            //    Control.Background.SetColorFilter(Android.Graphics.Color.Red, PorterDuff.Mode.SrcAtop);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                this.Control.SetBackgroundDrawable(gd);
            }
        }
    }
}