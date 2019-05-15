using Amazon;
using MedCon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedCon
{
    public class Constants
    {
        public const string IdentityPoolId = "us-east-1:1cbc3a46-8413-4302-8655-b747d85591cc";// "us-east-1:9bcb158d-d9f7-4eac-8ba8-c57be8bfbf29";
        public const string CognitoUserPoolId = "us-east-1_4hvjEw6VI";
        public const string CognitoClientId = "7rqdvt6o29ev00mck7btsncflr";// "eugps8e1vt4alo25n2et8lbf";

        public const string ApiBase = "https://qd7zrem01a.execute-api.us-east-1.amazonaws.com/dev/";
        public const string ContainerApiBase = "https://1senq8suqa.execute-api.us-east-1.amazonaws.com/dev/";
        public const string UpdateContainerApiBase = "https://imkjr9tsib.execute-api.us-east-1.amazonaws.com/dev/";
        public const string GetAgreementURL = "http://dev-test-frontend-oo6b2aqhfdyo-frontendbucket-1iuy67vrlxvy3.s3-website-us-east-1.amazonaws.com/assets/eula/eula.txt";
        public const string GetCountriesListUrl = "https://adt18nasck.execute-api.us-east-1.amazonaws.com/dev/masterdata/list/countries";
        public const string GetRacesList = "https://adt18nasck.execute-api.us-east-1.amazonaws.com/dev/masterdata/list/races";
        public const string MedicineImageBase = "https://s3.amazonaws.com/medcon-api-drugs-v2-dev-drugimages-4tb0ljduq777/";

        public const string HistoryApiBase = "https://imkjr9tsib.execute-api.us-east-1.amazonaws.com/dev/";

        public static readonly RegionEndpoint CognitoIdentityRegion = RegionEndpoint.USEast1;
        public static readonly RegionEndpoint CognitoSyncRegion = RegionEndpoint.USEast1;

        public const string SqliteDBName = "MedCon.db3";

        public static readonly TimeSpan AlertEarlyWarningMinutes = TimeSpan.FromMinutes(15);

        /// <summary>
        /// Minimum length of adherence threshold to send notification
        /// </summary>
        public static readonly TimeSpan UpcomingThresholdMinimumMinutes = TimeSpan.FromMinutes(30);


        //Messaging center keys
        public const string MenuKey = "Show_Hide_Menu_Key";
        public const string ShowPickerKey = "FocusPicker_Key";
        public const string ShowDashboardPickerKey = "FocusPicker_Key";
        public const string UpdateDashboardDateKey = "UpdateDashboardDate_Key";
        public const string ShowTrialPickerKey = "ShowTrialPicker_Key";
        public const string UpdateLeftMenuPickKey = "UpdateLeftMenuPic_Key";
        public const string UpdateContainerKey = "UpdateContainer_Key";
        public const string UpdateTrialTimeKey = "UpdatetrialTime_Key";
        public const string UpdateDashboardCalendar = "UpdateDashboard_key";
        public const string UpdateDashboardCalendarDate = "UpdateDashboardCalendarDate_Key";
        public const string FocusChangeTimePickerKey = "FocusChangeTimePicker_Key";
        //App properies
        public const string companyId = "CompanyID";

        public static DoseRemainder doseRemainder = new DoseRemainder();

        public static string PresentPatientId = string.Empty;
        public static string SelectedDate = DateTime.Now.ToString();
        public static int SelectedTrial = 0;
    }
}
