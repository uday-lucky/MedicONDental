using MedCon.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.Models
{
   public class DoseRemainder:ViewModelBase
    {
        ImageSource _medicineImage;
        string _doseName, _doseInTime, _time;
        bool _isVisible = true;
        Color _viewColor=Color.FromHex("#5C6BC0");
        public ImageSource MedicineImage { get { return _medicineImage; } set { _medicineImage = value; OnPropertyChanged("MedicineImage"); } }
        public string DoseName { get { return _doseName; } set { _doseName = value; OnPropertyChanged("DoseName"); } }
        public string DoseInTime { get { return _doseInTime; } set { _doseInTime = value; OnPropertyChanged("DoseInTime"); } }
        public string Time { get { return _time; } set { _time = value; OnPropertyChanged("Time"); } }
        public DateTime DoseTime { get; set; }
        public bool IsRemainderVisible { get { return _isVisible; } set { _isVisible = value; OnPropertyChanged("IsRemainderVisible"); } }
        public Color ViewColor { get { return _viewColor; } set { _viewColor = value; OnPropertyChanged("ViewColor"); } }
        public string drug_name { get; set; }
        public string dose_amount { get; set; }
        public string dose_unit { get; set; }
        public string total_dose { get; set; }
    }
}
