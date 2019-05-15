using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MedCon.Interfaces;
using MedCon.iOS.DependencyServices;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseApp_IOS))]
namespace MedCon.iOS.DependencyServices
{
    public class CloseApp_IOS : IExitApp
    {
        public void CloseApp()
        {
            int a = 0, b = 10;
            int c = b / a;
        }
    }
}