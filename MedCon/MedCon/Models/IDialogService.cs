using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Interfaces
{
    public interface IDialogService
    {
        void ShowLongToast(string message);
        void ShowShortToast(string message);

        void DisplayNativeAlert(string message,string title);
        bool ConfirmAlert(string message, string yes, string no,string title);

        /// <summary>
        /// This method uses dependancy service to access native progress dialog in android and BTProgressHUD in ios
        /// </summary>
        /// <param name="message"></param>
        void ShowProgress(string message = "");

        /// <summary>
        /// This method uses dependancy service to hide the native progress dialog in android and BTProgressHUD in ios
        /// </summary>
        void HideProgress();

    }
}
