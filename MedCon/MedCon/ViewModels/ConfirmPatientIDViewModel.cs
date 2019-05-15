using MedCon.LocalDB;
using MedCon.Models;
using MedCon.Services;
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
   public class ConfirmPatientIDViewModel:ViewModelBase
    {
        string _patientId,containerid;
        public string PatientID { get { return _patientId; } set { _patientId = value; OnPropertyChanged("PatientID"); } }
        public ICommand ConfirmPatientIDCommand { get; set; }
        public ConfirmPatientIDViewModel()
        {
            ConfirmPatientIDCommand = new Command(GotoDashboard);
        }
        async void GotoDashboard()
        {
            JObject jObjecRegimen = new JObject(); ;
            if (string.IsNullOrEmpty(PatientID))
            {
                DialogProvider.DisplayNativeAlert("Please enter Patient ID", "OK");
                return;
            }
            try
            {
                DialogProvider.ShowProgress();
                JObject jobjProfileResponse = await requestProvider.GetAsync<JObject>(Constants.ApiBase + "user/patient/profile");
                Profile profile = JsonConvert.DeserializeObject<Profile>(jobjProfileResponse.ToString());
                JObject jobjMapPatientToContainer = new JObject();
                jobjMapPatientToContainer.Add("containerId", containerid);
                jobjMapPatientToContainer.Add("patientId", PatientID);
                jobjMapPatientToContainer.Add("sub", profile.sub);
                jObjecRegimen = await requestProvider.PostAsync<JObject, JObject>(Constants.ContainerApiBase + "container/patient/mobile", jobjMapPatientToContainer);
                if (jObjecRegimen != null)
                {
                    
                    RegimenRoot regimenData = JsonConvert.DeserializeObject<RegimenRoot>(jObjecRegimen.ToString());
                    if(!string.IsNullOrEmpty(regimenData.description))
                    {
                        DialogProvider.DisplayNativeAlert(regimenData.description.ToString(), "OK");
                        return;
                    }
                    await App.Current.SavePropertiesAsync();
                  //  new SqliteService().InsertStaticData(InsertStaticData(regimenData));
                   // new DoseRemainderService().StartDoseRemainderService(60);
                    MedCon.Helpers.Settings.IsLoggedIn = true;
                    Constants.PresentPatientId = MedCon.Helpers.Settings.PatientId = regimenData.PatientId;
                }
                await NavigationService.NavigateToAsync<DashboardViewModel>();
            }
            catch (Exception ex)
            {
                if (jObjecRegimen.ToString().Contains("description"))
                {
                    DialogProvider.DisplayNativeAlert(jObjecRegimen["description"].ToString(), "OK");
                }
                else
                DialogProvider.DisplayNativeAlert(ex.Message,"MedCon");
            }
            finally
            {
                DialogProvider.HideProgress();
            }           
        }
        private RegimenRoot InsertStaticData(RegimenRoot regimen1=null)
        {
            RegimenRoot regimenRoot;
            if (regimen1 != null)
            {
                regimenRoot = regimen1;
                regimenRoot.PatientId = PatientID;
            }              
            else
            {
                regimenRoot = new RegimenRoot();
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
            } 
            return regimenRoot;

        }
        public override Task InitializeAsync(object navigationData)
        {
            containerid = (string)navigationData;
            return base.InitializeAsync(navigationData);
        }
    }
}
