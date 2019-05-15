using MedCon.ViewModels;
using MedCon.ViewModels.Base;
using MedCon.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.Services
{
    public class NavigationService : INavigationService
    {
        protected readonly Dictionary<Type, Type> _mappings;

        protected Application CurrentApplication
        {
            get
            {
                return Application.Current;
            }
        }
        public NavigationService()
        {
            _mappings = new Dictionary<Type, Type>();
            CreatePageViewModelMappings();
        }
        public Task InitializeAsync()
        {
            if(MedCon.Helpers.Settings.IsAgreementAccepted&&MedCon.Helpers.Settings.IsLoggedIn)
                return NavigateToAsync<DashboardViewModel>();
            else if(MedCon.Helpers.Settings.IsAgreementAccepted)
                return NavigateToAsync<LoginViewModel1>();
            else
                return NavigateToAsync<AgreementViewModel>();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage is LoginPage)
            {
                var LoginPage = CurrentApplication.MainPage as LoginPage;
                //await LoginPage.Detail.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            var LoginPage = CurrentApplication.MainPage as LoginPage;

            if (LoginPage != null)
            {
                //LoginPage.Detail.Navigation.RemovePage(
                //    LoginPage.Detail.Navigation.NavigationStack[LoginPage.Detail.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, parameter);
            NavigationPage.SetHasNavigationBar(page, false);
            if (page is AgreementPage)
            {
                CurrentApplication.MainPage =new CustomNavigationPage(page);
                // CurrentApplication.MainPage = page;
            }
           //else if (page is LoginPage)
           // {
           //     CurrentApplication.MainPage = page;
           //     // CurrentApplication.MainPage = page;
           // }
           else if (page is DashboardPage)
            {
                CurrentApplication.MainPage =new CustomNavigationPage(page);
                // CurrentApplication.MainPage = page;
            }
            else if (page is LoginPage)
            {
                CurrentApplication.MainPage = new CustomNavigationPage(page);
                // CurrentApplication.MainPage = page;
            }
            else
            {
                var navigationPage = CurrentApplication.MainPage;
                if (navigationPage != null)
                {
                    await navigationPage.Navigation.PushAsync(page);
                }
                else
                {
                    CurrentApplication.MainPage =new CustomNavigationPage(page);
                }
            }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }
            return _mappings[viewModelType];
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            ViewModelBase viewModel = ViewModelLocator.Instance.Resolve(viewModelType) as ViewModelBase;
            page.BindingContext = viewModel;

            if (page is IPageWithParameters)
            {
                ((IPageWithParameters)page).InitializeWith(parameter);
            }

            return page;
        }

        private void CreatePageViewModelMappings()
        {
            _mappings.Add(typeof(LoginViewModel1), typeof(LoginPage));
            _mappings.Add(typeof(AgreementViewModel), typeof(AgreementPage));
            _mappings.Add(typeof(RegistrationViewModel), typeof(RegistrationPage));
            _mappings.Add(typeof(ForgotPasswordViewModel), typeof(ForgotPasswordPage));
            _mappings.Add(typeof(RegistrationConfirmViewModel), typeof(RegistrationConfirmPage));
            _mappings.Add(typeof(DashboardViewModel), typeof(DashboardPage));
            _mappings.Add(typeof(HistoryViewModel), typeof(HistoryPage));
            _mappings.Add(typeof(HistoryDetailsViewModel), typeof(HistoryDetailsPage));
            _mappings.Add(typeof(SetPasswordViewModel), typeof(SetPasswordPage));
            _mappings.Add(typeof(NotificationsViewModel), typeof(NotificationsPage));
            _mappings.Add(typeof(ScanConfirmationViewModel), typeof(ScanConfirmationPage));
            _mappings.Add(typeof(TrialsViewModel), typeof(TrialsPage));
            _mappings.Add(typeof(ProfileViewModel), typeof(ProfilePage));
            _mappings.Add(typeof(MyPreferenceViewModel), typeof(MyPreferencePage));
            _mappings.Add(typeof(ConfirmPatientIDViewModel), typeof(ConfirmPatientIDPage));
            _mappings.Add(typeof(DashboardCalendarViewModel), typeof(DashboardCalendarPage));
            _mappings.Add(typeof(HistoryDetails2ViewModel), typeof(HistoryDetailsPage2));
            _mappings.Add(typeof(ScanNewContainerViewModel), typeof(ScanNewContainerPage));
            _mappings.Add(typeof(InvalidScanViewModel), typeof(InvalidScanPage));
        }
    }
}
