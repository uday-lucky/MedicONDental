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
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Graphics;

[assembly:ExportRenderer(typeof(CustomDatepicker),typeof(CustomDatepickerRenderer))]
namespace MedCon.Droid.CustomRenderers
{
   public class CustomDatepickerRenderer:DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets, "RobotoCondensed-Regular.ttf");
                Control.TextSize = 14;
                
                this.Control.SetBackgroundDrawable(gd);
            }
        }
    }
}