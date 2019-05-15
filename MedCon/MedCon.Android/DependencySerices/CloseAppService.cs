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

[assembly:Dependency(typeof(CloseAppService))]
namespace MedCon.Droid.DependencySerices
{
    public class CloseAppService : IExitApp
    {
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}