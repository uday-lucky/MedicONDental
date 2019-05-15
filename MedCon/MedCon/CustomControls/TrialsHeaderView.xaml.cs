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
	public partial class TrialsHeaderView : ContentView
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
            returnType: typeof(DoseTime),
            declaringType: typeof(TrialsHeaderView),
            defaultValue: DoseTime.Morning,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: DoseTimePropertyChanged);
        public TrialsHeaderView()
        {
            InitializeComponent();

        }
        private static void DoseTimePropertyChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            var headerView = (TrialsHeaderView)bindable;
            DoseTime doseTime = (DoseTime)newValue;
            switch (doseTime)
            {
                case DoseTime.Afternoon:
                    headerView.gridHeading.BackgroundColor = Color.FromHex("#4fc3f7");
                    headerView.lblTimeName.TextColor = Color.Black;
                    break;
                case DoseTime.Bedtime:
                    headerView.gridHeading.BackgroundColor = Color.FromHex("0288d1");
                    headerView.lblTimeName.TextColor = Color.White;
                    break;
                case DoseTime.Evening:
                    headerView.gridHeading.BackgroundColor = Color.FromHex("#ffa000");
                    headerView.lblTimeName.TextColor = Color.White;
                    break;
                case DoseTime.Morning:
                    headerView.gridHeading.BackgroundColor = Color.FromHex("#ffed8f");
                    headerView.lblTimeName.TextColor = Color.White;
                    break;
                default:
                    headerView.gridHeading.BackgroundColor = Color.FromHex("#ffed8f");
                    headerView.lblTimeName.TextColor = Color.White;
                    break;
            }
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