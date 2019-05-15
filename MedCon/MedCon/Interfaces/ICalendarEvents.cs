using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Interfaces
{
   public interface ICalendarEvents
    {
        void AddEvent(string title, string description, DateTime startDate, DateTime endDate,int remainderMin, string timeZone = "US/Eastern");
    }
}
