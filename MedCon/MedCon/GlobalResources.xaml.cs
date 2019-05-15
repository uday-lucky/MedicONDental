using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MedCon
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GlobalResources : ResourceDictionary
    {
        public GlobalResources()
        {
            InitializeComponent();
        }
    }
}