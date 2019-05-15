﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MedCon.CustomControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatusView : ContentView
	{
        public double Medicinesize
        {
            get
            {
                return (double)base.GetValue(MedicineSizeProperty);
            }
            set
            {
                if (this.Medicinesize > 0)
                    base.SetValue(MedicineSizeProperty, value);
            }
        }
        public double StatusSize
        {
            get
            {
                return (double)base.GetValue(StatusSizeProperty);
            }
            set
            {
                if (this.StatusSize > 0)
                    base.SetValue(StatusSizeProperty, value);
            }
        }
        public string MedicineIcon
        {
            get
            {
                return (string)base.GetValue(MedicineIconProperty);
            }
            set
            {
                if (this.MedicineIcon != null)
                    base.SetValue(MedicineIconProperty, value);
            }
        }
      
        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource StatusIconSource
        {
            get { return (ImageSource)GetValue(StatusIconSourceProperty); }
            set { SetValue(StatusIconSourceProperty, value); }
        }
        public static readonly BindableProperty MedicineSizeProperty = BindableProperty.Create(
                                                                propertyName: "Medicinesize",
                                                                returnType: typeof(double),
                                                                declaringType: typeof(StatusView),
                                                                defaultValue: 30.00,
                                                                defaultBindingMode: BindingMode.TwoWay,
                                                                propertyChanged: MedicineSizePropertyChanged);
        public static readonly BindableProperty StatusSizeProperty = BindableProperty.Create(
                                                              propertyName: "StatusSize",
                                                              returnType: typeof(double),
                                                              declaringType: typeof(StatusView),
                                                              defaultValue: 25.00,
                                                              defaultBindingMode: BindingMode.TwoWay,
                                                              propertyChanged: StatusSizePropertyChanged);
        public static readonly BindableProperty MedicineIconProperty = BindableProperty.Create(
                                                              propertyName: "MedicineIcon",
                                                              returnType: typeof(ImageSource),
                                                              declaringType:typeof(StatusView),
                                                              defaultValue: null,
                                                              defaultBindingMode: BindingMode.TwoWay,
                                                              propertyChanged: MedicineIconPropertyChanged);
        public static readonly BindableProperty StatusIconSourceProperty = BindableProperty.Create(
                                                             "StatusIconSource", typeof(ImageSource), typeof(StatusView),
                                                              null,
                                                              BindingMode.OneWay,
                                                              null,
                                                             (bindable, oldvalue, newvalue) => ((VisualElement)bindable).ToString());

        public StatusView ()
		{
			InitializeComponent ();
		}
        private static void MedicineIconPropertyChanged(BindableObject bindable,object oldValue,object newValue)
        {
            var statusview = (StatusView)bindable;
            statusview.imgMedicine.Source = (ImageSource)newValue;
        }
        private static void MedicineStatusPropertyChanged(BindableObject bindable,object oldValue,object newValue)
        {
            var statusview = (StatusView)bindable;
            statusview.imgStatus.Source = (string)newValue;
        }
        private static void MedicineSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var statusview = (StatusView)bindable;
            double prevHeight = statusview.stackMedicine.HeightRequest;
            double newHeight = prevHeight - double.Parse(newValue.ToString());
            statusview.stackMedicine.HeightRequest = statusview.stackMedicine.WidthRequest = double.Parse(newValue.ToString());
        }
        private static void StatusSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var statusview = (StatusView)bindable;
            statusview.imgStatus.HeightRequest = statusview.imgStatus.WidthRequest = double.Parse(newValue.ToString());
        }
    }
}