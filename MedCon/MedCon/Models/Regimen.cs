using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Models
{
    public class Regimen
    {
        public string afternoon_schedule { get; set; }
        public int thresholdTime { get; set; }
        public string bedtime_schedule { get; set; }
        public string monthly_yearly { get; set; }
        public int extraDoses { get; set; }
        public int medicationPerDose { get; set; }
        public int totalDoses { get; set; }
        public int id { get; set; }
        public string morning_schedule { get; set; }
        public string name { get; set; }
        public int durationBetweenDoses { get; set; }
        public string evening_schedule { get; set; }
        public string day_of_week { get; set; }
        public string schedule_type { get; set; }
    }

    public class Period
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class Drug
    {
        public string image { get; set; }
        public string medicationtype { get; set; }
        public string alias { get; set; }
        public string amount { get; set; }
        public string drugname { get; set; }
        public int id { get; set; }
        public string unit { get; set; }
    }

    public class Trial
    {
        public string startDate { get; set; }
        public string manualdose { get; set; }
        public string endDate { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Alertmessage
    {
        public string predosetext { get; set; }
        public int thresholddosetime { get; set; }
        public string thresholddosetext { get; set; }
        public int thresholdduetime { get; set; }
        public string ontimedosetext { get; set; }
        public int predosetime { get; set; }
    }

    public class RegimenRoot
    {
        public string description { get; set; }
        public int statusCode { get; set; }
        public string PatientId { get; set; }
        public Trial trial { get; set; }
        public Regimen regimen { get; set; }
        public Alertmessage alertmessage { get; set; }
        public string containerId { get; set; }
        public string labelId { get; set; }
        public string patientNum { get; set; }
        public int companyId { get; set; }
        public Drug drug { get; set; }
        public string companyName { get; set; }
        public Period period { get; set; }
    }
}
