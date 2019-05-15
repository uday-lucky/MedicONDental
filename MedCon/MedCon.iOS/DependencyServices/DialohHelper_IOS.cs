using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MedCon.Interfaces;
using Xamarin.Forms;
using MedCon.iOS.DependencyServices;

[assembly: Dependency(typeof(DialohHelper_IOS))]
namespace MedCon.iOS.DependencyServices
{
    public class DialohHelper_IOS : IDialogService
    {
        const double LONG_DELAY = 3.5;
        const double SHORT_DELAY = 2.0;

        NSTimer alertDelay;
        UIAlertController alert;
        public void HideDailog()
        {
            BigTed.BTProgressHUD.Dismiss();
        }

        /// <summary>
        /// Show progress dialog.
        /// </summary>
        /// <param name="message"></param>
        public void ShowDailog(string message)
        {
            BigTed.BTProgressHUD.Show(message, -1, BigTed.ProgressHUD.MaskType.Black);
        }

        public void ShowLongToast(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }

        public void ShowShortToast(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }
        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }
        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }

        public void DisplayNativeAlert(string message, string title)
        {

            UIAlertView alert = new UIAlertView("MedCon", message, null,title)
            {
                AlertViewStyle = UIAlertViewStyle.Default
            };

            alert.Clicked += (sender, e) => {
               
            };

            alert.ShouldEnableFirstOtherButton += (UIAlertView alertView) => {
                return !string.IsNullOrWhiteSpace(alertView.GetTextField(0).Text);
            };

            alert.Show();

        }

        public bool ConfirmAlert(string message, string yes, string no, string title)
        {
            return true;
        }

        public void ShowProgress(string message = "")
        {            
            BigTed.BTProgressHUD.Show(message, -1, BigTed.ProgressHUD.MaskType.Black);
        }

        public void HideProgress()
        {
            BigTed.BTProgressHUD.Dismiss();
        }
    }
}