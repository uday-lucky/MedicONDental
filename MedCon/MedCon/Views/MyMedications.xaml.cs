using MedCon.CustomControls;
using MedCon.ViewModels;
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
    public partial class MyMedications : ContentView
    {

        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(nameof(ItemSource), typeof(ObservableCollection<Trial>), typeof(MyMedications), null,BindingMode.TwoWay,propertyChanged:ItemSourceChanged);
      
        //protected override void OnPropertyChanged(string propertyName = null)
        //{
        //    base.OnPropertyChanged(propertyName);
        //    if (propertyName == nameof(ItemSource))
        //    {
        //        foreach (var item in ItemSource)
        //        {
                  
        //        }
        //    }
        //}
        static void ItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (MyMedications)bindable;
            view.stackContent.Children.Clear();
            if(view.ItemSource!=null&&view.ItemSource.Count>0)
            {
                for (int i = 0; i < 4; i++)
                {
                    MyMedicationView myMedicationView = new MyMedicationView();
                    myMedicationView.BindingContext = view.ItemSource[0];
                    view.stackContent.Children.Add(myMedicationView);
                }

                foreach (var item in view.ItemSource)
                {
                   

                }
            }
            // Property changed implementation goes here
        }
        public ObservableCollection<Trial> ItemSource
        {
            get
            {
                return (ObservableCollection<Trial>)GetValue(ItemSourceProperty);
            }
            set
            {
                SetValue(ItemSourceProperty, value);
            }
        }
        public MyMedications()
        {
            InitializeComponent();
        }
    }
}