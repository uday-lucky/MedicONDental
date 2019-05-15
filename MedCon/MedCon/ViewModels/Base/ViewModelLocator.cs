using MedCon.Interfaces;
using MedCon.Services;
using MedCon.Services.Base;
using MedCon.Services.Interfaces;
using System;
using Unity;
using Unity.Lifetime;

namespace MedCon.ViewModels.Base
{
    public class ViewModelLocator
    {
        private readonly IUnityContainer _unityContainer;

        private static readonly ViewModelLocator _instance = new ViewModelLocator();

        public static ViewModelLocator Instance
        {
            get
            {
                return _instance;
            }
        }

        protected ViewModelLocator()
        {
            _unityContainer = new UnityContainer();

            RegisterSingleton<INavigationService, NavigationService>();
            RegisterSingleton<IDialogService, DialogServices>();
            RegisterSingleton<IDashboardService, DashboardService>();

            // providers
            _unityContainer.RegisterType<IRequestProvider, RequestProvider>();

            // data services
            _unityContainer.RegisterType<IRegistrationService, RegistrationService>();
            _unityContainer.RegisterType<IUpdateContainerService, UpdateContainerService>();

            // View models
            _unityContainer.RegisterType<LoginViewModel1>();
            _unityContainer.RegisterType<AgreementViewModel>();
            _unityContainer.RegisterType<RegistrationViewModel>();
            _unityContainer.RegisterType<ForgotPasswordViewModel>();
            _unityContainer.RegisterType<RegistrationConfirmViewModel>();
            _unityContainer.RegisterType<DashboardViewModel>();
            _unityContainer.RegisterType<HistoryViewModel>();
            _unityContainer.RegisterType<HistoryDetailsViewModel>();
            _unityContainer.RegisterType<SetPasswordViewModel>();
            _unityContainer.RegisterType<NotificationsViewModel>();
            _unityContainer.RegisterType<ScanConfirmationViewModel>();
            _unityContainer.RegisterType<TrialsViewModel>();
            _unityContainer.RegisterType<ProfileViewModel>();
            _unityContainer.RegisterType<MyPreferenceViewModel>();
            _unityContainer.RegisterType<ConfirmPatientIDViewModel>();
            _unityContainer.RegisterType<DashboardCalendarViewModel>();
            _unityContainer.RegisterType<HistoryDetails2ViewModel>();
            _unityContainer.RegisterType<ScanNewContainerViewModel>();
            _unityContainer.RegisterType<InvalidScanViewModel>();
        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

        public void Register<T>(T instance)
        {
            _unityContainer.RegisterInstance<T>(instance);
        }

        public void Register<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>();
        }

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }
    }
}
