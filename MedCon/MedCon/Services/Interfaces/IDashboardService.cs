using MedCon.Models;
using MedCon.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Services.Interfaces
{
   public interface IDashboardService
    {
        Task<List<PatientIdsData>> GetTrials();
        Task<RegimenRoot> GetAllRegimen(string patientId);
    }
}
