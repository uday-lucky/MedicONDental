using awscognito;
using awscognito.Cognito.Continuations;
using MedCon.Models;
using MedCon.Services.Base;
using MedCon.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Services
{
    public class RegistrationService : IRegistrationService
    {
        CognitoAWSCredentialsService credentialsService;
        private readonly IRequestProvider _requestProvider;

        public RegistrationService(IRequestProvider requestProvider)
        {
            credentialsService = new CognitoAWSCredentialsService(null);
            _requestProvider = requestProvider;
        }
        public async Task<JObject> RegisterAsync(RegistrationInput registrationInput)
        {
            string input = JsonConvert.SerializeObject(registrationInput);
            JObject jobjInput = JObject.Parse(input);
            return await _requestProvider.PostAsync<JObject,JObject>(Constants.ApiBase+"user/patient", jobjInput);
        }
        public async Task<string> GetLoginToken(string username,string password)
        {
            string token=null;
            var details = new AuthenticationDetails(username, password, null);
            await new CognitoUser(details.getUserId(), credentialsService.Pool, null)
                .initiateUserAuthentication(details)
                .ContinueWith(async continuation =>
                {
                    if (continuation.IsFaulted)
                    {
                        
                    }
                    else if (continuation.IsCanceled)
                    {
                       
                    }
                    else
                    {
                        AuthenticationResult result = continuation.Result;
                        if (result.CognitoUserSession != null)
                        {
                            MedCon.Helpers.Settings.IsLoggedIn = true;
                            MedCon.Helpers.Settings.ProfileName = username;
                            MedCon.Helpers.Settings.Token =result.CognitoUserSession.IdToken.Token;
                            token= result.CognitoUserSession.IdToken.Token; ;
                        }
                    }
                });
            return await Task.Run(() => token);
        }

        public async Task DeleteCognitoUser()
        {
            try
            {
                var response = await App.CurrentUser.Pool.Client.DeleteUserAsync(new Amazon.CognitoIdentityProvider.Model.DeleteUserRequest
                {
                    AccessToken = Helpers.Settings.AccessToken
                });
            }
            catch (Exception)
            {

            }
        }
    }
}
