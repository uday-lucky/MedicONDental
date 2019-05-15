using MedCon.LocalDB;
using MedCon.Models;
using MedCon.Services.Base;
using MedCon.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.Services
{
    public class UpdateContainerService : IUpdateContainerService
    {
        private readonly IRequestProvider _requestProvider;


        public UpdateContainerService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<bool> UpdateContainer(string patientId, string containerId, int trialId, string scanType, string doseType, string time, int extra, int userId,int id,string doseWindows,string doseAmount,int companyId,string doseTime,string doseStatusImg)
        {
          //  string doseStatusImage = ScanConfirmationViewModel.DetermineDoseImage(doseType);
            MedicineItem medicineItem = new MedicineItem();
            medicineItem.ID = id;
            medicineItem.StatusImage = doseStatusImg;
            medicineItem.time1 = DateTime.Now.ToString("hh:mm tt");
            UpdateContainerInput updateContainerInput = new UpdateContainerInput();
            updateContainerInput.containerId = GetContainerID(containerId);
            updateContainerInput.patientcontainerId = containerId;
            updateContainerInput.DoseType = doseType;
            updateContainerInput.doseWindow = doseWindows;
            updateContainerInput.doseAmount =doseAmount;
            updateContainerInput.Extra = extra;
            updateContainerInput.patientId = patientId;
            updateContainerInput.ScanType = scanType;
            updateContainerInput.Time = time;
            updateContainerInput.trailId = trialId;
            updateContainerInput.userId = userId;
            updateContainerInput.companyId = companyId.ToString();
        
        var json = JsonConvert.SerializeObject(updateContainerInput);
            JObject jobjInput = JObject.Parse(json);
            try
            {
                JObject response =await _requestProvider.PostAsync<JObject, JObject>(Constants.UpdateContainerApiBase + "dose/mobile", jobjInput);
                MessagingCenter.Send<UpdateContainerService, MedicineItem>(this, Constants.UpdateContainerKey, medicineItem);

                // SqliteService.UpdateRegimen(containerId,doseTime, doseStatusImage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void UpdateContainerDB(string patientId, string containerId, int trialId, string scanType, string doseType, string time, int extra, int userId)
        {
           
        }
        string GetContainerID(string patientContainer)
        {
            int index = patientContainer.LastIndexOf("_");
            int noOfCharsRemove = patientContainer.Length - (index);
            return patientContainer.Remove((patientContainer.Length- noOfCharsRemove) , noOfCharsRemove);
        }
    }
  
}
