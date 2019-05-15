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
	public partial class TrailsHeadingView : ContentView
	{

        public DoseTime Time
        {
            get
            {
                return (DoseTime)base.GetValue(DoseTimeProperty);
            }
            set
            {
                if (true)
                    base.SetValue(DoseTimeProperty, value);
            }
        }
        public static readonly BindableProperty DoseTimeProperty = BindableProperty.Create(propertyName: "Time",
            returnType:typeof(DoseTime),
            declaringType:typeof(TrailsHeadingView),
            defaultValue:DoseTime.Morning,
            defaultBindingMode:BindingMode.TwoWay,
            propertyChanged: DoseTimePropertyChanged);
        public TrailsHeadingView ()
		{
			InitializeComponent ();
		}
        private static void DoseTimePropertyChanged(BindableObject bindable,Object oldValue,Object newValue)
        {

        }
	}
    public enum DoseTime
    {
        Morning,
        Afternoon,
        Evening,
        Bedtime
    }
}