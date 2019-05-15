using MedCon.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.CustomCalendar
{
   public class CalendarFrame:Frame
    {
        Label label;
        public CalendarFrame()
        {
            label = new Label();
            label.SetDynamicResource(VisualElement.StyleProperty, "LabelStyle");
            label.HorizontalTextAlignment = TextAlignment.Center;
            label.VerticalTextAlignment = TextAlignment.Center;
            Padding = new Thickness(5);
            CornerRadius = 20;
            StackLayout stack = new StackLayout();
            stack.Spacing = 2;
            stack.Children.Add(label);
            RoundedBoxView roundedBoxView = new RoundedBoxView();
            roundedBoxView.CornerRadius = 3;
            roundedBoxView.BackgroundColor = Color.Red;
            roundedBoxView.HeightRequest = 5;
            roundedBoxView.WidthRequest = 5;
            roundedBoxView.HorizontalOptions = LayoutOptions.Center;
            stack.Children.Add(roundedBoxView);
            Content = stack;
        }
        public string LabelText 
        {
            get
            {
                return (string)base.GetValue(LabelTextProperty);
            }
            set
            {
                if (true)
                    base.SetValue(LabelTextProperty, value);
            }
        }
        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
                                                               propertyName: "LabelText",
                                                               returnType: typeof(string),
                                                               declaringType: typeof(CalendarFrame),
                                                               defaultValue: "",
                                                               defaultBindingMode: BindingMode.TwoWay,
                                                               propertyChanged: LabelTextPropertyChanged);
        public FontAttributes FontAttributes
        {
            get
            {
                return (FontAttributes)base.GetValue(FontAttributesProperty);
            }
            set
            {
                if (true)
                    base.SetValue(FontAttributesProperty, value);
            }
        }
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(
                                                               propertyName: "FontAttributes",
                                                               returnType: typeof(FontAttributes),
                                                               declaringType: typeof(CalendarFrame),
                                                               defaultValue: FontAttributes.None,
                                                               defaultBindingMode: BindingMode.TwoWay);
        public double FontSize
        {
            get
            {
                return (double)base.GetValue(FontSizeProperty);
            }
            set
            {
                if (true)
                    base.SetValue(FontSizeProperty, value);
            }
        }
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
                                                               propertyName: "FontSize",
                                                               returnType: typeof(double),
                                                               declaringType: typeof(CalendarFrame),
                                                               defaultValue: double.Parse("13"),
                                                               defaultBindingMode: BindingMode.TwoWay);
        public Color TextColor
        {
            get
            {
                return (Color)base.GetValue(TextColorProperty);
            }
            set
            {
                if (true)
                    base.SetValue(TextColorProperty, value);
            }
        }
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
                                                               propertyName: "FontSize",
                                                               returnType: typeof(Color),
                                                               declaringType: typeof(CalendarFrame),
                                                               defaultValue: Color.FromHex("#c82727"),
                                                               defaultBindingMode: BindingMode.TwoWay,
                                                               propertyChanged: TextColorPropertyChanged);
        public Font Font
        {
            get
            {
                return (Font)base.GetValue(FontProperty);
            }
            set
            {
                if (true)
                    base.SetValue(FontProperty, value);
            }
        }
        public static readonly BindableProperty FontProperty = BindableProperty.Create(
                                                               propertyName: "Font",
                                                               returnType: typeof(Font),
                                                               declaringType: typeof(CalendarFrame),
                                                               defaultValue:Font.Default,
                                                               defaultBindingMode: BindingMode.TwoWay);
        public string FontFamily
        {
            get
            {
                return (string)base.GetValue(FontFamilyProperty);
            }
            set
            {
                if (true)
                    base.SetValue(FontFamilyProperty, value);
            }
        }
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
                                                               propertyName: "FontFamily",
                                                               returnType: typeof(string),
                                                               declaringType: typeof(CalendarFrame),
                                                               defaultValue:null,
                                                               defaultBindingMode: BindingMode.TwoWay);
        public FileImageSource Image
        {
            get
            {
                return (FileImageSource)base.GetValue(ImageProperty);
            }
            set
            {
                if (true)
                    base.SetValue(ImageProperty, value);
            }
        }
        public static readonly BindableProperty ImageProperty = BindableProperty.Create(
                                                               propertyName: "Image",
                                                               returnType: typeof(FileImageSource),
                                                               declaringType: typeof(CalendarFrame),
                                                               defaultValue: null,
                                                               defaultBindingMode: BindingMode.TwoWay);
        private static void LabelTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var currentView = (CalendarFrame)bindable;
            currentView.label.Text = (string)newValue;
        }
        private static void TextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var currentView = (CalendarFrame)bindable;
            currentView.label.TextColor = (Color)newValue;
        }
    }
}
