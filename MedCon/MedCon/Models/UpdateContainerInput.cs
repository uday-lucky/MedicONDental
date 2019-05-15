using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Models
{
    public class UpdateContainerInput
    {
        public string patientId { get; set; }
        public string containerId { get; set; }
        public string patientcontainerId { get; set; }
        public string companyId { get; set; }
        public int trailId { get; set; }
        public string ScanType { get; set; }
        public string DoseType { get; set; }
        public string doseWindow { get; set; }
        public string doseAmount { get; set; }
        public string Time { get; set; }
        public int Extra { get; set; }
        public int userId { get; set; }
    }
}
