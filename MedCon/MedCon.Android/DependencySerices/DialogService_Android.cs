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
using MedCon.Droid.DependencyServices;
using MedCon.Interfaces;
using System.Threading.Tasks;

[assembly:Dependency(typeof(DialogService_Android))]
namespace MedCon.Droid.DependencyServices
{
   public class DialogService_Android : IDialogService
    {
        ProgressDialog pd = new ProgressDialog(Forms.Context);
        public DialogService_Android()
        {
            pd.SetCancelable(false);
            pd.Indeterminate = false;
            pd.SetProgressStyle(ProgressDialogStyle.Spinner);

        }

        public bool ConfirmAlert(string message, string yes, string no,string title)
        {
            string result;
            AlertDialog.Builder alert = new AlertDialog.Builder(Forms.Context);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetPositiveButton(yes, (senderAlert, args) => {
                Toast.MakeText(Forms.Context, "Successfully logout!", ToastLength.Short).Show();
                result = "Yes";
            });

            alert.SetNegativeButton(no, (senderAlert, args) => {
                result = "No";
                Toast.MakeText(Forms.Context, "", ToastLength.Short).Show();
            });

            Dialog dialog = alert.Create();
            dialog.Show();
            return true;
        }

        public void DisplayNativeAlert(string message,string title)
        {
            Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(Forms.Context);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("MedCon");
            alert.SetMessage(message);
            alert.SetButton("OK", (c, ev) =>
            {
                // Ok button click task  
            });
            alert.Show();
        }

        public void HideProgress()
        {
            pd.Hide();

        }

        public void ShowLongToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShowProgress(string message = "Loading...")
        {
            pd.SetMessage(message);
            pd.Show();
        }

        public void ShowShortToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}