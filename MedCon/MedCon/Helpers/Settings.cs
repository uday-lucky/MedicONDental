// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MedCon.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
        private const string LoginKey = "login_key";
        private static readonly string SettingsDefault = string.Empty;


        private const string IsAgreementAcceptedKey = "IsAgreementAccepted_key";
        private static readonly bool IsAgreementAcceptedDefault;

        private const string IsLoggedinKey = "IsLoggedin_key";
        private static readonly bool IsLoggedinDefault;

        private const string ProfilePicSourceKey = "ProfilePic_Key";
        private static readonly string IsProfilePicDefualt=string.Empty;

        private const string ProfileNameKey = "ProfileName_Key";
        private static readonly string ProfileNameDefualt = string.Empty;

        private const string TokenKey = "Token_Key";
        private static readonly string TokenDefualt = string.Empty;

        private const string RefreshTokenKey = "RefreshToken_Key";
        private static readonly string RefreshTokenDefualt = string.Empty;

        private const string AccessTokenKey = "AccessToken_Key";
        private static readonly string AccessTokenDefualt = string.Empty;

        private const string PasswordKey = "Password_Key";
        private static readonly string PasswordDefault = string.Empty;

        public const string PatientIdkey = "PatientId_Key";
        private static readonly string PatientIdDefault = string.Empty;

        public const string CalendarIdskey = "PatientId_Key";
        private static readonly string CalendarIdDefault = string.Empty;

        #endregion


        public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(SettingsKey, value);
			}
		}
        public static bool IsAgreementAccepted
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsAgreementAcceptedKey, IsAgreementAcceptedDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsAgreementAcceptedKey, value);
            }
        }
        public static bool IsLoggedIn
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsLoggedinKey, IsLoggedinDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsLoggedinKey, value);
            }
        }
        public static string ProfilePic
        {
            get
            {
                return AppSettings.GetValueOrDefault(ProfilePicSourceKey, IsProfilePicDefualt);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ProfilePicSourceKey, value);
            }
        }
        public static string ProfileName
        {
            get
            {
                return AppSettings.GetValueOrDefault(ProfileNameKey, ProfileNameDefualt);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ProfileNameKey, value);
            }
        }
        public static string Token
        {
            get
            {
                return AppSettings.GetValueOrDefault(TokenKey, TokenDefualt);
            }
            set
            {
                AppSettings.AddOrUpdateValue(TokenKey, value);
            }
        }
        public static string RefreshToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(RefreshTokenKey, RefreshTokenDefualt);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RefreshTokenKey, value);
            }
        }
        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(AccessTokenKey, AccessTokenDefualt);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AccessTokenKey, value);
            }
        }
        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault(PasswordKey, PasswordDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PasswordKey, value);
            }
        }
        public static string PatientId
        {
            get { return AppSettings.GetValueOrDefault(PatientIdkey, PatientIdDefault); }
            set { AppSettings.AddOrUpdateValue(PatientIdkey, value); }
        }
        public static string CalendarAddedIds
        {
            get { return AppSettings.GetValueOrDefault(CalendarIdskey, CalendarIdDefault); }
            set { AppSettings.AddOrUpdateValue(CalendarIdskey, value); }
        }
    }
}