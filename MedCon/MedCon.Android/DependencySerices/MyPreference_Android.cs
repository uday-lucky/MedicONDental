using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using MedCon.Droid.DependencySerices;
using MedCon.Interfaces;
using MedCon.ViewModels;
using System.Globalization;
using Java.Util;
using Java.Text;
using Android.Locations;

[assembly:Dependency(typeof(MyPreference_Android))]
namespace MedCon.Droid.DependencySerices
{
    public class MyPreference_Android :IMyPreference
    {
        public MyPreference GetMyPreference()
        {
            MyPreference myPreference = new MyPreference();
            myPreference.Language = Java.Util.Locale.Default.GetDisplayLanguage(Java.Util.Locale.Default);
            myPreference.DoseTimeZone = GetGMTTime();
            myPreference.Is24TimeFormat = Android.Text.Format.DateFormat.Is24HourFormat(Android.App.Application.Context);
            return myPreference;
        }
        private string GetGMTTime()
        {
            DateFormat date = new SimpleDateFormat("z", Locale.Default);
            String localTime = date.Format(new Date());
            Java.Util.Calendar calender = Java.Util.Calendar.Instance;
            Java.Util.TimeZone timeZone = calender.TimeZone;
            return string.Format("({0}){1}", localTime, timeZone.ID);
        }
    }
}