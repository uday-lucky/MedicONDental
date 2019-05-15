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
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            Resources = new GlobalResources();

            MessagingCenter.Subscribe<string>(this, Constants.ShowPickerKey, (value) =>
            {
                if (value == "2") pickerCountry.Focus();
               else if (value != "0") pickerRace.Focus();
                    else
                        datepicker.Focus();
            });
        }
    }
}