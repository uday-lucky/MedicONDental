using MedCon.LocalDB;
using MedCon.ViewModels.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace MedCon.ViewModels
{
    public class LeftmenuViewModel : ViewModelBase
    {
        public ObservableCollection<MenuClass> menu { get; set; }
        public string ProfileName { get; set; }
        public LeftmenuViewModel()
        {
            MessagingCenter.Subscribe<string>(this, Constants.UpdateLeftMenuPickKey, (value) =>
            {
                ProfileImgSource = value;
            });
            menu = new ObservableCollection<MenuClass>();
            menu.Add(new MenuClass { ID = 1, MenuName = "My Profile", MenuIcon = "myprofile.png", MenuSelectedCommand = new Command<MenuClass>(MenuSelected) });
            menu.Add(new MenuClass { ID = 2, MenuName = "Dashboard", MenuIcon = "dashboard.png", MenuSelectedCommand = new Command<MenuClass>(MenuSelected) });
            menu.Add(new MenuClass { ID = 3, MenuName = "My Preference", MenuIcon = "settings.png", MenuSelectedCommand = new Command<MenuClass>(MenuSelected) });
            menu.Add(new MenuClass { ID = 4, MenuName = "History", MenuIcon = "history.png", MenuSelectedCommand = new Command<MenuClass>(MenuSelected) });
            menu.Add(new MenuClass { ID = 5, MenuName = "Scan New Container", MenuIcon = "scan_new_container.png", MenuSelectedCommand = new Command<MenuClass>(MenuSelected) });
            menu.Add(new MenuClass { ID = 6, MenuName = "Latest Updates and Notifications", MenuIcon = "notification.png", MenuSelectedCommand = new Command<MenuClass>(MenuSelected) });
            menu.Add(new MenuClass { ID = 7, MenuName = "My Medications",MenuIcon= "my_medication.png", MenuSelectedCommand = new Command<MenuClass>(MenuSelected) });
            menu.Add(new MenuClass { ID = 8, MenuName = "Logout", MenuIcon = "logout.png", MenuSelectedCommand = new Command<MenuClass>(MenuSelected) });
            ProfileName = MedCon.Helpers.Settings.ProfileName;
        }
        private async void MenuSelected(MenuClass menu)
        {
            try
            {
                int SelectedMenu = menu.ID;
                switch (SelectedMenu)
                {
                    case 1:
                        DialogProvider.ShowProgress();
                        {
                                JObject profileResponse = await requestProvider.GetAsync<JObject>(Constants.ApiBase + "user/patient/profile");

                                await NavigationService.NavigateToAsync<ProfileViewModel>(profileResponse);
                        }
                        break;
                    case 2:
                        DialogProvider.ShowProgress();
                        await NavigationService.NavigateToAsync<DashboardViewModel>();
                        break;
                    case 3:
                        DialogProvider.ShowProgress();
                        await NavigationService.NavigateToAsync<MyPreferenceViewModel>();
                        break;
                    case 4:
                        DialogProvider.ShowProgress();
                        await NavigationService.NavigateToAsync<HistoryViewModel>();
                        break;
                    case 5:
                        try
                        {
                           // string scanResult = await GetScanResultAsync();
                            await NavigationService.NavigateToAsync<ScanNewContainerViewModel>();
                           // MessagingCenter.Send("1", Constants.MenuKey);
                        }
                        catch (Exception ex)
                        {

                        }

                        break;
                    case 6:
                        DialogProvider.ShowProgress();
                        await NavigationService.NavigateToAsync<NotificationsViewModel>();
                        break;
                    case 7:
                        DialogProvider.ShowProgress();
                        await NavigationService.NavigateToAsync<TrialsViewModel>();
                        break;
                    case 8:
                        bool answer = await Application.Current.MainPage.DisplayAlert("Medcon", "Are you sure? Do you want to logout?", "Yes", "No");
                        if (answer)
                        {
                            MedCon.Helpers.Settings.IsLoggedIn = false;
                            await NavigationService.NavigateToAsync<LoginViewModel1>();
                            //SqliteService.DeleteTables();
                            //SqliteService.CreateTables();
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                DialogProvider.DisplayNativeAlert(ex.Message, "MedCon");
            }
            finally
            {
                DialogProvider.HideProgress();
            }
            //  MessagingCenter.Send("1", Constants.MenuKey);
        }
        private async Task<string> GetScanResultAsync()
        {
            string scanResult = string.Empty;
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();
                return scanResult = result.Text;
            }
            catch (Exception ex)
            {
                return "";
            }
            return scanResult;
        }
    }

    public class MenuClass
    {
        public ICommand MenuSelectedCommand { get; set; }
        public int ID { get; set; }
        public string MenuName { get; set; }
        public ImageSource MenuIcon { get; set; }
    }
}
