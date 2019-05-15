using MedCon.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MedCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryCalendarPopup : PopupPage
    {
        public ContainerData Data { get; set; }
        public HistoryCalendarPopup(ContainerData containerData)
        {
            Data = containerData;
            Data.DateOrTime = Data.DateOrTime.Replace("| ", "");
            Data.DoseTakenTime= DateTime.ParseExact(Data.DateOrTime, "MM/dd/yyyy hh:mm tt", null).ToString(GlobalSettings.MedConDateFormat);
            // Data.DoseTakenTime=ReuiredDate(DateTime.ParseExact(Data.DateOrTime));

            Data.Window = GetWindowName(Data.Win);
            Data.ScanType = GetScanType(Data.Entry);
            InitializeComponent();
            BindingContext = Data;
        }
        string ReuiredDate(DateTime dateTime)
        {
            return string.Format("{0}/{1}/{2}, {3}:{4} {5}", dateTime.Month, dateTime.Day, dateTime.Year, dateTime.Hour, dateTime.Minute, dateTime.ToString("tt"));
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
        string GetWindowName(string windowShort)
        {
            string Window = string.Empty;
            switch (windowShort)
            {
                case "M":
                    Window = "Morning";
                    break;
                case "A":
                    Window = "Afternoon";
                    break;
                case "E":
                    Window = "Evening";
                    break;
                case "B":
                    Window = "Bedtime";
                    break;
            }
            return Window;
        }
        string GetScanType(string scanShort)
        {
            if (scanShort == "S")
                return "Scan";
            else
                return "Manual";
        }
    }
}