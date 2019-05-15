using MedCon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.Services
{
    public class DialogServices : IDialogService
    {
        IDialogService dependency;
        public DialogServices()
        {
            dependency = DependencyService.Get<IDialogService>();
        }
        public bool ConfirmAlert(string message, string yes, string no, string title)
        {
          return  dependency.ConfirmAlert(message, yes, no, title);

        }

        public void DisplayNativeAlert(string message, string Yes)
        {
                dependency.DisplayNativeAlert(message, Yes);           
        }     

        public void HideProgress()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                dependency.HideProgress();
            });
        }       

        public void ShowLongToast(string message)
        {
            dependency.ShowLongToast(message);
        }

        public void ShowProgress(string message = "Loading...")
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                dependency.ShowProgress(message);
            });
        }

        public void ShowShortToast(string message)
        {
            dependency.ShowShortToast(message);
        }
    }
}
