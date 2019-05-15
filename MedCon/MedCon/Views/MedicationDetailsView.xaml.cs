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
    public partial class MedicationDetailsView : ContentView
    {
        public static readonly BindableProperty DataContentProperty = BindableProperty.Create("DataContent",returnType:typeof(DetailspopUpModel),declaringType:typeof(MedicationDetailsView),defaultBindingMode:BindingMode.TwoWay,defaultValue:null,propertyChanged: ContentDataChanged);

        public static readonly BindableProperty IsMyMedicationProperty = BindableProperty.Create("IsMyMedication", returnType: typeof(bool), declaringType: typeof(MedicationDetailsView), defaultBindingMode: BindingMode.TwoWay, defaultValue: false, propertyChanged: IsMyMedicationChanged);

        public DetailspopUpModel DataContent
        {
            get { return (DetailspopUpModel)GetValue(DataContentProperty); }
            set { SetValue(DataContentProperty, value); }
        }
        public bool IsMyMedication
        {
            get { return (bool)GetValue(IsMyMedicationProperty); }
            set { SetValue(IsMyMedicationProperty, value); }
        }
        public MedicationDetailsView()
        {
            InitializeComponent();
        }
        private static void ContentDataChanged(BindableObject bindable, object oldValue, object newValue)
        {
            MedicationDetailsView medicationDetailsView = bindable as MedicationDetailsView;
            medicationDetailsView.detailsView.BindingContext = (DetailspopUpModel)newValue;
        }
        private static void IsMyMedicationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            MedicationDetailsView medicationDetailsView = bindable as MedicationDetailsView;
            medicationDetailsView.gridDetails1.IsVisible = !(bool)newValue;
            medicationDetailsView.gridDetails.IsVisible = false;

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if(IsMyMedication)
            {
                if (!gridDetails1.IsVisible)
                {
                    gridDetails1.IsVisible = true;
                    stackAdherence.IsVisible = false;
                    stackRemaining.IsVisible = false;

                    imgDetails.Source = "uparrow.png";
                    imgAdherence.Source = "downarrow1.png";
                    imgRemaning.Source = "downarrow1.png";
                }
                else
                {
                    gridDetails1.IsVisible = false;
                    stackAdherence.IsVisible = false;
                    stackRemaining.IsVisible = false;

                    imgDetails.Source = "downarrow1.png";
                    imgAdherence.Source = "downarrow1.png";
                    imgRemaning.Source = "downarrow1.png";
                }
            }
            else
            {
                if (!gridDetails.IsVisible)
                {
                    gridDetails.IsVisible = true;
                    stackAdherence.IsVisible = false;
                    stackRemaining.IsVisible = false;

                    imgDetails.Source = "uparrow.png";
                    imgAdherence.Source = "downarrow1.png";
                    imgRemaning.Source = "downarrow1.png";
                }
                else
                {
                    gridDetails.IsVisible = false;
                    stackAdherence.IsVisible = false;
                    stackRemaining.IsVisible = false;

                    imgDetails.Source = "downarrow1.png";
                    imgAdherence.Source = "downarrow1.png";
                    imgRemaning.Source = "downarrow1.png";
                }
            }          
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            if(!stackAdherence.IsVisible)
            {
                gridDetails.IsVisible = false;
                stackAdherence.IsVisible = true;
                stackRemaining.IsVisible = false;

                imgDetails.Source = "downarrow1.png";
                imgAdherence.Source = "uparrow.png";
                imgRemaning.Source = "downarrow1.png";
            }
            else
            {
                gridDetails.IsVisible = false;
                stackAdherence.IsVisible = false;
                stackRemaining.IsVisible = false;

                imgDetails.Source = "downarrow1.png";
                imgAdherence.Source = "downarrow1.png";
                imgRemaning.Source = "downarrow1.png";
            }
            
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            if(!stackRemaining.IsVisible)
            {
                gridDetails.IsVisible = false;
                stackAdherence.IsVisible = false;
                stackRemaining.IsVisible = true;

                imgDetails.Source = "downarrow1.png";
                imgAdherence.Source = "downarrow1.png";
                imgRemaning.Source = "uparrow.png";
            }
            else
            {
                gridDetails.IsVisible = false;
                stackAdherence.IsVisible = false;
                stackRemaining.IsVisible = false;

                imgDetails.Source = "downarrow1.png";
                imgAdherence.Source = "downarrow1.png";
                imgRemaning.Source = "downarrow1.png";
            }
            
        }
    }
}