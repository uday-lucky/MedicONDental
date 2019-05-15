using MedCon.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MedCon.Models
{
   public class MedicineItem:ViewModelBase
    {
        ImageSource _imageSource,_medicineImage;
        string _time1;
        bool _isScanVisible=true, _isManualVisible;
        public ICommand DetailsCommand { get; set; }

        public int CompanyId { get; set; }
        public int TrialId { get; set; }
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string wieght { get; set; }
        public string time { get; set; }
        public string time1 { get { return _time1; } set { _time1 = value; OnPropertyChanged("time1"); } }
        public ImageSource medicine_image { get { return _medicineImage; } set { _medicineImage = value; OnPropertyChanged("medicine_image"); } }
        public string container_id { get; set; }
        public ICommand ScanCommand { get; set; }
        public string type { get; set; }
        public string DoseTotal { get; set; }
        public int medicinePerDose { get; set; }
        public string patientId { get; set; }
        public int RemainingDoses { get; set; }
        public int TotalDoses { get; set; }
        public ImageSource StatusImage { get { return _imageSource; } set { _imageSource = value; OnPropertyChanged("StatusImage"); } }
        public string DrugWeight { get; set; }
        public ImageSource WindowImage { get; set; }
        public bool IsScanVisible { get { return _isScanVisible; } set { _isScanVisible = value; OnPropertyChanged("IsScanVisible"); } }
        public bool IsManualVisible { get { return _isManualVisible; } set { _isManualVisible = value; OnPropertyChanged("IsManualVisible"); } }
        public RegimenRoot RegimenData { get; set; }
    }
}
