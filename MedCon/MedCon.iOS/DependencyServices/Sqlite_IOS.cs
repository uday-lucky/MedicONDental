using MedCon.Interfaces;
using MedCon.iOS.DependencyServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

[assembly:Dependency(typeof(Sqlite_IOS))]
namespace MedCon.iOS.DependencyServices
{
    public class Sqlite_IOS : ISqlite
    {
        public SQLiteConnection GetConnection()
        {
            string personalFolder =
              System.Environment.
              GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryFolder =
              Path.Combine(personalFolder, "..", "Library");
            var path = Path.Combine(libraryFolder, Constants.SqliteDBName);
            return new SQLiteConnection(path);
        }
    }
}
