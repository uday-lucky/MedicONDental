using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MedCon.Droid.DependencySerices;
using MedCon.Interfaces;
using SQLite;
using Xamarin.Forms;

[assembly:Dependency(typeof(Sqlite_Android))]
namespace MedCon.Droid.DependencySerices
{
    public class Sqlite_Android : ISqlite
    {
        public SQLiteConnection GetConnection()
        {
            var path = Path.Combine(System.Environment.
              GetFolderPath(System.Environment.
              SpecialFolder.Personal), Constants.SqliteDBName);
            return new SQLiteConnection(path);
        }
    }
}