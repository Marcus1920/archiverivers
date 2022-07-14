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
    public partial class Attendancy : ContentPage
    {
        public Attendancy()
        {
            InitializeComponent();

            zxing.OnScanResult += (result) => Device.BeginInvokeOnMainThread(() => {
                lblResult.Text = result.Text;
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            zxing.IsScanning = true;
        }
        protected override void OnDisappearing()
        {
            zxing.IsScanning = false;
            base.OnDisappearing();
        }
    

       private async void checkin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CheckIn
            {
                //  BindingContext = new TodoItem()
            });
        }

        private async void checkout_Clicked(object sender, EventArgs e)
        {
            
                 await Navigation.PushAsync(new Checkout
                 {
                     //  BindingContext = new TodoItem()
                 });
        }

        private void envoyer_Clicked(object sender, EventArgs e)
        {

        }
    }
}