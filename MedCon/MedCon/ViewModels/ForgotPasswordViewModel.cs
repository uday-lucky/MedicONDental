using Amazon.CognitoIdentityProvider;
using awscognito;
using MedCon.Services;
using MedCon.Utilities;
using MedCon.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MedCon.ViewModels
{
   public class ForgotPasswordViewModel:ViewModelBase
    {
        string _email, _mobile,_temporaryPwd,_newPwd,_cfmPwd;
        public string EmailID { get { return _email; } set { _email = value; OnPropertyChanged("EmailID"); } }
        public string Mobile { get { return _mobile; } set { _mobile = value; OnPropertyChanged("Mobile"); } }
        public string TemporaryPassword { get { return _temporaryPwd; } set { _temporaryPwd = value; OnPropertyChanged("TemporaryPassword"); } }
        public string NewPassword { get { return _newPwd; } set { _newPwd = value; OnPropertyChanged("NewPassword"); } }
        public string ConfirmPassword { get { return _cfmPwd; } set { _cfmPwd = value; OnPropertyChanged("ConfirmPassword"); } }

        public ICommand SendPasswordCommand { get; set; }
        public ForgotPasswordViewModel()
        {
            SendPasswordCommand = new Command(async () => await GeneratePassword());
        }
        async void SendPassword()
        {
            if(string.IsNullOrEmpty(TemporaryPassword))
            {
                DialogProvider.DisplayNativeAlert("Please enter temporary password", "OK");
                return;
            }
            if (string.IsNullOrEmpty(NewPassword))
            {
                DialogProvider.DisplayNativeAlert("Please enter new password", "OK");
                return;
            }
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                DialogProvider.DisplayNativeAlert("Please enter confirm password", "OK");
                return;
            }
            if (NewPassword!=ConfirmPassword)
            {
                DialogProvider.DisplayNativeAlert("New password and Confirm password should match", "OK");
                return;
            }
            try
            {
                var response = await App.CurrentUser.Pool.Client.ChangePasswordAsync(new Amazon.CognitoIdentityProvider.Model.ChangePasswordRequest
                {
                    AccessToken = Helpers.Settings.AccessToken,
                    PreviousPassword = TemporaryPassword,
                    ProposedPassword = ConfirmPassword,
                });
                DialogProvider.DisplayNativeAlert("Password reset successful", "OK");
               await NavigationService.NavigateToAsync<LoginViewModel1>();
            }
            catch (Exception ex)
            {
                DialogProvider.DisplayNativeAlert(ex.Message, "OK");
            }
           
            //await GeneratePassword(EmailID);
        }
        private async Task<CognitoUser> ResetPassword(string username)
        {
            AmazonCognitoIdentityProviderClient provider =
                   new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());

            //CognitoUserPool userPool = new CognitoUserPool(this.POOL_ID, this.CLIENTAPP_ID, provider);

            //CognitoUser user = new CognitoUser(username, this.CLIENTAPP_ID, userPool, provider);
            //await user.ForgotPasswordAsync();
            //return user;
            return null;
        }
        private async Task GeneratePassword()
        {
            string _username = string.Empty;
            try
            {
                if(string.IsNullOrWhiteSpace(EmailID)&&string.IsNullOrWhiteSpace(Mobile))
                {
                    DialogProvider.DisplayNativeAlert("Please enter email id OR mobile number", "OK");
                    return;
                }
                if (!string.IsNullOrWhiteSpace(EmailID))
                    _username = EmailID;
                else
                    _username = EmailID;
                DialogProvider.ShowProgress();
                CognitoAWSCredentialsService credentialsService = new CognitoAWSCredentialsService(null);
               
                var result = await credentialsService.Pool.Client.ForgotPasswordAsync(new Amazon.CognitoIdentityProvider.Model.ForgotPasswordRequest()
                {
                    Username =_username,
                    ClientId = Constants.CognitoClientId
                });
                if(result.HttpStatusCode==System.Net.HttpStatusCode.OK)
                {
                    await NavigationService.NavigateToAsync<SetPasswordViewModel>(_username);
                }
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
        private async Task<CognitoUser> UpdatePassword(string username, string code, string newpassword)
        {
            AmazonCognitoIdentityProviderClient provider =
                   new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials());
            return null;
            //CognitoUserPool userPool = new CognitoUserPool(this.POOL_ID, this.CLIENTAPP_ID, provider);

            //CognitoUser user = new CognitoUser(username, this.CLIENTAPP_ID, userPool, provider);
            //await user.ConfirmForgotPasswordAsync(code, newpassword);
            //return user;
        }

    }
}
