using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedCon.CustomControls
{
    [ContentProperty("ItemContent")]
    public class BindableGrid : ContentView
    {
        private ScrollView _scrollview;
        private Grid _stacklayout { get; set; }
        public BindableGrid()
        {
            _stacklayout = new Grid();
            _stacklayout.RowSpacing = 5;
            _stacklayout.ColumnSpacing = 5;
            _stacklayout.Margin = 0;
            _scrollview = new ScrollView()
            {
                Orientation = ScrollOrientation.Vertical,               
                Content = _stacklayout
            };
            Content = _scrollview;
        }
        public static readonly BindableProperty ItemContentProperty = BindableProperty.Create("ItemContent", typeof(DataTemplate), typeof(BindableGrid), default(ElementTemplate));

        public DataTemplate ItemContent
        {
            get { return (DataTemplate)GetValue(ItemContentProperty); }
            set { SetValue(ItemContentProperty, value); }
        }


        //private ScrollOrientation _scrollOrientation;
        //public ScrollOrientation Orientation
        //{
        //    get
        //    {
        //        return _scrollOrientation;
        //    }
        //    set
        //    {
        //        _scrollOrientation = value;
        //        _stacklayout.Orientation = value == ScrollOrientation.Horizontal ? StackOrientation.Horizontal : StackOrientation.Vertical;
        //        _scrollview.Orientation = value;
        //    }
        //}

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(BindableGrid), default(IEnumerable), propertyChanged: GetEnumerator);
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void GetEnumerator(BindableObject bindable, object oldValue, object newValue)
        {
            int row=0, column = 0;
            (bindable as BindableGrid)._stacklayout.Children.Clear();

            foreach (object child in (newValue as IEnumerable))
            {
                View view = (View)(bindable as BindableGrid).ItemContent.CreateContent();
                view.BindingContext = child;
                (bindable as BindableGrid)._stacklayout.Children.Add(view,column,row);
                column++;
                if(column==2)
                {
                    row++;
                    column = 0;
                }
            }
        }
    }
}
