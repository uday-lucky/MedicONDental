using Autofac;
using MedCon.Models;
using MedCon.Services;
using MedCon.ViewModels.Base;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MedCon.LocalDB;
using MedCon.LocalDB.Tables;
using awscognito;
using awscognito.Cognito.Continuations;

namespace MedCon
{
    public partial class App : Application
    {
        CognitoAWSCredentialsService credentialsService;
        ISecureStorage storage;
        public static CognitoUser CurrentUser
        {
            get;set;
        }
        public App()
        {
            Constants.PresentPatientId = MedCon.Helpers.Settings.PatientId;
           // InitializeComponent();
            Resources =new GlobalResources();
            InitNavigation();
        }
        private Task InitNavigation()
        {
            var navigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }
        protected override async void OnStart()
        {
            try
            {
                App.Current.MainPage = new NavigationPage(new Views.DashboardCalendarPage());
                //CalendarEvent calendarEvent = new CalendarEvent();
                //calendarEvent.Name = "Hello";
                //calendarEvent.Start = DateTime.Now.AddMinutes(1);
                //calendarEvent.End = DateTime.Now.AddMinutes(10);
                //CalendarEventReminder calendarEventReminder = new CalendarEventReminder();
                //calendarEventReminder.TimeBefore = new TimeSpan(0, 9, 0);
                //var dds = CrossCalendars.Current.AddEventReminderAsync(calendarEvent, calendarEventReminder);


            // Handle when your app starts
            //  await new CognitoAWSCredentialsService(null).GetCredentials();

                //App Center Initialization
                AppCenter.Start("android=e808cf87-4749-4485-a4b2-2a192929e629;" + "uwp={Your UWP App secret here};" + "ios=187a3cda-346b-4caa-b460-6fdddb5e7541;",typeof(Analytics));

                SqliteService.CreateTables();
                if(MedCon.Helpers.Settings.IsLoggedIn)
                {
                   /// login();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        private async Task login()
        {
            string username = MedCon.Helpers.Settings.ProfileName;
            string password = MedCon.Helpers.Settings.Password;
            try
            {
                CognitoAWSCredentialsService credentialsService;
                credentialsService = new CognitoAWSCredentialsService(null);

                var details = new AuthenticationDetails(username, password, null);
                CognitoUser  cognitoUser = new CognitoUser(details.getUserId(), credentialsService.Pool, storage);
                App.CurrentUser = cognitoUser;
                await cognitoUser.initiateUserAuthentication(details)
                 .ContinueWith(async continuation =>
                 {
                     if (continuation.IsFaulted)
                     {
                       
                     }                     
                     else
                     {
                         AuthenticationResult result = continuation.Result;
                         if (result.CognitoUserSession != null)
                         {
                             MedCon.Helpers.Settings.Token =  result.CognitoUserSession.IdToken.Token;
                             MedCon.Helpers.Settings.RefreshToken = result.CognitoUserSession.RefreshToken.Token;
                             MedCon.Helpers.Settings.AccessToken = result.CognitoUserSession.AccessToken.Token;                         
                        }
                     }
                 });
            }
            catch (Exception ex)
            {
            }
        }
    }
}
