using MedCon.Interfaces;
using MedCon.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MedCon.ViewModels
{
   public class AgreementViewModel:ViewModelBase
    {
        string _agrrementText;
        public string Heading1 { get; set; }
        public string Heading2 { get; set; }
        public string AgreementText { get { return _agrrementText; } set { _agrrementText = value; OnPropertyChanged("AgreementText"); } }
        public ICommand AcceptCommand { get; set; }
        public ICommand DeclineCommand { get; set; }
        public AgreementViewModel()
        {
            
            Heading1= "Accept end user license agreement";
            Heading2 = "EULA";
            AgreementText =string.Empty;            
            method1();
            AcceptCommand = new Command(AcceptMethod);
            DeclineCommand = new Command(DeclineMethod);
        }
        private void DeclineMethod()
        {
           
                IExitApp exitApp = DependencyService.Get<IExitApp>();
                exitApp.CloseApp();
        }
        private void AcceptMethod()
        {
            DialogProvider.ShowProgress("Loading...");
            MedCon.Helpers.Settings.IsAgreementAccepted = true;
            NavigationService.NavigateToAsync<LoginViewModel1>();
            DialogProvider.HideProgress();
        }
        async void method1()
        {
            await DisplayAgreement();
        }
        async Task DisplayAgreement()
        {
            try
            {
                IsBusy = true;
              //  DialogProvider.ShowProgress("");
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(Constants.GetAgreementURL);
              AgreementText = await httpResponseMessage.Content.ReadAsStringAsync();
                AgreementText = AgreementText.Substring(0, 11000);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
                //DialogProvider.HideProgress();
            }
        }
    }
}
