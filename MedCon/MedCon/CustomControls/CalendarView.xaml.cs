using MedCon.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MedCon.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarView : ContentView
    {
        DateTime date;
        public static readonly BindableProperty SpecialDatesProperty = BindableProperty.Create("SpecialDates",
                   typeof(ObservableCollection<CustomDate>), typeof(CalendarView), null, propertyChanged: OnItemsSourceChanged);
        public static readonly BindableProperty SelectedDateCommandProperty = BindableProperty.Create("SelectedDateCommand",
                  typeof(ICommand), typeof(CalendarView), null, propertyChanged: OnCommandPropertyChanged);
        public ObservableCollection<CustomDate> SpecialDates
        {
            get { return (ObservableCollection<CustomDate>)GetValue(SpecialDatesProperty); }
            set { SetValue(SpecialDatesProperty, value); }
        }
        public ICommand SelectedDateCommand
        {
            get { return (ICommand)GetValue(SelectedDateCommandProperty); }
            set { SetValue(SelectedDateCommandProperty, value); }
        }
        public CalendarView()
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
                Image img = new Image { HeightRequest = 15, WidthRequest = 15, VerticalOptions = LayoutOptions.Center,Margin=new Thickness(0,-9,0,0) };

                CustomDate custom;
                if (SpecialDates != null&&SpecialDates.Count>0)
                {
                    var ddsds = SpecialDates[0].Date.Day;
                    var ddsds1 = dateTime.Day;
                    custom = SpecialDates.Where(x => x.Date.Year == dateTime.Year && x.Date.Month == dateTime.Month && x.Date.Day == i).FirstOrDefault();
                    if (custom != null)
                    {
                        img.Source = custom.Image;
                        img.BindingContext = custom.Data;
                    }                       
                }
                StackLayout stack = new StackLayout() { Spacing = 0, HeightRequest = 50, VerticalOptions = LayoutOptions.Fill };
                CustomControls.RoundedLabel lbl = new CustomControls.RoundedLabel() { HeightRequest = 30, WidthRequest = 30, CurvedCornerRadius = 15, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
                if (dateTime.Year == DateTime.Now.Year && dateTime.Month == DateTime.Now.Month && DateTime.Now.Day== i)
                    lbl.CurvedBackgroundColor = Color.FromHex("#7986CB");
                lbl.Text = i.ToString();
                lbl.FontSize = 16;
                lbl.Style = (Style)Application.Current.Resources["LabelStyle"];
                lbl.HorizontalTextAlignment = TextAlignment.Center;
                lbl.TextColor = Color.Black;
                stack.Children.Add(lbl);
                stack.Children.Add(img);
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
            StackLayout stack1 = sender as StackLayout;
            Image image1 = stack1.Children[1] as Image;
            ContainerData data =image1.BindingContext as ContainerData;
            if(data!=null)
            {
                if (data.DoseTimeImage != null)
                    await App.Current.MainPage.Navigation.PushPopupAsync(new Views.HistoryCalendarPopup(data));
            }
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

                var calendar = (CalendarView)bindable;
                calendar.SpecialDates = (ObservableCollection<CustomDate>)newValue;
                calendar.DisplayCurrentMonth(DateTime.Now);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private static void OnCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var calendar = (CalendarView)bindable;
            calendar.SelectedDateCommand = (ICommand)newValue;
            calendar.DisplayCurrentMonth(DateTime.Now);
        }
    }
    public enum WeekDays
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6

    }
    public class CustomDate
    {
        public ImageSource Image { get; set; }
        public DateTime Date { get; set; }

        public ContainerData Data { get; set; }
    }
}