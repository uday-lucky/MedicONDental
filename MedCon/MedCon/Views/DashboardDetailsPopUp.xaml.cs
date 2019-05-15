using MedCon.LocalDB;
using MedCon.LocalDB.Tables;
using MedCon.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MedCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardDetailsPopUp : Rg.Plugins.Popup.Pages.PopupPage
    {

        public MedicineItem Medicine { get; set; }
        public DetailspopUpModel Data { get; set; }
        List<RegimenHistory> _regimenHistories;
        public List<RegimenRoot> Regimens { get; set; }

        public DashboardDetailsPopUp(MedicineItem medicineItem, List<RegimenHistory> regimenHistories,List<RegimenRoot> regimenRoots)
        {
            Regimens = regimenRoots;
            _regimenHistories = regimenHistories;
            Data = new DetailspopUpModel();
            Data.HistoryData = regimenHistories;
            Medicine = medicineItem;
            InitializeComponent();
            LoadData();
            CalculateDoses();
            BindingContext = this;
        }
        void LoadData()
        {
            //List<RegimenTable> regimenTables=  SqliteService.GetRegimen();
            //  var obj = regimenTables.Where(x => x.container_id == Medicine.container_id).FirstOrDefault();
            //  if(obj!=null)
            //  {
            Data.name = Medicine.name;
            Data.medicine_image = "";
            Data.medicine_image = Medicine.medicine_image;
            Data.StatusImage = Medicine.StatusImage;
            Data.RegimenName = Medicine.RegimenData.regimen.name;
            Data.MedicationPerDose = Medicine.RegimenData.regimen.medicationPerDose.ToString();
            Data.TotalDoses = Medicine.TotalDoses.ToString();
            Data.ExtraDoses = Medicine.RegimenData.regimen.extraDoses.ToString();
            Data.DurationBetweenDoses = ConvertMinIntoHoursDay(Medicine.RegimenData.regimen.durationBetweenDoses);
            Data.ScheduleType = Medicine.RegimenData.regimen.schedule_type;
            //  Data.DailyDosingSchedule = DetermingDoseWindow();
            Data.DailyDosingSchedule = Medicine.time;

            Data.CloseCommand = new Command(close);
            DetermingDoseWindow();
            foreach (var item in _regimenHistories)
            {
                CalculateDoses(item.category,item.doseAmount);
            }
            //  }
        }
        void CalculateDoses(string time,string amount)
        {
            switch (time)
            {
                case "normal":
                    string[] sArray = amount.Split(' ');
                    int doses = (int.Parse(sArray[0]) /int.Parse(Medicine.DrugWeight))/Medicine.medicinePerDose;
                    Data.OntimeNormal += doses;
                    break;
                case "missed":
                    string[] sArray1 = amount.Split(' ');
                    int doses1 = (int.Parse(sArray1[0]) / int.Parse(Medicine.DrugWeight)) / Medicine.medicinePerDose;
                    Data.MissedNormal += doses1;
                    break;
                case "late":
                    string[] sArray2 = amount.Split(' ');
                    int doses2 = (int.Parse(sArray2[0]) / int.Parse(Medicine.DrugWeight)) / Medicine.medicinePerDose;
                    Data.LateNormal += doses2;
                    break;
                case "overdose":
                    string[] sArray3 = amount.Split(' ');
                    int doses3 = (int.Parse(sArray3[0]) / int.Parse(Medicine.DrugWeight)) / Medicine.medicinePerDose;
                    Data.OverdoseNormal += doses3;
                    break;
                default:
                    break;
            }
        }
        void CalculateDoses()
        {
            if(_regimenHistories.Count==0)
            {
                Data.Doses = 11;
                Data.Medications = 10;
            }
            else
            {
                Data.Doses = Data.OntimeNormal+Data.OntimeExtra+Data.MissedNormal+Data.MissedExtra+Data.LateNormal+Data.LateExtra+Data.OverdoseNormal+Data.OverdoseExtra;
                Data.Medications = 1;
            }
        }
        public string ConvertMinIntoHoursDay(int minutes)
        {
            int tot_mins = minutes;
            int days = tot_mins / 1440;
            int hours = (tot_mins % 1440) / 60;
            int mins = tot_mins % 60;
            return string.Format("{0}D {1}HRS {2}MINS", days, hours, mins);
        }
        async void close()
        {
            await Navigation.PopPopupAsync();
        }
        string DetermingDoseWindow()
        {
            string dosingTimes = string.Empty;
            var imageSource = Medicine.WindowImage.ToString().Replace("File: ", "");
            switch (imageSource)
            {
                case "morning.png":
                    Data.WindowImage = "morning_white.png";
                    Data.WindowName = "Morning";
                    Data.HeaderTextColor = Color.Black;
                    // dosingTimes = regimenTable.MorningSchedule;
                    Data.WindowColor = Color.FromHex("#ffeb3b");
                    break;
                case "afternoon.png":
                    Data.WindowImage = "afternoon_white.png";
                    Data.WindowName = "Afternoon";
                    Data.HeaderTextColor = Color.White;
                    // dosingTimes = regimenTable.AfternoonSchedule;
                    Data.WindowColor = Color.FromHex("#03a9f4");

                    break;
                case "evening.png":
                    Data.WindowImage = "evening_white.png";
                    Data.WindowName = "Evening";
                    Data.HeaderTextColor = Color.White;
                    //  dosingTimes = regimenTable.EveningSchedule;
                    Data.WindowColor = Color.FromHex("#ff9800");
                    break;
                case "bedtime.png":
                    Data.WindowImage = "bedtime_white.png";
                    Data.WindowName = "Bedtime";
                    Data.HeaderTextColor = Color.White;
                    //  dosingTimes = regimenTable.BedtimeSchedule;
                    Data.WindowColor = Color.FromHex("#0277bd");
                    break;
            }
            return dosingTimes;
        }

    }
    public class DetailspopUpModel : MedicineItem
    {
        string _windowName;
        Color _windowColor, _headerTextColor = Color.White;
        bool _isDetailsVisible = true, _isAdherenceVisible, _isRemainingVisible;
        public string RegimenName { get; set; }
        public string MedicationPerDose { get; set; }
        public string TotalDoses { get; set; }
        public string ExtraDoses { get; set; }
        public string DurationBetweenDoses { get; set; }
        public string ScheduleType { get; set; }
        public string DailyDosingSchedule { get; set; }
        public string WindowName { get { return _windowName; } set { _windowName = value; OnPropertyChanged("WindowName"); } }
        public ICommand CloseCommand { get; set; }
        public Color WindowColor { get { return _windowColor; } set { _windowColor = value; OnPropertyChanged("WindowColor"); } }
        public Color HeaderTextColor { get { return _headerTextColor; } set { _headerTextColor = value; OnPropertyChanged("HeaderTextColor"); } }
        //public ICommand ShowSubmenuCommand { get; set; }
        //public ICommand DisplayAdherenceCommand { get; set; }
        //public ICommand DisplayDetailsCommand { get; set; }
        //public ICommand DisplayRemainingCommand { get; set; }
        //public bool IsDetailsVisible { get { return _isDetailsVisible; } set { _isDetailsVisible = value; OnPropertyChanged("IsDetailsVisible"); } }
        //public bool IsAdherenceVisible { get { return _isAdherenceVisible; } set { _isAdherenceVisible = value; OnPropertyChanged("IsAdherenceVisible"); } }
        //public bool IsRemainingVisible { get { return _isRemainingVisible; } set { _isRemainingVisible = value; OnPropertyChanged("IsRemainingVisible"); } }

        public int OntimeNormal { get; set; }
        public int OntimeExtra { get; set; }
        public int OverdoseNormal { get; set; }
        public int OverdoseExtra { get; set; }
        public int MissedNormal { get; set; }
        public int MissedExtra { get; set; }
        public int LateNormal { get; set; }
        public int LateExtra { get; set; }
        public int Doses { get; set; }
        public int Medications { get; set; }
        public int ExtraDosesRemaining { get; set; }
        public int ExtraMedicationss { get; set; }

        public List<RegimenHistory> HistoryData { get; set; }
    }
}