using MedCon.Interfaces;
using MedCon.ViewModels.Base;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MedCon.ViewModels
{
    public class MyPreferenceViewModel : ViewModelBase
    {
        public MyPreference MyPreference1 { get; set; }
        public MyPreferenceViewModel()
        {

            GetLatLang();
            var dependency = DependencyService.Get<IMyPreference>();
            MyPreference1 = dependency.GetMyPreference();
            MyPreference1.TapCommand = new Command<string>(ItemTapped);
            if (MyPreference1.Is24TimeFormat)
            {
                MyPreference1.TimeFormatImage = "switch_on.png";
                MyPreference1.TimeFormat = "13:00";
            }
            else
            {
                MyPreference1.TimeFormatImage = "switch_off.png";
                MyPreference1.TimeFormat = "01:00 PM";
            }
        }
        private async void GetLatLang()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync(new TimeSpan(0, 0, 10));
                Position position1 = new Position { Longitude = position.Longitude, Latitude = position.Latitude };
                var address = await locator.GetAddressesForPositionAsync(position1);
                if (address.ToList().Count > 0)
                {
                    Address presentAddress = address.ToList()[0];
                    MyPreference1.Location = string.Format("{0},{1},{2},{3},{4},{5}", presentAddress.FeatureName, presentAddress.SubLocality, presentAddress.Locality, presentAddress.AdminArea, presentAddress.CountryName, presentAddress.PostalCode);
                   MyPreference1.IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                MyPreference1.Location = ex.Message;
               MyPreference1.IsBusy = false;
            }
        }
        private void ItemTapped(string type)
        {
            switch (type)
            {
                case "0":
                    LanguageTapped();
                    break;
                case "1":
                    DoseTimeTapped();
                    break;
                case "2":
                    TimeFormatTapped();
                    break;
                case "3":
                    LocationTapped();
                    break;

            }
        }

        private void LanguageTapped()
        {
            if (MyPreference1.IsLanguageVisible)
            {
                MyPreference1.IsLanguageVisible = false;
                MyPreference1.LanguageArrowImage = "downarrow2.png";
            }
            else
            {
                MyPreference1.IsLanguageVisible = true;
                MyPreference1.LanguageArrowImage = "uparrow2.png";
            }
        }
        private void DoseTimeTapped()
        {
            if (MyPreference1.IsDosingTimeZoneVisible)
            {
                MyPreference1.IsDosingTimeZoneVisible = false;
                MyPreference1.DoseTimeZoneArrowImage = "downarrow2.png";
            }
            else
            {
                MyPreference1.IsDosingTimeZoneVisible = true;
                MyPreference1.DoseTimeZoneArrowImage = "uparrow2.png";
            }
        }
        private void TimeFormatTapped()
        {
            if (MyPreference1.IsTimeFormatVisible)
            {
                MyPreference1.IsTimeFormatVisible = false;
                MyPreference1.TimeFormatArrowImage = "downarrow2.png";
            }
            else
            {
                MyPreference1.IsTimeFormatVisible = true;
                MyPreference1.TimeFormatArrowImage = "uparrow2.png";
            }
        }
        private void LocationTapped()
        {
            if (MyPreference1.IsLocationVisible)
            {
                MyPreference1.IsLocationVisible = false;
                MyPreference1.LocationArrowImage = "downarrow2.png";
            }
            else
            {
                MyPreference1.IsLocationVisible = true;
                MyPreference1.LocationArrowImage = "uparrow2.png";
            }
        }
    }

    public class MyPreference :ViewModelBase, INotifyPropertyChanged
    {
        string _location;
        ImageSource _timeFormatImageSource, _languageArrowImage = "downarrow2.png", _doseTimeZoneArrowImage = "downarrow2.png", _timeFormatArrowImage = "downarrow2.png", _locationArrowImage = "downarrow2.png";
        bool _isLanguageVisible, _isDosingTimeZoneVisible, _isTimeFormatVisible, _isLocationVisible,_isBusy=true;
        public bool IsLanguageVisible { get { return _isLanguageVisible; } set { _isLanguageVisible = value; OnPropertyChanged("IsLanguageVisible"); } }
        public bool IsDosingTimeZoneVisible { get { return _isDosingTimeZoneVisible; } set { _isDosingTimeZoneVisible = value; OnPropertyChanged("IsDosingTimeZoneVisible"); } }
        public bool IsTimeFormatVisible { get { return _isTimeFormatVisible; } set { _isTimeFormatVisible = value; OnPropertyChanged("IsTimeFormatVisible"); } }
        public bool IsLocationVisible { get { return _isLocationVisible; } set { _isLocationVisible = value; OnPropertyChanged("IsLocationVisible"); } }

        public string Language { get; set; }
        public string DoseTimeZone { get; set; }
        public bool Is24TimeFormat { get; set; }
        public bool IsBusy { get { return _isBusy; } set { _isBusy = value; OnPropertyChanged("IsBusy"); } }
        public string TimeFormat { get; set; }
        public string Location { get { return _location; } set { _location = value; OnPropertyChanged("Location"); } }
        public ICommand TapCommand { get; set; }

        public ImageSource LanguageArrowImage { get { return _languageArrowImage; } set { _languageArrowImage = value; OnPropertyChanged("LanguageArrowImage"); } }
        public ImageSource DoseTimeZoneArrowImage { get { return _doseTimeZoneArrowImage; } set { _doseTimeZoneArrowImage = value; OnPropertyChanged("DoseTimeZoneArrowImage"); } }
        public ImageSource TimeFormatArrowImage { get { return _timeFormatArrowImage; } set { _timeFormatArrowImage = value; OnPropertyChanged("TimeFormatArrowImage"); } }
        public ImageSource LocationArrowImage { get { return _locationArrowImage; } set { _locationArrowImage = value; OnPropertyChanged("LocationArrowImage"); } }


        public ImageSource TimeFormatImage { get { return _timeFormatImageSource; } set { _timeFormatImageSource = value; OnPropertyChanged("TimeFormatImage"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
