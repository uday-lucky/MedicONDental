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
	public partial class ScanConfirmationPage : CustomPageTemplate
	{
		public ScanConfirmationPage ()
		{
			InitializeComponent ();
            MessagingCenter.Subscribe<string>(this, Constants.FocusChangeTimePickerKey, (value) =>
            {
                pickerTime.Focus();
            });
        }
	}
}