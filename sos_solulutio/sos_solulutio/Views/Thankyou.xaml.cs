using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sos_solulutio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Thankyou : ContentPage
    {
        public Thankyou()
        {
            InitializeComponent();
        }

        private async void Envoyer_ClickedAsync(object sender, EventArgs e)
        {


            await Navigation.PopAsync();

        }
    }
}