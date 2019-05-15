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
using Android.Locations;

[assembly:Dependency(typeof(LocationManager_Android))]
namespace MedCon.Droid.DependencySerices
{
    public class LocationManager_Android : ILocationManager
    {
        public string GetCurrentAddress()
        {
            return "";

        }
    }
}