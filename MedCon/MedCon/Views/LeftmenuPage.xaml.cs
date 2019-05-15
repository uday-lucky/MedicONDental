using MedCon.CustomControls;
using SlideOverKit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MedCon.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeftmenuPage : SlideMenuView
    {
        public LeftmenuPage()
        {   
            InitializeComponent();
            Resources = new GlobalResources();

            // You must set IsFullScreen in this case, 
            // otherwise you need to set HeightRequest, 
            this.IsFullScreen = true;
            // You must set WidthRequest in this case
            this.WidthRequest = 250;
            this.MenuOrientations = MenuOrientation.LeftToRight;

            // You must set BackgroundColor, 
            // and you cannot put another layout with background color cover the whole View
            // otherwise, it cannot be dragged on Android
            this.BackgroundColor = Color.FromHex("#5C6BC0");

            // This is shadow view color, you can set a transparent color
            this.BackgroundViewColor = new Color(0, 0, 0, 0.5);
        }
    }
}