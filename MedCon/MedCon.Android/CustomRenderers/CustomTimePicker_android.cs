using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MedCon.CustomControls;
using MedCon.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(CustomTimePicker),typeof(CustomTimePicker_android))]
namespace MedCon.Droid.CustomRenderers
{
   public class CustomTimePicker_android:TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TimePicker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.TextSize = 13;
                Control.SetTextColor(global::Android.Graphics.Color.White);
                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(0, Android.Graphics.Color.LightGray);
                Control.SetBackgroundDrawable(gd);
            }
        }
    }
}