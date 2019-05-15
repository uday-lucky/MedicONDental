using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventKit;
using Foundation;
using MedCon.Interfaces;
using MedCon.iOS.DependencyServices;
using UIKit;
using Xamarin.Forms;

[assembly:Dependency(typeof(CalendarEvents_IOS))]
namespace MedCon.iOS.DependencyServices
{
    public class CalendarEvents_IOS : ICalendarEvents
    {
        public static EKEventStore eventStore;

        public CalendarEvents_IOS()
        {
            eventStore = new EKEventStore();
            SetPermissions();
        }
        public void AddEvent(string title, string description, DateTime startDate, DateTime endDate, int remainderMin, string timeZone = "US/Eastern")
        {
            EKEvent newEvent = EKEvent.FromStore(eventStore);
            // set the alarm for 10 minutes from now
            newEvent.AddAlarm(EKAlarm.FromDate(DateTimeToNSDate(endDate.AddMinutes(-15))));
            // make the event start 20 minutes from now and last 30 minutes
            newEvent.StartDate = DateTimeToNSDate(startDate);
            newEvent.EndDate = DateTimeToNSDate(endDate);
            newEvent.Title =title;
            newEvent.Notes = description;
            newEvent.Calendar = eventStore.DefaultCalendarForNewEvents;
            NSError e;
            eventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out e);
        }
        public static NSDate DateTimeToNSDate(DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = DateTime.SpecifyKind(date, DateTimeKind.Local);
            return (NSDate)date;
        }
        public static void SetPermissions()
        {
                eventStore.RequestAccess(EKEntityType.Event, (granted, calerror) => { });
        }

    }
}