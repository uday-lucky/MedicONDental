using MedCon.Models;
using MedCon.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamForms.Controls;

namespace MedCon.ViewModels
{
    public class DashboardCalendarViewModel:ViewModelBase
    {
        string _selectedDay, _selectedMonth;
        DashboardCalendarData _calendarData;
        public ObservableCollection<SpecialDate> MyDates { get; set; }
        public string SelectedDay { get { return _selectedDay; } set { _selectedDay = value; OnPropertyChanged("SelectedDay"); } }
        public string SelectedMonth { get { return _selectedMonth; } set { _selectedMonth = value; OnPropertyChanged("SelectedMonth"); } }
        public ICommand DateSelectionCommand { get; set; }
        private DateTime _date;
        public DashboardCalendarData CalendarData { get { return _calendarData; } set { _calendarData = value; OnPropertyChanged("CalendarData"); } }
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public DashboardCalendarViewModel()
        {
            MessagingCenter.Subscribe<string>(this, Constants.UpdateDashboardCalendar, (value) =>
            {
                SelectedMethod(value);

            });
            //  DateSelectionCommand = new Command<Object>(SelectedMethod);
            SelectedDay =DateTime.Now.ToString("dd,dddd");
            SelectedMonth =DateTime.Now.ToString("MMMM,yyyy");
            MyDates = new ObservableCollection<SpecialDate>();
           // CalendarData = new DashboardCalendarData();
            //MyDates.Add(new SpecialDate(DateTime.Now.AddDays(-5)) { BackgroundColor = Color.FromHex("#ff9800"), TextColor = Color.White });
            //MyDates.Add(new SpecialDate(DateTime.Now.AddDays(10)) { BackgroundColor = Color.FromHex("#4caf50"), TextColor = Color.White });
            //MyDates.Add(new SpecialDate(DateTime.Now.AddDays(-15)) { BackgroundColor = Color.FromHex("#7986CB"), TextColor = Color.White });
        }
        void SelectedMethod(string obj)
        {
            Date = DateTime.Parse(obj);
            string selectedDate = Date.ToString();
            SelectedDay = Date.ToString("dd,dddd");
            SelectedMonth = Date.ToString("MMMM,yyyy");
            //MessagingCenter.Send(Date.ToString(), Constants.UpdateDashboardDateKey);
        }
        public override Task InitializeAsync(object navigationData)
        {
            CalendarData = (DashboardCalendarData)navigationData;
            return base.InitializeAsync(navigationData);
        }
    }
}
