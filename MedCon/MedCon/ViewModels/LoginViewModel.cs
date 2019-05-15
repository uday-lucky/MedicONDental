using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using MedCon.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using System.Globalization;
using awscognito;
using awscognito.Cognito.Continuations;
using System.Threading.Tasks;
using Amazon.CognitoIdentity;
using MedCon.Services;
using MedCon.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ZXing.Net.Mobile.Forms;
using MedCon.CustomControls;
using MedCon.LocalDB;
using System.Linq;
using MedCon.Services.Interfaces;

namespace MedCon.ViewModels
{
    public class LoginViewModel1:ViewModelBase
    {
        ZXingScannerPage scanPage;

        ISecureStorage storage;
        CognitoUser cognitoUser;
        private string _username, _password;
        public ICommand RegistrationCommand { get; set; }
        public ICommand ForgotPwdCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public string LoginDescription { get; set; }="Medcon, a proven Patient Adherence Solution app that help you self-manage your medication";
        public string Username { get { return _username; } set { _username = value; OnPropertyChanged("Username"); } }
        public String Password { get { return _password; } set { _password = value; OnPropertyChanged("Password"); } }

        CognitoAWSCredentials credentials;
        CognitoAWSCredentialsService credentialsService;
        AmazonCognitoIdentityProviderClient providerClient;
        string accessToken = string.Empty;
        private AmazonCognitoIdentityProviderClient _client;
        IDashboardService _dashboardService;
        public LoginViewModel1(IDashboardService dashboardService)
        {
            DialogProvider.ShowProgress("Loading...");

            _dashboardService = dashboardService;
            storage = DependencyService.Get<ISecureStorage>();
            credentialsService = new CognitoAWSCredentialsService(null);
            RegistrationCommand = new Command(GotoRegistration);
            ForgotPwdCommand = new Command(GotoForgotPassword);
            LoginCommand = new Command(DoLogin);
            DialogProvider.HideProgress();
        }
        private async void DoLogin()
        {
            try
            {
              //  string dssdds = await GetScanResultAsync();

                DialogProvider.ShowProgress("Authenticating...");

                if (string.IsNullOrEmpty(Username))
                {
                    DialogProvider.DisplayNativeAlert("Please enter valid username", "OK");
                    return;
                }
                if(string.IsNullOrEmpty(Password))
                {
                    DialogProvider.DisplayNativeAlert("Please enter password", "OK");
                    return;
                }
               await Task.Run(async () =>
                {
                    await login(Username, Password);
                });
              
            }
            catch (Exception ex)
            {
                DialogProvider.DisplayNativeAlert(ex.Message, "OK");
            }
            finally
            {
                DialogProvider.HideProgress();
            }
            //Username = "52_" + Username;
            //try
            //{
            //    AuthenticationHelper authenticationHelper = new AuthenticationHelper(Constants.CognitoUserPoolId);
            //    DialogProvider.ShowProgress("Authenticating...");
            //    _client = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), Amazon.RegionEndpoint.USEast1);
            //    var authReq = new InitiateAuthRequest()
            //    {
            //        ClientId = Constants.CognitoClientId,
            //        AuthFlow = AuthFlowType.USER_SRP_AUTH,
            //        AuthParameters = new Dictionary<string, string>() {
            //        { "USERNAME", Username},
            //        { "SRP_A", authenticationHelper.A.ToString(16)},
            //        { "SECRET_HASH", null },
            //        {"DEVICE_KEY",null }
            //    }
            //};
            //    var initiateAuthResult = await _client.InitiateAuthAsync(authReq);
            //    if (initiateAuthResult.ChallengeName == ChallengeNameType.PASSWORD_VERIFIER)
            //    {
            //        RespondToAuthChallengeRequest challengeRequest = userSrpAuthRequest(initiateAuthResult,new AuthenticationHelper(Constants.CognitoUserPoolId));
            //     var response= await _client.RespondToAuthChallengeAsync(challengeRequest);
            //    }


            //  //  AdminInitiateAuthResponse authResp = await _client(authReq);

            //}
            //catch (Exception ex)
            //{
            //    DialogProvider.DisplayNativeAlert("Something not correct.Please try again!", "OK");
            //}
            //finally
            //{
            //    DialogProvider.HideProgress();
            //}
        }

        private RespondToAuthChallengeRequest userSrpAuthRequest(InitiateAuthResponse challenge, AuthenticationHelper authenticationHelper)
        {
            string usernameInternal = Username;
            string deviceKey = null;
           string secretHash = null;

            DateTime timestamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Utc);
            //The timestamp format returned to AWS _needs_ to be in US Culture
            CultureInfo usCulture = new CultureInfo("en-US");
            String timeStr = timestamp.ToString("ddd MMM d HH:mm:ss \"UTC\" yyyy", usCulture);

