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
	public partial class CustomPage : SlideOverKit.MenuContainerPage
	{
		public CustomPage ()
		{
			InitializeComponent ();
            MessagingCenter.Subscribe<string>(this, Constants.MenuKey, (isShow) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (isShow != "0") HideMenuAction();
                    else
                        ShowMenuAction();
                });
            });
            this.SlideMenu = new LeftmenuPage();
        }
	}
}