using MedCon.Models;
using MedCon.Services.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Services.Interfaces
{
   public interface IRegistrationService
    {
        Task<JObject> RegisterAsync(RegistrationInput registrationInput);
        Task<string> GetLoginToken(string username, string password);

        Task DeleteCognitoUser();
    }
}