            //Do the hard work to generate the claim we return to AWS
            byte[] claim = authenticationHelper.authenticateUser(
                challenge.ChallengeParameters["USERNAME"],
                Password,
                challenge.ChallengeParameters["SALT"],
                challenge.ChallengeParameters["SRP_B"],
                challenge.ChallengeParameters["SECRET_BLOCK"],
                timeStr
            );

            String claimBase64 = System.Convert.ToBase64String(claim);

            //Our response to AWS. If successful it will return an object with Tokens,
            //if unsuccessful, it will throw an Exception that you should catch and handle.

            var ChallengeResponses = new Dictionary<string, string>() {
                { "PASSWORD_CLAIM_SECRET_BLOCK", challenge.ChallengeParameters["SECRET_BLOCK"] },
                { "PASSWORD_CLAIM_SIGNATURE", claimBase64 },
                { "USERNAME", usernameInternal },
                { "TIMESTAMP", timeStr },
                { "DEVICE_KEY", deviceKey },
                { "SECRET_HASH", secretHash }
            };

            return new RespondToAuthChallengeRequest
            {
                ChallengeName = "PASSWORD_VERIFIER",
                ClientId = Constants.CognitoClientId,
                ChallengeResponses = ChallengeResponses,
                Session = null
            };
        }

