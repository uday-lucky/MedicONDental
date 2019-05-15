using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using MedCon.iOS.CustomRenderers;
using FFImageLoading.Forms.Touch;
using XamForms.Controls;

namespace MedCon.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();
            SlideOverKit.iOS.SlideOverKit.Init();
            global::Xamarin.Forms.Forms.Init();

            CachedImageRenderer.Init();

            XamForms.Controls.iOS.Calendar.Init();

            //IOS status bar style
            UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
            statusBar.BackgroundColor = UIColor.FromRGB(121, 134, 203);

            //UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            //UIApplication.SharedApplication.SetStatusBarHidden(false, false);
            //UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(121, 134, 203);

            ShapeRenderer.Init();
            LoadApplication(new App());

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            return base.FinishedLaunching(app, options);
        }
    }
}
