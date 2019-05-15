using MedCon.Models;
using MedCon.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MedCon.ViewModels
{
   public class InvalidScanViewModel:ViewModelBase
    {
        public MedicineItem ScannedItem { get; set; }
        public ICommand ScanCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public InvalidScanViewModel()
        {
            ScanCommand = new Command(ScanClick);
            CancelCommand = new Command(CancelClick);
        }
       async void ScanClick()
        {
            string scanResult = await GetScanResultAsync();
            if(scanResult==ScannedItem.container_id)
            {
                await NavigationService.NavigateBackAsync();
                await NavigationService.NavigateToAsync<ScanConfirmationViewModel>(ScannedItem);
            }
        }
        void CancelClick()
        {
            NavigationService.NavigateBackAsync();
        }
        private async Task<string> GetScanResultAsync()
        {
            string scanResult = string.Empty;
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner() { TopText = "Please scan the container barcode", CancelButtonText = "Enter Manually" };
                var result = await scanner.Scan();
                return scanResult = result.Text;
            }
            catch (Exception ex)
            {
                return "";
            }
            return scanResult;
        }
        public override Task InitializeAsync(object navigationData)
        {
            ScannedItem = (MedicineItem)navigationData;
            return base.InitializeAsync(navigationData);
        }
    }
}
