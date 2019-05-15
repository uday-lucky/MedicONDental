using MedCon.Interfaces;
using MedCon.Models;
using MedCon.ViewModels.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.ViewModels
{
   public class HistoryViewModel:ViewModelBase
    {
        public ObservableCollection<HistoryModel> History { get; set; }
        public List<RegimenRoot> Regimens { get; set; }

        public HistoryViewModel()
        {
            IsBusy = true;
            History = new ObservableCollection<HistoryModel>();
            //History.Add(new HistoryModel {Name="Trail1",StartDate="10/20/2017",EndDate="11/21/2017",Status="Active",BackColor= "#f5f5f5",GotoHistoryDetailsCommand=new Command<HistoryModel>(GotoDetails) });
            //History.Add(new HistoryModel { Name = "Trail2", StartDate = "09/13/2017", EndDate = "10/29/2017", Status = "Inactive", BackColor = "#ffffff", GotoHistoryDetailsCommand = new Command<HistoryModel>(GotoDetails) });
            //History.Add(new HistoryModel { Name = "Trail3", StartDate = "08/21/2017", EndDate = "09/12/2017", Status = "Inactive", BackColor = "#f5f5f5", GotoHistoryDetailsCommand = new Command<HistoryModel>(GotoDetails) });
            //History.Add(new HistoryModel { Name = "Trail4", StartDate = "07/04/2017", EndDate = "08/22/2017", Status = "Active", BackColor = "#ffffff", GotoHistoryDetailsCommand = new Command<HistoryModel>(GotoDetails) });
            //History.Add(new HistoryModel { Name = "Trail5", StartDate = "06/02/2017", EndDate = "07/03/2017", Status = "Active", BackColor = "#f5f5f5", GotoHistoryDetailsCommand = new Command<HistoryModel>(GotoDetails) });
           
        }
        public override Task InitializeAsync(object navigationData)
        {            
           LoadHistory();
            return base.InitializeAsync(navigationData);
        }
        async void LoadHistory()
        {
            try
            {
                Regimens = new List<RegimenRoot>();
                int count = 0;
                List<PatientIdsData> jarry2 = await requestProvider.GetAsync<List<PatientIdsData>>(Constants.ApiBase + "user/patient/attached");
                foreach (var item in jarry2)
                {
                    count++;
                    HistoryModel historyModel = new HistoryModel();
                    JObject jarry1 = await requestProvider.GetAsync<JObject>(Constants.ContainerApiBase + "/container/info?patientnum=" + item.Patient);
                    RegimenRoot regimenRoot = JsonConvert.DeserializeObject<RegimenRoot>(jarry1.ToString());
                    historyModel.StartDate = DateTime.ParseExact(regimenRoot.period.startDate, "yyyy-MM-dd", null).ToString(GlobalSettings.MedConDateFormat);
                    historyModel.EndDate = DateTime.ParseExact(regimenRoot.period.endDate, "yyyy-MM-dd", null).ToString(GlobalSettings.MedConDateFormat);
                    historyModel.Name = regimenRoot.trial.name;
                    historyModel.PatientNum = item.Patient;
                    historyModel.ContainerNum = item.PatientContainer;
                    historyModel.TrialId = regimenRoot.trial.id;
                    Regimens.Add(regimenRoot);
                    DateTime date = DateTime.Parse(regimenRoot.period.endDate);
                    if (date > DateTime.Now)
                        historyModel.Status = "Active";
                    else
                        historyModel.Status = "Inactive";
                    if(count%2==0)
                    historyModel.BackColor = "#ffffff";
                    else
                        historyModel.BackColor = "#f5f5f5";
                    historyModel.GotoHistoryDetailsCommand =new Command<HistoryModel>(GotoDetails);
                    History.Add(historyModel);

                  
                }
            }
            catch (Exception ex)
            {
                DialogProvider.DisplayNativeAlert(ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async void GotoDetails(HistoryModel history)
        {
            try
            {
                DialogProvider.ShowProgress();
                List<RegimenHistory> jobj10 = await requestProvider.GetAsync<List<RegimenHistory>>(string.Format(Constants.HistoryApiBase + "dose/list?patientnum={0}&containerId={1}", history.PatientNum, history.ContainerNum));
                if(jobj10.Count==0)
                {
                    DialogProvider.DisplayNativeAlert("No History Found for this trial!", "MedCon");
                    return;
                }
                HistoryRoot historyRoot = new HistoryRoot();
              var obj=  Regimens.Where(x => x.trial.id == history.TrialId).ToList();
                if (obj != null)
                    historyRoot.Regimen = obj;                
                historyRoot.History = history;
                historyRoot.Data = jobj10;
                await NavigationService.NavigateToAsync<HistoryDetailsViewModel>(historyRoot);

            }
            catch (Exception)
            {

            }
            finally
            {
                DialogProvider.HideProgress();
            }
        }
    }
    public class HistoryRoot
    {
        public HistoryModel History { get; set; }
        public List<RegimenRoot> Regimen { get; set; }

        public List<RegimenHistory> Data { get; set; }
    }
}
