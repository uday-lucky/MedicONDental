using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Models
{
  
    public class RegimenData
    {
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
