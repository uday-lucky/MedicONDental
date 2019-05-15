using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using App1.Droid.CustomRenderers;
using MedCon.CustomControls;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigation))]
namespace App1.Droid.CustomRenderers
{
    public class CustomNavigation : NavigationPageRenderer
    {
        

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //if (e.PropertyName == CustomNavigationPage.TransitionTypeProperty.PropertyName)
                //UpdateTransitionType();
        }

        protected override void SetupPageTransition(Android.Support.V4.App.FragmentTransaction transaction, bool isPush)
        {
            //switch (_transitionType)
            //{
            //    case TransitionType.Fade:
            //        if (isPush)
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.fade_in, Resource.Animation.fade_out,
            //                Resource.Animation.fade_out, Resource.Animation.fade_in);
            //        }
            //        else
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.fade_in, Resource.Animation.fade_out,
            //                 Resource.Animation.fade_out, Resource.Animation.fade_in);
            //        }
            //        break;
            //    case TransitionType.Flip:
            //        if (isPush)
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.flip_in, Resource.Animation.flip_out,
            //                Resource.Animation.fade_out, Resource.Animation.flip_in);
            //        }
            //        else
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.flip_in, Resource.Animation.flip_out,
            //                 Resource.Animation.flip_out, Resource.Animation.flip_in);
            //        }
            //        break;
            //    case TransitionType.Scale:
            //        if (isPush)
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.scale_in, Resource.Animation.scale_out,
            //                Resource.Animation.scale_out, Resource.Animation.scale_in);
            //        }
            //        else
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.scale_in, Resource.Animation.scale_out,
            //                 Resource.Animation.scale_out, Resource.Animation.scale_in);
            //        }
            //        break;
            //    case TransitionType.SlideFromLeft:
            //        if (isPush)
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.enter_left, Resource.Animation.exit_right,
            //                Resource.Animation.enter_right, Resource.Animation.exit_left);
            //        }
            //        else
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.enter_right, Resource.Animation.exit_left,
            //                Resource.Animation.enter_left, Resource.Animation.exit_right);
            //        }
            //        break;
            //    case TransitionType.SlideFromRight:
            //        if (isPush)
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.enter_right, Resource.Animation.exit_left,
            //                Resource.Animation.enter_left, Resource.Animation.exit_right);
            //        }
            //        else
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.enter_left, Resource.Animation.exit_right,
            //                Resource.Animation.enter_right, Resource.Animation.exit_left);
            //        }
            //        break;
            //    case TransitionType.SlideFromTop:
            //        if (isPush)
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.enter_top, Resource.Animation.exit_bottom,
            //                Resource.Animation.enter_bottom, Resource.Animation.exit_top);
            //        }
            //        else
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.enter_bottom, Resource.Animation.exit_top,
            //                Resource.Animation.enter_top, Resource.Animation.exit_bottom);
            //        }
            //        break;
            //    case TransitionType.SlideFromBottom:
            //        if (isPush)
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.enter_bottom, Resource.Animation.exit_top,
            //                Resource.Animation.enter_top, Resource.Animation.exit_bottom);
            //        }
            //        else
            //        {
            //            transaction.SetCustomAnimations(Resource.Animation.enter_top, Resource.Animation.exit_bottom,
            //                Resource.Animation.enter_bottom, Resource.Animation.exit_top);
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }

        private void UpdateTransitionType()
        {
            var CustomNavigationPage = (CustomNavigationPage)Element;
            _transitionType = CustomNavigationPage.TransitionType;
        }
    }
}