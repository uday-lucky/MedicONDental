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
    public partial class HistoryDetailsPage :CustomPageTemplate
    {
        public HistoryDetailsPage()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}