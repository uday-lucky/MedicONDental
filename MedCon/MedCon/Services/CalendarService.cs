using MedCon.Interfaces;
using MedCon.Models;
using MedCon.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.Services
{
    public class CalendarService
    {
        public ObservableCollection<RegimenRoot> Regimens { get; set; }
        public CalendarService(ObservableCollection<RegimenRoot> regimenRoots)
        {
            Regimens = regimenRoots;

            AddEvents();
        }
        async void AddEvents()
        {
            List<Event> Dates = new List<Event>();
            var dependancy = DependencyService.Get<ICalendarEvents>();
            if (Device.RuntimePlatform == Device.iOS)
                await Task.Delay(5000);
            foreach (var item1 in Regimens)
            {
                switch (item1.regimen.schedule_type)
                {
                    case "Daily":
                        Dates = GetDailyDates(item1);
                        break;
                    case "Weekly":
                        Dates = GetWeeklyDates(item1);
                        break;
                }
                foreach (var item in Dates)
                {
                    dependancy.AddEvent(item.Title, item.Description, item.StartDate, item.EndDate, item.RemainderMin);
                }

            }
            // List<Event> weeklyDates = GetWeeklyDates();

        }
        List<Event> GetDailyDates(RegimenRoot item)
        {
            List<Event> dates = new List<Event>();
            //foreach (var item in Regimens)
            //{
            string alreadyAddedRegimens = Helpers.Settings.CalendarAddedIds;
            if (!alreadyAddedRegimens.Contains(item.regimen.id.ToString()))
            {
                Helpers.Settings.CalendarAddedIds = alreadyAddedRegimens + "," + item.regimen.id;
                string morningTimes = item.regimen.morning_schedule;
                string afternoonTimes = item.regimen.afternoon_schedule;
                string eveningTimes = item.regimen.evening_schedule;
                string bedtimeTimes = item.regimen.bedtime_schedule;
                if (morningTimes != "undefined" && morningTimes != "")
                {
                    string[] morningTimesArray = item.regimen.morning_schedule.Split(',');
                    foreach (var time in morningTimesArray)
                    {
                        string[] times = time.Split(':');
                        if (times[0].Length == 1)
                            times[0] = times[0].Insert(0, "0");
                        if (times[1].Length == 1)
                            times[1] = times[1].Insert(0, "0");
                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                        DateTime d = DateTime.Parse(exactString);
                        TimeSpan timeSpan = new TimeSpan(0, 0, 0);
                        DateTime startDate = DateTime.Parse(item.trial.startDate).Date + timeSpan;
                        DateTime endDate = DateTime.Parse(item.trial.endDate).Date + timeSpan;
                        DateTime todayDate = d;
                        if (startDate <= todayDate && endDate >= todayDate)
                        {
                            startDate = todayDate;
                            while (startDate <= endDate.AddDays(1))
                            {
                                Event _event = new Event();
                                _event.StartDate = startDate.AddMinutes(-item.alertmessage.predosetime);
                                _event.EndDate = startDate.AddMinutes(item.alertmessage.thresholdduetime);
                                _event.RemainderMin = item.alertmessage.predosetime;
                                _event.Title = string.Format("Your need to take {0} of size {1} {2}x{3}", item.drug.drugname, item.drug.amount, "MG", item.regimen.medicationPerDose);
                                dates.Add(_event);
                                startDate = startDate.AddDays(1);
                            }
                        }
                    }
                }
                if (afternoonTimes != "undefined" && afternoonTimes != "")
                {
                    string[] afternoonTimesArray = item.regimen.afternoon_schedule.Split(',');
                    foreach (var time in afternoonTimesArray)
                    {
                        string[] times = time.Split(':');
                        if (times[0].Length == 1)
                            times[0] = times[0].Insert(0, "0");
                        if (times[1].Length == 1)
                            times[1] = times[1].Insert(0, "0");
                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                        DateTime d = DateTime.Parse(exactString); TimeSpan timeSpan = new TimeSpan(0, 0, 0);
                        DateTime startDate = DateTime.Parse(item.trial.startDate).Date + timeSpan;
                        DateTime endDate = DateTime.Parse(item.trial.endDate).Date + timeSpan;
                        DateTime todayDate = d;
                        if (startDate <= todayDate && endDate >= todayDate)
                        {
                            startDate = todayDate;
                            while (startDate <= endDate.AddDays(1))
                            {
                                Event _event = new Event();
                                _event.StartDate = startDate.AddMinutes(-item.alertmessage.predosetime);
                                _event.EndDate = startDate.AddMinutes(item.alertmessage.thresholdduetime);
                                _event.RemainderMin = item.alertmessage.predosetime;
                                _event.Title = string.Format("Your need to take {0} of size {1} {2}x{3}", item.drug.drugname, item.drug.amount, "MG", item.regimen.medicationPerDose);
                                dates.Add(_event);
                                startDate = startDate.AddDays(1);
                            }
                        }
                    }
                }
                if (eveningTimes != "undefined" && eveningTimes != "")
                {
                    string[] eveningTimesArray = item.regimen.evening_schedule.Split(',');
                    foreach (var time in eveningTimesArray)
                    {
                        string[] times = time.Split(':');
                        if (times[0].Length == 1)
                            times[0] = times[0].Insert(0, "0");
                        if (times[1].Length == 1)
                            times[1] = times[1].Insert(0, "0");
                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                        DateTime d = DateTime.Parse(exactString);
                        TimeSpan timeSpan = new TimeSpan(0, 0, 0);
                        DateTime startDate = DateTime.Parse(item.trial.startDate).Date + timeSpan;
                        DateTime endDate = DateTime.Parse(item.trial.endDate).Date + timeSpan;
                        DateTime todayDate = d;
                        if (startDate <= todayDate && endDate >= todayDate)
                        {
                            startDate = todayDate;
                            while (startDate <= endDate.AddDays(1))
                            {
                                Event _event = new Event();
                                _event.StartDate = startDate.AddMinutes(-item.alertmessage.predosetime);
                                _event.EndDate = startDate.AddMinutes(item.alertmessage.thresholdduetime);
                                _event.RemainderMin = item.alertmessage.predosetime;
                                _event.Title = string.Format("Your need to take {0} of size {1} {2}x{3}", item.drug.drugname, item.drug.amount, "MG", item.regimen.medicationPerDose);
                                dates.Add(_event);
                                startDate = startDate.AddDays(1);
                            }
                        }
                    }
                }
                if (bedtimeTimes != "undefined" && bedtimeTimes != "")
                {
                    string[] bedtimeTimesArray = item.regimen.bedtime_schedule.Split(',');
                    foreach (var time in bedtimeTimesArray)
                    {
                        string[] times = time.Split(':');
                        if (times[0].Length == 1)
                            times[0] = times[0].Insert(0, "0");
                        if (times[1].Length == 1)
                            times[1] = times[1].Insert(0, "0");
                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                        DateTime d = DateTime.Parse(exactString);
                        TimeSpan timeSpan = new TimeSpan(0, 0, 0);
                        DateTime startDate = DateTime.Parse(item.trial.startDate).Date + timeSpan;
                        DateTime endDate = DateTime.Parse(item.trial.endDate).Date + timeSpan;
                        DateTime todayDate = d;
                        if (startDate <= todayDate && endDate >= todayDate)
                        {
                            startDate = todayDate;
                            while (startDate <= endDate.AddDays(1))
                            {
                                Event _event = new Event();
                                _event.StartDate = startDate.AddMinutes(-item.alertmessage.predosetime);
                                _event.EndDate = startDate.AddMinutes(item.alertmessage.thresholdduetime);
                                _event.RemainderMin = item.alertmessage.predosetime;
                                _event.Title = string.Format("Your need to take {0} of size {1} {2}x{3}", item.drug.drugname, item.drug.amount, "MG", item.regimen.medicationPerDose);
                                dates.Add(_event);
                                startDate = startDate.AddDays(1);
                            }
                        }
                    }
                }
            }
            //  }
            return dates;
        }
        List<Event> GetWeeklyDates(RegimenRoot item)
        {
            List<Event> dates = new List<Event>();
            if (item.regimen.schedule_type == ScheduleType.Weekly.ToString())
            {
                string[] b = item.regimen.day_of_week.Split(',');
                var weekDays = new HashSet<string>(b);
                foreach (var weekday in weekDays)
                {
                    DateTime trialStartDate = DateTime.ParseExact(item.trial.startDate, "yyyy-MM-dd", null);
                    DateTime trialEndDate = DateTime.ParseExact(item.trial.endDate, "yyyy-MM-dd", null);
                    List<DateTime> dateTimes = new List<DateTime>();
                    TimeSpan timeSpan = new TimeSpan(0, 0, 0);
                    DateTime today = DateTime.Now.Date + timeSpan;
                    if (today<=(trialEndDate))
                    {
                        string morningTimes = item.regimen.morning_schedule;
                        string afternoonTimes = item.regimen.afternoon_schedule;
                        string eveningTimes = item.regimen.evening_schedule;
                        string bedtimeTimes = item.regimen.bedtime_schedule;
                        while (today<=trialEndDate)
                        {
                            if(today.ToString("ddd")==weekday)
                            {
                                if (morningTimes != "undefined" && morningTimes != "")
                                {
                                    string[] morningTimesArray = item.regimen.morning_schedule.Split(',');
                                    foreach (var time in morningTimesArray)
                                    {
                                        string[] times = time.Split(':');
                                        if (times[0].Length == 1)
                                            times[0] = times[0].Insert(0, "0");
                                        if (times[1].Length == 1)
                                            times[1] = times[1].Insert(0, "0");
                                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                                        DateTime d = DateTime.Parse(exactString);
                                        DateTime dd = today + d.TimeOfDay;
                                        Event _event = new Event();
                                        _event.StartDate = dd.AddMinutes(-item.alertmessage.predosetime);
                                        _event.EndDate = dd.AddMinutes(item.alertmessage.thresholdduetime);
                                        _event.RemainderMin = item.alertmessage.predosetime;
                                        _event.Title = string.Format("Your need to take {0} of size {1} {2}x{3}", item.drug.drugname, item.drug.amount, "MG", item.regimen.medicationPerDose);
                                        dates.Add(_event);
                                    }
                                }
                                if (afternoonTimes != "undefined" && afternoonTimes != "")
                                {
                                    string[] afternoonTimesArray = item.regimen.afternoon_schedule.Split(',');
                                    foreach (var time in afternoonTimesArray)
                                    {
                                        string[] times = time.Split(':');
                                        if (times[0].Length == 1)
                                            times[0] = times[0].Insert(0, "0");
                                        if (times[1].Length == 1)
                                            times[1] = times[1].Insert(0, "0");
                                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                                        DateTime d = DateTime.Parse(exactString);
                                        DateTime dd = today + d.TimeOfDay;

                                        Event _event = new Event();
                                        _event.StartDate = dd.AddMinutes(-item.alertmessage.predosetime);
                                        _event.EndDate = dd.AddMinutes(item.alertmessage.thresholdduetime);
                                        _event.RemainderMin = item.alertmessage.predosetime;
                                        _event.Title = string.Format("Your need to take {0} of size {1} {2}x{3}", item.drug.drugname, item.drug.amount, "MG", item.regimen.medicationPerDose);
                                        dates.Add(_event);
                                    }
                                }
                                if (eveningTimes != "undefined" && eveningTimes != "")
                                {
                                    string[] eveningTimesArray = item.regimen.evening_schedule.Split(',');
                                    foreach (var time in eveningTimesArray)
                                    {
                                        string[] times = time.Split(':');
                                        if (times[0].Length == 1)
                                            times[0] = times[0].Insert(0, "0");
                                        if (times[1].Length == 1)
                                            times[1] = times[1].Insert(0, "0");
                                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                                        DateTime d = DateTime.Parse(exactString);
                                        DateTime dd = today + d.TimeOfDay;

                                        Event _event = new Event();
                                        _event.StartDate = dd.AddMinutes(-item.alertmessage.predosetime);
                                        _event.EndDate = dd.AddMinutes(item.alertmessage.thresholdduetime);
                                        _event.RemainderMin = item.alertmessage.predosetime;
                                        _event.Title = string.Format("Your need to take {0} of size {1} {2}x{3}", item.drug.drugname, item.drug.amount, "MG", item.regimen.medicationPerDose);
                                        dates.Add(_event);
                                    }
                                }
                                if (bedtimeTimes != "undefined" && bedtimeTimes != "")
                                {
                                    string[] bedtimeTimesArray = item.regimen.bedtime_schedule.Split(',');
                                    foreach (var time in bedtimeTimesArray)
                                    {
                                        string[] times = time.Split(':');
                                        if (times[0].Length == 1)
                                            times[0] = times[0].Insert(0, "0");
                                        if (times[1].Length == 1)
                                            times[1] = times[1].Insert(0, "0");
                                        string exactString = string.Format("{0}:{1} {2}", times[0], times[1], times[2]);
                                        DateTime d = DateTime.Parse(exactString);
                                        DateTime dd = today + d.TimeOfDay;

                                        Event _event = new Event();
                                        _event.StartDate = dd.AddMinutes(-item.alertmessage.predosetime);
                                        _event.EndDate = dd.AddMinutes(item.alertmessage.thresholdduetime);
                                        _event.RemainderMin = item.alertmessage.predosetime;
                                        _event.Title = string.Format("Your need to take {0} of size {1} {2}x{3}", item.drug.drugname, item.drug.amount, "MG", item.regimen.medicationPerDose);
                                        dates.Add(_event);
                                    }
                                }
                            }
                         today= today.AddDays(1);
                        }
                    }
                }  
            }
            return dates;
        }
        class Event
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int RemainderMin { get; set; }

        }
    }
    public enum ScheduleType
    {
        Daily,
        Weekly,
        Monthly,
        Yearly
    }
}
