using System;
using Android.Provider;
using Java.Util;
using Xamarin.Forms;
using MedCon.Droid.DependencySerices;
using MedCon.Interfaces;
using Android.Content;
using Android.Icu.Text;

[assembly: Dependency(typeof(CalendarEvents_android))]
namespace MedCon.Droid.DependencySerices
{
    public class CalendarEvents_android : ICalendarEvents
    {
        public void AddEvent(string title, string description, DateTime startDate, DateTime endDate, int remainderMin, string timeZone = "US/Eastern")
        {
            ContentResolver resolver = Android.App.Application.Context.ContentResolver;

            ContentValues values = new ContentValues();

            values.Put(CalendarContract.Reminders.InterfaceConsts.Dtstart, GetDateTimeMS(startDate,timeZone));
            values.Put(CalendarContract.Reminders.InterfaceConsts.Dtend, GetDateTimeMS(endDate,timeZone));
            values.Put(CalendarContract.Reminders.InterfaceConsts.Title, title);
            values.Put(CalendarContract.Reminders.InterfaceConsts.Description, description);

            values.Put(CalendarContract.Reminders.InterfaceConsts.EventTimezone, GetDeviceTimeZone().ID);

            // Default calendar
            values.Put(CalendarContract.Reminders.InterfaceConsts.CalendarId, GetCalendarId());

            // values.Put(CalendarContract.Events.InterfaceConsts.Rrule, "FREQ=DAILY;UNTIL="+ GetDateTimeMS(DateTime.Now.AddHours(2)));

            // Set Period for 1 Hour
           // values.Put(CalendarContract.Events.InterfaceConsts.Duration, "+P1H");

            values.Put(CalendarContract.Reminders.InterfaceConsts.HasAlarm, 1);
            // Insert event to calendar
            Android.Net.Uri uri = resolver.Insert(CalendarContract.Events.ContentUri, values);

            long eventID = long.Parse(uri.LastPathSegment);
            ContentValues remindervalues = new ContentValues();
            remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.Minutes, remainderMin);
            remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.EventId, eventID);
            remindervalues.Put(CalendarContract.Reminders.InterfaceConsts.Method, (int)Android.Provider.RemindersMethod.Alert);
            resolver.Insert(CalendarContract.Reminders.ContentUri, remindervalues);
        }
        Java.Util.TimeZone GetDeviceTimeZone()
        {
            DateFormat date = new SimpleDateFormat("z", Locale.Default);
            String localTime = date.Format(new Date());
            Java.Util.Calendar calender = Java.Util.Calendar.Instance;
            return calender.TimeZone;
        }
        long GetDateTimeMS(DateTime date,string timezone)
        {
            var c = Calendar.GetInstance(GetDeviceTimeZone());

            c.Set(CalendarField.DayOfMonth, date.Day);
            c.Set(CalendarField.HourOfDay, date.Hour);
            c.Set(CalendarField.Minute, date.Minute);
            c.Set(CalendarField.Month, date.Month - 1);
            c.Set(CalendarField.Year, date.Year);

            return c.TimeInMillis;
        }
        long GetDateTimeMS(int yr, int month, int day, int hr, int min)
        {
            Calendar c = Calendar.GetInstance(Java.Util.TimeZone.Default);

            c.Set(Java.Util.CalendarField.DayOfMonth, 15);
            c.Set(Java.Util.CalendarField.HourOfDay, hr);
            c.Set(Java.Util.CalendarField.Minute, min);
            c.Set(Java.Util.CalendarField.Month, Calendar.December);
            c.Set(Java.Util.CalendarField.Year, 2011);

            return c.TimeInMillis;
        }
        int GetCalendarId()
        {

            var calendarsUri = CalendarContract.Calendars.ContentUri;

            string[] calendarsProjection = {
                CalendarContract.Calendars.InterfaceConsts.Id,
                CalendarContract.Calendars.InterfaceConsts.CalendarDisplayName,
                CalendarContract.Calendars.InterfaceConsts.AccountName
            };



            if (Xamarin.Forms.Forms.Context == null)
                return -1;

            var cursor = Android.App.Application.Context.ContentResolver.Query(calendarsUri, calendarsProjection, null, null, null);

            if (cursor.Count == 0)
                return -1;


            cursor.MoveToPosition(0);
            int calId = cursor.GetInt(cursor.GetColumnIndex(calendarsProjection[0]));
            return calId;
        }
    }
}