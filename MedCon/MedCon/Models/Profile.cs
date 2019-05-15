using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Models
{
   public class Profile
    {
        public bool status { get; set; }
        public int userId { get; set; }
        public string emailAddress { get; set; }
        public string gender { get; set; }
        public string sub { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public object age { get; set; }
        public string race { get; set; }
        public string phoneNumber { get; set; }
        public string location { get; set; }
        public int roleId { get; set; }
        public string roleName { get; set; }
    }
}
