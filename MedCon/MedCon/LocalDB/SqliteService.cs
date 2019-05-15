using MedCon.Interfaces;
using MedCon.LocalDB.Tables;
using MedCon.Models;
using MedCon.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.LocalDB
{
    public class SqliteService
    {
        private static object collisionLock = new object();
        public static SQLiteConnection GetConnection
        {
            get
            {
                var dependency = DependencyService.Get<ISqlite>();
                return dependency.GetConnection();
            }
        }
        public static void CreateTables()
        {
            //GetConnection.CreateTable<RegimenTable>();
            //GetConnection.CreateTable<TrialTable>();
            //GetConnection.CreateTable<Medicine>();
            //GetConnection.CreateTable<AlertTable>();
            //GetConnection.CreateTable<HistotyTable>();
            GetConnection.CreateTable<TrialTimesTable>();

        }
        public static void DeleteTables()
        {
            GetConnection.DropTable<RegimenTable>();
            GetConnection.DropTable<TrialTable>();
            GetConnection.DropTable<Medicine>();
            GetConnection.DropTable<AlertTable>();
            GetConnection.DropTable<HistotyTable>();
        }
        public static List<Medicine> GetMorningMedicines(int trialId)
        {
            lock (collisionLock)
            {
                var allMedicines = GetConnection.Table<Medicine>().ToList();
                return allMedicines.Where(x => x.DoseDayTime == "Morning" && x.PatientId == Constants.PresentPatientId && x.trial_id == trialId).ToList();
            }
        }
        public static List<Medicine> GetAfternoonMedicines(int trialId)
        {
            lock (collisionLock)
            {
                var allMedicines = GetConnection.Table<Medicine>().ToList();
                return allMedicines.Where(x => x.DoseDayTime == "Afternoon" && x.PatientId == Constants.PresentPatientId && x.trial_id == trialId).ToList();
            }
        }
        public static List<Medicine> GetEveningMedicines(int trialId)
        {
            lock (collisionLock)
            {
                var allMedicines = GetConnection.Table<Medicine>().ToList();
                return allMedicines.Where(x => x.DoseDayTime == "Evening" && x.PatientId == Constants.PresentPatientId && x.trial_id == trialId).ToList();
            }
        }
        public static List<Medicine> GetBedtimeMedicines(int trialId)
        {
            lock (collisionLock)
            {
                var allMedicines = GetConnection.Table<Medicine>().ToList();
                return allMedicines.Where(x => x.DoseDayTime == "Bedtime" && x.PatientId == Constants.PresentPatientId&&x.trial_id==trialId).ToList();
            }
        }
        public void InsertStaticData(RegimenRoot regimenRoot)
        {
            InsertRegimen(regimenRoot);
            InsertTrials(regimenRoot);
            if (regimenRoot.regimen.morning_schedule != "undefined")
            {
                List<string> morningTimes = regimenRoot.regimen.morning_schedule.Split(',').ToList();
                InsertMedicines(morningTimes, regimenRoot, DayTime.Morning);

            }
            if (regimenRoot.regimen.afternoon_schedule != "undefined")
            {
                List<string> afternoonTimes = regimenRoot.regimen.afternoon_schedule.Split(',').ToList();
                InsertMedicines(afternoonTimes, regimenRoot, DayTime.Afternoon);
            }
            if (regimenRoot.regimen.evening_schedule != "undefined")
            {
                List<string> eveningTimes = regimenRoot.regimen.evening_schedule.Split(',').ToList();
                InsertMedicines(eveningTimes, regimenRoot, DayTime.Evening);
            }
            if (regimenRoot.regimen.evening_schedule != "undefined")
            {
                List<string> bedtimeTimes = regimenRoot.regimen.bedtime_schedule.Split(',').ToList();
                InsertMedicines(bedtimeTimes, regimenRoot, DayTime.Bedtime);
            }

            InsertAlerts(regimenRoot);

            //InsertAfternoonMedicines(afternoonTimes,regimenRoot,DayTime.Afternoon);
            //InsertEveningMedicines(eveningTimes,regimenRoot,DayTime.Evening);
            //InsertBedtimeMedicines(bedtimeTimes,regimenRoot,DayTime.Bedtime);
        }
        void InsertMedicines(List<string> doseTimes, RegimenRoot regimenRoot, DayTime time)
        {
            lock (collisionLock)
            {
                foreach (var item in doseTimes)
                {

                    Medicine morningMedicines = new Medicine();
                    morningMedicines.CompanyId = regimenRoot.companyId;
                    morningMedicines.PatientId = regimenRoot.PatientId;
                    morningMedicines.conatiner_id = regimenRoot.containerId;
                    morningMedicines.image = Constants.MedicineImageBase + regimenRoot.drug.image;
                    // if(regimenRoot.drug.medicationtype== "Tablet")
                    // morningMedicines.image ="tablet.png";
                    //else if (regimenRoot.drug.medicationtype == "Capsule")
                    //     morningMedicines.image = "capsule.png";
                    // else if (regimenRoot.drug.medicationtype == "Powder")
                    //     morningMedicines.image = "powder.png";
                    morningMedicines.id = regimenRoot.drug.id;
                    morningMedicines.status_image = "";
                    morningMedicines.drugname = regimenRoot.drug.drugname;
                    morningMedicines.alias = regimenRoot.drug.alias;
                    morningMedicines.medicationtype = regimenRoot.drug.medicationtype;
                    morningMedicines.unit = regimenRoot.drug.unit;
                    morningMedicines.amount = regimenRoot.drug.amount;
                    morningMedicines.trial_id = regimenRoot.trial.id;
                    morningMedicines.dose_time = item;
                    morningMedicines.DoseDayTime = time.ToString();
                    morningMedicines.medicineperdose = regimenRoot.regimen.medicationPerDose;
                    morningMedicines.total_doses = regimenRoot.regimen.totalDoses;
                    morningMedicines.extra_doses = regimenRoot.regimen.extraDoses;
                
                    GetConnection.Insert(morningMedicines);
                }
            }
        }
        void InsertAfternoonMedicines(List<string> doseTimes, RegimenRoot regimenRoot)
        {
            lock (collisionLock)
            {
                foreach (var item in doseTimes)
                {
                    Medicine morningMedicines = new Medicine();
                    morningMedicines.conatiner_id = regimenRoot.containerId;
                    morningMedicines.image = regimenRoot.drug.image;
                    morningMedicines.id = regimenRoot.drug.id;
                    morningMedicines.drugname = regimenRoot.drug.drugname;
                    morningMedicines.alias = regimenRoot.drug.alias;
                    morningMedicines.medicationtype = regimenRoot.drug.medicationtype;
                    morningMedicines.unit = regimenRoot.drug.unit;
                    morningMedicines.amount = regimenRoot.drug.amount;
                    morningMedicines.trial_id = regimenRoot.trial.id;
                    morningMedicines.dose_time = item;
                    morningMedicines.medicineperdose = regimenRoot.regimen.medicationPerDose;
                    morningMedicines.DoseDayTime = "Afternoon";
                    GetConnection.Insert(morningMedicines);
                }
            }
        }
        void InsertEveningMedicines(List<string> doseTimes, RegimenRoot regimenRoot)
        {
            lock (collisionLock)
            {
                foreach (var item in doseTimes)
                {
                    Medicine morningMedicines = new Medicine();
                    morningMedicines.conatiner_id = regimenRoot.containerId;
                    morningMedicines.image = regimenRoot.drug.image;
                    morningMedicines.id = regimenRoot.drug.id;
                    morningMedicines.drugname = regimenRoot.drug.drugname;
                    morningMedicines.alias = regimenRoot.drug.alias;
                    morningMedicines.medicationtype = regimenRoot.drug.medicationtype;
                    morningMedicines.unit = regimenRoot.drug.unit;
                    morningMedicines.amount = regimenRoot.drug.amount;
                    morningMedicines.trial_id = regimenRoot.trial.id;
                    morningMedicines.dose_time = item;
                    morningMedicines.medicineperdose = regimenRoot.regimen.medicationPerDose;
                    morningMedicines.DoseDayTime = "Evening";
                    GetConnection.Insert(morningMedicines);
                }
            }
        }
        void InsertBedtimeMedicines(List<string> doseTimes, RegimenRoot regimenRoot)
        {
            lock (collisionLock)
            {
                foreach (var item in doseTimes)
                {
                    Medicine morningMedicines = new Medicine();
                    morningMedicines.conatiner_id = regimenRoot.containerId;
                    morningMedicines.image = regimenRoot.drug.image;
                    morningMedicines.id = regimenRoot.drug.id;
                    morningMedicines.drugname = regimenRoot.drug.drugname;
                    morningMedicines.alias = regimenRoot.drug.alias;
                    morningMedicines.medicationtype = regimenRoot.drug.medicationtype;
                    morningMedicines.unit = regimenRoot.drug.unit;
                    morningMedicines.amount = regimenRoot.drug.amount;
                    morningMedicines.trial_id = regimenRoot.trial.id;
                    morningMedicines.dose_time = item;
                    morningMedicines.DoseDayTime = "Bedtime";
                    morningMedicines.medicineperdose = regimenRoot.regimen.medicationPerDose;
                    GetConnection.Insert(morningMedicines);
                }
            }
        }
        void InsertTrials(RegimenRoot regimenRoot2)
        {
            TrialTable trialTable = new TrialTable();
            trialTable.ContainerId = regimenRoot2.containerId;
            trialTable.EndDate = regimenRoot2.trial.endDate;
            trialTable.id = regimenRoot2.trial.id;
            trialTable.manualdose = regimenRoot2.trial.manualdose;
            trialTable.name = regimenRoot2.trial.name;
            trialTable.start_date = regimenRoot2.trial.startDate;
            trialTable.PatientId = regimenRoot2.PatientId;
            lock (collisionLock)
            {
                GetConnection.Insert(trialTable);
            }
        }
        void InsertRegimen(RegimenRoot regimenRoot1)
        {
            RegimenTable regimenTable = new RegimenTable();
            Constants.PresentPatientId = regimenTable.PatientId = regimenRoot1.PatientId;
            regimenTable.id = regimenRoot1.regimen.id;
            regimenTable.container_id = regimenRoot1.containerId;
            regimenTable.daysof_week = null;
            regimenTable.duration_between_doses = regimenRoot1.regimen.durationBetweenDoses;
            regimenTable.DurationBetweenDoses =ConvertMinIntoHoursDay(regimenRoot1.regimen.durationBetweenDoses);
            regimenTable.extra_doses = regimenRoot1.regimen.extraDoses;
            regimenTable.medication_per_dose = regimenRoot1.regimen.medicationPerDose;
            regimenTable.monthly_yearly = regimenRoot1.regimen.monthly_yearly;
            regimenTable.name = regimenRoot1.regimen.name;
            regimenTable.schedule_type = regimenRoot1.regimen.schedule_type;
            regimenTable.threshold_time = regimenRoot1.regimen.thresholdTime;
            regimenTable.total_doses = regimenRoot1.regimen.totalDoses;
            regimenTable.RemainingDoses = regimenRoot1.regimen.totalDoses;
            regimenTable.MorningSchedule = regimenRoot1.regimen.morning_schedule;
            regimenTable.AfternoonSchedule = regimenRoot1.regimen.afternoon_schedule;
            regimenTable.EveningSchedule = regimenRoot1.regimen.evening_schedule;
            regimenTable.BedtimeSchedule = regimenRoot1.regimen.bedtime_schedule;
            try
            {
                lock (collisionLock)
                {
                    GetConnection.Insert(regimenTable);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        void InsertAlerts(RegimenRoot regimenRoot)
        {
            AlertTable alertTable = new AlertTable();
            alertTable.ContainerId = regimenRoot.containerId;
            alertTable.ontimedosetext = regimenRoot.alertmessage.ontimedosetext;
            alertTable.predosetext = regimenRoot.alertmessage.predosetext;
            alertTable.predosetime = regimenRoot.alertmessage.predosetime;
            alertTable.thresholddosetext = regimenRoot.alertmessage.thresholddosetext;
            alertTable.thresholddosetime = regimenRoot.alertmessage.thresholddosetime;
            alertTable.thresholdduetime = regimenRoot.alertmessage.thresholdduetime;
            lock (collisionLock)
            {
                GetConnection.Insert(alertTable);
            }
        }
        public static AlertTable GetAlerts()
        {
            lock (collisionLock)
            {
                return GetConnection.Table<AlertTable>().FirstOrDefault();
            }
        }
        public static List<AlertTable> GetAlerts1()
        {
            lock (collisionLock)
            {
                return GetConnection.Table<AlertTable>().ToList();
            }
        }
        public static List<TrialTable> GetTrials()
        {
            lock (collisionLock)
            {
                return GetConnection.Table<TrialTable>().ToList();
            }
        }
        public static int InsertHistory(HistotyTable histotyTable)
        {
            lock (collisionLock)
            {
                return GetConnection.Insert(histotyTable);
            }
        }
        public static List<HistotyTable> GetTodayHistory(string containerId)
        {
            lock (collisionLock)
            {
                var history = GetConnection.Table<HistotyTable>().ToList();
                if (history != null && history.Count > 0)
                    history = history.Where(x => DateTime.Parse(x.DoseTakenTime).Month == DateTime.Now.Month && DateTime.Parse(x.DoseTakenTime).Day == DateTime.Now.Day && x.ContainerId == containerId).ToList();
                return history;
            }
        }
        public static List<HistotyTable> GetHistory()
        {
            lock (collisionLock)
            {
                var history = GetConnection.Table<HistotyTable>().ToList();
                return history;
            }
        }
        public static List<RegimenTable> GetRegimen()
        {
            lock (collisionLock)
            {
                var regimen = GetConnection.Table<RegimenTable>().ToList();
                return regimen;
            }
        }
        public static int UpdateRegimen(string container, string time, string image)
        {
            string query = string.Format("Update {0} SET status_image='{1}' WHERE conatiner_id='{2}' AND dose_time='{3}'", "Medicine", image, container, time);
            lock (collisionLock)
            {
                var list = GetConnection.Query<Medicine>(query);
                return list.Count;
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
        public static int InsertTrialTime(TrialTimesTable trialTimesTable)
        {
            lock (collisionLock)
            {
                return GetConnection.Insert(trialTimesTable);
            }
        }
        public static string GetUpdatedTimes(string containerId,string window)
        {
            string time = string.Empty;
            lock (collisionLock)
            {
                var times = GetConnection.Table<TrialTimesTable>().ToList();
                if(times.Count>0)
                {
                    var obj = times.Where(x => x.ContainerId == containerId && x.Window == window).LastOrDefault();
                    if (obj != null)
                        time= obj.Time;
                }
                return time;
            }
        }
    }
}
