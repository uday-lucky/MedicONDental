using MedCon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Interfaces
{
   public interface IHistoryDetails
    {
        Task<HistoryModel> GetSelectedHistoryAsync();
    }
}
