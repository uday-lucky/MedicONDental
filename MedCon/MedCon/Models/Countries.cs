using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon.Models
{  
    public class Country
    {
        public int phonecode { get; set; }
        public int id { get; set; }
        public string sortname { get; set; }
        public string name { get; set; }
        public string NameWithCode { get; set; }
    }
    public class CountriesAndRaces
    {
        public List<Country> CountriesList { get; set; }
        public List<Race> RacesList { get; set; }
    }
}
