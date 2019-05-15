using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MedCon.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyMedicationView : ContentView
    {
        public MyMedicationView()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            pickerTrialTime.Focus();
        }
    }
}