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
    public partial class DoseRemainderView : ContentView
    {
        public Color ViewColor
        {
            get
            {
                return (Color)base.GetValue(ColorProperty);
            }
            set
            {
                if (true)
                    base.SetValue(ColorProperty, value);
            }
        }
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(
                                                               propertyName: "ViewColor",
                                                               returnType: typeof(Color),
                                                               declaringType: typeof(DoseRemainderView),
                                                               defaultValue: Color.White,
                                                               defaultBindingMode: BindingMode.TwoWay,
                                                               propertyChanged: ColorPropertyChanged);
        public DoseRemainderView()
        {
            InitializeComponent();

        }
        private static void ColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var currentView = (DoseRemainderView)bindable;
            currentView.BackgroundColor =(Color)newValue;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Constants.doseRemainder.IsRemainderVisible = false;
        }
    }
}