using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Services
{
   public interface IUpdateContainerService
    {
        void UpdateContainerDB(string patientId,string containerId,int trialId,string scanType,string doseType,string time,int extra,int userId);
      Task<bool> UpdateContainer(string patientId, string containerId, int trialId, string scanType, string doseType, string time, int extra, int userId, int id,string doseTye,string doseAmount,int companyID,string doseTime,string doseStatusImage);
    }
}
