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
    public class ScanNewContainerViewModel : ViewModelBase
    {
        public ICommand ScanCommand { get; set; }
        public ScanNewContainerViewModel()
        {
            ScanCommand = new Command(ScanNewContainer);

        }
        async void ScanNewContainer()
        {
            try
            {
                string scanResult = await GetScanResultAsync();
                if (!string.IsNullOrWhiteSpace(scanResult))
                {
                    await ValidateContainer(scanResult);
                }
            }
            catch (Exception ex)
            {
                DialogProvider.DisplayNativeAlert(ex.Message, "OK");
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
        }
        private async Task ValidateContainer(string containerId)
        {
            try
            {
                DialogProvider.ShowProgress("Validating...");

                JObject jObject1 = await requestProvider.GetAsync<JObject>(string.Format("{0}container/validate/mobile?containerId={1}", Constants.ContainerApiBase, containerId));
                if (jObject1 != null && jObject1["description"].ToString() == "Valid Container")
                {
                    await NavigationService.NavigateToAsync<ConfirmPatientIDViewModel>(containerId);
                   // GotoDashboard(containerId);
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
        async void GotoDashboard(string containerId)
        {
            JObject jObjecRegimen = new JObject();           
            try
            {
                DialogProvider.ShowProgress("Validating...");
                JObject jobjProfileResponse = await requestProvider.GetAsync<JObject>(Constants.ApiBase + "user/patient/profile");
                Profile profile = JsonConvert.DeserializeObject<Profile>(jobjProfileResponse.ToString());
                JObject jobjMapPatientToContainer = new JObject();
                jobjMapPatientToContainer.Add("containerId", containerId);
                jobjMapPatientToContainer.Add("patientId", MedCon.Helpers.Settings.PatientId);
                jobjMapPatientToContainer.Add("sub", profile.sub);
                jObjecRegimen = await requestProvider.PostAsync<JObject, JObject>(Constants.ContainerApiBase + "container/patient/mobile", jobjMapPatientToContainer);
                if (jObjecRegimen != null)
                {
                    if (!IsAlreadyScanned(containerId))
                    {
                        RegimenRoot regimenData = JsonConvert.DeserializeObject<RegimenRoot>(jObjecRegimen.ToString());

                        await App.Current.SavePropertiesAsync();
                        new SqliteService().InsertStaticData(InsertStaticData(regimenData));
                       // new DoseRemainderService().StartDoseRemainderService(60);
                        await NavigationService.NavigateToAsync<DashboardViewModel>();
                    }
                    else
                        DialogProvider.DisplayNativeAlert("The container is already scanned for this user!", "OK");
                }
            }
            catch (Exception ex)
            {
                if (jObjecRegimen.ToString().Contains("description"))
                {
                    DialogProvider.DisplayNativeAlert(jObjecRegimen["description"].ToString(), "MedCon");
                }
                else
                    DialogProvider.DisplayNativeAlert(ex.Message, "MedCon");
            }
            finally
            {
                DialogProvider.HideProgress();
            }
        }
        bool IsAlreadyScanned(string containerId)
        {
            var regimen = SqliteService.GetRegimen();
            if(regimen!=null&&regimen.Count>0)
            {
                var obj = regimen.Where(x => x.container_id == containerId).FirstOrDefault();
                if (obj != null) return true;
            }
            return false;
        }
        private RegimenRoot InsertStaticData(RegimenRoot regimen1 = null)
        {
            RegimenRoot regimenRoot=new RegimenRoot();
            if (regimen1 != null)
            {
                regimenRoot = regimen1;
                regimenRoot.PatientId = MedCon.Helpers.Settings.PatientId;
            }         
            return regimenRoot;

        }
    }
}
