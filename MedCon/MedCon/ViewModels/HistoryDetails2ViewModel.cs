using MedCon.CustomControls;
using MedCon.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MedCon.ViewModels
{
   public class HistoryDetails2ViewModel:ViewModelBase
    {
        string _containerName,_startDate,_endDate;
        bool _isCalendarVisible, _isListVisible=true;
        ObservableCollection<CustomDate> _customdates1;
        public string ContainerName { get { return _containerName; } set { _containerName = value; OnPropertyChanged("ContainerName"); } }
        public string StartDate { get { return _startDate; } set { _startDate = value; OnPropertyChanged("StartDate"); } }
        public string EndDate { get { return _endDate; } set { _endDate = value; OnPropertyChanged("EndDate"); } }
        public ObservableCollection<ContainerData> ContainerMedicines { get; set; }
        public bool IsCalendarVisible { get { return _isCalendarVisible; } set { _isCalendarVisible = value; OnPropertyChanged("IsCalendarVisible"); } }
        public bool IsListVisible { get { return _isListVisible; } set { _isListVisible = value; OnPropertyChanged("IsListVisible"); } }
        public ObservableCollection<CustomDate> CustomDates { get { return _customdates1; } set { _customdates1 = value; OnPropertyChanged("CustomDates"); } }
        public ICommand CalendarTappedCommand { get; set; }

        public ObservableCollection<ContainerData> ContainerData { get; set; }
        public HistoryDetails2ViewModel()
        {
            ContainerMedicines = new ObservableCollection<ContainerData>();
            ContainerName = "Container0001";
            CalendarTappedCommand = new Command(CalendarTapped);
        }
        void CalendarTapped()
        {
            if(IsCalendarVisible)
            {
                IsCalendarVisible = false;
                IsListVisible = true;
            }
            else
            {
                IsCalendarVisible = true;
                IsListVisible = false;
            }

        }
       void InializeDates()
        {
            ObservableCollection<CustomDate> _customDates = new ObservableCollection<CustomDate>();
            foreach (var item in ContainerData)
            {
                item.DateOrTime = item.DateOrTime.Replace("| ", "");
                CustomDate customDate = new CustomDate();
                customDate.Date =DateTime.ParseExact(item.DateOrTime,"MM/dd/yyyy hh:mm tt",null);
                customDate.Image = item.DoseTimeImage;
                customDate.Data = new ContainerData();
                customDate.Data = item;
                _customDates.Add(customDate);
            }
            CustomDates = _customDates;
        }
        public override Task InitializeAsync(object navigationData)
        {
           var data = (ObservableCollection<ContainerData>)navigationData;
            ContainerData = data;
           ContainerName = data[0].Data.ContainerNum;
            StartDate = data[0].Data.StartDate;
            EndDate = data[0].Data.EndDate;
            foreach (var item in data)
            {
                ContainerMedicines.Add(item);
            }
            InializeDates();
            return base.InitializeAsync(navigationData);
        }
    }
}
