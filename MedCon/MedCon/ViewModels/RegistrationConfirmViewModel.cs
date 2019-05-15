using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using MedCon.LocalDB;
using MedCon.Models;
using MedCon.Services;
using MedCon.Services.Base;
using MedCon.Services.Interfaces;
using MedCon.ViewModels.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MedCon.ViewModels
{
   public class RegistrationConfirmViewModel:ViewModelBase
    {
        private readonly IRegistrationService _registrationService;

        private static RegistrationInput registrationInput;
        public ICommand RegistrationConfirmCommand { get; set; }
        string username,_code;
        bool IsUserConfirmed;
        public string AccessCode { get { return _code; } set { _code = value; OnPropertyChanged("AccessCode"); } }
        public RegistrationConfirmViewModel(IRegistrationService registrationService)
        {
            _registrationService = registrationService;

            RegistrationConfirmCommand = new Command(GotoDashboard);
            IsBusy = true;
        }
       async void GotoDashboard()
        {
            if(string.IsNullOrEmpty(AccessCode))
            {
                DialogProvider.DisplayNativeAlert("Please enter verification code", "OK");
                return;
            }
            try
            {
                DialogProvider.ShowProgress("Submitting...");
                bool IsSuccess=false;
                if(!IsUserConfirmed)
                {
                    IsSuccess = await VerifyAccessCode(username, AccessCode);
                    if (IsSuccess) IsUserConfirmed = true;
                }                
                if (IsUserConfirmed)
                {
                    DialogProvider.ShowProgress("Submitting...");
                    IsBusy = false;
                    await Task.Run(async () =>
                     {
                       
                        string token = await _registrationService.GetLoginToken(registrationInput.emailAddress, registrationInput.Password);
                         var response = await _registrationService.RegisterAsync(registrationInput);
                     });
                    MedCon.Helpers.Settings.ProfileName = registrationInput.emailAddress;
                    MedCon.Helpers.Settings.Password = registrationInput.Password;
                bool answer=  await  Application.Current.MainPage.DisplayAlert("Medcon", "Do you want to continue without scanning?", "Yes", "No");
                    if(!answer)
                    {
                        string scanResult = await GetScanResultAsync();
                        if (string.IsNullOrEmpty(scanResult))
                        {
                            DialogProvider.HideProgress();
                            return;
                        }
                        await ValidateContainer(scanResult);
                    }
                   else
                    {
                        Constants.PresentPatientId = MedCon.Helpers.Settings.PatientId ="";
                        await NavigationService.NavigateToAsync<DashboardViewModel>();
                    }
                    // await NavigationService.NavigateToAsync<DashboardViewModel>();
                }
            }
            catch (ServiceAuthenticationException ex)
            {
                JObject jObject = JObject.Parse(ex.Content);
                DialogProvider.DisplayNativeAlert(jObject["message"].ToString(), "MedCon");
              await  _registrationService.DeleteCognitoUser();
            }  
            catch(Exception ex)
            {
                DialogProvider.DisplayNativeAlert(ex.Message, "OK");
                await _registrationService.DeleteCognitoUser();
            }
            finally
            {
                IsBusy = true;
                DialogProvider.HideProgress();
            }
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
                  //  await GotoDashboard(containerId);
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
            return scanResult;
        }
        async Task GotoDashboard(string contrinaerId)
        {           
            try
            {
                DialogProvider.ShowProgress();
                JObject jobjProfileResponse = await requestProvider.GetAsync<JObject>(Constants.ApiBase + "user/patient/profile");
                Profile profile = JsonConvert.DeserializeObject<Profile>(jobjProfileResponse.ToString());
                JObject jobjMapPatientToContainer = new JObject();
                jobjMapPatientToContainer.Add("containerId", contrinaerId);
                jobjMapPatientToContainer.Add("patientId", null);
                jobjMapPatientToContainer.Add("sub", profile.sub);
                JObject jObjecRegimen = await requestProvider.PostAsync<JObject, JObject>(Constants.ContainerApiBase + "container/patient/mobile", jobjMapPatientToContainer);
                if (jObjecRegimen != null)
                {
                    new SqliteService().InsertStaticData(InsertStaticData());
                   // new DoseRemainderService().StartDoseRemainderService(60);
                }
                await NavigationService.NavigateToAsync<DashboardViewModel>();
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
        private RegimenRoot InsertStaticData()
        {
            RegimenRoot regimenRoot = new RegimenRoot();
            regimenRoot.containerId = "1_9_21";

            Regimen regimen = new Regimen();
            regimen.afternoon_schedule = "12:30 PM , 2:30 PM";
            regimen.bedtime_schedule = "10:00 PM";
            regimen.day_of_week = null;
            regimen.durationBetweenDoses = 1;
            regimen.evening_schedule = "6:00 PM, 8:00 PM";
            regimen.extraDoses = 1;
            regimen.id = 17;
            regimen.medicationPerDose = 1;
            regimen.monthly_yearly = null;
            regimen.morning_schedule = "7:30 AM, 9:30 AM, 11:00 AM";
            regimen.name = "ABC Pharma Global Regimen";
            regimen.schedule_type = "Daily";
            regimen.thresholdTime = 1;
            regimen.totalDoses = 1;

            Period period = new Period();
            period.endDate = "2018-03-31";
            period.startDate = "2018-01-01";

            Drug drug = new Drug();
            drug.alias = "Emdopar";
            drug.amount = "250";
            drug.drugname = "ABC Pharma Global  Emdopa Trial";
            drug.id = 18;
            drug.image = "1_2b0ec320-b4bf-46cf-9070-cd4352d80fb7.png";
            drug.medicationtype = "Tablet";
            drug.unit = "MG";

            Models.Trial trial1 = new Models.Trial();
            trial1.endDate = "2018-03-31";
            trial1.id = 9;
            trial1.manualdose = "N";
            trial1.name = "Biocon DrugCodeX Trail";
            trial1.startDate = "2018-01-01";

            Alertmessage alertmessage = new Alertmessage();
            alertmessage.ontimedosetext = "Your next dose {{drug_name}} of size {{dose_amount}} {{dose_unit}} {{total_dose}} in threshold";
            alertmessage.predosetext = "Your need to take {{drug_name}} of size {{dose_amount}} {{dose_unit}} {{total_dose}}";
            alertmessage.predosetime = 15;
            alertmessage.thresholddosetext = "Your threshold window to take {{drug_name}} is nearing";
            alertmessage.thresholddosetime = 15;
            alertmessage.thresholdduetime = 15;

            regimenRoot.regimen = regimen;
            regimenRoot.alertmessage = alertmessage;
            regimenRoot.drug = drug;
            regimenRoot.period = period;
            regimenRoot.trial = trial1;
            return regimenRoot;

        }
        public override Task InitializeAsync(object navigationData)
        {
           registrationInput = (RegistrationInput)navigationData;
            
            username = registrationInput.emailAddress;
            return base.InitializeAsync(navigationData);
        }
        private async Task<bool> VerifyAccessCode(string username, string code)
        {
            DialogProvider.ShowProgress();
            AmazonCognitoIdentityProviderClient provider =
                new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(),Constants.CognitoIdentityRegion);
            ConfirmSignUpRequest confirmSignUpRequest = new ConfirmSignUpRequest();
            confirmSignUpRequest.Username = username;
            confirmSignUpRequest.ConfirmationCode = code;
            confirmSignUpRequest.ClientId = Constants.CognitoClientId;
            try
            {
                var res = await provider.ConfirmSignUpAsync(confirmSignUpRequest);
                if (res.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return true;
            }
            catch (Exception ex)
            {
                DialogProvider.DisplayNativeAlert(ex.Message, "OK");
                return false;
            }
            finally
            {
                DialogProvider.HideProgress();
            }
            return true;

        }
       
    }
}
