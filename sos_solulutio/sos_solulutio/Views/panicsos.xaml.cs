using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace sos_solulutio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class panicsos : ContentPage
    {
        private readonly string BaseUrl = "https://stormy-wildwood-40195.herokuapp.com/api/incidents/create-incident-sos";
        
        private readonly string lat = null;
        private readonly string longt = null;

        CancellationTokenSource cts;
        public panicsos()
        {
            InitializeComponent();
        }
     
        protected override void OnAppearing()
        {
            base.OnAppearing();
          
        }

      
        private async void Btn2_ClickedAsync(object sender, EventArgs e)
        {

            var ans = await DisplayAlert("Question?", "Est vous sure de fair cette demand ?", "Oui", "Non");
            if (ans == true)
            {

                var current = Connectivity.NetworkAccess;



                if (current == NetworkAccess.Internet && current != NetworkAccess.ConstrainedInternet && current != NetworkAccess.None && current != NetworkAccess.Unknown)
                {


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


                            var user_id = Preferences.Get("user_id", "");

                            var values = new FormUrlEncodedContent(new[]
                           {

                                new KeyValuePair<string, string>("user_id", user_id),
                                new KeyValuePair<string, string>("long",  location.Longitude.ToString("N6").Replace(",", ".")),
                                new KeyValuePair<string, string>("lat",  location.Latitude.ToString("N6").Replace(",", ".")),
                                new KeyValuePair<string, string>("comments","En danger"),
                                new KeyValuePair<string, string>("category","En danger"),

                          });

                            var client = new HttpClient();
                            var response = await client.PostAsync(BaseUrl, values);
                            var respond = await response.Content.ReadAsStringAsync();

                            if (respond == "OK")
                            {
                                loader.IsVisible = false;
                                UserDialogs.Instance.HideLoading();
                                label.IsVisible = false;
                                main_gride.IsVisible = true;

                                /// await DisplayAlert("Notification", "Votre Requete a etait recu Merci Patiente la police et en route", "OK");

                                await Navigation.PushAsync(new Thankyou
                                {

                                });


                            }

                            else
                            {

                                await DisplayAlert("Désolée", "Connexion interrompue Veuillez vérifier l'état de votre réseau, actualiser la page ou réessayer plus tard", "OK");
                                //   UserDialogs.Instance.HideLoading();
                                loader.IsVisible = false;
                                label.IsVisible = false;
                                UserDialogs.Instance.HideLoading();

                            }
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
                    // unable  to connnet to  the  network



                    await DisplayAlert("Désolée", "Connexion interrompue Veuillez vérifier l'état de votre réseau, actualiser la page ou réessayer plus tard", "OK");
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

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
    }
}