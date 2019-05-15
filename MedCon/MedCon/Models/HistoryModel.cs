using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedCon.Models
{
   public class HistoryModel
    {
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public String Status { get; set; }
        public string BackColor { get; set; }
        public ICommand GotoHistoryDetailsCommand { get; set; }
        public int TrialId { get; set; }
        public string PatientNum { get; set; }
        public string ContainerNum { get; set; }
    }
}
