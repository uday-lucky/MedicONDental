using MedCon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Services
{
   public class DashboardHelper
    {
        RegimenRoot _regimenRoot;
        public DashboardHelper(RegimenRoot regimenRoot)
        {
            _regimenRoot = regimenRoot;
        }
        public DashboardDataMedicines GetMorningMedinices()
        {
            DashboardDataMedicines dashboardDataMedicine = new DashboardDataMedicines();
            if (_regimenRoot.regimen.morning_schedule != "undefined")
            {
                List<MedicineItem> medicineItems = new List<MedicineItem>();
                List<string> afternoonTimes = _regimenRoot.regimen.afternoon_schedule.Split(',').ToList();
            }
            if (_regimenRoot.regimen.afternoon_schedule != "undefined")
            {
               
            }
            if (_regimenRoot.regimen.evening_schedule != "undefined")
            {
               
            }
            if (_regimenRoot.regimen.evening_schedule != "undefined")
            {
               
            }
            return dashboardDataMedicine;
        }

        public class DashboardDataMedicines
        {
            public List<MedicineItem> MorningMedicines { get; set; }
            public List<MedicineItem> AfternoonMedicines { get; set; }
            public List<MedicineItem> EveningMedicines { get; set; }
            public List<MedicineItem> BedtimeMedicines { get; set; }

        }
    }
}
