using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Models
{
     public class RegimenHistory
    {
        public string doseAmount { get; set; }
        public string patientNum { get; set; }
        public int trialId { get; set; }
        public int containerId { get; set; }
        public string category { get; set; }
        public string doseWindow { get; set; }
        public int companyId { get; set; }
        public int manual { get; set; }
        public string syncid { get; set; }
        public string time { get; set; }
        public string patientcontainerId { get; set; }
        public DateTime DoseDate { get; set; }
    }
}
