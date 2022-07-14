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
    public partial class Checkout : ContentPage
    {
        public Checkout()
        {
            InitializeComponent();
        }

        private async void envoyerout_Clicked(object sender, EventArgs e)
        {
            var ans = await DisplayAlert("Question?", "Est vous sure de fair cette demand ?", "Oui", "Non");
            if (ans == true)
            {
                await Navigation.PushAsync(new Attendancy
                {
                    //  BindingContext = new TodoItem()
                });
            }
        }
    }
}