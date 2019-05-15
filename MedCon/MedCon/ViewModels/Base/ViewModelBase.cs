using MedCon.Interfaces;
using MedCon.Models;
using MedCon.Services;
using MedCon.Services.Base;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.ViewModels.Base
{
    public class ViewModelBase : ExtendedBindableObject
    {
        ImageSource _imageSource;

        // protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        protected readonly RequestProvider requestProvider;

        protected readonly IDialogService DialogService;
        protected readonly DialogServices DialogProvider;

        public DoseRemainder NextDose { get; set; }

        public ImageSource ProfileImgSource { get { return _imageSource; } set { _imageSource = value; OnPropertyChanged("ProfileImgSource"); } }

        private bool _isBusy;
        private string _title;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        public ViewModelBase()
        {
            ProfileImgSource = string.IsNullOrEmpty(MedCon.Helpers.Settings.ProfilePic) ? "Profile.jpg" : MedCon.Helpers.Settings.ProfilePic;

            NextDose = Constants.doseRemainder;
            // DialogService = ViewModelLocator.Instance.Resolve<IDialogService>();
            NavigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
            requestProvider = new RequestProvider();

            DialogService = ViewModelLocator.Instance.Resolve<IDialogService>();
            DialogProvider = new DialogServices();
        }
        
        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
