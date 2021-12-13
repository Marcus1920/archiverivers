using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sos_solulutio.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sos_solulutio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportDetails : ContentPage
    {
        private readonly Models.Reports _reports;

     
        public ReportDetails(Reports reports)
        {

            _reports = reports;
            BindingContext = _reports ?? throw new ArgumentNullException();
            InitializeComponent();
        }

        private async void mapView_Clicked(object sender, EventArgs e)
        {
         
            //await DisplayAlert("Notification", + "Votre Requete a etait recu Merci Patiente la police et en route", "OK");

            var location = new Location(this._reports.Lat,this._reports.Lng);
            var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

            try
            {
                await Map.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                // No map application available to open
            }

        }
    }
}