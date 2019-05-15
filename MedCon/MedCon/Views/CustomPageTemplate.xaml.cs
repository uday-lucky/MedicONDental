using MedCon.Services;
using MedCon.ViewModels;
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
    public partial class CustomPageTemplate : SlideOverKit.MenuContainerPage
    {
        NavigationService navigationService;
        public View CustomDataTemplate
        {
            get
            {
                return (View)base.GetValue(DataTemplatetProperty);
            }
            set
            {
                if (true)
                    base.SetValue(DataTemplatetProperty, value);
            }
        }
        public Color HeaderColor
        {
            get
            {
                return (Color)base.GetValue(HeaderColorProperty);
            }
            set
            {
                if (true)
                    base.SetValue(HeaderColorProperty, value);
            }
        }
        public string  TitleText
        {
            get
            {
                return (string)base.GetValue(TitleTextProperty);
            }
            set
            {
                if (true)
                    base.SetValue(TitleTextProperty, value);
            }
        }
        public static readonly BindableProperty DataTemplatetProperty = BindableProperty.Create(
                                                               propertyName: "CustomDataTemplate",
                                                               returnType: typeof(View),
                                                               declaringType: typeof(CustomPageTemplate),
                                                               defaultValue: new Label { Text = "OOPS, You Missed your content" },
                                                               defaultBindingMode: BindingMode.TwoWay,
                                                               propertyChanged: DataTemplatePropertyChanged);
        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
                                                              propertyName: "TitleText",
                                                              returnType: typeof(string),
                                                              declaringType: typeof(CustomPageTemplate),
                                                              defaultValue: "",
                                                              defaultBindingMode: BindingMode.TwoWay,
                                                              propertyChanged: TitleTextPropertyChanged);
        public bool IsBackVisible { get; set; }
        public static readonly BindableProperty IsBackVisibleProperty = BindableProperty.Create(
                                                              propertyName: "IsBackVisible",
                                                              returnType: typeof(bool),
                                                              declaringType: typeof(CustomPageTemplate),
                                                              defaultValue: true,
                                                              defaultBindingMode: BindingMode.TwoWay,
                                                              propertyChanged: IsBackVisibletPropertyChanged);
        public static readonly BindableProperty HeaderColorProperty = BindableProperty.Create(propertyName: "HeaderColor",
            returnType: typeof(Color),
            declaringType: typeof(CustomPageTemplate),
            defaultValue: Color.FromHex("#5C6BC0"),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: HeaderColorPropertyChanged);
        
        public CustomPageTemplate()
        {
            InitializeComponent();
            Resources = new GlobalResources();

            navigationService = new NavigationService();
            this.SlideMenu = new LeftmenuPage();
        }
        protected override void OnAppearing()
        {
            HideMenu();
            base.OnAppearing();
        }
        //protected override void OnDisappearing()
        //{
        //    if(TitleText!="Dashboard")
        //    base.OnDisappearing();
       // }
        private static void HeaderColorPropertyChanged(BindableObject bindable,Object oldValue,Object newValue)
        {
            var myPage = (CustomPageTemplate)bindable;
            myPage.stackHeading.BackgroundColor = (Color)newValue;
        }
        private static void IsBackVisibletPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var headerView = (CustomPageTemplate)bindable;
            headerView.imgBack.IsVisible = (bool)newValue;
        }
        private static void DataTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var myPage = (CustomPageTemplate)bindable;
            myPage.grid.Children.Add((View)newValue, 0, 1);
        }
        private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var myPage = (CustomPageTemplate)bindable;
            myPage.headerTitle.Text =(string)newValue;
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            navigationService.NavigateToAsync<NotificationsViewModel>();
        }
        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            navigationService.NavigateToAsync<MyPreferenceViewModel>();
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            ShowMenu();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}