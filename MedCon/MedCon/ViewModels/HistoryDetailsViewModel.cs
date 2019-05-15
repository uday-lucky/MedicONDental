using MedCon.Interfaces;
using MedCon.LocalDB;
using MedCon.LocalDB.Tables;
using MedCon.Models;
using MedCon.ViewModels.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MedCon.ViewModels
{
   public class HistoryDetailsViewModel:ViewModelBase
    {
        string _trialName, _patientId, _startDate, _endDate, _activeStatus;
        public HistoryModel HistoryModel { get; set; }
        public ObservableCollection<History> HistoryRecords { get; set; }
        public string TrialName { get { return _trialName; } set { _trialName = value; OnPropertyChanged("TrialName"); } }
        public string PatientId { get { return _patientId; } set { _patientId = value; OnPropertyChanged("PatientId"); } }
        public string StartDate { get { return _startDate; } set { _startDate = value; OnPropertyChanged("StartDate"); } }
        public string EndDate { get { return _endDate; } set { _endDate = value; OnPropertyChanged("EndDate"); } }
        public string ActiveStatus { get { return _activeStatus; } set { _activeStatus = value; OnPropertyChanged("ActiveStatus"); } }
        public ObservableCollection<HistotyTable> PresentHistory { get; set; }
        public List<HistotyTable> histotyTables { get; set; }

        public HistoryRoot historyRoot { get; set; }
        public HistoryDetailsViewModel()
        {
            histotyTables = new List<HistotyTable>();
            PresentHistory = new ObservableCollection<HistotyTable>();
            InitializeRecords();
        }
        private void InitializeRecords()
        {
            HistoryRecords = new ObservableCollection<History>();
            //HistoryRecords.Add(new History { ContainerDetails = InitializeData(), ContainerColor=Color.Green,BackColor=Color.FromHex("#f5f5f5"),   ContainerName="Container001",DrugName="Drug1",DrugTypeImage="capsule.png",DrugTypeName="Capsule",TotalDose=90,RemainingDose=0,ContainerTappedCommand=new Command<History>(ContainerTapped)});
            //HistoryRecords.Add(new History { ContainerDetails = InitializeData(), ContainerColor=Color.Green,BackColor = Color.FromHex("#ffffff"), ContainerName="Container001",DrugName="Drug1",DrugTypeImage="capsule.png",DrugTypeName="Capsule",TotalDose=90,RemainingDose=0,ContainerTappedCommand=new Command<History>(ContainerTapped)});
            //HistoryRecords.Add(new History { ContainerDetails = InitializeData(), ContainerColor = Color.Green, BackColor = Color.FromHex("#f5f5f5"), ContainerName = "Container001", DrugName = "Drug1", DrugTypeImage = "capsule.png", DrugTypeName = "Capsule", TotalDose = 90, RemainingDose = 0, ContainerTappedCommand = new Command<History>(ContainerTapped) });

        }
        void GetTrialsData()
        {
            try
            {

                    TrialName =historyRoot.Regimen[0].trial.name;
                    PatientId =historyRoot.Regimen[0].patientNum;
                    StartDate = historyRoot.Regimen[0].trial.startDate;
                    EndDate = historyRoot.Regimen[0].trial.endDate;
                   LoadHistoryDetails();          
            }
            catch (Exception ex)
            {

            }            
        }
        void LoadHistoryDetails()
        {
          var HistoryData=  historyRoot.Data;
           // histotyTables = SqliteService.GetHistory();
            //if(histotyTables.Count>0)
            //{
            //    histotyTables=histotyTables.Where(x=>x.TrialId==HistoryModel.TrialId).ToList();

                HistoryRecords.Clear();
                int oldCount = 0;
                //foreach (var item1 in histotyTables)
                //{

                //    oldCount =item1.RemainingDoses;
                //}
                foreach (var item in historyRoot.Regimen)
                {
                    History history = new History
                    {
                        ContainerDetails = InitializeData(),
                        ContainerColor = Color.Green, BackColor = Color.FromHex("#f5f5f5"),
                        ContainerName =item.containerId,
                        DrugName = item.drug.drugname,
                        DrugTypeImage = "capsule.png",
                        DrugTypeName = item.drug.medicationtype,
                        TotalDose =item.regimen.totalDoses,
                        RemainingDose =item.regimen.totalDoses- GetPreviousDoseCount(item.containerId,int.Parse(item.drug.amount)),
                        ContainerTappedCommand = new Command<History>(ContainerTapped)
                    };
                    //var obj = HistoryRecords.Where(a => a.ContainerName == history.ContainerName).FirstOrDefault();
                    //if(obj!=null)
                    //{
                    //    int a = history.TotalDose - history.RemainingDose;
                    //    int b = obj.TotalDose - obj.RemainingDose;
                    //    // history.RemainingDose = history.TotalDose - (a+b);
                    //    history.RemainingDose =oldCount;

                    //    HistoryRecords.Remove(obj);
                    //}
                    HistoryRecords.Add(history);

                    // PresentHistory.Add(item)
                }
           // }
        }
        int GetPreviousDoseCount(string container,int perdose)
        {
            int count = 0;
          var records=  historyRoot.Data.Where(x => x.patientcontainerId.ToString() == container).ToList();
            if(records!=null&&records.Count>0)
            {
                foreach (var item in records)
                {
                    string[] dose = item.doseAmount.Split(' ');
                    int medPerDose = int.Parse(dose[0]) / perdose;
                    count = count + medPerDose;
                }
            }
            return count;
        }
        string GetStr(string container)
        {
            string str = string.Empty;
            var record = historyRoot.Data.Where(x => x.patientcontainerId.ToString() == container).FirstOrDefault();
            if(record!=null)
            {
                var reg = historyRoot.Regimen.Where(a => a.containerId == record.patientcontainerId).FirstOrDefault();
                if(reg!=null)
                {
                    string[] dose = record.doseAmount.Split(' ');
                    int medPerDose = int.Parse(dose[0]) /int.Parse(reg.drug.amount);
                    str = string.Format("{0}x{1} {2}", medPerDose, reg.drug.amount, "MG");
                }
            }
            return str;
        }
        private ObservableCollection<ContainerData> InitializeData()
        {
            ObservableCollection<ContainerData> abc = new ObservableCollection<ContainerData>();
            int count = 0;
            foreach (var item in historyRoot.Data)
            {
                count++;
                var data = new ContainerData
                {
                    LabelBackColor = GetWindowsColors(item.doseWindow),
                    DoseTimeImage = DetermineDoseImage(item.category),
                    DateOrTime = item.time,
                    Str = GetStr(item.patientcontainerId),
                    Total = item.doseAmount,
                    Type = item.category,
                    Win = item.doseWindow.Substring(0,1).ToUpper(),
                    Number = count.ToString(),
                    BackColor1 = Color.FromHex("#ffffff"),
                    Data = HistoryModel
               };
                if (item.manual == 2)
                    data.Entry = "S";
                else
                    data.Entry = "M";
                abc.Add(data);
            }
//abc.Add(new ContainerData {LabelBackColor=Color.FromHex("#fcd75f"), DoseTimeImage="ontime.png", DateOrTime = "12/13/2017,08:40 AM", Str = "2x20 MG", Total = "40 MG", Type = "On-Time", Win = "M", Entry = "s", Number = "10", BackColor1 = Color.FromHex("#ffffff") });
         //   abc.Add(new ContainerData { LabelBackColor = Color.FromHex("#40c9d1"), DoseTimeImage ="missed.png", DateOrTime="12/13/2017,08:40 AM",Str="2x20 MG", Total="40 MG",Type="On-Time",Win="M",Entry="s", Number = "10", BackColor1 = Color.FromHex("#f5f5f5") });
            return abc;
        }
        Color GetWindowsColors(string window)
        {
            Color _color = Color.White;
            switch (window)
            {
                case "morning":
                    _color = (Color)Application.Current.Resources["MorningColor"];
                    break;
                case "afternoon":
                    _color = (Color)Application.Current.Resources["AfternoonColor"];
                    break;
                case "evening":
                    _color = (Color)Application.Current.Resources["EveningColor"];
                    break;
                case "bedtime":
                    _color = (Color)Application.Current.Resources["BedtimeColor"];
                    break;
            }
            return _color;
        }
        string DetermineDoseImage(string window)
        {          
            string doseImage = string.Empty;
            if (window == "missed")
            {
                doseImage = "missed.png";
            }
            else if (window == "late"||window== "overdose")
            {
                doseImage = "overdose.png";
            }            
            else
            {
                doseImage = "ontime.png";
            }
            return doseImage;
        }
        private void ContainerTapped(History history)
        {
            try
            {
                DialogProvider.ShowProgress();
                NavigationService.NavigateToAsync<HistoryDetails2ViewModel>(history.ContainerDetails);
            }
            catch (Exception ex)
            {
                DialogProvider.DisplayNativeAlert(ex.Message, "OK");
            }
            finally
            {
                DialogProvider.HideProgress();
            }
            //if (history.IsContainerDetailsVisible)
            //    history.IsContainerDetailsVisible = false;
            //else
            //    history.IsContainerDetailsVisible = true;
        }
        public override Task InitializeAsync(object navigationData)
        {
             historyRoot = new HistoryRoot();
            historyRoot = (HistoryRoot)navigationData;
            HistoryModel = historyRoot.History;
            GetHistoryDetails(HistoryModel.PatientNum, HistoryModel.ContainerNum);
            GetTrialsData();
            return base.InitializeAsync(navigationData);
        }
        async void GetHistoryDetails(string patientNum,string containerNum)
        {
            JObject response = await requestProvider.GetAsync<JObject>(Constants.ContainerApiBase + "/container/info?patientnum="+patientNum+"&containerId="+containerNum);
        }
    }
    public class History:ViewModelBase
    {
        bool _isContainerDetailsVisible;
        public bool IsContainerDetailsVisible { get { return _isContainerDetailsVisible; } set { _isContainerDetailsVisible = value; OnPropertyChanged("IsContainerDetailsVisible"); } }
        public ICommand ContainerTappedCommand { get; set; }
        public Color ContainerColor { get; set; }
        public string ContainerName { get; set; }
        public string DrugName { get; set; }
        public ImageSource DrugTypeImage { get; set; }
        public string DrugTypeName { get; set; }
        public int TotalDose { get; set; }
        public int RemainingDose { get; set; }
        public Color BackColor { get; set; }
        public ObservableCollection<ContainerData> ContainerDetails { get; set; }
    }
    public class ContainerData
    {
        public HistoryModel Data { get; set; }
        public string DateOrTime { get; set; }
        public Color BackColor1 { get; set; }
        public string  Number { get; set; }
        public string Str { get; set; }
        public string Total { get; set; }
        public string Type { get; set; }
        public string Win { get; set; }
        public string Entry { get; set; }
        public ImageSource DoseTimeImage { get; set; }
        public Color LabelBackColor { get; set; }
        public string DoseTakenTime { get; set; }
        public string Window { get; set; }
        public string ScanType { get; set; }
    }
}
