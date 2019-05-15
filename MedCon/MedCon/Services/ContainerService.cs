using MedCon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Services
{
    public class ContainerService :ViewModels.Base.ViewModelBase, IContainerService
    {

        public Task<string> AttachPatientToContainer(string containerId, string patientId)
        {
            return null;
        }

        public Task<string> ValidateContainer(string containerId)
        {
            return null;
        }
    }
}
