using MedCon.CustomControls;
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
	public partial class TrialsPage : CustomPageTemplate
	{
		public TrialsPage ()
		{	
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, Constants.ShowTrialPickerKey, (value) =>
            {
                trialPicker.Focus();
            });
            TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped += GestureRecognizer_Tapped;
          //  frame.GestureRecognizers.Add(gestureRecognizer);
            //MessagingCenter.Subscribe<string>(this, Constants.ShowDashboardPickerKey, (value) =>
            //{
            //   // pickerTrialTime.Focus();
            //});
        }

        private void GestureRecognizer_Tapped(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
          //  listview.SelectedItem = null;
        }
    }
}