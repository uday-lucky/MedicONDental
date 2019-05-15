using MedCon.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Amazon;
using Amazon.CognitoIdentityProvider.Model;
using MedCon.Cognito;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentity;
using System;
using System.Text.RegularExpressions;
using MedCon.Utilities;
using MedCon.Services.Interfaces;
using MedCon.Models;
using System.Collections.ObjectModel;

namespace MedCon.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        RegistrationInput registrationInput = new RegistrationInput();
        public CountriesAndRaces _CountriesAndRaces { get; set; }
        string _firstname, _lastname, _password, _confirmPassword, _mobile, _email,_selectedRace="Selecte Race",_selectedCountry="Select Country",_selectedDate="DOB";
        DateTime _dob = DateTime.Now;
        bool _isMale, _isFemale;
        int _racesIndex = -1, _countriesIndex = -1;
        ObservableCollection<Race> _racesList;
        ObservableCollection<Country> _countriesList;
        public ICommand RegistrationCommand { get; set; }
        CognitoAWSCredentials credentials;
        public string Firstname { get { return _firstname; } set { _firstname = value; OnPropertyChanged("Firstname"); } }
        public string Lastname { get { return _lastname; } set { _lastname = value; OnPropertyChanged("Lastname"); } }
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged("Password"); } }
        public string ConfirmPassword { get { return _confirmPassword; } set { _confirmPassword = value; OnPropertyChanged("ConfirmPassword"); } }
        public string Mobile { get { return _mobile; } set { _mobile = value; OnPropertyChanged("Mobile"); } }
        public DateTime DOB { get { return _dob; } set { _dob = value; OnPropertyChanged("DOB"); } }
        public string EmailID { get { return _email; } set { _email = value; OnPropertyChanged("EmailID"); } }
        public string SelectedRace { get { return _selectedRace; } set { _selectedRace = value; OnPropertyChanged("SelectedRace"); } }
        public string SelectedDate { get { return _selectedDate; } set { _selectedDate = value; OnPropertyChanged("SelectedDate"); } }

        public string SelectedCountry { get { return _selectedCountry; } set { _selectedCountry = value; OnPropertyChanged("SelectedCountry"); } }
        public bool IsMale { get { return _isMale; }
            set
            {
                _isMale = value;
                OnPropertyChanged("IsMale"); }
        }
        public bool IsFemale { get { return _isFemale; }
            set
            {
                _isFemale = value;
                OnPropertyChanged("IsFemale"); }
        }
        public int RacesIndex { get { return _racesIndex; } set { _racesIndex = value; OnPropertyChanged("RacesIndex"); } }
        public int CountriesIndex { get { return _countriesIndex; } set { _countriesIndex = value; OnPropertyChanged("CountryIndex"); } }
        public ICommand MaleCheckCommand { get; set; }
        public ICommand FemaleCheckCommand { get; set; }
        public ICommand ShowDateCommand { get; set; }
        public ICommand ShowPickerCommand { get; set; }
        public ICommand ShowCountryPickerCommand { get; set; }
        public ICommand RaceSelectedCommand { get; set; }
        public ICommand CountrySelectedCommand { get; set; }
        public ICommand DOBSelectedCommand { get; set; }
        public ObservableCollection<Race> RacesList { get { return _racesList; } set { _racesList = value; OnPropertyChanged("RacesList"); } }
        public ObservableCollection<Country> CountriesList { get { return _countriesList; } set { _countriesList = value; OnPropertyChanged("CountriesList"); } }
        public RegistrationViewModel()
        {          
            RegistrationCommand = new Command(GotoRegistrationConfrm);
            FemaleCheckCommand = new Command(FemaleCheckMethod);
            MaleCheckCommand = new Command(MaleCheckMethod);
            ShowDateCommand = new Command(ShowDatePicker);
            ShowPickerCommand = new Command(ShowPicker);
            RaceSelectedCommand = new Command(RaceSelected);
            ShowCountryPickerCommand = new Command(ShowCountryPicker);
            CountrySelectedCommand = new Command(CountrySelected);
            DOBSelectedCommand = new Command(DateSelected);
        }
        private void DateSelected()
        {
            SelectedDate = DOB.ToString("dd/MM/yyyy");
        }
        private void CountrySelected()
        {
            SelectedCountry = _CountriesAndRaces.CountriesList[CountriesIndex].NameWithCode;
            registrationInput.countryId = _CountriesAndRaces.CountriesList[CountriesIndex].id;
        }
        private void RaceSelected()
        {
            SelectedRace = _CountriesAndRaces.RacesList[RacesIndex].race;
            registrationInput.race = _CountriesAndRaces.RacesList[RacesIndex].race;
        }
        private void ShowDatePicker()
        {
            MessagingCenter.Send("0", Constants.ShowPickerKey);
        }
        private void ShowPicker()
        {
            MessagingCenter.Send("1", Constants.ShowPickerKey);
        }
        private void ShowCountryPicker()
        {
            MessagingCenter.Send("2", Constants.ShowPickerKey);
        }
        private void MaleCheckMethod()
        {
            if (IsMale)
                IsFemale = false;
            else
                IsFemale = true;
        }
        private void FemaleCheckMethod()
        {
            if (IsFemale)
                IsMale = false;
            else
                IsMale = true;
        }
        private async void GotoRegistrationConfrm()
        {
            if (string.IsNullOrEmpty(Firstname))
            {
                DialogProvider.DisplayNativeAlert("Please enter firstname", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Lastname))
            {
                DialogProvider.DisplayNativeAlert("Please enter lastname", "OK");
                return;
            }
            if (string.IsNullOrEmpty(EmailID)||!ValidationRules.IsValidEmailID(EmailID))
            {
                DialogProvider.DisplayNativeAlert("Entered email id not in correct format", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                DialogProvider.DisplayNativeAlert("Please enter password", "OK");
                return;
            }
            if (Password != ConfirmPassword)
            {
                DialogProvider.DisplayNativeAlert("Password and confirm password should match", "OK");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                DialogProvider.DisplayNativeAlert("Please enter password", "OK");
                return;
            }
            //if (CountriesIndex<0)
            //{
            //    DialogProvider.DisplayNativeAlert("Please select a country", "OK");
            //    return;
            //}
            if (!IsMale && !IsFemale)
            {
                DialogProvider.DisplayNativeAlert("Please select gender", "OK");
                return;
            }
            var response = await SignupUser();
            if (response != null)
            {                
                if (!response.UserConfirmed)
                {
                    registrationInput.dob = DOB.ToString();
                    registrationInput.emailAddress = EmailID;
                    registrationInput.firstName = Firstname;
                    if(IsMale)
                    registrationInput.gender = "Male";
                    else
                        registrationInput.gender = "Female";
                    registrationInput.lastName = Lastname;
                    registrationInput.phoneNumber = Mobile;
                    registrationInput.roleId = 6;
                    registrationInput.roleName = "Patient";
                   // registrationInput.sub = null;
                    registrationInput.Password = Password;
                    await NavigationService.NavigateToAsync<RegistrationConfirmViewModel>(registrationInput);
                  //  MedCon.Helpers.Settings.ProfileName = EmailID;
                }
            }
            //  NavigationService.NavigateToAsync<RegistrationConfirmViewModel>();
        }
        public async Task<SignUpResponse> SignupUser()
        {
            DialogProvider.ShowProgress();
            try
            {
                credentials = new CognitoAWSCredentials(
                    Constants.IdentityPoolId, // Identity Pool ID
                    Constants.CognitoIdentityRegion // Region
                );

                //providerClient = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.USEast1);

                AmazonCognitoIdentityProviderClient providerClient =
                    new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), RegionEndpoint.USEast1);

                SignUpRequest sr = new SignUpRequest();

                sr.ClientId = Constants.CognitoClientId; // "7rqdvt6o29ev00mck7btsncflr";
                //sr.SecretHash = "1vuar943ctqnj7baf04sncubi0i7tdtf93m79q24chr2jncnf3ov";
                sr.Username = EmailID;
                sr.Password = Password;
                sr.UserAttributes = new List<AttributeType> {
                new AttributeType
                {
                    Name = "email",
                    Value = EmailID,
                },
                new AttributeType
                {
                    Name = "phone_number",
                    Value ="+91"+Mobile
                }
            };
                SignUpResponse res = await providerClient.SignUpAsync(sr);
                return res;
            }
            catch (Exception e)
            {
                DialogProvider.DisplayNativeAlert(e.Message, "OK");
            }
            finally
            {
                DialogProvider.HideProgress();
            }
            return null;

        }

        public async Task SignupCognitoUserIntoUserPoolAsync()
        {
            try
            {
                await NavigationService.NavigateToAsync<RegistrationConfirmViewModel>();
                // Identify your Cognito UserPool Provider
                //using (var provider = new Amazon.CognitoIdentityProvider.AmazonCognitoIdentityProviderClient(CognitoUtils.Credentials,Constants.CognitoIdentityRegion))
                //{
                //    // Sign up new User into your Cognito User Pool
                //    var signup = await provider.SignUpAsync(new SignUpRequest
                //    {

                //        ClientId = Constants.CognitoClientId,
                //        Username = "MysuraReddy",
                //        Password = "test@123",
                //        UserAttributes = new List<Amazon.CognitoIdentityProvider.Model.AttributeType>
                //    {
                //        // INPUT ANY ATTRIBUTES TO REGISTER
                //        // Must include Email and Phone, if MFA is enabled
                //        new AttributeType
                //        {
                //            Name = "email",
                //            Value = "abc@contoso.com",
                //        },
                //        new AttributeType
                //        {
                //            Name = "phone_number",
                //            Value = "+919581753562",
                //        }
                //    }
                //    });
                //    var sss = signup.HttpStatusCode;
                //}

            }
            catch (System.Exception ex)
            {


            }
        }

       public override Task InitializeAsync(object navigationData)
        {
            _CountriesAndRaces = (CountriesAndRaces)navigationData;
            RacesList =new ObservableCollection<Race>(_CountriesAndRaces.RacesList);
            CountriesList = new ObservableCollection<Country>(_CountriesAndRaces.CountriesList);
            return base.InitializeAsync(navigationData);
        }
    }
}
