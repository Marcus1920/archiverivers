using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sos_solulutio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SmsPanic : ContentPage
    {
        private readonly string BaseUrl = "https://stormy-wildwood-40195.herokuapp.com/api/incidents/create-incident-sos";

        private readonly string lat = null;
        private readonly string longt = null;

        CancellationTokenSource cts;
        public SmsPanic()
        {
            InitializeComponent();
        }
        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }

        private async void Btn2_ClickedAsync(object sender, EventArgs e)
        {

            var ans = await DisplayAlert("Question?", "Est vous sure de fair cette demand ?", "Oui", "Non");
            if (ans == true)
            {

                var current = Connectivity.NetworkAccess;


                try
                {
                    //   loader.IsVisible = true;
                    //   label.IsVisible = true;
                    // main_gride.IsVisible = false;
                    UserDialogs.Instance.ShowLoading("Veuillez patienter, nous traitons votre demande", MaskType.Black);
                    var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    cts = new CancellationTokenSource();
                    var location = await Geolocation.GetLocationAsync(request, cts.Token);

                    if (location != null)
                    {
                        ///  Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                        ////    await DisplayAlert("Notification", location.Latitude + " permission exception", "OK");
                       


                        Preferences.Set("lattide", location.Latitude.ToString("N6").Replace(",", "."));
                        Preferences.Set("longitude", location.Longitude.ToString("N6").Replace(",", "."));


                        var user_id = Preferences.Get("user_id", "");

                       // Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                        var name = Preferences.Get("name", "");
                        var longit =location.Longitude.ToString("N6").Replace(",", ".");
                        var lattid = location.Latitude.ToString("N6").Replace(",", ".");
                        var messageText = "SOS ...C'est " + "Marcus" + "je suis en Danger voici ma lacation  " + "http://www.google.com/maps/place/" + lattid + "," + longit;
                        var recipient = "+243848478160";
                        var message = new SmsMessage(messageText, new[] { recipient });
                        await Sms.ComposeAsync(message);
                        UserDialogs.Instance.HideLoading();

                    }

                    else
                    {
                        // await DisplayAlert("Notification", " Cant get Location", "OK");
                        // UserDialogs.Instance.HideLoading();

                        var name = Preferences.Get("name", "");
                        var longit = Preferences.Get("longitude", "");
                        var lattid = Preferences.Get("lattide", "");
                        var messageText = "SOS ...C'est " + "Marcus" + "je suis en Danger voici ma lacation  " + "http://www.google.com/maps/place/" + lattid + "," + longit;
                        var recipient = "+243848478160";
                        var message = new SmsMessage(messageText, new[] { recipient });
                        await Sms.ComposeAsync(message);
                        UserDialogs.Instance.HideLoading();


                    }
                }

                catch (FeatureNotSupportedException fnsEx)
                {
                    // Handle not supported on device exception
                    await DisplayAlert("Notification", " not supported on device", "OK");
                    loader.IsVisible = false;
                    label.IsVisible = false;
                    UserDialogs.Instance.HideLoading();
                }
                catch (FeatureNotEnabledException fneEx)
                {

                    // Handle not enabled on device exception
                    await DisplayAlert("Notification", " Veuillez active votre Location ", "OK");
                    loader.IsVisible = false;
                    label.IsVisible = false;
                    UserDialogs.Instance.HideLoading();
                }
                catch (PermissionException pEx)
                {
                    // Handle permission exception

                    await DisplayAlert("Notification", " permission exception", "OK");
                    loader.IsVisible = false;
                    label.IsVisible = false;
                    UserDialogs.Instance.HideLoading();
                }
                catch (Exception ex)
                {
                    // Unable to get location

                    ///    await DisplayAlert("Notification", " Unable to get location", "OK");
                    loader.IsVisible = false;
                    label.IsVisible = false;
                    UserDialogs.Instance.HideLoading();
                }


            }
            else
            {
                await DisplayAlert("Notification", "Merci pour votre patience...", "OK");
                loader.IsVisible = false;
                label.IsVisible = false;
                UserDialogs.Instance.HideLoading();
                main_gride.IsVisible = true;
            }



        }

      
    }
}