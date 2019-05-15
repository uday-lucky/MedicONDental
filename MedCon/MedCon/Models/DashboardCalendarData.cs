using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Models
{
   public class DashboardCalendarData
    {
        public ObservableCollection<RegimenRoot> Regimens { get; set; }
        public List<RegimenHistory> Histories { get; set; }
    }
}
