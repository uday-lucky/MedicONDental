using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.CustomControls
{
    public enum TransitionType
    {
        Fade,
        Flip,
        Scale,
        SlideFromLeft,
        SlideFromRight,
        SlideFromTop,
        SlideFromBottom
    }
    public class CustomNavigationPage: NavigationPage
    {
        public static readonly BindableProperty TransitionTypeProperty =
                    BindableProperty.Create("TransitionType", typeof(TransitionType), typeof(CustomNavigationPage), TransitionType.SlideFromLeft);
        public TransitionType TransitionType
        {
            get { return (TransitionType)GetValue(TransitionTypeProperty); }
            set { SetValue(TransitionTypeProperty, value); }
        }

        public CustomNavigationPage() : base()
        {

        }

        public CustomNavigationPage(Page root) : base(root)
        {

        }

    }
}
