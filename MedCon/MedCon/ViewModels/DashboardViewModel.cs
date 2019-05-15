using MedCon.LocalDB;
using MedCon.LocalDB.Tables;
using MedCon.Models;
using MedCon.Services;
using MedCon.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using Rg.Plugins.Popup.Extensions;
using System.Globalization;
using MedCon.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace MedCon.ViewModels
{
    public class DashboardViewModel:ViewModelBase
    {
        int _trialSelectedIndex;
        Color _presentColor, _footer1Color, _footer2Color, _footer3Color;
        string _dayName, _selectedDate,_dayTimeName,_footer1Text,_footer2Text,_footer3Text, _selectedtrialName;
        int trialId;
        decimal _doseAdherence=0;
        ImageSource _footer1Image, _footer2Image, _footer3Image,_presentDayImage,_doseAdherenceImage;
        List<Footer> _footerlist1, _footerlist2, _footerlist3;
        private string accessToken = string.Empty;
        public ICommand GotoCalendarCommand { get; set; }
        public ObservableCollection<MedicineItem> PresentMedicines { get; set; }
        public decimal DoseAdherence { get { return _doseAdherence; } set { _doseAdherence = value; OnPropertyChanged("DoseAdherence"); } }
        public ImageSource DoseAdherenceImage { get { return _doseAdherenceImage; } set { _doseAdherenceImage = value; OnPropertyChanged("DoseAdherenceImage"); } }
        public ICommand SelectTrialsCommand { get; set; }
        public ObservableCollection<TrialOption> TrialsList { get; set; }
        public ImageSource PresentDayImage { get { return _presentDayImage; } set { _presentDayImage = value;OnPropertyChanged("PresentDayImage"); } }
        public string DayName { get { return _dayName; } set { _dayName = value; OnPropertyChanged("DayName"); } }
        public string DayTimeName { get { return _dayTimeName; } set { _dayTimeName = value; OnPropertyChanged("DayTimeName"); } }
        public string SelectedDate { get { return _selectedDate; } set { _selectedDate = value; OnPropertyChanged("SelectedDate"); } }
        public string Footer1Text { get { return _footer1Text; } set { _footer1Text = value; OnPropertyChanged("Footer1Text"); } }
        public string Footer2Text { get { return _footer2Text; } set { _footer2Text = value; OnPropertyChanged("Footer2Text"); } }
        public string Footer3Text { get { return _footer3Text; } set { _footer3Text = value; OnPropertyChanged("Footer3Text"); } }
        public ImageSource Footer1Image { get {return _footer1Image; } set { _footer1Image = value; OnPropertyChanged("Footer1Image"); } }
        public ImageSource Footer2Image { get { return _footer2Image; } set { _footer2Image = value; OnPropertyChanged("Footer2Image"); } }
        public ImageSource Footer3Image { get { return _footer3Image; } set { _footer3Image = value; OnPropertyChanged("Footer3Image"); } }
        public List<Footer> Footer1List { get { return _footerlist1; } set { _footerlist1 = value; OnPropertyChanged("Footer1List"); } }
        public List<Footer> Footer2List { get { return _footerlist2; } set { _footerlist2 = value; OnPropertyChanged("Footer2List"); } }
        public List<Footer> Footer3List { get { return _footerlist3; } set { _footerlist3 = value; OnPropertyChanged("Footer3List"); } }
        public List<TrialTable> TrialNames { get; set; }
        public int TrialSelectedIndex { get { return _trialSelectedIndex; } set { _trialSelectedIndex = value; OnPropertyChanged("TrialSelectedIndex"); } }
        public string SelectedtrialName { get { return _selectedtrialName; } set { _selectedtrialName = value; OnPropertyChanged("SelectedtrialName"); } }
        public ICommand FooterCommand { get; set; }
        public ICommand TrialSelectedCommand { get; set; }
        public Color PresentColor { get { return _presentColor; } set { _presentColor = value; OnPropertyChanged("PresentColor"); } }
        public Color Footer1Color { get { return _footer1Color; } set { _footer1Color = value; OnPropertyChanged("Footer1Color"); } }
        public Color Footer2Color { get { return _footer2Color; } set { _footer2Color = value; OnPropertyChanged("Footer2Color"); } }
        public Color Footer3Color { get { return _footer3Color; } set { _footer3Color = value; OnPropertyChanged("Footer3Color"); } }
        public ObservableCollection<RegimenRoot> Regimens { get; set; }
        private readonly IDashboardService _dashboardService;
        public List<PatientIdsData> patientIds { get; set; }
        public RegimenRoot _regimenRoot { get; set; }
        public List<RegimenHistory> History { get; set; }
        public DashboardViewModel(IDashboardService dashboardService)
        {
            SelectedtrialName = "";
            Regimens = new ObservableCollection<RegimenRoot>();
            _dashboardService = dashboardService;
            TrialNames = new List<TrialTable>();
            TrialsList = new ObservableCollection<TrialOption>();

            GetTrials();
            MessagingCenter.Subscribe<UpdateContainerService, MedicineItem>(this, Constants.UpdateContainerKey, (sender, arg) => {
              var updatableObj=  PresentMedicines.Where(x => x.ID == arg.ID).FirstOrDefault();
                if (updatableObj != null)
                {
                    updatableObj.StatusImage = arg.StatusImage;
                    updatableObj.time1 = arg.time1;
                }
            });
            MessagingCenter.Subscribe<string>(this, Constants.UpdateTrialTimeKey, (sender) => {
                DisplayPresentMedicines();
            });
            MessagingCenter.Subscribe<string>(this, Constants.UpdateDashboardCalendarDate, (sender) => {
                DateTime date11 = DateTime.ParseExact(sender, GlobalSettings.MedConSelectedDateFormat, null);
                DayName = date11.DayOfWeek.ToString();
                SelectedDate = date11.ToString(GlobalSettings.MedConDateFormat);
            });

            DayName =DateTime.ParseExact(Constants.SelectedDate, GlobalSettings.MedConSelectedDateFormat, null).DayOfWeek.ToString();
            SelectedDate = DateTime.ParseExact(Constants.SelectedDate,GlobalSettings.MedConSelectedDateFormat, null).ToString(GlobalSettings.MedConDateFormat);
            MessagingCenter.Subscribe<string>(this, Constants.UpdateDashboardDateKey, (value) =>
            {
                DateTime senderDate = DateTime.ParseExact(value,GlobalSettings.MedConSelectedDateFormat, null);
                DayName = senderDate.DayOfWeek.ToString();
                SelectedDate = senderDate.ToString(GlobalSettings.MedConDateFormat);

            });
            SelectTrialsCommand = new Command(FocusPicker);

            GotoCalendarCommand = new Command(GotoCalendar);
            PresentMedicines = new ObservableCollection<MedicineItem>();
            FooterCommand = new Command<string>(FooterMethod);
            TrialSelectedCommand = new Command(TrialSelected);
           }
        async void GetTrials()
        {
            try
            {

                DialogProvider.ShowProgress("Initializing data...");
                // List<RegimenRoot> regimens = new List<RegimenRoot>();
                 patientIds = await _dashboardService.GetTrials();
                if (patientIds != null && patientIds.Count > 0)
                {
                    foreach (var item in patientIds)
                    {
                        RegimenRoot regimenRoot = await _dashboardService.GetAllRegimen(item.Patient);
                        DateTime endDate = DateTime.ParseExact(regimenRoot.trial.endDate, "yyyy-MM-dd", null);
                        if(endDate>=DateTime.Now)
                        {
                            TrialOption trial = new TrialOption();
                            trial.ID = regimenRoot.trial.id;
                            trial.Name = regimenRoot.trial.name;
                            trial.StartDate = regimenRoot.trial.startDate;
                            trial.EndDate = regimenRoot.trial.endDate;
                            var existItem = TrialsList.Where(x => x.ID == regimenRoot.trial.id).FirstOrDefault();
                            if (existItem == null)
                                TrialsList.Add(trial);
                            Regimens.Add(regimenRoot);
                        }                      
                    }
                }
                if (Regimens.Count > 0)
                {
                    CalendarService calendarService = new CalendarService(Regimens);
                    TrialSelectedIndex = 0;
                    trialId = Regimens[0].trial.id;
                    SelectedtrialName = Regimens[0].trial.name;
                    Constants.SelectedTrial = Regimens[0].trial.id;
                    _regimenRoot = Regimens[0];

                     History = await requestProvider.GetAsync<List<RegimenHistory>>(string.Format(Constants.HistoryApiBase+"dose/list?patientnum={0}&containerId={1}", patientIds[0].Patient, patientIds[0].PatientContainer));
                    DisplayPresentMedicines();
                    new DoseRemainderService().StartDoseRemainderService(60,Regimens);
                    CalculateAdherence(trialId);
                }
            }
            catch (MedCon.Services.Base.ServiceAuthenticationException)
            {
                GetTrials();
            }
            catch(Exception ex)
            {
                DialogService.DisplayNativeAlert(ex.Message, "OK");
            }
            finally
            {
                DialogProvider.HideProgress();
            }
        }
       async void TrialSelected()
        {
            try
            {
                DialogProvider.ShowProgress("Loading history...");
                // TrialSelectedIndex = 0;

             Constants.SelectedTrial= trialId = TrialsList[TrialSelectedIndex].ID;
                SelectedtrialName = TrialsList[TrialSelectedIndex].Name;
                //  trialId = Regimens[TrialSelectedIndex].trial.id;
                _regimenRoot = Regimens.Where(x => x.trial.id == trialId).FirstOrDefault();

                History = await requestProvider.GetAsync<List<RegimenHistory>>(string.Format(Constants.HistoryApiBase + "dose/list?patientnum={0}&containerId={1}", patientIds[TrialSelectedIndex].Patient, patientIds[TrialSelectedIndex].PatientContainer));
                DisplayPresentMedicines();
            }
            catch (Exception ex)
            {
                DialogProvider.HideProgress();
                DialogProvider.DisplayNativeAlert(ex.Message, "OK");
            }
            finally
            {
                DialogProvider.HideProgress();
            }
            //int selectdIndex = TrialSelectedIndex;
            //SelectedtrialName = TrialNames[selectdIndex].name;
            //trialId = TrialNames[selectdIndex].id;
            //DisplayPresentMedicines();
        }
        double CalculateAdherence(int trialId)
        {
            double adherence = 0;
            var numberOfDrugs = Regimens.Where(x => x.trial.id == trialId).ToList();
            if(numberOfDrugs!=null)
            {
                foreach (var item in numberOfDrugs)
                {
                    int mornings = item.regimen.morning_schedule.Split(',').Count();
                    int afternoons = item.regimen.afternoon_schedule.Split(',').Count();
                    int evenings = item.regimen.evening_schedule.Split(',').Count();
                    int bedtimes = item.regimen.bedtime_schedule.Split(',').Count();
                    var hist = History.Where(a => a.patientcontainerId == item.containerId);
                    if(hist!=null)
                    {
                        decimal OntimeNormal=0, MissedNormal=0, LateNormal=0, OverdoseNormal=0;
                        foreach (var historyObj in hist)
                        {
                            switch (historyObj.category)
                            {
                                case "normal":
                                    string[] sArray = historyObj.doseAmount.Split(' ');
                                    int doses = (int.Parse(sArray[0]) / int.Parse(item.drug.amount)) / item.regimen.medicationPerDose;
                                    OntimeNormal += doses;
                                    break;
                                case "missed":
                                    string[] sArray1 = historyObj.doseAmount.Split(' ');
                                    int doses1 = (int.Parse(sArray1[0]) / int.Parse(item.drug.amount)) / item.regimen.medicationPerDose;
                                    MissedNormal += doses1;
                                    break;
                                case "late":
                                    string[] sArray2 = historyObj.doseAmount.Split(' ');
                                    int doses2 = (int.Parse(sArray2[0]) / int.Parse(item.drug.amount)) / item.regimen.medicationPerDose;
                                    LateNormal += doses2;
                                    break;
                                case "overdose":
                                    string[] sArray3 = historyObj.doseAmount.Split(' ');
                                    int doses3 = (int.Parse(sArray3[0]) / int.Parse(item.drug.amount)) / item.regimen.medicationPerDose;
                                    OverdoseNormal += doses3;
                                    break;
                                default:
                                    break;
                            }
                            var selectedTrial = TrialsList.Where(c => c.ID == trialId).FirstOrDefault();
                            DateTime trialStart = DateTime.ParseExact(selectedTrial.StartDate, "yyyy-MM-dd", null);
                            DateTime trialend = DateTime.ParseExact(selectedTrial.EndDate, "yyyy-MM-dd", null);
                            TimeSpan remainingDate = DateTime.Now.AddDays(-1).Date - trialStart.Date;
                         int compltedDoses= GetTodayCompletedDoses(item.regimen);
                            int totaldoses = (remainingDate.Days * (mornings + afternoons + evenings + bedtimes))+compltedDoses;
                            var ddd = OntimeNormal / totaldoses;
                            DoseAdherence += (OntimeNormal / totaldoses) * 100;
                        }
                    }
                }

                DoseAdherence = Math.Round((DoseAdherence / numberOfDrugs.Count), 2);
                if (DoseAdherence >= 75)
                    DoseAdherenceImage = "ok.png";
                else
                    DoseAdherenceImage = "warning.png";
            }

            return adherence;
        }
        int GetTodayCompletedDoses(Regimen regimen)
        {
            int totlDoses = 0;
            switch (DetermainDayTime())
            {
                case DayTime.Morning:
                    break;
                case DayTime.Afternoon:
                    totlDoses += regimen.morning_schedule.Split(',').Count();
                    break;
                case DayTime.Evening:
                    totlDoses += regimen.morning_schedule.Split(',').Count();
                    totlDoses += regimen.afternoon_schedule.Split(',').Count();
                    break;
                case DayTime.Bedtime:
                    totlDoses += regimen.morning_schedule.Split(',').Count();
                    totlDoses += regimen.afternoon_schedule.Split(',').Count();
                    totlDoses += regimen.evening_schedule.Split(',').Count();
                    break;
            }
            return totlDoses;
        }
        private void FooterMethod(string dayTime)
        {
            DateTime dateTime = DateTime.Now;
            switch (dayTime)
            {                
                case "Morning":
                    DisplayMorningSmall();
                    DisplayMorningMedicines(DayTime.Morning);
                    SetWindowsColors(DayTime.Morning);
                    break;
                case "Afternoon":
                    DisplayAfternoonSmall();
                    DisplayMorningMedicines(DayTime.Afternoon);
                    SetWindowsColors(DayTime.Afternoon);
                    break;
                case "Evening":
                    DisplayEveningSmall();
                    DisplayMorningMedicines(DayTime.Evening);
                    SetWindowsColors(DayTime.Evening);
                    break;
                case "Bedtime":
                    DisplayBedtimeSmall();
                    DisplayMorningMedicines(DayTime.Bedtime);
                    SetWindowsColors(DayTime.Bedtime);
                    break;
            }
        }
        private void FocusPicker()
        {
            MessagingCenter.Send("1", Constants.ShowDashboardPickerKey);          

        }
        private async void GotoCalendar()
        {
            DashboardCalendarData dashboardCalendarData = new DashboardCalendarData();
            dashboardCalendarData.Histories = History;
            dashboardCalendarData.Regimens = Regimens;
            await NavigationService.NavigateToAsync<DashboardCalendarViewModel>(dashboardCalendarData);
        }
        public override Task InitializeAsync(object navigationData)
        {
             accessToken =(string)navigationData;

            return base.InitializeAsync(navigationData);
        }
        private void DisplayPresentMedicines()
        {
            if(PresentMedicines!=null)
            PresentMedicines.Clear();
            if (Footer1List != null)
                Footer1List.Clear();
            if (Footer2List != null)
                Footer2List.Clear();
            if (Footer3List != null)
                Footer3List.Clear();
            DayTime time = DetermainDayTime();
            switch (time)
            {
                case DayTime.Morning:
                    DisplayMorningMedicines(DayTime.Morning);
                    DisplayMorningSmall();
                    SetWindowsColors(DayTime.Morning);
                    break;
                case DayTime.Afternoon:
                    DisplayMorningMedicines(DayTime.Afternoon);
                    DisplayAfternoonSmall();
                    SetWindowsColors(DayTime.Afternoon);
                    break;
                case DayTime.Evening:
                    DisplayMorningMedicines(DayTime.Evening);
                    DisplayEveningSmall();
                    SetWindowsColors(DayTime.Evening);
                    break;
                case DayTime.Bedtime:
                    DisplayMorningMedicines(DayTime.Bedtime);
                    DisplayBedtimeSmall();
                    SetWindowsColors(DayTime.Bedtime);
                    break;
                case DayTime.Midnight:
                    break;
                default:
                    break;
            }
        }
        void ScanTypeService()
        {
            DetermineScanType();
           
            Device.StartTimer(TimeSpan.FromMinutes(1), () =>
               {
                   DetermineScanType();
                   return true;
               });
        }
        void DetermineScanType()
        {
            var obj = Regimens.Where(x => x.trial.id == trialId).FirstOrDefault();
            //   AlertTable alertTable = SqliteService.GetAlerts();
            if (obj == null) return;
            foreach (var item in PresentMedicines)
            {
                string minutes1 = "00";  
                var trimmedTime = item.time.Remove(item.time.Length - 3, 1).Insert(item.time.Length - 3, " ");
                string[] timeArray = trimmedTime.Split(' ');
                string[] finalTime = timeArray[0].Split(':');
                string hour = finalTime[0];
                if (hour.Length == 1)
                  hour=  hour.Insert(0, "0");
                int minutes = int.Parse(finalTime[1]) + obj.alertmessage.thresholddosetime;
                minutes1 = minutes.ToString();
                if (minutes.ToString().Length == 1)
                    minutes1 = (minutes.ToString().Insert(0, "0"));
              //  DateTime dateTimeWithKind = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minutes, 00, DateTimeKind.Local);
                string dddd = string.Format("{0} {1}:{2}:{3} {4}", DateTime.Now.ToString("dd/MM/yyyy"), hour, minutes, "00", trimmedTime.Substring(trimmedTime.Length - 2));
                string datetimeNow = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                DayTime dayTimeToCheck = DetermainDayTime();
                int index = (int)dayTimeToCheck;
                int pIndex = (int)((DayTime)Enum.Parse(typeof(DayTime), DayTimeName));
                TimeSpan timeSpan = new TimeSpan(0, 0, 0);
                Constants.SelectedDate = (DateTime.ParseExact(Constants.SelectedDate, GlobalSettings.MedConSelectedDateFormat, null).Date + timeSpan).ToString();
                if (DateTime.ParseExact(Constants.SelectedDate, GlobalSettings.MedConSelectedDateFormat, null) > DateTime.Now + timeSpan)
                {
                    item.IsScanVisible = false;
                    item.IsManualVisible = false;
                }                             
               else if (DateTime.ParseExact(Constants.SelectedDate, GlobalSettings.MedConSelectedDateFormat, null) < DateTime.Now.Date+timeSpan)
                {
                    item.IsScanVisible = false;
                    item.IsManualVisible = true;
                }
                else if (pIndex>index)
                {
                    item.IsScanVisible = false;
                    item.IsManualVisible = false;
                }
               else if (DateTime.ParseExact(dddd, "dd/MM/yyyy hh:mm:ss tt", null) < DateTime.ParseExact(datetimeNow, "dd/MM/yyyy hh:mm:ss tt", null))
                {
                    item.IsScanVisible = false;
                    item.IsManualVisible = true;
                }
            }
        }
        void SetWindowsColors(DayTime dayTime)
        {
            switch (dayTime)
            {
                case DayTime.Morning:
                    PresentColor =(Color)Application.Current.Resources["MorningColor"];
                    Footer1Color=(Color)Application.Current.Resources["AfternoonColor"];
                    Footer2Color =(Color)Application.Current.Resources["EveningColor"];
                    Footer3Color =(Color)Application.Current.Resources["BedtimeColor"];
                    break;
                case DayTime.Afternoon:
                    Footer1Color =(Color)Application.Current.Resources["MorningColor"];
                    PresentColor =(Color)Application.Current.Resources["AfternoonColor"];
                    Footer2Color =(Color)Application.Current.Resources["EveningColor"];
                    Footer3Color =(Color)Application.Current.Resources["BedtimeColor"];
                    break;
                case DayTime.Evening:
                    Footer1Color =(Color)Application.Current.Resources["MorningColor"];
                    Footer2Color =(Color)Application.Current.Resources["AfternoonColor"];
                    PresentColor =(Color)Application.Current.Resources["EveningColor"];
                    Footer3Color =(Color)Application.Current.Resources["BedtimeColor"];
                    break;
                case DayTime.Bedtime:
                    Footer1Color =(Color)Application.Current.Resources["MorningColor"];
                    Footer2Color =(Color)Application.Current.Resources["AfternoonColor"];
                    Footer3Color =(Color)Application.Current.Resources["EveningColor"];
                    PresentColor =(Color)Application.Current.Resources["BedtimeColor"];
                    break;
            }
        }
        private void DisplayMorningMedicines(DayTime time)
        {
            try
            {
                if (_regimenRoot == null) return;
                DayTimeName = time.ToString();

                List<Medicine> presentMedicines = new List<Medicine>();
                switch (time)
                {
                    case DayTime.Morning:
                        PresentDayImage = "morning.png";
                        presentMedicines = GetMorningMedicines(_regimenRoot, DayTime.Morning);
                        //  presentMedicines = SqliteService.GetMorningMedicines(trialId);
                        DisplayMorningSmall();
                        break;
                    case DayTime.Afternoon:
                        PresentDayImage = "afternoon.png";
                        presentMedicines = GetMorningMedicines(_regimenRoot, DayTime.Afternoon);
                        //  presentMedicines = SqliteService.GetAfternoonMedicines(trialId);
                        DisplayAfternoonSmall();
                        break;
                    case DayTime.Evening:
                        PresentDayImage = "evening.png";
                        presentMedicines = GetMorningMedicines(_regimenRoot, DayTime.Evening);
                        // presentMedicines = SqliteService.GetEveningMedicines(trialId);
                        DisplayEveningSmall();
                        break;
                    case DayTime.Bedtime:
                        PresentDayImage = "bedtime.png";
                        presentMedicines = GetMorningMedicines(_regimenRoot, DayTime.Bedtime);
                        // presentMedicines = SqliteService.GetBedtimeMedicines(trialId);
                        DisplayBedtimeSmall();
                        break;
                    case DayTime.Midnight:
                        break;
                    default:
                        break;
                }
                int count = 0;
                if (PresentMedicines.Count > 0)
                    PresentMedicines.Clear();
                foreach (var item in presentMedicines)
                {

                    MedicineItem medicineItem = new MedicineItem { RegimenData = _regimenRoot, DetailsCommand = new Command<MedicineItem>(ShowDetails), ScanCommand = new Command<MedicineItem>(ScanContainer), WindowImage = PresentDayImage, DrugWeight = item.amount, TotalDoses = item.total_doses, StatusImage = "", RemainingDoses = item.total_doses, CompanyId = item.CompanyId, patientId = item.PatientId, TrialId = item.trial_id, ID = count, medicinePerDose = item.medicineperdose, DoseTotal = item.amount + " " + item.unit.ToString(), type = item.medicationtype, container_id = item.conatiner_id, medicine_image = item.image, name = item.drugname, description = item.alias, time = item.dose_time, time1 = "", wieght = string.Format("{0}x{1} {2}", item.medicineperdose, item.amount, item.unit) };
                    if (History != null && History.Count > 0)
                    {
                        foreach (var historyItem in History)
                        {
                            if (IsHistoryExist(historyItem.time, historyItem.doseWindow, DayTimeName.ToLower()))
                            {
                                medicineItem.StatusImage = DetermineDoseImage(DayTimeName.ToLower());
                                medicineItem.time1 = GetDoseUpdatedTime(DayTimeName.ToLower());
                            }
                        }
                    }
                    medicineItem.time = CheckTrialChange(item.conatiner_id, item.dose_time);
                    if (IsTodayMedicine(trialId))
                        PresentMedicines.Add(medicineItem);

                    count++;
                }
                ScanTypeService();
            }
            catch (Exception ex)
            {

            }

        }
        bool IsHistoryExist(string date,string doseTime,string windowname)
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, 0);
            var arr = date.Replace(" ","").Split('|');
            DateTime histDate = DateTime.ParseExact(arr[0], "MM/dd/yyyy", null);
            var selectedDate = DateTime.ParseExact(Constants.SelectedDate, GlobalSettings.MedConSelectedDateFormat, null);
            selectedDate = selectedDate.Date + timeSpan;
            if (histDate == selectedDate&&windowname==doseTime)
                return true;
            return false;
        }
        string CheckTrialChange(string container,string time)
        {            
          string updatedTime= SqliteService.GetUpdatedTimes(container, DayTimeName.Substring(0,1));
            if (string.IsNullOrEmpty(updatedTime))
                return time;
            return updatedTime;
        }
        string DetermineDoseImage(string window)
        {
            var objHist = History.Where(x => x.doseWindow == window&&x.trialId==trialId).ToList();          
            if (objHist.Count==0)
                return "";
            string doseImage = string.Empty;
            switch (objHist[objHist.Count-1].category)
            {
                case "missed":
                    doseImage = "missed.png";
                    break;
                case "normal":
                    doseImage = "ontime.png";
                    break;
                case "late":
                    doseImage = "overdose.png";
                    break;
                case "overdose":
                    doseImage = "overdose.png";
                    break;
            }
            return doseImage;
        }
        string GetDoseUpdatedTime(string window)
        {
            var objHist = History.Where(x => x.doseWindow == window && x.trialId == trialId).ToList();
            if (objHist.Count == 0)
                return "";
            string[] timeParts = objHist[objHist.Count - 1].time.Split('|');
            return timeParts[1];
           
        }
        async void ShowDetails(MedicineItem selectedMedicine)
        {
            await App.Current.MainPage.Navigation.PushPopupAsync(new Views.DashboardDetailsPopUp(selectedMedicine,History,Regimens.ToList()));

        }
        private DayTime DetermainDayTime()
        {
            TimeSpan date = DateTime.Now.TimeOfDay;
            if (date.Hours >= 0 && date.Hours < 12)
                return DayTime.Morning;
            if (date.Hours >= 12 && date.Hours < 16)
                return DayTime.Afternoon;
            if (date.Hours >= 16 && date.Hours < 20)
                return DayTime.Evening;
            if (date.Hours >= 20 && date.Hours <= 23)
                return DayTime.Bedtime;
            return DayTime.Midnight;
        }
        private void DisplayMorningSmall()
        {
            Footer2Image = "evening.png";
            Footer2Text = "Evening";
            Footer1Image = "afternoon.png";
            Footer1Text = "Afternoon";
            Footer3Image = "bedtime.png";
            Footer3Text = "Bedtime";
            Footer1List = new List<Footer>();
            Footer2List = new List<Footer>();
            Footer3List = new List<Footer>();
            var list = Regimens.Where(x => x.trial.id == trialId).ToList();
            foreach (var item10 in list)
            {
                if (item10.regimen.afternoon_schedule != "undefined" && item10.regimen.afternoon_schedule != "")
                {
                    string[] times = _regimenRoot.regimen.afternoon_schedule.Split(',');
                    foreach (var item in times)
                    {
                        Footer footer = new Footer();
                        footer.MedicineImage = Constants.MedicineImageBase + _regimenRoot.drug.image;
                        footer.DayStatus = 2;
                        if (History != null && History.Count > 0)
                        {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time,historyItem.doseWindow, Footer1Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer1Text.ToLower());
                                }
                            }
                        }
                        if(IsTodayMedicine(item10.trial.id))
                        Footer1List.Add(footer);
                    }
                }
                if (item10.regimen.evening_schedule != "undefined" && item10.regimen.evening_schedule != "")
                {
                    string[] times = _regimenRoot.regimen.evening_schedule.Split(',');
                    foreach (var item in times)
                    {
                        Footer footer = new Footer();
                        footer.MedicineImage = Constants.MedicineImageBase + _regimenRoot.drug.image;
                        footer.DayStatus = 2;
                        if (History != null && History.Count > 0)
                        {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time, historyItem.doseWindow, Footer2Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer2Text.ToLower());
                                }
                            }
                        }
                        if (IsTodayMedicine(item10.trial.id))
                            Footer2List.Add(footer);
                    }
                }
                if (item10.regimen.bedtime_schedule != "undefined" && item10.regimen.bedtime_schedule != "")
                {
                    string[] times = _regimenRoot.regimen.bedtime_schedule.Split(',');
                    foreach (var item in times)
                    {
                        Footer footer = new Footer();
                        footer.MedicineImage = Constants.MedicineImageBase + _regimenRoot.drug.image;
                        footer.DayStatus = 2;
                        if (History != null && History.Count > 0)
                        {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time, historyItem.doseWindow, Footer3Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer3Text.ToLower());
                                }
                            }
                        }
                        if (IsTodayMedicine(item10.trial.id))
                            Footer3List.Add(footer);
                    }
                }
            }
        }
        private void DisplayAfternoonSmall()
        {
            Footer2Image = "evening.png";
            Footer2Text = "Evening";
            Footer1Image = "morning.png";
            Footer1Text = "Morning";
            Footer3Image = "bedtime.png";
            Footer3Text = "Bedtime";
            Footer1List = new List<Footer>();
            Footer2List = new List<Footer>();
            Footer3List = new List<Footer>();
            var list = Regimens.Where(x => x.trial.id == trialId).ToList();
            foreach (var item10 in list)
            {
            if (item10.regimen.morning_schedule != "undefined" && item10.regimen.morning_schedule != "")
            {
                string[] times = _regimenRoot.regimen.morning_schedule.Split(',');
                foreach (var item in times)
                {
                    Footer footer = new Footer();
                    footer.MedicineImage = Constants.MedicineImageBase + _regimenRoot.drug.image;
                    footer.DayStatus = 2;
                    if (History != null && History.Count > 0)
                    {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time, historyItem.doseWindow, Footer1Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer1Text.ToLower());
                                }
                            }
                        }
                        if (IsTodayMedicine(item10.trial.id))
                            Footer1List.Add(footer);
                }
            }
            if (item10.regimen.evening_schedule != "undefined" && item10.regimen.evening_schedule != "")
            {
                string[] times = _regimenRoot.regimen.evening_schedule.Split(',');
                foreach (var item in times)
                {
                    Footer footer = new Footer();
                    footer.MedicineImage = Constants.MedicineImageBase + _regimenRoot.drug.image;
                    footer.DayStatus = 2;
                    if (History != null && History.Count > 0)
                    {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time, historyItem.doseWindow, Footer2Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer2Text.ToLower());
                                }
                            }
                        }
                        if (IsTodayMedicine(item10.trial.id))
                            Footer2List.Add(footer);
                }
            }
            if (item10.regimen.bedtime_schedule != "undefined" && item10.regimen.bedtime_schedule != "")
            {
                string[] times = _regimenRoot.regimen.bedtime_schedule.Split(',');
                foreach (var item in times)
                {
                    Footer footer = new Footer();
                    footer.MedicineImage = Constants.MedicineImageBase + _regimenRoot.drug.image;
                    footer.DayStatus = 2;
                    if (History != null && History.Count > 0)
                    {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time, historyItem.doseWindow, Footer3Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer3Text.ToLower());
                                }
                            }
                        }
                        if (IsTodayMedicine(item10.trial.id))
                            Footer3List.Add(footer);
                }
            }
            }
        }
        private void DisplayEveningSmall()
        {
            Footer1Image = "morning.png";
            Footer1Text = "Morning";
            Footer2Image = "afternoon.png";
            Footer2Text = "Afternoon";
            Footer3Image = "bedtime.png";
            Footer3Text = "Bedtime";
            Footer1List = new List<Footer>();
            Footer2List = new List<Footer>();
            Footer3List = new List<Footer>();
            var list = Regimens.Where(x => x.trial.id == trialId).ToList();
            foreach (var item10 in list)
            {
            if (item10.regimen.morning_schedule != "undefined"&& item10.regimen.morning_schedule!="")
            {
                string[] times = _regimenRoot.regimen.morning_schedule.Split(',');
                foreach (var item in times)
                {
                    Footer footer = new Footer();
                    footer.MedicineImage =Constants.MedicineImageBase+_regimenRoot.drug.image;
                    footer.DayStatus = 2;
                    if (History != null && History.Count > 0)
                    {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time, historyItem.doseWindow, Footer1Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer1Text.ToLower());
                                }
                            }
                        }
                        if (IsTodayMedicine(item10.trial.id))
                            Footer1List.Add(footer);
                }
            }
            if (item10.regimen.afternoon_schedule != "undefined" && item10.regimen.afternoon_schedule != "")
            {
                string[] times = _regimenRoot.regimen.afternoon_schedule.Split(',');
                foreach (var item in times)
                {
                    Footer footer = new Footer();
                    footer.MedicineImage =Constants.MedicineImageBase+_regimenRoot.drug.image;
                    footer.DayStatus = 2;
                    if (History != null && History.Count > 0)
                    {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time, historyItem.doseWindow, Footer2Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer2Text.ToLower());
                                }
                            }
                        }
                        if (IsTodayMedicine(item10.trial.id))
                            Footer2List.Add(footer);
                }
            }
            if (item10.regimen.bedtime_schedule != "undefined" && item10.regimen.bedtime_schedule != "")
            {
                string[] times = _regimenRoot.regimen.bedtime_schedule.Split(',');
                foreach (var item in times)
                {
                    Footer footer = new Footer();
                    footer.MedicineImage =Constants.MedicineImageBase+_regimenRoot.drug.image;
                    footer.DayStatus = 2;
                    if (History != null && History.Count > 0)
                    {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time, historyItem.doseWindow, Footer3Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer3Text.ToLower());
                                }
                            }
                        }
                        if (IsTodayMedicine(item10.trial.id))
                            Footer3List.Add(footer);
                }
            }
            }
        }
        private void DisplayBedtimeSmall()
        {
            Footer3Image = "evening.png";
            Footer3Text = "Evening";
            Footer2Image = "afternoon.png";
            Footer2Text = "Afternoon";
            Footer1Image = "morning.png";
            Footer1Text = "Morning";
            Footer1List = new List<Footer>();
            Footer2List = new List<Footer>();
            Footer3List = new List<Footer>();
            var list = Regimens.Where(x => x.trial.id == trialId).ToList();
            foreach (var item10 in list)
            {
            if (item10.regimen.morning_schedule != "undefined" && item10.regimen.morning_schedule != "")
            {
                string[] times = _regimenRoot.regimen.morning_schedule.Split(',');
                foreach (var item in times)
                {
                    Footer footer = new Footer();
                    footer.MedicineImage = Constants.MedicineImageBase + _regimenRoot.drug.image;
                    footer.DayStatus = 2;
                    if (History != null && History.Count > 0)
                    {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time, historyItem.doseWindow, Footer1Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer1Text.ToLower());
                                }
                            }
                        }
                        if (IsTodayMedicine(item10.trial.id))
                            Footer1List.Add(footer);
                }
            }
            if (item10.regimen.afternoon_schedule != "undefined" && item10.regimen.afternoon_schedule != "")
            {
                string[] times = _regimenRoot.regimen.afternoon_schedule.Split(',');
                foreach (var item in times)
                {
                    Footer footer = new Footer();
                    footer.MedicineImage = Constants.MedicineImageBase + _regimenRoot.drug.image;
                    footer.DayStatus = 2;
                    if (History != null && History.Count > 0)
                    {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time, historyItem.doseWindow, Footer2Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer2Text.ToLower());
                                }
                            }
                        }
                        if (IsTodayMedicine(item10.trial.id))
                            Footer2List.Add(footer);
                }
            }
            if (item10.regimen.evening_schedule != "undefined" && item10.regimen.evening_schedule != "")
            {
                string[] times = _regimenRoot.regimen.evening_schedule.Split(',');
                foreach (var item in times)
                {
                    Footer footer = new Footer();
                    footer.MedicineImage = Constants.MedicineImageBase + _regimenRoot.drug.image;
                    footer.DayStatus = 2;
                    if (History != null && History.Count > 0)
                    {
                            foreach (var historyItem in History)
                            {
                                if (IsHistoryExist(historyItem.time, historyItem.doseWindow, Footer3Text.ToLower()))
                                {
                                    footer.StatusImage = DetermineDoseImage(Footer3Text.ToLower());
                                }
                            }
                        }
                        if (IsTodayMedicine(item10.trial.id))
                            Footer3List.Add(footer);
                }
            }
            }
        }
        private async void ScanContainer(MedicineItem validatableMedicine)
        {
            RegimenRoot scannedRegimen = Regimens.Where(x => x.containerId == validatableMedicine.container_id).FirstOrDefault();
            if(scannedRegimen!=null)
            {
                DateTime endDate = DateTime.ParseExact(scannedRegimen.trial.endDate, "yyyy-MM-dd", null);
                if(endDate<DateTime.Now)
                {
                    DialogProvider.DisplayNativeAlert("This trial has expired!", "OK");
                    return;
                }
            }
            if (validatableMedicine.IsManualVisible)
            {
                string validTime = validatableMedicine.time.Remove(validatableMedicine.time.Length - 3, 1);
                validTime = validTime.Insert(validTime.Length - 2, " ");
                DateTime actualTime = DateTime.Parse(validTime);
                DateTime presentTime = DateTime.Now;
                var difference = presentTime - actualTime;
                await NavigationService.NavigateToAsync<ScanConfirmationViewModel>(validatableMedicine);
                return;
            }
            string scanResult = await GetScanResultAsync();
            if (scanResult == "" || scanResult == null)
                return;
            if (scanResult == validatableMedicine.container_id)
            {
                string validTime = validatableMedicine.time.Remove(validatableMedicine.time.Length - 3, 1);
                validTime = validTime.Insert(validTime.Length - 2, " ");
                DateTime actualTime = DateTime.Parse(validTime);
                DateTime presentTime = DateTime.Now;
                var difference = presentTime - actualTime;
                await NavigationService.NavigateToAsync<ScanConfirmationViewModel>(validatableMedicine);
            }
            else
                await NavigationService.NavigateToAsync<InvalidScanViewModel>(validatableMedicine);
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

        List<Medicine> GetMorningMedicines(RegimenRoot regimenRoot,DayTime dayTime)
        {
            List<Medicine> medicines = new List<Medicine>();
            List<RegimenRoot> RegimensList = new List<RegimenRoot>();
            var list = Regimens.Where(x => x.trial.id == regimenRoot.trial.id).ToList();
            foreach (var item10 in list)
            {
                string schedules = null;
                switch (dayTime)
                {
                    case DayTime.Morning:
                        schedules = regimenRoot.regimen.morning_schedule;
                        break;
                    case DayTime.Afternoon:
                        schedules = regimenRoot.regimen.afternoon_schedule;
                        break;
                    case DayTime.Evening:
                        schedules = regimenRoot.regimen.evening_schedule;
                        break;
                    case DayTime.Bedtime:
                        schedules = regimenRoot.regimen.bedtime_schedule;
                        break;
                    case DayTime.Midnight:
                        break;
                }
                if (schedules!= "undefined"&&schedules!="")
                {
                    List<string> Timings = new List<string>();

                    switch (dayTime)
                    {
                        case DayTime.Morning:
                            Timings = regimenRoot.regimen.morning_schedule.Split(',').ToList();
                            break;
                        case DayTime.Afternoon:
                            Timings = regimenRoot.regimen.afternoon_schedule.Split(',').ToList();
                            break;
                        case DayTime.Evening:
                            Timings = regimenRoot.regimen.evening_schedule.Split(',').ToList();
                            break;
                        case DayTime.Bedtime:
                            Timings = regimenRoot.regimen.bedtime_schedule.Split(',').ToList();
                            break;
                    }
                    foreach (var item in Timings)
                    {
                        Medicine medicine = new Medicine();
                        medicine.alias = regimenRoot.drug.alias;
                        medicine.amount = regimenRoot.drug.amount;
                        medicine.amount = regimenRoot.drug.amount;
                        medicine.CompanyId = regimenRoot.companyId;
                        medicine.conatiner_id = item10.containerId;
                        medicine.DoseDayTime = item;
                        medicine.dose_time = item;
                        medicine.drugname = regimenRoot.drug.drugname;
                        medicine.extra_doses = regimenRoot.regimen.extraDoses;
                        medicine.id = regimenRoot.regimen.id;
                        medicine.image = Constants.MedicineImageBase + regimenRoot.drug.image; medicine.medicationtype = regimenRoot.drug.medicationtype;
                        medicine.medicineperdose = regimenRoot.regimen.medicationPerDose;
                        medicine.PatientId = regimenRoot.PatientId;
                        medicine.status_image = "";
                        medicine.total_doses = regimenRoot.regimen.totalDoses;
                        medicine.trial_id = regimenRoot.trial.id;
                        medicine.unit = regimenRoot.drug.unit;
                        medicines.Add(medicine);
                    }
                }

            }
            return medicines;
        }
        List<Medicine> GetAfternoonMedicines(RegimenRoot regimenRoot)
        {
            List<Medicine> medicines = new List<Medicine>();

            if (regimenRoot.regimen.afternoon_schedule != "undefined" && regimenRoot.regimen.afternoon_schedule != "")
            {
                List<string> morningTimes = regimenRoot.regimen.afternoon_schedule.Split(',').ToList();
                foreach (var item in morningTimes)
                {
                    Medicine medicine = new Medicine();
                    medicine.alias = regimenRoot.drug.alias;
                    medicine.amount = regimenRoot.drug.amount;
                    medicine.CompanyId = regimenRoot.companyId;
                    medicine.conatiner_id = regimenRoot.containerId;
                    medicine.DoseDayTime = item;
                    medicine.dose_time = item;
                    medicine.drugname = regimenRoot.drug.drugname;
                    medicine.extra_doses = regimenRoot.regimen.extraDoses;
                    medicine.id = regimenRoot.regimen.id;
                    medicine.image = Constants.MedicineImageBase + regimenRoot.drug.image;
                    medicine.medicationtype = regimenRoot.drug.medicationtype;
                    medicine.medicineperdose = regimenRoot.regimen.medicationPerDose;
                    medicine.PatientId = regimenRoot.PatientId;
                    medicine.status_image = "";
                    medicine.total_doses = regimenRoot.regimen.totalDoses;
                    medicine.trial_id = regimenRoot.trial.id;
                    medicine.unit = regimenRoot.drug.unit;
                    medicines.Add(medicine);
                }
            }
            return medicines;
        }
        List<Medicine> GetEveningMedicines(RegimenRoot regimenRoot)
        {
            List<Medicine> medicines = new List<Medicine>();

            if (regimenRoot.regimen.evening_schedule != "undefined"&& regimenRoot.regimen.evening_schedule!="")
            {
                List<string> morningTimes = regimenRoot.regimen.evening_schedule.Split(',').ToList();
                foreach (var item in morningTimes)
                {
                    Medicine medicine = new Medicine();
                    medicine.alias = regimenRoot.drug.alias;
                    medicine.amount = regimenRoot.drug.amount;
                    medicine.CompanyId = regimenRoot.companyId;
                    medicine.conatiner_id = regimenRoot.containerId;
                    medicine.DoseDayTime = item;
                    medicine.dose_time = item;
                    medicine.drugname = regimenRoot.drug.drugname;
                    medicine.extra_doses = regimenRoot.regimen.extraDoses;
                    medicine.id = regimenRoot.regimen.id;
                    medicine.image = Constants.MedicineImageBase + regimenRoot.drug.image;
                    medicine.medicationtype = regimenRoot.drug.medicationtype;
                    medicine.medicineperdose = regimenRoot.regimen.medicationPerDose;
                    medicine.PatientId = regimenRoot.PatientId;
                    medicine.status_image = "";
                    medicine.total_doses = regimenRoot.regimen.totalDoses;
                    medicine.trial_id = regimenRoot.trial.id;
                    medicine.unit = regimenRoot.drug.unit;
                    medicines.Add(medicine);
                }
            }
            return medicines;
        }
        List<Medicine> GetBedtimeMedicines(RegimenRoot regimenRoot)
        {
            List<Medicine> medicines = new List<Medicine>();

            if (regimenRoot.regimen.bedtime_schedule != "undefined" && regimenRoot.regimen.bedtime_schedule != "")
            {
                List<string> morningTimes = regimenRoot.regimen.bedtime_schedule.Split(',').ToList();
                foreach (var item in morningTimes)
                {
                    Medicine medicine = new Medicine();
                    medicine.alias = regimenRoot.drug.alias;
                    medicine.amount = regimenRoot.drug.amount;
                    medicine.CompanyId = regimenRoot.companyId;
                    medicine.conatiner_id = regimenRoot.containerId;
                    medicine.DoseDayTime = item;
                    medicine.dose_time = item;
                    medicine.drugname = regimenRoot.drug.drugname;
                    medicine.extra_doses = regimenRoot.regimen.extraDoses;
                    medicine.id = regimenRoot.regimen.id;
                    medicine.image =Constants.MedicineImageBase+regimenRoot.drug.image;
                    medicine.medicationtype = regimenRoot.drug.medicationtype;
                    medicine.medicineperdose = regimenRoot.regimen.medicationPerDose;
                    medicine.PatientId = regimenRoot.PatientId;
                    medicine.status_image = "";
                    medicine.total_doses = regimenRoot.regimen.totalDoses;
                    medicine.trial_id = regimenRoot.trial.id;
                    medicine.unit = regimenRoot.drug.unit;
                    medicines.Add(medicine);
                }
            }
            return medicines;
        }
        bool IsTodayMedicine(int trialId)
        {
            var trial = TrialsList.Where(x => x.ID == trialId).FirstOrDefault();
            if(trial!=null)
            {
                string dateFormat = "dd/MM/yyyy";
                string dateFormat1 = "yyyy-MM-dd";
                if (DateTime.ParseExact(SelectedDate,dateFormat,null) >= DateTime.ParseExact(trial.StartDate,dateFormat1,null) && DateTime.ParseExact(SelectedDate,dateFormat,null) <= DateTime.ParseExact(trial.EndDate,dateFormat1,null))
                    return true;
            }
            return false;
        }
    }
    public enum DayTime
    {
        Morning=1,
        Afternoon=2,
        Evening=3,
        Bedtime=4,
        Midnight=5
    }
   
    public class TrialOption
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public String EndDate { get; set; }
    }
    public class Footer
    {
        public ImageSource MedicineImage { get; set; }
        public ImageSource StatusImage { get; set; }

        public int DayStatus { get; set; }
    }
}
