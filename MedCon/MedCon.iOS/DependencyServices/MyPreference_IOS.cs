using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using MedCon.iOS.DependencyServices;
using MedCon.Interfaces;
using MedCon.ViewModels;
using System.Globalization;

[assembly:Dependency(typeof(MyPreference_IOS))]
namespace MedCon.iOS.DependencyServices
{
    public class MyPreference_IOS : IMyPreference
    {
        public MyPreference GetMyPreference()
        {
            MyPreference myPreference = new MyPreference();
            myPreference.Language = new CultureInfo(NSBundle.MainBundle.PreferredLocalizations[0]).DisplayName;
            myPreference.DoseTimeZone = GetGMTTimeZone();
            myPreference.Is24TimeFormat = Is24HrFormat();
            return myPreference;
        }
        private string GetGMTTimeZone()
        {
            nint secs = NSLocale.CurrentLocale.Calendar.TimeZone.GetSecondsFromGMT;

            TimeSpan t = TimeSpan.FromSeconds(secs);

            string answer = string.Format("{0:D2}:{1:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds,
                            t.Milliseconds);
            string regionName = NSLocale.CurrentLocale.Calendar.TimeZone.Name;
            if(secs>0)
            return string.Format("(+{0}){1}",answer,regionName);
            else
                return string.Format("(-{0}){1}", answer, regionName);

        }
        private bool Is24HrFormat()
        {
            var formater = new NSDateFormatter()
            {
                DateStyle = NSDateFormatterStyle.None,
                TimeStyle = NSDateFormatterStyle.Short
            };
            var stringDate = formater.ToString(NSDate.Now);
            bool is12Hour = stringDate.Contains(formater.AMSymbol) || stringDate.Contains(formater.PMSymbol);
            return is12Hour;
        }
    }
}