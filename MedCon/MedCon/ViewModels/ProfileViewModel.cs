using MedCon.Models;
using MedCon.ViewModels.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MedCon.ViewModels
{
   public class ProfileViewModel:ViewModelBase
    {
        string imgPath;
        JObject profileResponse;
        bool _isEditVisible=true, _isSaveVisible;
        string _name,_email,_mobile,_dob,_gender,_location;
        public string Username { get { return _name; } set { _name = value; OnPropertyChanged("Username"); } }
        public string Email { get { return _email; } set { _email = value; OnPropertyChanged("Email"); } }
        public string Mobile { get { return _mobile; } set { _mobile = value; OnPropertyChanged("Mobile"); } }
        public string DOB { get { return _dob; } set { _dob = value; OnPropertyChanged("DOB"); } }
        public string Gender { get { return _gender; } set { _gender = value; OnPropertyChanged("Gender"); } }
        public string Location { get { return _location; } set { _location = value; OnPropertyChanged("Location"); } }
        public bool IsEditVisible { get { return _isEditVisible; } set { _isEditVisible = value; OnPropertyChanged("IsEditVisible"); } }
        public bool IsSaveVisible { get { return _isSaveVisible; } set { _isSaveVisible = value; OnPropertyChanged("IsSaveVisible"); } }
        public ObservableCollection<ProfileTrial> ProfileTrials { get; set; }
        public ICommand EditPicCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand CameraCommand { get; set; }
        public ICommand GalleryCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public List<RegimenRoot> Regimens { get; set; }
        public ObservableCollection<HistoryModel> History { get; set; }

        public ProfileViewModel()
        {
          //  GetPatientIdData();
            imgPath = MedCon.Helpers.Settings.ProfilePic;
            ProfileTrials = new ObservableCollection<ProfileTrial>();
            //ProfileTrials.Add(new ProfileTrial {PatientID="MCMU0088",TrialName="TestTrial1",Status="Active",Date="10/12/2017" });
            //ProfileTrials.Add(new ProfileTrial { PatientID = "MCMU0073", TrialName = "TestTrial1", Status = "Inactive", Date = "10/12/2017" });
            //ProfileTrials.Add(new ProfileTrial { PatientID = "MCMU0055", TrialName = "TestTrial2", Status = "Inactive", Date = "09/10/2017" });
            //ProfileTrials.Add(new ProfileTrial { PatientID = "MCMU0032", TrialName = "TestTrial3", Status = "Inactive", Date = "10/12/2017" });
            //ProfileTrials.Add(new ProfileTrial { PatientID = "MCMU0022", TrialName = "TestTrial4", Status = "Inactive", Date = "08/12/2017" });
            //ProfileTrials.Add(new ProfileTrial { PatientID = "MCMU0054", TrialName = "TestTrial5", Status = "Active", Date = "07/20/2017" });
            //ProfileTrials.Add(new ProfileTrial { PatientID = "MCMU0065", TrialName = "TestTrial6", Status = "Inactive", Date = "06/18/2017" });
            //ProfileTrials.Add(new ProfileTrial { PatientID = "MCMU0076", TrialName = "TestTrial7", Status = "Active", Date = "11/06/2017" });

            EditPicCommand = new Command(EditPick);
            ChangePasswordCommand = new Command(ChangePassword);
            CameraCommand = new Command(TakePicFromCamera);
            GalleryCommand = new Command(SelectPicFromGallery);
            SaveCommand = new Command(SaveProfile);
            EditCommand = new Command(EditTapped);
            History = new ObservableCollection<HistoryModel>();
            Regimens = new List<RegimenRoot>();
        }
        void EditTapped()
        {
            IsSaveVisible = true;
            IsEditVisible = false;
        }

        private async void GotoDetails(ProfileTrial history)
        {
            try
            {
                DialogProvider.ShowProgress();
                List<RegimenHistory> jobj10 = await requestProvider.GetAsync<List<RegimenHistory>>(string.Format(Constants.HistoryApiBase + "dose/list?patientnum={0}&containerId={1}", history.PatientID, history.ContainerNum));
                if (jobj10.Count == 0)
                {
                    DialogProvider.DisplayNativeAlert("No History Found for this trial!", "MedCon");
                    return;
                }
                HistoryRoot historyRoot = new HistoryRoot();
                var obj = Regimens.Where(x => x.trial.id == history.TrialId).ToList();
                if (obj != null)
                    historyRoot.Regimen = obj;
                var h = History.Where(x => x.TrialId == history.TrialId).FirstOrDefault();
                historyRoot.History = h;
                historyRoot.Data = jobj10;
                await NavigationService.NavigateToAsync<HistoryDetailsViewModel>(historyRoot);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                DialogProvider.HideProgress();
            }
        }
        private async void TakePicFromCamera()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DialogProvider.DisplayNativeAlert(":( No camera available.", "No Camera");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "MedCon",
                Name = "Profile.jpg",
                AllowCropping = true,
                SaveToAlbum = true,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 92
            });

            if (file == null)
                return;
            ProfileImgSource = file.Path;
            imgPath = file.Path;

        }
        private async void SelectPicFromGallery()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                DialogProvider.DisplayNativeAlert("Photos Not Supported" + ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                CompressionQuality = 92
            });

            if (file == null)
                return;
            ProfileImgSource = file.Path;
            imgPath = file.Path;

        }
        private void SaveProfile()
        {
            DialogService.DisplayNativeAlert("Profile successfully saved", "MedCon");
            MedCon.Helpers.Settings.ProfilePic = imgPath;
            MessagingCenter.Send(imgPath, Constants.UpdateLeftMenuPickKey);
            IsEditVisible = true;
            IsSaveVisible = false;
        }
        private async void ChangePassword()
        {
            await NavigationService.NavigateToAsync<ForgotPasswordViewModel>();
        }
        private async void EditPick()
        {
            var answer=await Application.Current.MainPage.DisplayActionSheet(null, "Cancel", null, new string[] {"Take a pic","Choose from Gallery" });
           
            await CrossMedia.Current.Initialize();
            if (answer == "Take a pic")
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    DialogProvider.DisplayNativeAlert(":( No camera available.", "No Camera");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "MedCon",
                    Name = "Profile.jpg",
                    AllowCropping = true,
                    SaveToAlbum = true,
                    PhotoSize=Plugin.Media.Abstractions.PhotoSize.Medium,
                    CompressionQuality=92
                });

                if (file == null)
                    return;
            
                ProfileImgSource = file.Path;
                MedCon.Helpers.Settings.ProfilePic = file.Path;
                MessagingCenter.Send(file.Path, Constants.UpdateLeftMenuPickKey);
            }
            else if(answer== "Choose from Gallery")
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    DialogProvider.DisplayNativeAlert("Photos Not Supported"+":( Permission not granted to photos.", "OK");
                    return;
                }
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    CompressionQuality = 92
                });

                if (file == null)
                    return;

                ProfileImgSource = file.Path;
                MedCon.Helpers.Settings.ProfilePic = file.Path;
                MessagingCenter.Send(file.Path, Constants.UpdateLeftMenuPickKey);
            }
        }
        public override Task InitializeAsync(object navigationData)
        {
            GetPatientIdData();
            profileResponse = (JObject)navigationData;
            if(profileResponse!=null)
            {
                Profile profile = JsonConvert.DeserializeObject<Profile>(profileResponse.ToString());
                Username = profile.firstName + " " + profile.lastName;
                Email = profile.emailAddress;
                Mobile = profile.phoneNumber;
                Gender = profile.gender;
                Location = profile.location;
            }
            return base.InitializeAsync(navigationData);
        }

        public async void GetPatientIdData()
        {
            try
            {
                int count = 0;
                List<PatientIdsData> jarry = await requestProvider.GetAsync<List<PatientIdsData>>(Constants.ApiBase + "user/patient/attached");
                foreach (var item in jarry)
                {
                    count++;
                    HistoryModel historyModel = new HistoryModel();
                    JObject jarry1 = await requestProvider.GetAsync<JObject>(Constants.ContainerApiBase + "/container/info?patientnum=" + item.Patient);
                    RegimenRoot regimenRoot = JsonConvert.DeserializeObject<RegimenRoot>(jarry1.ToString());
                    historyModel.StartDate = regimenRoot.period.startDate;
                    historyModel.EndDate = regimenRoot.period.endDate;
                    historyModel.Name = regimenRoot.trial.name;
                    historyModel.PatientNum = item.Patient;
                    historyModel.ContainerNum = item.PatientContainer;
                    historyModel.TrialId = regimenRoot.trial.id;
                    Regimens.Add(regimenRoot);
                    DateTime date = DateTime.Parse(regimenRoot.period.endDate);
                    if (date > DateTime.Now)
                        historyModel.Status = "Active";
                    else
                        historyModel.Status = "Inactive";
                    if (count % 2 == 0)
                        historyModel.BackColor = "#ffffff";
                    else
                        historyModel.BackColor = "#f5f5f5";
                    historyModel.GotoHistoryDetailsCommand = new Command<ProfileTrial>(GotoDetails);
                    History.Add(historyModel);


                }
                foreach (var item in jarry)
                {
                    ProfileTrial profileTrial = new ProfileTrial();
                    JObject jarry1 = await requestProvider.GetAsync<JObject>(Constants.ContainerApiBase + "/container/info?patientnum="+item.Patient);
                    RegimenRoot regimenRoot = JsonConvert.DeserializeObject<RegimenRoot>(jarry1.ToString());
                    Regimens.Add(regimenRoot);
                    profileTrial.Date =DateTime.ParseExact(regimenRoot.period.startDate,"yyyy-MM-dd",null).ToString(GlobalSettings.MedConDateFormat);
                    profileTrial.PatientID = item.Patient;
                    profileTrial.TrialName = regimenRoot.trial.name;
                    profileTrial.ContainerNum = item.PatientContainer;
                    DateTime date = DateTime.Parse(regimenRoot.period.endDate);
                    if (date > DateTime.Now)
                        profileTrial.Status = "Active";
                    else
                        profileTrial.Status = "Inactive";
                    profileTrial.TrialId = regimenRoot.trial.id;
                    profileTrial.GotoHistoryDetailsCommand = new Command<ProfileTrial>(GotoDetails);
                    ProfileTrials.Add(profileTrial);
                }
            }
            catch (Exception ex)
            {
              
            }
        }
    }
    public class PatientIdsData
    {
        public string PatientContainer { get; set; }
        public string Container { get; set; }
        public string Patient { get; set; }
    }
    public class ProfileTrial
    {
        public string PatientID { get; set; }
        public string TrialName { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public int TrialId { get; set; }
        public string ContainerNum { get; set; }
        public ICommand GotoHistoryDetailsCommand { get; set; }
    }
}
