using MedCon.Models;
using MedCon.Services;
using MedCon.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MedCon.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardCalendar : ContentView
    {
        DateTime date;
        public static readonly BindableProperty DataProperty = BindableProperty.Create("Data",
                   typeof(DashboardCalendarData), typeof(DashboardCalendar), null,BindingMode.TwoWay, propertyChanged: OnItemsSourceChanged);
        public static readonly BindableProperty SelectedDateCommandProperty = BindableProperty.Create("SelectedDateCommand",
                  typeof(ICommand), typeof(DashboardCalendar), null, propertyChanged: OnCommandPropertyChanged);
        public DashboardCalendarData Data
        {
            get { return (DashboardCalendarData)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public ICommand SelectedDateCommand
        {
            get { return (ICommand)GetValue(SelectedDateCommandProperty); }
            set { SetValue(SelectedDateCommandProperty, value); }
        }
        public DashboardCalendar()
        {

            date = DateTime.Now;
            InitializeComponent();

            TapGestureRecognizer prevTapped = new TapGestureRecognizer();
            prevTapped.Tapped += BtnPrev_Clicked;
            imgPrev.GestureRecognizers.Add(prevTapped);

            TapGestureRecognizer nextTapped = new TapGestureRecognizer();
            nextTapped.Tapped += BtnNext_Clicked;
            imgNext.GestureRecognizers.Add(nextTapped);
            //btnNext.Clicked += BtnNext_Clicked;
            //btnPrev.Clicked += BtnPrev_Clicked;
            DisplayCurrentMonth(date);
        }

        private void BtnPrev_Clicked(object sender, EventArgs e)
        {
            DisplayCurrentMonth(date.AddMonths(-1));
        }

        private void BtnNext_Clicked(object sender, EventArgs e)
        {
            DisplayCurrentMonth(date.AddMonths(1));
        }

        private void DisplayCurrentMonth(DateTime dateTime)
        {
            grid.Children.Clear();
            date = dateTime;
            var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var firstDay = new DateTime(dateTime.Year, dateTime.Month, 1).DayOfWeek;
            int lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).Day;
            string dayname = firstDay.ToString();
            int startDayColumn = GetStartDate(dayname);
            lblMonth.Text = dateTime.ToString("MMMM,yyyy");
            int row = 0;
            for (int i = 1; i <= lastDayOfMonth; i++)
            {
                Image img = new Image { HeightRequest = 11, WidthRequest = 11, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(0, -9, 0, 0) };
                RegimenHistory custom;
                if (Data != null && Data.Histories!=null&& Data.Histories.Count > 0)
                {
                    foreach (var item in Data.Histories)
                    {
                        string doseTime = item.time;
                        string[] dateSplit = doseTime.Split('|');
                        doseTime = dateSplit[0].TrimEnd();
                        DateTime doseDate = DateTime.ParseExact(doseTime, "MM/dd/yyyy", null);
                        var ddsds1 = dateTime.Day;
                        if(doseDate.Year==dateTime.Year&&doseDate.Month==dateTime.Month&&doseDate.Day==i)
                        {
                                img.Source = "dosetaken.png";
                                img.BindingContext = item;                         
                        }
                    }
                }               
                StackLayout stack = new StackLayout() { Spacing = 0, HeightRequest = 50, VerticalOptions = LayoutOptions.Fill };
                DateTime dateTime1 = new DateTime(dateTime.Year, dateTime.Month, i);
                stack.BindingContext = dateTime1;
                Label lblheading = new Label();
                lblheading.Text = "Holiday";
                CustomControls.RoundedLabel lbl = new CustomControls.RoundedLabel() { HeightRequest = 30, WidthRequest = 30, CurvedCornerRadius = 15, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
                if (dateTime.Year == DateTime.Now.Year && dateTime.Month == DateTime.Now.Month && DateTime.Now.Day == i)
                    lbl.CurvedBackgroundColor = Color.FromHex("#7986CB");
                lbl.Text = i.ToString();
                lbl.FontSize = 16;
                lblheading.FontSize=8;
                lblheading.HorizontalTextAlignment = TextAlignment.Center;
                lblheading.TextColor = Color.Red;
                lbl.Style = (Style)Application.Current.Resources["LabelStyle"];
                lbl.HorizontalTextAlignment = TextAlignment.Center;
                lbl.TextColor = Color.Black;
                stack.Children.Add(lblheading);
                stack.Children.Add(lbl);

                stack.Children.Add(img);
                if (Data != null && Data.Regimens != null && Data.Regimens.Count > 0)
                {
                    DateTime pDateTime =new DateTime(dateTime.Year,dateTime.Month,i);
                    foreach (var regimen in Data.Regimens)
                    {
                        if (regimen.regimen.schedule_type == "Daily")
                        {
                            DateTime startDate = DateTime.ParseExact(regimen.trial.startDate, "yyyy-MM-dd", null);
                            DateTime endDate = DateTime.ParseExact(regimen.trial.endDate, "yyyy-MM-dd", null);
                           if(pDateTime >= startDate && pDateTime <= endDate&&pDateTime>DateTime.Now)
                            {
                                    CustomControls.RoundedLabel lbl10 = new CustomControls.RoundedLabel() { HeightRequest = 30, WidthRequest = 30, CurvedCornerRadius = 15, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
                                    lbl10.CurvedBackgroundColor = Color.FromHex("#efbb40");
                                    lbl10.Text = i.ToString();
                                    lbl10.FontSize = 16;
                                    lbl10.Style = (Style)Application.Current.Resources["LabelStyle"];
                                    lbl10.HorizontalTextAlignment = TextAlignment.Center;
                                    lbl10.TextColor = Color.Black;
                                    stack.Children.Clear();
                                    stack.Children.Add(lbl10);
                            }
                        }
                    }
                }
                TapGestureRecognizer dateTapped = new TapGestureRecognizer();
                dateTapped.Tapped += DateTapped_Tapped;
                //dateTapped.Command = SelectedDateCommand;
                // dateTapped.CommandParameter = i;
                stack.GestureRecognizers.Add(dateTapped);
                grid.Children.Add(stack, startDayColumn, row);
                startDayColumn++;
                if (startDayColumn == 7)
                {
                    row++;
                    startDayColumn = 0;
                }
            }
        }

        private async void DateTapped_Tapped(object sender, EventArgs e)
        {
            StackLayout stack = sender as StackLayout;
            var dateTime10= stack.BindingContext ;
            MessagingCenter.Send(dateTime10.ToString(), Constants.UpdateDashboardCalendar);
           // var control = stack.Children[0] as RoundedLabel;
           //if(control.CurvedBackgroundColor.R==-1)
           // {
           //    await Application.Current.MainPage.DisplayAlert("MedCon", "No data found for this date!", "Ok");
           //     return;
           // }
            MessagingCenter.Send<string>((stack.BindingContext).ToString(), Constants.UpdateDashboardCalendarDate);
            MessagingCenter.Send((stack.BindingContext).ToString(), Constants.UpdateDashboardDateKey);

            Constants.SelectedDate = (stack.BindingContext).ToString();
          await new NavigationService().NavigateToAsync<DashboardViewModel>();
        }
        private int GetStartDate(string day)
        {
            int start = 0;
            switch (day)
            {
                case "Monday":
                    start = (int)WeekDays.Monday;
                    break;
                case "Tuesday":
                    start = (int)WeekDays.Tuesday;
                    break;
                case "Wednesday":
                    start = (int)WeekDays.Wednesday;
                    break;
                case "Thursday":
                    start = (int)WeekDays.Thursday;
                    break;
                case "Friday":
                    start = (int)WeekDays.Friday;
                    break;
                case "Saturday":
                    start = (int)WeekDays.Saturday;
                    break;
                case "Sunday":
                    start = (int)WeekDays.Sunday;
                    break;
                default:
                    break;
            }
            return start;
        }
        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var calendar = (DashboardCalendar)bindable;
                calendar.Data = (DashboardCalendarData)newValue;
                var selectedTrialRegimen = calendar.Data.Regimens.Where(x => x.trial.id == Constants.SelectedTrial).ToList();
                calendar.Data.Regimens.Clear();
                if(selectedTrialRegimen!=null)
                {
                    foreach (var item in selectedTrialRegimen)
                    {
                        calendar.Data.Regimens.Add(item);
                    }
                }
                calendar.DisplayCurrentMonth(DateTime.Now);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static void OnCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendar = (DashboardCalendar)bindable;
            calendar.SelectedDateCommand = (ICommand)newValue;
            calendar.DisplayCurrentMonth(DateTime.Now);
        }
    }
}