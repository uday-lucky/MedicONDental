using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedCon.ViewModels.Base;
using System.Windows.Input;
using Xamarin.Forms;
using MedCon.Services;

namespace MedCon.ViewModels
{
   public class SetPasswordViewModel: ViewModelBase
    {
        string _password, _cfmPassword,_code;
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged("Password"); } }
        public string ConfirmPassword { get { return _cfmPassword; } set { _cfmPassword = value; OnPropertyChanged("ConfirmPassword"); } }
        public string VerificationCode { get { return _code; } set { _code = value; OnPropertyChanged("VerificationCode"); } }
        public ICommand SetPasswordCommand { get; set; }
        string username;
        public SetPasswordViewModel()
        {
            SetPasswordCommand = new Command(SetPassword);
        }
        private async void SetPassword()
        {
            try
            {
                DialogProvider.ShowProgress();
                CognitoAWSCredentialsService credentialsService = new CognitoAWSCredentialsService(null);
                var result = await credentialsService.Pool.Client.ConfirmForgotPasswordAsync(new Amazon.CognitoIdentityProvider.Model.ConfirmForgotPasswordRequest()
                {
                    ClientId = Constants.CognitoClientId,
                    Username = username,
                    ConfirmationCode = VerificationCode,
                    Password = Password,
                });
                if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    await NavigationService.NavigateToAsync<LoginViewModel1>();
            }
            catch (Exception ex)
            {
                DialogProvider.DisplayNativeAlert(ex.Message, "OK");                
            }
            finally
            {
                DialogProvider.HideProgress();
            }
        }
        public override Task InitializeAsync(object navigationData)
        {
            username = (string)navigationData;
            return base.InitializeAsync(navigationData);
        }
    }
}
