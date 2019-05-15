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
using System.ComponentModel;

[assembly:ExportRenderer(typeof(RoundedBoxView),typeof(RoundedBoxBoxViewRenderer))]
namespace MedCon.Droid.CustomRenderers
{
   public class RoundedBoxBoxViewRenderer : ViewRenderer<RoundedBoxView, Android.Views.View>
    {
        public static void Init()
        {
        }

        private RoundedBoxView _formControl
        {
            get { return Element; }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<RoundedBoxView> e)
        {
            base.OnElementChanged(e);

            this.InitializeFrom(_formControl);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            this.UpdateFrom(_formControl, e.PropertyName);
        }
    }
    public static class UIViewExtensions
    {
        public static void InitializeFrom(this Android.Views.View nativeControl, RoundedBoxView formsControl)
        {
            if (nativeControl == null || formsControl == null)
                return;

            var background = new GradientDrawable();

            background.SetColor(formsControl.BackgroundColor.ToAndroid());

            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
            {
                nativeControl.Background = background;
            }
            else
            {
                nativeControl.SetBackgroundDrawable(background);
            }

            nativeControl.UpdateCornerRadius(formsControl.CornerRadius);
            nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
        }

        public static void UpdateFrom(this Android.Views.View nativeControl, RoundedBoxView formsControl,
          string propertyChanged)
        {
            if (nativeControl == null || formsControl == null)
                return;

            if (propertyChanged == RoundedBoxView.CornerRadiusProperty.PropertyName)
            {
                nativeControl.UpdateCornerRadius(formsControl.CornerRadius);
            }
            if (propertyChanged == VisualElement.BackgroundColorProperty.PropertyName)
            {
                var background = nativeControl.Background as GradientDrawable;

                if (background != null)
                {
                    background.SetColor(formsControl.BackgroundColor.ToAndroid());
                }
            }

            if (propertyChanged == RoundedBoxView.BorderColorProperty.PropertyName)
            {
                nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
            }

            if (propertyChanged == RoundedBoxView.BorderThicknessProperty.PropertyName)
            {
                nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
            }
        }

        public static void UpdateBorder(this Android.Views.View nativeControl, Color color, int thickness)
        {
            var backgroundGradient = nativeControl.Background as GradientDrawable;

            if (backgroundGradient != null)
            {
                var relativeBorderThickness = thickness * 3;
                backgroundGradient.SetStroke(relativeBorderThickness, color.ToAndroid());
            }
        }

        public static void UpdateCornerRadius(this Android.Views.View nativeControl, double cornerRadius)
        {
            var backgroundGradient = nativeControl.Background as Android.Graphics.Drawables.GradientDrawable;

            if (backgroundGradient != null)
            {
                var relativeCornerRadius = (float)(cornerRadius * 3.7);
                backgroundGradient.SetCornerRadius(relativeCornerRadius);
            }
        }
    }
}