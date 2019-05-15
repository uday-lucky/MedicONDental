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
    public partial class HistoryPage : CustomPageTemplate
    {
        public HistoryPage()
        {
            InitializeComponent();
            listview.ItemSelected += Listview_ItemSelected;
        }

        private void Listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView lstView = (ListView)sender;
            lstView.SelectedItem = null;
        }
    }
}