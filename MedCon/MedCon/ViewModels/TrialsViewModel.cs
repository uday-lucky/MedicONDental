using MedCon.LocalDB;
using MedCon.LocalDB.Tables;
using MedCon.Models;
using MedCon.Services;
using MedCon.Services.Interfaces;
using MedCon.ViewModels.Base;
using MedCon.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MedCon.ViewModels
{
   public class TrialsViewModel:ViewModelBase
    {
        int _trialSelectedIndex = -1;
        string _selectedTrial="Select Trial",_startDate,_endDate;
        Color _headerColor=Color.FromHex("#f3cb51");
        ObservableCollection<Trial> _trials;
        public TrailHeadingBinding HeaderData { get; set; }
        public Color HeaderColor { get { return _headerColor; } set { _headerColor = value; OnPropertyChanged("HeaderColor"); } }
        public ObservableCollection<Trial> Trials { get { return _trials; } set { _trials = value; OnPropertyChanged("Trials"); } }
        public ICommand SelectTrialsCommand { get; set; }
        public ObservableCollection<TrialTable> TrialNames { get; set; }
        public int TrialSelectedIndex { get { return _trialSelectedIndex; } set { _trialSelectedIndex = value; OnPropertyChanged("TrialSelectedIndex"); } }
        public string SelectedTrial { get { return _selectedTrial; } set { _selectedTrial = value; OnPropertyChanged("SelectedTrial"); } }
        public string StartDate { get { return _startDate; } set { _startDate = value; OnPropertyChanged("StartDate"); } }
        public string EndData { get { return _endDate; } set { _endDate = value; OnPropertyChanged("EndData"); } }
        int trialId;
        protected readonly IDashboardService _dashboardService;
        public List<PatientIdsData> patientIds { get; set; }
        public ObservableCollection<RegimenRoot> Regimens { get; set; }
        public List<RegimenHistory> History { get; set; }
        public ICommand TrialSelectedCommand { get; set; }

        string day = string.Empty;
        public TrialsViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
            Regimens = new ObservableCollection<RegimenRoot>();
           
            Trials = new ObservableCollection<Trial>();
            GetTrials();
            HeaderData = new TrailHeadingBinding();
            HeaderData.TimeName = "Morning";
            HeaderData.Time = "6:00 AM - 11:59 AM";
         //   HeaderData.MorningTapped = new Command(ShowMoringTrials);
          //  HeaderData.AfternoonTapped = new Command(ShowAfternoonTrials);
          //  HeaderData.EveningTapped = new Command(ShowEveningTrials);
          //  HeaderData.BedtimeTapped = new Command(ShowBedtimeTrials);
            SelectTrialsCommand = new Command(PickerTapped);
            TrialNames=new ObservableCollection<TrialTable>();
           // TrialNames = SqliteService.GetTrials();
            if (TrialNames!=null&&TrialNames.Count>0)
            {
                TrialSelectedIndex = 1;
                SelectedTrial = TrialNames[0].name;
                StartDate = DateTime.ParseExact(TrialNames[0].start_date, "yyyy-MM-dd", null).ToString(GlobalSettings.MedConDateFormat);
                EndData = DateTime.ParseExact(TrialNames[0].EndDate, "yyyy-MM-dd", null).ToString(GlobalSettings.MedConDateFormat);
                trialId= TrialNames[0].id;
            }
            // ShowMorningMedicines();

            // DateTime date = new DateTime();
            // MedicationSchedules medicationSchedules = new MedicationSchedules();
            //  medicationSchedules.morning_from = date.Add(TimeSpan.Parse("06:30"));
            //   SqliteService
            TrialSelectedCommand = new Command(TrialSelected);
        }
      async void TrialSelected()
        {
            try
            {
                DialogProvider.ShowProgress("Loading...");
                trialId = TrialNames[TrialSelectedIndex].id;
                SelectedTrial = TrialNames[TrialSelectedIndex].name;
                StartDate = DateTime.ParseExact(TrialNames[TrialSelectedIndex].start_date, "yyyy-MM-dd", null).ToString(GlobalSettings.MedConDateFormat);
                EndData = DateTime.ParseExact(TrialNames[TrialSelectedIndex].EndDate, "yyyy-MM-dd", null).ToString(GlobalSettings.MedConDateFormat);
                History = await requestProvider.GetAsync<List<RegimenHistory>>(string.Format(Constants.HistoryApiBase + "dose/list?patientnum={0}&containerId={1}", patientIds[TrialSelectedIndex].Patient, patientIds[TrialSelectedIndex].PatientContainer));

                switch (day)
                {
                    case "M":
                        ShowMorningMedicines();
                        break;
                    case "A":
                        ShowAfternoonMedicines();
                        break;
                    case "E":
                        ShowEveningMedicines();
                        break;
                    case "B":
                        ShowBedtimeMedicines();
                        break;
                    default:
                        ShowMorningMedicines();
                        break;
                }
            }
            catch (Exception)
            {
              
            }
            finally
            {
                DialogProvider.HideProgress();
            }
           
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
        async void GetTrials()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    DialogProvider.ShowProgress("Initializing data...");
                // List<RegimenRoot> regimens = new List<RegimenRoot>();
                patientIds = await _dashboardService.GetTrials();
                if (patientIds != null && patientIds.Count > 0)
                {
                    foreach (var item in patientIds)
                    {
                        RegimenRoot regimenRoot = await _dashboardService.GetAllRegimen(item.Patient);
                        Regimens.Add(regimenRoot);
                        TrialTable trial = new TrialTable();
                        trial.id = regimenRoot.trial.id;
                        trial.name = regimenRoot.trial.name;
                        trial.PatientId = regimenRoot.PatientId;
                        trial.start_date = regimenRoot.trial.startDate;
                        trial.EndDate = regimenRoot.trial.endDate;
                        trial.ContainerId = regimenRoot.containerId;
                        trial.manualdose = regimenRoot.trial.manualdose;
                        var existItem = TrialNames.Where(x => x.id == regimenRoot.trial.id).FirstOrDefault();
                        if (existItem == null)
                            TrialNames.Add(trial);
                    }
                }
                if (Regimens.Count > 0)
                {
                    TrialSelectedIndex = 0;
                    trialId = Regimens[0].trial.id;
                    SelectedTrial= Regimens[0].trial.name;
                    StartDate = Regimens[0].trial.startDate;
                    EndData = Regimens[0].trial.endDate;
                    History = await requestProvider.GetAsync<List<RegimenHistory>>(string.Format(Constants.HistoryApiBase + "dose/list?patientnum={0}&containerId={1}", patientIds[0].Patient, patientIds[0].PatientContainer));
                   // ShowMorningMedicines();
                        DisplayPresentMedicines();
                }

                });
            }
            catch (MedCon.Services.Base.ServiceAuthenticationException)
            {
                GetTrials();
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DialogProvider.HideProgress();
                });
                DialogService.DisplayNativeAlert(ex.Message, "OK");

            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DialogProvider.HideProgress();
                });
            }
        }
        void DisplayPresentMedicines()
        {
            switch (DetermainDayTime())
            {
                case DayTime.Morning:
                    ShowMoringTrials();
                    break;
                case DayTime.Afternoon:
                    ShowAfternoonTrials();
                    break;
                case DayTime.Evening:
                    ShowEveningTrials();
                    break;
                case DayTime.Bedtime:
                    ShowBedtimeTrials();
                    break;
            }
        }
        private void PickerTapped()
        {
            MessagingCenter.Send("1", Constants.ShowTrialPickerKey);
        }
        private void ShowSubmenu( Trial trial)
        {
            if (trial.IsSubmenuVisible)
                trial.IsSubmenuVisible = false;
            else
                trial.IsSubmenuVisible = true;
        }
        TimeSpan GetTimeSpan(string time)
        {
            time = time.Replace(" ", ":");
            string[] times = time.Split(':');
            string s1 = times[0], s2 = times[1];
            if (times[0].Length == 1)
                s1 = "0" + times[0];
            if (times[1].Length == 1)
                s2 = "0" + times[1];
            string validTime = string.Format("{0}:{1} {2}", s1, s2, times[2]);
            DateTime dateTime = DateTime.ParseExact(validTime,
                                "hh:mm tt", CultureInfo.InvariantCulture);
            TimeSpan span = dateTime.TimeOfDay;
            return span;
        }
        void TimeChangedEvent(object obj)
        {
            FocusEventArgs focusEventArgs = (FocusEventArgs)obj;
          var control=  focusEventArgs.VisualElement;
            var data = control.BindingContext as Trial;
            if(!IsvalidTime(data))
            {
                data.TrialTime = data.TrialTime1;
                return;
            }
            TrialTimesTable trialTimesTable = new TrialTimesTable();
            trialTimesTable.ContainerId = data.conatiner_id;
            trialTimesTable.Time = Get12HrFormat(data.TrialTime);
            trialTimesTable.Window =day;
            SqliteService.InsertTrialTime(trialTimesTable);
            MessagingCenter.Send<string>("0", Constants.UpdateTrialTimeKey);
        }
        bool IsvalidTime(Trial trial)
        {
            bool validTime = true;
            switch (trial.TimeOfDay)
            {
                case DayTime.Morning:
                    if(!(trial.TrialTime.Hours>=6&& trial.TrialTime.Hours<12))
                    {
                        DialogProvider.DisplayNativeAlert("Morning Dose Time should be between 6 AM and 12 PM","OK");
                        validTime = false;
                    }
                    break;
                case DayTime.Afternoon:
                    if (!(trial.TrialTime.Hours >= 12 && trial.TrialTime.Hours < 16))
                    {
                        DialogProvider.DisplayNativeAlert("Afternoon Dose Time should be between 12 PM and 4 PM", "OK");
                        validTime = false;
                    }
                    break;
                case DayTime.Evening:
                    if (!(trial.TrialTime.Hours >= 16 && trial.TrialTime.Hours < 20))
                    {
                        DialogProvider.DisplayNativeAlert("Evening Dose Time should be between 4 PM and 8 PM", "OK");
                        validTime = false;
                    }
                    break;
                case DayTime.Bedtime:
                    if (!(trial.TrialTime.Hours >= 20 && trial.TrialTime.Hours < 24))
                    {
                        DialogProvider.DisplayNativeAlert("Bedtime Dose Time should be between 8 PM and 12 PM", "OK");
                        validTime = false;
                    }
                    break;
                default:
                    break;
            }
            return validTime;
        }
        string Get12HrFormat(TimeSpan time)
        {
            DateTime dateTime = DateTime.Now;
            DateTime s =DateTime.Now;
            TimeSpan ts = time;
            s = s.Date + ts;
            var validTime = s.ToString("hh:mm tt");
            return validTime;
        }
        private void ShowMorningMedicines()
        {
            day = "M";
            Trials = new ObservableCollection<Trial>();

            //Trials.Clear();
            List<Medicine> medicines = new List<Medicine>();
            //   medicines = Regimens.Where(x => x.trial.id == trialId).ToList();
            var regimen = Regimens.Where(x => x.trial.id == trialId).ToList();
            foreach (var item10 in regimen)
            {
                medicines = GetMorningMedicines(item10, DayTime.Morning);
                foreach (var item in medicines)
                {
                    Trial trial = new Trial();

                    trial.TimeOfDay = DayTime.Morning;
                    trial.alias = item.alias;
                    trial.amount = item.amount;
                    trial.conatiner_id = item.conatiner_id;
                    trial.DoseDayTime = item.DoseDayTime;
                    trial.dose_time = item.dose_time;
                    trial.drugname = trial.MedicineName = item.drugname;
                    trial.id = item.id;
                    trial.image = item.image;
                    trial.medicationtype = item.medicationtype;
                    trial.trial_id = item.trial_id;
                    trial.unit = item.unit;
                    trial.Wieght = item.amount + " " + item.unit;
                    trial.MedicineIcon = item.image;
                    trial.ContainerName = item.conatiner_id;
                    trial.Quantity = item.medicineperdose;
                    trial.Time = item.dose_time;
                    item.dose_time = CheckTrialChange(item.conatiner_id, item.dose_time);
                    trial.TrialTime = GetTimeSpan(item.dose_time);
                    trial.TrialTime1 = GetTimeSpan(item.dose_time);
                    trial.total_doses = item.total_doses;
                    trial.extra_doses = item.extra_doses;
                    trial.DurationbetweenDoses = ConvertMinIntoHoursDay(Regimens[TrialSelectedIndex].regimen.durationBetweenDoses);
                    trial.medicineperdose = item.medicineperdose;

                    trial.Data = new DetailspopUpModel();
                    trial.Data.container_id = item.conatiner_id;
                    trial.Data.TotalDoses = item.total_doses.ToString();
                    trial.Data.RegimenName = item10.regimen.name;
                    trial.Data.ExtraDoses = item.extra_doses.ToString();
                    trial.Data.DurationBetweenDoses = ConvertMinIntoHoursDay(Regimens[TrialSelectedIndex].regimen.durationBetweenDoses);
                    foreach (var historyItem in History)
                    {
                        CalculateDoses(historyItem.category, historyItem.doseAmount, trial);
                    }

                    trial.ShowSubmenuCommand = new Command<Trial>(ShowSubmenu);
                    trial.TimeChanged = new Command<object>(TimeChangedEvent);
                    Trials.Add(trial);
                }
            }


            //  List<Medicine> medicines = SqliteService.GetMorningMedicines(trialId);
                
            //Trials.Add(new Trial
            //{
            //    MedicineIcon = "capsule.png",
            //    MedicineName = "Drug Alias Name",
            //    ContainerName = "00000",
            //    Quantity = 3,
            //    Wieght = "10 MG",
            //    Time = "07:00 AM",
            //    ShowSubmenuCommand = new Command<Trial>(ShowSubmenu)
            //});
            //Trials.Add(new Trial
            //{
            //    MedicineIcon = "capsule.png",
            //    MedicineName = "Drug Alias Name",
            //    ContainerName = "00000",
            //    Quantity = 3,
            //    Wieght = "10 MG",
            //    Time = "08:30 AM",
            //    ShowSubmenuCommand = new Command<Trial>(ShowSubmenu)
            //});
            //Trials.Add(new Trial
            //{
            //    MedicineIcon = "capsule.png",
            //    MedicineName = "Drug Alias Name",
            //    ContainerName = "00000",
            //    Quantity = 3,
            //    Wieght = "10 MG",
            //    Time = "09:00 AM",
            //    ShowSubmenuCommand = new Command<Trial>(ShowSubmenu)
            //});

        }
        private void ShowAfternoonMedicines()
        {
            day = "A";
            Trials = new ObservableCollection<Trial>();

            // Trials.Clear();
            List<Medicine> medicines = new List<Medicine>();
            var regimen = Regimens.Where(x => x.trial.id == trialId).ToList();
            foreach (var item10 in regimen)
            {
                medicines = GetMorningMedicines(item10, DayTime.Afternoon);
                foreach (var item in medicines)
                {
                    Trial trial = new Trial();
                    trial.TimeOfDay = DayTime.Afternoon;

                    trial.alias = item.alias;
                    trial.amount = item.amount;
                    trial.conatiner_id = item.conatiner_id;
                    trial.DoseDayTime = item.DoseDayTime;
                    trial.dose_time = item.dose_time;
                    trial.drugname = trial.MedicineName = item.drugname;
                    trial.id = item.id;
                    trial.image = item.image;
                    trial.medicationtype = item.medicationtype;
                    trial.trial_id = item.trial_id;
                    trial.unit = item.unit;
                    trial.Wieght = item.amount + " " + item.unit;
                    trial.MedicineIcon = item.image;
                    trial.ContainerName = item.conatiner_id;
                    trial.Quantity = item.medicineperdose;
                    trial.Time = item.dose_time;
                    item.dose_time = CheckTrialChange(item.conatiner_id, item.dose_time);
                    trial.TrialTime = GetTimeSpan(item.dose_time);
                    trial.TrialTime1 = GetTimeSpan(item.dose_time);
                    trial.total_doses = item.total_doses;
                    trial.extra_doses = item.extra_doses;
                    trial.DurationbetweenDoses = ConvertMinIntoHoursDay(Regimens[TrialSelectedIndex].regimen.durationBetweenDoses);
                    trial.medicineperdose = item.medicineperdose;

                    trial.Data = new DetailspopUpModel();
                    trial.Data.container_id = item.conatiner_id;
                    trial.Data.TotalDoses = item.total_doses.ToString();
                    trial.Data.RegimenName = item10.regimen.name;
                    trial.Data.ExtraDoses = item.extra_doses.ToString();
                    trial.Data.DurationBetweenDoses = ConvertMinIntoHoursDay(Regimens[TrialSelectedIndex].regimen.durationBetweenDoses);
                    foreach (var historyItem in History)
                    {
                        CalculateDoses(historyItem.category, historyItem.doseAmount, trial);
                    }

                    trial.ShowSubmenuCommand = new Command<Trial>(ShowSubmenu);
                    trial.TimeChanged = new Command<object>(TimeChangedEvent);
                    Trials.Add(trial);
                }
            }
        }
        private void ShowEveningMedicines()
        {

            day = "E";
            Trials = new ObservableCollection<Trial>();

            //  Trials.Clear();
            List<Medicine> medicines = new List<Medicine>();
            var regimen = Regimens.Where(x => x.trial.id == trialId).ToList();
            foreach (var item10 in regimen)
            {
                medicines = GetMorningMedicines(item10, DayTime.Evening);
                foreach (var item in medicines)
                {
                    Trial trial = new Trial();
                    trial.TimeOfDay = DayTime.Evening;

                    trial.alias = item.alias;
                    trial.amount = item.amount;
                    trial.conatiner_id = item.conatiner_id;
                    trial.DoseDayTime = item.DoseDayTime;
                    trial.dose_time = item.dose_time;
                    trial.drugname = trial.MedicineName = item.drugname;
                    trial.id = item.id;
                    trial.image = item.image;
                    trial.medicationtype = item.medicationtype;
                    trial.trial_id = item.trial_id;
                    trial.unit = item.unit;
                    trial.Wieght = item.amount + " " + item.unit;
                    trial.MedicineIcon = item.image;
                    trial.ContainerName = item.conatiner_id;
                    trial.Quantity = item.medicineperdose;
                    trial.Time = item.dose_time;
                    item.dose_time = CheckTrialChange(item.conatiner_id, item.dose_time);
                    trial.TrialTime = GetTimeSpan(item.dose_time);
                    trial.TrialTime1 = GetTimeSpan(item.dose_time);
                    trial.total_doses = item.total_doses;
                    trial.extra_doses = item.extra_doses;
                    trial.DurationbetweenDoses = ConvertMinIntoHoursDay(Regimens[TrialSelectedIndex].regimen.durationBetweenDoses);
                    trial.medicineperdose = item.medicineperdose;

                    trial.Data = new DetailspopUpModel();
                    trial.Data.container_id = item.conatiner_id;
                    trial.Data.TotalDoses = item.total_doses.ToString();
                    trial.Data.RegimenName = item10.regimen.name;
                    trial.Data.ExtraDoses = item.extra_doses.ToString();
                    trial.Data.DurationBetweenDoses = ConvertMinIntoHoursDay(Regimens[TrialSelectedIndex].regimen.durationBetweenDoses);
                    foreach (var historyItem in History)
                    {
                        CalculateDoses(historyItem.category, historyItem.doseAmount, trial);
                    }
                    trial.ShowSubmenuCommand = new Command<Trial>(ShowSubmenu);
                    trial.TimeChanged = new Command<object>(TimeChangedEvent);
                    Trials.Add(trial);
                }
            }
        }
        private void ShowBedtimeMedicines()
        {

            day = "B";
            Trials = new ObservableCollection<Trial>();
          //  Trials.Clear();
            List<Medicine> medicines = new List<Medicine>();
            var regimen = Regimens.Where(x => x.trial.id == trialId).ToList();
            foreach (var item10 in regimen)
            {
                medicines = GetMorningMedicines(item10, DayTime.Bedtime);
                foreach (var item in medicines)
                {
                    Trial trial = new Trial();
                    trial.TimeOfDay = DayTime.Bedtime;

                    trial.alias = item.alias;
                    trial.amount = item.amount;
                    trial.conatiner_id = item.conatiner_id;
                    trial.DoseDayTime = item.DoseDayTime;
                    trial.dose_time = item.dose_time;
                    trial.drugname = trial.MedicineName = item.drugname;
                    trial.id = item.id;
                    trial.image = item.image;
                    trial.medicationtype = item.medicationtype;
                    trial.trial_id = item.trial_id;
                    trial.unit = item.unit;
                    trial.Wieght = item.amount + " " + item.unit;
                    trial.MedicineIcon = item.image;
                    trial.ContainerName = item.conatiner_id;
                    trial.Quantity = item.medicineperdose;
                    trial.Time = item.dose_time;
                    item.dose_time = CheckTrialChange(item.conatiner_id, item.dose_time);
                    trial.TrialTime = GetTimeSpan(item.dose_time);
                    trial.TrialTime1 = GetTimeSpan(item.dose_time);
                    trial.total_doses = item.total_doses;
                    trial.extra_doses = item.extra_doses;
                    trial.DurationbetweenDoses = ConvertMinIntoHoursDay(Regimens[TrialSelectedIndex].regimen.durationBetweenDoses);
                    trial.medicineperdose = item.medicineperdose;

                    trial.Data = new DetailspopUpModel();
                    trial.Data.container_id = item.conatiner_id;
                    trial.Data.TotalDoses = item.total_doses.ToString();
                    trial.Data.RegimenName = item10.regimen.name;
                    trial.Data.ExtraDoses = item.extra_doses.ToString();
                    trial.Data.DurationBetweenDoses = ConvertMinIntoHoursDay(Regimens[TrialSelectedIndex].regimen.durationBetweenDoses);
                    foreach (var historyItem in History)
                    {
                        CalculateDoses(historyItem.category, historyItem.doseAmount, trial);
                    }
                    trial.ShowSubmenuCommand = new Command<Trial>(ShowSubmenu);
                    trial.TimeChanged = new Command<object>(TimeChangedEvent);
                    Trials.Add(trial);
                }
            }
        }
        void CalculateDoses(string time, string amount,Trial trial)
        {
            switch (time)
            {
                case "normal":
                    string[] sArray = amount.Split(' ');
                    int doses = (int.Parse(sArray[0]) / int.Parse(trial.amount)) / trial.medicineperdose;
                    trial.Data.OntimeNormal += doses;
                    break;
                case "missed":
                    string[] sArray1 = amount.Split(' ');
                    int doses1 = (int.Parse(sArray1[0]) / int.Parse(trial.amount)) / trial.medicineperdose;
                    trial.Data.MissedNormal += doses1;
                    break;
                case "late":
                    string[] sArray2 = amount.Split(' ');
                    int doses2 = (int.Parse(sArray2[0]) / int.Parse(trial.amount)) / trial.medicineperdose;
                    trial.Data.OntimeNormal += doses2;
                    break;
                case "overdose":
                    string[] sArray3 = amount.Split(' ');
                    int doses3 = (int.Parse(sArray3[0]) / int.Parse(trial.amount)) / trial.medicineperdose;
                    trial.Data.OntimeNormal += doses3;
                    break;
                default:
                    break;
            }
        }

        string CheckTrialChange(string container, string time)
        {
            string updatedTime = SqliteService.GetUpdatedTimes(container, day);
            if (string.IsNullOrEmpty(updatedTime))
                return time;
            return updatedTime;
        }
        private void ShowMoringTrials()
        {
            if (DetermainDayTime() != DayTime.Morning) return;

            DialogProvider.ShowProgress();
            ShowMorningMedicines();
            HeaderColor = Color.FromHex("#f3cb51");
            HeaderData.SubHeaderColor = Color.FromHex("#ffed8f");

            HeaderData.TimeName = "Morning";
            HeaderData.Time = "06:00 AM - 11:59 AM";

            HeaderData.MorningSelectedColor = Color.FromHex("#7986cb");
            HeaderData.AfternoonSelectedColor = Color.FromHex("#e8ebf6");
            HeaderData.EveningSelectedColor = Color.FromHex("#e8ebf6");
            HeaderData.BedtimeSelectedColor = Color.FromHex("#e8ebf6");

            HeaderData.MorningOpacity = 1f;
            HeaderData.AfternonnOpacity = 0.5f;
            HeaderData.EveningOpacity = 0.5f;
            HeaderData.BedtimeOpacity = 0.5f;
            DialogProvider.HideProgress();
        }
        private void ShowAfternoonTrials()
        {
            if (DetermainDayTime() != DayTime.Afternoon) return;

            DialogProvider.ShowProgress();
            ShowAfternoonMedicines();
            HeaderColor = Color.FromHex("#0288d1");
            HeaderData.SubHeaderColor = Color.FromHex("#4fc3f7");

            HeaderData.TimeName = "Afternoon";
            HeaderData.Time = "12:00 PM - 03:59 PM";

            HeaderData.MorningSelectedColor = Color.FromHex("#e8ebf6");
            HeaderData.AfternoonSelectedColor = Color.FromHex("#7986cb");
            HeaderData.EveningSelectedColor = Color.FromHex("#e8ebf6");
            HeaderData.BedtimeSelectedColor = Color.FromHex("#e8ebf6");

            HeaderData.MorningOpacity = 0.5f;
            HeaderData.AfternonnOpacity = 1f;
            HeaderData.EveningOpacity = 0.5f;
            HeaderData.BedtimeOpacity = 0.5f;
            DialogProvider.HideProgress();
        }
        private void ShowEveningTrials()
        {
            if (DetermainDayTime() != DayTime.Evening) return;

            DialogProvider.ShowProgress();
            ShowEveningMedicines();
            this.HeaderColor = Color.FromHex("#ff6f00");
            HeaderData.SubHeaderColor = Color.FromHex("#ffa000");

            HeaderData.TimeName = "Evening";
            HeaderData.Time = "04:00 PM - 07:59 PM";

            HeaderData.MorningSelectedColor = Color.FromHex("#e8ebf6");
            HeaderData.AfternoonSelectedColor = Color.FromHex("#e8ebf6");
            HeaderData.EveningSelectedColor = Color.FromHex("#7986cb");
            HeaderData.BedtimeSelectedColor = Color.FromHex("#e8ebf6");

            HeaderData.MorningOpacity = 0.5f;
            HeaderData.AfternonnOpacity = 0.5f;
            HeaderData.EveningOpacity = 1f;
            HeaderData.BedtimeOpacity = 0.5f;
            DialogProvider.HideProgress();
        }
        private void ShowBedtimeTrials()
        {
            if (DetermainDayTime() != DayTime.Bedtime) return;

            DialogProvider.ShowProgress();
            ShowBedtimeMedicines();
            this.HeaderColor = Color.FromHex("#01579b");
            HeaderData.SubHeaderColor = Color.FromHex("#0288d1");

            HeaderData.TimeName = "Bedtime";
            HeaderData.Time = "08:00 PM - 11:59 PM";

            HeaderData.MorningSelectedColor = Color.FromHex("#e8ebf6");
            HeaderData.AfternoonSelectedColor = Color.FromHex("#e8ebf6");
            HeaderData.EveningSelectedColor = Color.FromHex("#e8ebf6");
            HeaderData.BedtimeSelectedColor = Color.FromHex("#7986cb");

            HeaderData.MorningOpacity = 0.5f;
            HeaderData.AfternonnOpacity = 0.5f;
            HeaderData.EveningOpacity = 0.5f;
            HeaderData.BedtimeOpacity = 1f;
            DialogProvider.HideProgress();
        }
        public string ConvertMinIntoHoursDay(int minutes)
        {
            int tot_mins = minutes;
            int days = tot_mins / 1440;
            int hours = (tot_mins % 1440) / 60;
            int mins = tot_mins % 60;
            return string.Format("{0}D {1}HRS {2}MINS", days, hours, mins);
        }
        List<Medicine> GetMorningMedicines(RegimenRoot regimenRoot, DayTime dayTime)
        {
            List<Medicine> medicines = new List<Medicine>();
            List<RegimenRoot> RegimensList = new List<RegimenRoot>();
            //var list = Regimens.Where(x => x.trial.id == regimenRoot.trial.id).ToList();
            //foreach (var item10 in list)
            //{
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
            if (schedules != "undefined" && schedules != "")
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
                    medicine.conatiner_id = regimenRoot.containerId;
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

            // }
            return medicines;
        }

    }
    public class TrailHeadingBinding: ViewModelBase
    {
        string _timeName, _time;
        Color _subHeaderColor = Color.FromHex("#ffed8f"),_morningColor=Color.FromHex("#7986cb"),_afternoonColor=Color.FromHex("#e8ebf6"),_eveningColor=Color.FromHex("#e8ebf6"),_bedtimeColor=Color.FromHex("#e8ebf6");
        double _morningOpacity=1f, _afertnoonOpacity=0.5f, _eveningOpacity=0.5f, _bedtimeOpacity=0.5f;
        public Color SubHeaderColor { get { return _subHeaderColor; } set { _subHeaderColor = value; OnPropertyChanged("SubHeaderColor"); } }
        public string TimeName { get { return _timeName; } set { _timeName = value; OnPropertyChanged("TimeName"); } }
        public string Time { get { return _time; } set { _time = value; OnPropertyChanged("Time"); } }
        public ICommand MorningTapped { get; set; }
        public ICommand AfternoonTapped { get; set; }
        public ICommand EveningTapped { get; set; }
        public ICommand BedtimeTapped { get; set; }
        public Color MorningSelectedColor { get { return _morningColor; } set { _morningColor = value; OnPropertyChanged("MorningSelectedColor"); } }
        public Color AfternoonSelectedColor { get { return _afternoonColor; } set { _afternoonColor = value; OnPropertyChanged("AfternoonSelectedColor"); } }
        public Color EveningSelectedColor { get { return _eveningColor; } set { _eveningColor = value; OnPropertyChanged("EveningSelectedColor"); } }
        public Color BedtimeSelectedColor { get { return _bedtimeColor; } set { _bedtimeColor = value; OnPropertyChanged("BedtimeSelectedColor"); } }
        public double MorningOpacity { get { return _morningOpacity; } set { _morningOpacity = value; OnPropertyChanged("MorningOpacity"); } }
        public double AfternonnOpacity { get { return _afertnoonOpacity; } set { _afertnoonOpacity = value; OnPropertyChanged("AfternonnOpacity"); } }
        public double EveningOpacity { get { return _eveningOpacity; } set { _eveningOpacity = value; OnPropertyChanged("EveningOpacity"); } }
        public double BedtimeOpacity { get { return _bedtimeOpacity; } set { _bedtimeOpacity = value; OnPropertyChanged("BedtimeOpacity"); } }
    }
    public class Trial:Medicine,INotifyPropertyChanged
    {
        bool _isSubmenuVisible;
        TimeSpan _timeSpan,_startTime,_endTime;
        public DetailspopUpModel Data { get; set; }

        public ICommand ShowSubmenuCommand { get; set; }
        public ImageSource MedicineIcon { get; set; }
        public string MedicineName { get; set; }
        public string ContainerName { get; set; }
        public string Time { get; set; }
        public int Quantity { get; set; }
        public string Wieght { get; set; }
        public string DurationbetweenDoses { get; set; }
        public ICommand TimeChanged { get; set; }
        public TimeSpan StartTime { get { return _startTime; } set { _startTime = value; OnPropertyChanged("StartTime"); } }
        public TimeSpan EndTime { get { return _endTime; } set { _endTime = value; OnPropertyChanged("EndTime"); } }
        public DayTime TimeOfDay { get; set; }
        public TimeSpan TrialTime
        {
            get
            { return _timeSpan; }
            set
            {
                _timeSpan = value; OnPropertyChanged("TrialTime");
            }
        }
        public TimeSpan TrialTime1 { get; set; }
        public bool IsSubmenuVisible { get { return _isSubmenuVisible; } set { _isSubmenuVisible = value; OnPropertyChanged("IsSubmenuVisible"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
