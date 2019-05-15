using MedCon.LocalDB;
using MedCon.LocalDB.Tables;
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
    public class ScanConfirmationViewModel : ViewModelBase
    {
        private readonly IUpdateContainerService _updateContainerService;
        int _value1 = 1, _value2 = 0, _value3 = 2, ID;
        string doseTime,_doseWeight,_currentDate,_currentDay;
        TimeSpan _defaultTime;
        ImageSource _dosetakenImgSource, _medicineImage;
        public string PatientId { get; set; }
        public string ContainerId { get; set; }
        string _barcode, _typeText, _accurateText, _medicineName, _medicineType, _doseTotal, _doseTokenText;
        public string BarcodeText { get { return _barcode; } set { _barcode = value; OnPropertyChanged("BarcodeText"); } }
        public String TypeText { get { return _typeText; } set { _typeText = value; OnPropertyChanged("TypeText"); } }
        public string DoseTokenText { get { return _doseTokenText; } set { _doseTokenText = value; OnPropertyChanged("DoseTokenText"); } }
        public string AccurateType { get { return _accurateText; } set { _accurateText = value; OnPropertyChanged("AccurateType"); } }
        public ImageSource DoseTakenImage { get { return _dosetakenImgSource; } set { _dosetakenImgSource = value; OnPropertyChanged("DoseTakenImage"); } }
        public MedicineItem ScannedMedicine { get; set; }
        public ImageSource MedicineImage { get { return _medicineImage; } set { _medicineImage = value; OnPropertyChanged("MedicineImage"); } }
        public string MedicineName { get { return _medicineName; } set { _medicineName = value; OnPropertyChanged("MedicineName"); } }
        public string MedicineType { get { return _medicineType; } set { _medicineType = value; OnPropertyChanged("MedicineType"); } }
        public string DoseTotal { get { return _doseTotal; } set { _doseTotal = value; OnPropertyChanged("DoseTotal"); } }
        public string DoseWeight { get { return _doseWeight; } set { _doseWeight = value; OnPropertyChanged("DoseWeight"); } }
        public string CurrentDate { get { return _currentDate; } set { _currentDate = value; OnPropertyChanged("CurrentDate"); } }
        public string CurrentDay { get { return _currentDay; } set { _currentDay = value; OnPropertyChanged("CurrentDay"); } }
        public int MedicinePerDose { get; set; }
        public ICommand CancelCommand { get; set; }
        public int Value1 { get { return _value1; } set { _value1 = value; OnPropertyChanged("Value1"); } }
        public int Value2 { get { return _value2; } set { _value2 = value; OnPropertyChanged("Value2"); } }
        public int Value3 { get { return _value3; } set { _value3 = value; OnPropertyChanged("Value3"); } }
        public ICommand DecrementCommand { get; set; }
        public ICommand IncrementCommand { get; set; }
        public ICommand ConfirmDoseCommand { get; set; }
        public Profile ProfileData { get; set; }
        public ICommand ChangeTimeCommand { get; set; }
        public TimeSpan DefaultTime { get { return _defaultTime; } set { _defaultTime = value; OnPropertyChanged("DefaultTime"); } }
        public ICommand TimeChanged { get; set; }
        DateTime dt;
        public ScanConfirmationViewModel(IUpdateContainerService updateContainerService)
        {
            GetProfile();
            _updateContainerService = updateContainerService;
            ConfirmDoseCommand = new Command(async () => await ConfirmDoseAsync());
            DecrementCommand = new Command<string>(DecrementQuantity);
            IncrementCommand = new Command<string>(IncrementQuantity);
            CancelCommand = new Command(CancelDose);
            dt = DateTime.Parse(Constants.SelectedDate);
            dt = dt + DateTime.Now.TimeOfDay;
            CurrentDate = dt.ToString(GlobalSettings.MedConDateFormat);
            CurrentDay = dt.ToString("dddd, hh:mm ttt");
            DefaultTime = dt.TimeOfDay;
            ChangeTimeCommand = new Command(ChangeTime);
            TimeChanged = new Command(TimeChangedEvent);
        }
        void ChangeTime()
        {
            MessagingCenter.Send("1", Constants.FocusChangeTimePickerKey);
        }
        void TimeChangedEvent(object obj)
        {
            FocusEventArgs focusEventArgs = (FocusEventArgs)obj;
            var control = focusEventArgs.VisualElement as TimePicker;
            dt = dt.Date+control.Time;
            CurrentDate = dt.ToString(GlobalSettings.MedConDateFormat);
            CurrentDay = dt.ToString("dddd, hh:mm ttt");
        }
        private async Task ConfirmDoseAsync()
        {
            try
            {
                string doseType = GetDoseType1(AccurateType);
                //  doseType = DetermineDoseType();
                //string doseStatusImage = DetermineDoseImage(doseType);
                string doseStatusImage = DoseTakenImage.ToString().Replace("File: ", "");
                string scanType = string.Empty;
                if (ScannedMedicine.IsScanVisible)
                    scanType = "SCAN";
                else
                    scanType = "MANUAL";
                string windowname = DeterminingDoseWindowName();
                DialogProvider.ShowProgress();

                bool result = await _updateContainerService.UpdateContainer(ScannedMedicine.RegimenData.patientNum, ContainerId, ScannedMedicine.TrialId, scanType, doseType, dt.ToString("yyyy-MM-dd HH:mm:ss"), 0, ProfileData.userId, ID, windowname, DoseTotal, ScannedMedicine.CompanyId, doseTime, doseStatusImage);
                //bool result = true;
                if (result)
                {
                    HistotyTable histotyTable = new HistotyTable();
                    histotyTable.ContainerId = ContainerId;
                    histotyTable.DoseTakenStatusImage = doseStatusImage;
                    histotyTable.DoseTakenTime = DateTime.Now.ToString();
                    histotyTable.DoseTime = doseTime;
                    //  histotyTable.DoseType = doseType;
                    histotyTable.DoseType = GetDoseType1(AccurateType);
                    histotyTable.DrugImage = ScannedMedicine.medicine_image.ToString();
                    histotyTable.DrugName = ScannedMedicine.name;
                    histotyTable.DrugType = ScannedMedicine.type;
                    if (ScannedMedicine.IsScanVisible)
                        histotyTable.Entry = "S";
                    else
                        histotyTable.Entry = "M";
                    histotyTable.WindowColor = DeterminingDoseWindowColor();
                    histotyTable.Str = Value2 + "x" + ScannedMedicine.DrugWeight + "MG";
                    histotyTable.TotalDoses = ScannedMedicine.TotalDoses;
                    histotyTable.RemainingDoses = ScannedMedicine.RemainingDoses - Value2;
                    histotyTable.Win = DeterminingDoseWindow();
                    histotyTable.TrialId = ScannedMedicine.TrialId;
                    histotyTable.TotalDoseAmount = (Value2 * int.Parse(ScannedMedicine.DrugWeight)).ToString();
                  //  SqliteService.InsertHistory(histotyTable);
                    await NavigationService.NavigateBackAsync();
                }
                else
                    DialogProvider.DisplayNativeAlert("Failed to update dose!", "OK");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                DialogProvider.HideProgress();
            }
        }
        async void GetProfile()
        {
            JObject profileResponse = await requestProvider.GetAsync<JObject>(Constants.ApiBase + "user/patient/profile");
             ProfileData = JsonConvert.DeserializeObject<Profile>(profileResponse.ToString());

        }
        string DeterminingDoseWindow()
        {
            string win = string.Empty;
            var imageSource = ScannedMedicine.WindowImage.ToString().Replace("File: ", "");
            switch (imageSource)
            {
                case "morning.png":
                    win = "M";
                    break;
                case "afternoon.png":
                    win = "A";
                    break;
                case "evening.png":
                    win = "E";
                    break;
                case "bedtime.png":
                    win = "B";
                    break;
            }
            return win;
        }
        bool IsLate()
        {
            int hour = dt.Hour;
            bool _isLate = true;
            string win = string.Empty;
            var imageSource = ScannedMedicine.WindowImage.ToString().Replace("File: ", "");
            switch (imageSource)
            {
                case "morning.png":
                    if (hour >= 6 && hour < 12)
                        _isLate = false;
                    break;
                case "afternoon.png":
                    if (hour >= 12 && hour < 16)
                        _isLate = false;
                    break;
                case "evening.png":
                    if (hour >= 16 && hour < 20)
                        _isLate = false;
                    break;
                case "bedtime.png":
                    if (hour >= 20 && hour < 24)
                        _isLate = false;
                    break;
            }
            return _isLate;
        }
        string DeterminingDoseWindowName()
        {
            string win = string.Empty;
            var imageSource = ScannedMedicine.WindowImage.ToString().Replace("File: ", "");
            switch (imageSource)
            {
                case "morning.png":
                    win = "morning";
                    break;
                case "afternoon.png":
                    win = "afternoon";
                    break;
                case "evening.png":
                    win = "evening";
                    break;
                case "bedtime.png":
                    win = "bedtime";
                    break;
            }
            return win;
        }
        string DeterminingDoseWindowColor()
        {
            string windowColor = string.Empty;
            var imageSource = ScannedMedicine.WindowImage.ToString().Replace("File: ", "");
            switch (imageSource)
            {
                case "morning.png":
                    windowColor = GetHexString((Color)Application.Current.Resources["MorningColor"]);
                    break;
                case "afternoon.png":
                    windowColor = GetHexString((Color)Application.Current.Resources["AfternoonColor"]);
                    break;
                case "evening.png":
                    windowColor = GetHexString((Color)Application.Current.Resources["EveningColor"]);
                    break;
                case "bedtime.png":
                    windowColor = GetHexString((Color)Application.Current.Resources["BedtimeColor"]);
                    break;
            }
            return windowColor;
        }
        public static string GetHexString(Xamarin.Forms.Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";

            return hex;
        }
        private void DecrementQuantity(string qty)
        {
            if (qty == "0") return;
            if (int.Parse(qty) > MedicinePerDose)
            {
                TypeText = "WARNING!";
                AccurateType = "OVERDOSE!!";
                DoseTakenImage = "overdose";
                DoseTokenText = "Improper dose taken";
            }
            else if (int.Parse(qty) < MedicinePerDose)
            {
                TypeText = "WARNING!";
                AccurateType = "UNDER Dose!!";
                DoseTakenImage = "overdose";
                DoseTokenText = "Improper dose taken";
            }
            else
            {
                DisplayDefault();
                //TypeText = "AWESOME!";
                //AccurateType = "ON-TIME";
                //DoseTakenImage = "ontime";
                //DoseTokenText = "Dose Taken";
            }
            Value2 = int.Parse(qty);
            Value1 = Value2 - 1;
            Value3 = Value2 + 1;
            DoseTotal = (int.Parse(ScannedMedicine.DrugWeight) * Value2).ToString() + " MG";

        }
        private void IncrementQuantity(string qty)
        {
            if (int.Parse(qty) > MedicinePerDose)
            {
                TypeText = "WARNING!";
                AccurateType = "OVERDOSE!!";
                DoseTakenImage = "overdose";
                DoseTokenText = "Improper dose taken";
            }
            else if (int.Parse(qty) < MedicinePerDose)
            {
                TypeText = "WARNING!";
                AccurateType = "UNDER DOSE!!";
                DoseTakenImage = "overdose";
                DoseTokenText = "Improper dose taken";
            }
            else
            {
                DisplayDefault();
                //TypeText = "AWESOME!";
                //AccurateType = "ON-TIME";
                //DoseTakenImage = "ontime";
                //DoseTokenText = "Dose Taken";               
            }
            Value2 = int.Parse(qty);
            Value3 = Value2 + 1;
            Value1 = Value2 - 1;
            DoseTotal = (int.Parse(ScannedMedicine.DrugWeight) * Value2).ToString() + " MG";
        }
        private void CancelDose()
        {
            NavigationService.NavigateBackAsync();
        }
        string GetThresholdDoseTime()
        {
            var alerts = SqliteService.GetAlerts1();
            var obj = alerts.Where(x => x.ContainerId == ScannedMedicine.container_id).FirstOrDefault();
            if (obj != null)
                return obj.thresholddosetime.ToString();
            return null;
        }
        public override Task InitializeAsync(object navigationData)
        {
            ScannedMedicine = (MedicineItem)navigationData;
            MedicineImage = ScannedMedicine.medicine_image;
            MedicineName = ScannedMedicine.name;
            MedicineType = ScannedMedicine.type;
            DoseTotal = ScannedMedicine.DoseTotal;
            MedicinePerDose = ScannedMedicine.medicinePerDose;
            ID = ScannedMedicine.ID;
            PatientId = ScannedMedicine.patientId;
            ContainerId = ScannedMedicine.container_id;
            doseTime = ScannedMedicine.time;
            DoseWeight = ScannedMedicine.DoseTotal;
            IncrementQuantity(ScannedMedicine.medicinePerDose.ToString());
            return base.InitializeAsync(navigationData);
        }
        void DisplayDefault()
        {
            if (ScannedMedicine.IsScanVisible)
                Title = "Confirmation";
            else
                Title = "Manual";
            string abc = DetermineDoseType();
            if (abc == "LATE")
            {
                TypeText = "WARNING!";
                AccurateType = "LATE DOSE!!";
                DoseTakenImage = "overdose";
                DoseTokenText = "Improper dose taken";
            }
            else if (abc == "MISSED")
            {
                TypeText = "OH NO!";
                AccurateType = "MISSED DOSE!!";
                DoseTakenImage = "missed";
                DoseTokenText = "Proper time to take your dose has passed";
            }
            else
            {
                TypeText = "AWESOME!";
                AccurateType = "ON-TIME";
                DoseTakenImage = "ontime";
                DoseTokenText = "Dose Taken";
            }
        }
        string GetDoseType(string doseType)
        {
            return doseType.Replace("!!", "");
        }
        string GetDoseType1(string type1)
        {
            string type = string.Empty;
            switch (type1)
            {
                case "ON-TIME":
                    type = DoseType.ONTIME.ToString();
                    break;
                case "MISSED DOSE!!":
                    type = DoseType.MISSED.ToString();
                    break;
                case "LATE DOSE!!":
                    type = DoseType.LATE.ToString();
                    break;
                case "OVERDOSE!!":
                    type = DoseType.OVERDOSE.ToString();
                    break;
                case "UNDER DOSE!!":
                    type = DoseType.UNDERDOSE.ToString();
                    break;
            }
            return type;
        }
        string DetermineDoseType()
        {
            string ActualTime = ScannedMedicine.time;
            string validTime = ActualTime.Remove(ActualTime.Length - 3, 1);
            validTime = validTime.Insert(validTime.Length - 2, " ");
            DateTime a = DateTime.Parse(validTime);
            DateTime b = DateTime.Now;
            var minutes = (b.Subtract(a).TotalMinutes);
            bool sddsdsd = IsLate();

            //else if (minutes <= alertTable.predosetime&&minutes>0)
            if (DateTime.Now.Minute == a.Minute && DateTime.Now.Hour == a.Hour|| minutes <= ScannedMedicine.RegimenData.alertmessage.thresholddosetime && minutes > 0)
                return "On-Time";
           // else if (minutes <= ScannedMedicine.RegimenData.alertmessage.thresholddosetime && minutes > 0)
           else if(!IsLate())
                return "LATE";
            else
                return DoseType.MISSED.ToString();
        }
        public static string DetermineDoseImage(string dosetype1)
        {
            if (dosetype1 == DoseType.ONTIME.ToString())
                return "ontime.png";
            else if (dosetype1 == DoseType.MISSED.ToString())
                return "missed.png";
            else if (dosetype1 == DoseType.OVERDOSE.ToString())
                return "overdose.png";
            else return "missed.png";
        }
    }

    public enum DoseType
    {
        ONTIME, OVERDOSE, MISSED, LATE, UNDERDOSE
    }
}
