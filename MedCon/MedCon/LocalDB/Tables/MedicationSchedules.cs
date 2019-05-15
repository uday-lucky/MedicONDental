using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.LocalDB.Tables
{
    public class TrialTimesTable
    {
        public string ContainerId { get; set; }
        public string Window { get; set; }
        public string Time { get; set; }
    }
   public class Medicine
    {

        [NotNull]
        public int CompanyId { get; set; }
        [NotNull]
        public string PatientId { get; set; }
        [NotNull]
        public string conatiner_id { get; set; }
        [NotNull]
        public ImageSource image { get; set; }
        [NotNull]
        public String status_image { get; set; }
        [NotNull]
        public String medicationtype { get; set; }
        [NotNull]
        public String alias { get; set; }
        [NotNull]
        public String amount { get; set; }
        [NotNull]
        public String drugname { get; set; }
        [NotNull]
        public int id { get; set; }
        [NotNull]
        public String unit { get; set; }
        [NotNull]
        public String dose_time { get; set; }
        [NotNull]
        public int trial_id { get; set; }
        [NotNull]
        public string DoseDayTime { get; set; }
        [NotNull]
        public int medicineperdose { get; set; }
        [NotNull]
        public int total_doses { get; set; }
        [NotNull]
        public int extra_doses { get; set; }
    }
    public class AfternoonMedicines
    {
        [NotNull]
        public string PatientId { get; set; }
        [NotNull]
        public string conatiner_id { get; set; }
        [NotNull]
        public String image { get; set; }
        [NotNull]
        public String medicationtype { get; set; }
        [NotNull]
        public String alias { get; set; }
        [NotNull]
        public String amount { get; set; }
        [NotNull]
        public String drugname { get; set; }
        [NotNull]
        public int id { get; set; }
        [NotNull]
        public String unit { get; set; }
        [NotNull]
        public String dose_time { get; set; }
        [NotNull]
        public int trial_id { get; set; }
    }
    public class EveningMedicines
    {
        [NotNull]
        public string PatientId { get; set; }
        [NotNull]
        public string conatiner_id { get; set; }
        [NotNull]
        public String image { get; set; }
        [NotNull]
        public String medicationtype { get; set; }
        [NotNull]
        public String alias { get; set; }
        [NotNull]
        public String amount { get; set; }
        [NotNull]
        public String drugname { get; set; }
        [NotNull]
        public int id { get; set; }
        [NotNull]
        public String unit { get; set; }
        [NotNull]
        public String dose_time { get; set; }
        [NotNull]
        public int trial_id { get; set; }
    }
    public class BedtimeMedicines
    {
        [NotNull]
        public string PatientId { get; set; }
        [NotNull]
        public string conatiner_id { get; set; }
        [NotNull]
        public String image { get; set; }
        [NotNull]
        public String medicationtype { get; set; }
        [NotNull]
        public String alias { get; set; }
        [NotNull]
        public String amount { get; set; }
        [NotNull]
        public String drugname { get; set; }
        [NotNull]
        public int id { get; set; }
        [NotNull]
        public String unit { get; set; }
        [NotNull]
        public String dose_time { get; set; }
        [NotNull]
        public int trial_id { get; set; }
    }
    public class TrialTable
    {
        [PrimaryKey]
        public int id { get; set; }
        [NotNull]
        public string ContainerId { get; set; }
        [NotNull]
        public string start_date { get; set; }
        [NotNull]
        public string EndDate { get; set; }        
        public string manualdose { get; set; }
        [NotNull]
        public string name { get; set; }
        [NotNull]
        public string PatientId { get; set; }
    }
    public class RegimenTable
    {
        [NotNull]
        public string PatientId { get; set; }
        [NotNull]
        public string container_id { get; set; }
        [NotNull]
        public int threshold_time { get; set; }
        public string monthly_yearly { get; set; }
        [NotNull]
        public int extra_doses { get; set; }
        [NotNull]
        public int medication_per_dose { get; set; }
        [NotNull]
        public int total_doses { get; set; }
        [NotNull]
        public int id { get; set; }
        [NotNull]
        public string name { get; set; }
        [NotNull]
        public int duration_between_doses { get; set; }
        public string daysof_week { get; set; }
        [NotNull]
        public string schedule_type { get; set; }
        [NotNull]
        public int RemainingDoses { get; set; }

        public string MorningSchedule { get; set; }
        public string AfternoonSchedule { get; set; }
        public string EveningSchedule { get; set; }
        public string BedtimeSchedule { get; set; }
        public string DurationBetweenDoses { get; set; }
    }
    public class AlertTable
    {
        [NotNull]
        public string predosetext { get; set; }

        [NotNull]
        public string ContainerId { get; set; }
        [NotNull]
        public string thresholddosetext { get; set; }
        [NotNull]
        public string ontimedosetext { get; set; }
        [NotNull]
        public int thresholddosetime { get; set; }
        [NotNull]
        public int thresholdduetime { get; set; }
        [NotNull]
        public int predosetime { get; set; }
    }
    public class HistotyTable
    {

        [NotNull]
        public string ContainerId { get; set; }
        [NotNull]
        public string DrugName { get; set; }
        [NotNull]
        public string DrugType { get; set; }
        [NotNull]
        public string DrugImage { get; set; }
        [NotNull]
        public int TotalDoses { get; set; }
        [NotNull]
        public int RemainingDoses { get; set; }
        [NotNull]
        public string DoseTakenStatusImage { get; set; }
        [NotNull]
        public string DoseTakenTime { get; set; }
        [PrimaryKey,AutoIncrement]
        public int Num { get; set; }
        [NotNull]
        public string Str { get; set; }
        [NotNull]
        public string DoseTime { get; set; }
        [NotNull]
        public string DoseType { get; set; }
        [NotNull]
        public string Win { get; set; }
        [NotNull]
        public string Entry { get; set; }

        [NotNull]
        public int TrialId { get; set; }

        [NotNull]
        public string TotalDoseAmount { get; set; }
        [NotNull]
        public string WindowColor { get; set; }
    }
    
}
