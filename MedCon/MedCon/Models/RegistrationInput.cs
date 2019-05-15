using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Models
{
   public class RegistrationInput
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string sub { get; set; }
        public string emailAddress { get; set; }
        public int roleId { get; set; }
        public string roleName { get; set; }
        public string phoneNumber { get; set; }
        public string dob { get; set; }
        public string race { get; set; }
        public string gender { get; set; }
        public int countryId { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
    }
}