        private async void GotoForgotPassword()
        {
            await NavigationService.NavigateToAsync<ForgotPasswordViewModel>();
        }
        private async void GotoRegistration()
        {
            try
            {
                DialogProvider.ShowProgress();
                CountriesAndRaces countriesAndRaces = new CountriesAndRaces();
                JArray jobjCountries = await requestProvider.GetAsync<JArray>(Constants.GetCountriesListUrl);
                countriesAndRaces.CountriesList = JsonConvert.DeserializeObject<List<Country>>(jobjCountries.ToString());
                JArray jobRaces = await requestProvider.GetAsync<JArray>(Constants.GetRacesList);
                countriesAndRaces.RacesList = JsonConvert.DeserializeObject<List<Models.Race>>(jobRaces.ToString());
                if(countriesAndRaces.CountriesList.Count>0)
                {
                    foreach (var item in countriesAndRaces.CountriesList)
                    {
                        item.NameWithCode =string.Format("(+{0}) {1}", item.phonecode, item.name);
                    }
                }
                await NavigationService.NavigateToAsync<RegistrationViewModel>(countriesAndRaces);
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
        private async Task login(string username, string pwd)
        {
          //  try
          //  {
          //AdminInitiateAuthRequest adminInitiateAuthRequest=  new AdminInitiateAuthRequest();
          //  adminInitiateAuthRequest.AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH;
          //  adminInitiateAuthRequest.AuthParameters.Add("REFRESH_TOKEN", MedCon.Helpers.Settings.RefreshToken);
          //  adminInitiateAuthRequest.ClientId = Constants.CognitoClientId;
          //  adminInitiateAuthRequest.UserPoolId = Constants.CognitoUserPoolId;   
          //  AmazonCognitoIdentityProviderClient client = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), Constants.CognitoIdentityRegion);
          //  var dsfdsfds=  await client.AdminInitiateAuthAsync(adminInitiateAuthRequest);
       
          //  }
          //  catch (Exception ex)
          //  {

                
          //  }
            try
            {
            var details = new AuthenticationDetails(username, pwd, null);
                cognitoUser = new CognitoUser(details.getUserId(), credentialsService.Pool, storage);
                App.CurrentUser = cognitoUser;
               await cognitoUser.initiateUserAuthentication(details)
                .ContinueWith(async continuation =>
                {
                    if (continuation.IsFaulted)
                    {
                        string reason = continuation.Exception.InnerException.Message;
                        if (reason == "User does not exist." || reason == "Incorrect username or password.")
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                DialogProvider.DisplayNativeAlert("Incorrect username/password", "OK");
                            });
                            //ViewModel.Error = "Incorrect username/password";
                        }
                        else if(reason== "User is not confirmed.")
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await NavigationService.NavigateToAsync<RegistrationConfirmViewModel>(Username);
                            });
                        }
                        else
                        {
                            DialogProvider.DisplayNativeAlert(reason, "OK");
                            // ViewModel.Error = reason;
                        }
                    }
                    else if (continuation.IsCanceled)
                    {
                        //Console.WriteLine("IsCanceled");
                    }
                    else
                    {
                        AuthenticationResult result = continuation.Result;
                        if (result.CognitoUserSession != null)
                        {
                           //var session=await cognitoUser.getSession();
                            MedCon.Helpers.Settings.ProfileName = Username;
                            MedCon.Helpers.Settings.Password = Password;
                            MedCon.Helpers.Settings.Token =accessToken =  result.CognitoUserSession.IdToken.Token;
                            MedCon.Helpers.Settings.RefreshToken = result.CognitoUserSession.RefreshToken.Token;
                            MedCon.Helpers.Settings.AccessToken = result.CognitoUserSession.AccessToken.Token;
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                bool isUserdataExist = await IsUserDataExists();
                                if(!isUserdataExist)
                                {
                                    string scanResult = await GetScanResultAsync();
                                    if (string.IsNullOrEmpty(scanResult))
                                    {
                                        DialogProvider.HideProgress();
                                        return;
                                    }
                                    //string scanResult = "1_9_21_6";
                                    await ValidateContainer(scanResult);
                                }
                                 else
                                {
                                    MedCon.Helpers.Settings.IsLoggedIn = true;
                                    await NavigationService.NavigateToAsync<DashboardViewModel>();

                                }

                                //   await NavigationService.NavigateToAsync<DashboardViewModel>(accessToken);
                            });
                            // Login success
                            //SetResult(Result.Ok, Intent);
                            //Finish();
                        }
                        else if (result.ChallengeContinuation != null)
                        {
                            if (ChallengeNameType.NEW_PASSWORD_REQUIRED == result.ChallengeContinuation.GetChallengeName())
                            {
                                var passwordContinuation = (NewPasswordContinuation)result.ChallengeContinuation;
                            }
                            else
                            {
                                //ViewModel.Error = $"Unhandled challenge {result.ChallengeContinuation.GetChallengeName()}";
                            }
                        }
                    }
                });
            }
            catch (Exception)
            {

             
            }
        }
        private async Task<bool> IsUserDataExists()
        {
            try
            {
                DialogProvider.ShowProgress("Checking for trials...");
                List<PatientIdsData> patientIds=await _dashboardService.GetTrials();
                if (patientIds.Count > 0)
                    return true;
                //List<PatientIdsData> jarry = await requestProvider.GetAsync<List<PatientIdsData>>(Constants.ApiBase + "user/patient/attached");
                //if(jarry!=null&&jarry.Count>0)
                //{
                //  var trials=  SqliteService.GetTrials();
                //    if (trials != null && trials.Count>0)
                //    {
                //        foreach (var item in jarry)
                //        {
                //            var obj = trials.Where(x => x.PatientId == item.Patient).FirstOrDefault();
                //            if (obj != null)
                //            {
                //              MedCon.Helpers.Settings.PatientId= Constants.PresentPatientId = item.Patient;
                //                return true;
                //            }
                //        }
                       
                //    }
                //}
            }
            catch (Exception)
            {
               
                return false;
            }
            finally
            {
                DialogProvider.HideProgress();
            }
            return false;
        }
        private async Task ValidateContainer(string containerId)
        {
            try
            {
                DialogProvider.ShowProgress();
                JObject jObject1 = await requestProvider.GetAsync<JObject>(string.Format("{0}container/validate/mobile?containerId={1}", Constants.ContainerApiBase, containerId));
                if (jObject1 != null && jObject1["description"].ToString() == "Valid Container")
                {
                    await NavigationService.NavigateToAsync<ConfirmPatientIDViewModel>(containerId);
                }
                else
                    DialogProvider.DisplayNativeAlert(jObject1["description"].ToString(), "MedCon");
            }
            catch (Exception ex)
            {
                DialogProvider.DisplayNativeAlert(ex.Message, "MedCon");
            }
            finally
            {
                DialogProvider.HideProgress();
            }
        }
        private async Task<string> GetScanResultAsync()
        {
            string scanResult = string.Empty;
            //try
            //{

            //    // Pass in the custom overlay
            //    scanPage = new ZXingScannerPage(customOverlay: new CustomOverlayView());
            //    scanPage.OnScanResult += (result) =>
            //    {
            //        scanPage.IsScanning = false;

            //        Device.BeginInvokeOnMainThread(() =>
            //        {
            //            App.Current.MainPage.Navigation.PopAsync();
            //            scanResult = result.Text;
            //        });
            //    };

            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        App.Current.MainPage.Navigation.PushAsync(scanPage);
            //    });
            //    // Navigation.PushAsync(new ImageButtonDemoPage());
            //    return scanResult;
            //}
            //catch (Exception ex)
            //{
            //    return scanResult;
            //}
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner() { TopText = "Please scan the container barcode", CancelButtonText = "Enter Manually" };
                var result = await scanner.Scan();
                return scanResult = result.Text;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public string GetAccessToken()
        {
            return accessToken;
        }
    }
}
