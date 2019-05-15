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
using MedCon.Droid.DependencySerices;
using MedCon.Services;
using Xamarin.Forms;

[assembly:Dependency(typeof(Storage))]
namespace MedCon.Droid.DependencySerices
{
    public class Storage : IStorage
    {
        private static readonly string name = "Storage";

        public string Get(string key)
        {
            var preferences = Android.App.Application.Context.GetSharedPreferences(name, Android.Content.FileCreationMode.Private);

            return preferences.GetString(key, null);
        }

        public void Remove(string key)
        {
            var preferences = Android.App.Application.Context.GetSharedPreferences(name, Android.Content.FileCreationMode.Private);

            if (preferences.Contains(key))
            {
                var editor = preferences.Edit();
                editor.Remove(key);
                editor.Apply();
            }
        }

        public void Set(string key, string obj)
        {
            var preferences = Android.App.Application.Context.GetSharedPreferences(name, Android.Content.FileCreationMode.Private);

            var editor = preferences.Edit();
            editor.PutString(key, obj);
            editor.Apply();
        }
    }
}