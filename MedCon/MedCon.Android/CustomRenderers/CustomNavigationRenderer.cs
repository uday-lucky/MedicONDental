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
using MedCon.CustomControls;
using Xamarin.Forms;
using MedCon.Droid.CustomRenderers;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Android.Support.V4.App;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(MedCon.Views.CustomNavigationPage), typeof(CustomNavigationRenderer))]
namespace MedCon.Droid.CustomRenderers
{
   public class CustomNavigationRenderer:NavigationPageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);
           
        }
        protected override void SetupPageTransition(Android.Support.V4.App.FragmentTransaction transaction, bool isPush)
        {
            base.SetupPageTransition(transaction, isPush);
            if (isPush)
            {
                transaction.SetCustomAnimations(Resource.Animation.enter_right, Resource.Animation.exit_left,
                    Resource.Animation.enter_left, Resource.Animation.exit_right);
            }
            else
            {
                transaction.SetCustomAnimations(Resource.Animation.enter_left, Resource.Animation.exit_right,
                    Resource.Animation.enter_right, Resource.Animation.exit_left);
            }
        }
    }
}