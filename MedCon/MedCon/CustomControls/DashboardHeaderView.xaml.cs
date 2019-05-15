﻿using MedCon.Services;
using MedCon.ViewModels;
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
    public partial class DashboardHeaderView : ContentView
    {
        public bool IsBackVisible { get; set; }
        public string TitleText { get; set; }
        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
                                                                propertyName: "TitleText",
                                                                returnType: typeof(string),
                                                                declaringType: typeof(HeaderView),
                                                                defaultValue: "",
                                                                defaultBindingMode: BindingMode.TwoWay,
                                                                propertyChanged: TitleTextPropertyChanged);
        public static readonly BindableProperty IsBackVisibleProperty = BindableProperty.Create(
                                                              propertyName: "IsBackVisible",
                                                              returnType: typeof(bool),
                                                              declaringType: typeof(HeaderView),
                                                              defaultValue: true,
                                                              defaultBindingMode: BindingMode.TwoWay,
                                                              propertyChanged: IsBackVisibletPropertyChanged);
        public DashboardHeaderView()
        {
            InitializeComponent();

        }
        private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var headerView = (DashboardHeaderView)bindable;
            headerView.headerTitle.Text = newValue.ToString();
        }
        private static void IsBackVisibletPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var headerView = (DashboardHeaderView)bindable;
            headerView.imgBack.IsVisible = (bool)newValue;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            try
            {
                MessagingCenter.Send("0", Constants.MenuKey);

            }
            catch (Exception ex)
            {

               
            }
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            NavigationService navigationService = new NavigationService();
            navigationService.NavigateToAsync<NotificationsViewModel>();
        }
       
    }
}