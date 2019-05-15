using MedCon.Models;
using MedCon.Services.Base;
using MedCon.Services.Interfaces;
using MedCon.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IRequestProvider _requestProvider;
        public DashboardService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<RegimenRoot> GetAllRegimen(string patientId)
        {
            RegimenRoot regimen = await _requestProvider.GetAsync<RegimenRoot>(Constants.ContainerApiBase + "/container/info?patientnum=" + patientId);
            return regimen;
        }

        public async Task<List<PatientIdsData>> GetTrials()
        {

            List<PatientIdsData> jarry = new List<PatientIdsData>();
            jarry = await _requestProvider.GetAsync<List<PatientIdsData>>(Constants.ApiBase + "user/patient/attached");
            return jarry;
        }
    }
}
