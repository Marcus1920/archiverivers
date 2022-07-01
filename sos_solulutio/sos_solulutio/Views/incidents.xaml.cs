using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.AudioRecorder;
using Plugin.Geolocator.Abstractions;
using Plugin.Media.Abstractions;
using sos_solulutio.ViewModels.logic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sos_solulutio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class incidents : ContentPage
    {
        private readonly string BaseUrl = "http://stormy-wildwood-40195.herokuapp.com/api/incidents/create-incident";
        Position position = null;
        CancellationTokenSource cts;
        //  private readonly AudioRecorderService recorder = new AudioRecorderService();
        private readonly AudioRecorderService recorder = new AudioRecorderService()
        {
            StopRecordingOnSilence = false,
            StopRecordingAfterTimeout = false,
            // TotalAudioTimeout = TimeSpan.FromSeconds(180) //audio will stop recording after 3 minutes
          //  StopRecordingAfterTimeout = true,
            TotalAudioTimeout = TimeSpan.FromSeconds(15),
          //  AudioSilenceTimeout = TimeSpan.FromSeconds(2)
        };

        private readonly AudioPlayer player = new AudioPlayer();
        private readonly AppFunctions _appFunctions = new AppFunctions();
        private MediaFile _mediafile;
        public incidents()
        {
            InitializeComponent();

           
        }

        private async void Envoyer_ClickedAsync(object sender, EventArgs e)
        {
           


            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet && current != NetworkAccess.ConstrainedInternet && current != NetworkAccess.None && current != NetworkAccess.Unknown)
            {
                try
                {
                    loaders.IsVisible = true;
                    labels.IsVisible = true;
                    UserDialogs.Instance.ShowLoading("Contacting server. Please wait", MaskType.Black);
                    var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(3));
                    cts = new CancellationTokenSource();
                    var location = await Geolocation.GetLocationAsync(request, cts.Token);

                    if (location != null)
                    {
                        // Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                        //    await DisplayAlert("Notification", location.Latitude + " permission exception", "OK");

                  

                        var user_id = Preferences.Get("user_id", "");

                        var content = new MultipartFormDataContent();

                        var values = new[]
                                   {


                            new KeyValuePair<string, string>("user_id",  user_id),
                            new KeyValuePair<string,  string>("Province", Province.SelectedItem.ToString()),
                            new KeyValuePair<string, string>("comments",descrition.Text),
                            new KeyValuePair<string, string>("long",  location.Longitude.ToString("N6").Replace(",", ".")),
                            new KeyValuePair<string, string>("lat",   location.Latitude.ToString("N6").Replace(",", ".")),
                            new KeyValuePair<string, string>("Commune",communes.SelectedItem.ToString()),
                            new KeyValuePair<string, string>("type",infractions.SelectedItem.ToString()),
                            new KeyValuePair<string, string>("Address", Adresse.Text)

                            };



                        foreach (var keyValuePair in values)
                        {
                            content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                        }

                        if (_mediafile.Path != null)
                            content.Add(new StreamContent(_mediafile.GetStream()), "\"image\"", $"\"{_mediafile}\"");


                        var client = new HttpClient();
                        var response = await client.PostAsync(BaseUrl, content);
                        var respond = await response.Content.ReadAsStringAsync();
                        //  await DisplayAlert("Erro", respond + "Severver Problem", "OK");

                        if (respond == "OK")
                        {

                            loaders.IsVisible = false;
                            labels.IsVisible = false;
                            UserDialogs.Instance.HideLoading();
                            /// await DisplayAlert("Notification", "Votre Requete a etait recu Merci Patiente la police et en route", "OK");
                            descrition.Text = "";
                            _ = communes.SelectedIndex - 1;
                            _ = Province.SelectedIndex - 1;
                            _ = infractions.SelectedIndex - 1;


                            await Navigation.PushAsync(new Thankyou
                            {
                           
                            });
                        }

                        else
                        {

                            await DisplayAlert("Désolée", "Connexion interrompue Veuillez vérifier l'état de votre réseau, actualiser la page ou réessayer plus tard", "OK");
                            loaders.IsVisible = false;
                            labels.IsVisible = false;
                            UserDialogs.Instance.HideLoading();

                        }
                    }



                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    // Handle not supported on device exception
                    await DisplayAlert("Notification", " not supported on device", "OK");
                    loaders.IsVisible = false;
                    UserDialogs.Instance.HideLoading();
                    labels.IsVisible = false;
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    // Handle not enabled on device exception
                    // Handle not enabled on device exception
                    await DisplayAlert("Notification", "Veuillez active votre location pour une bonne performance !", "OK");
                    loaders.IsVisible = false;
                    labels.IsVisible = false;
                    UserDialogs.Instance.HideLoading();
                }
                catch (PermissionException pEx)
                {
                    // Handle permission exception

                    await DisplayAlert("Notification", " permission exception", "OK");
                    loaders.IsVisible = false;
                    labels.IsVisible = false;
                    UserDialogs.Instance.HideLoading();
                }
                catch (Exception ex)
                {
                    // Unable to get location

                    await DisplayAlert("Désolé", "Veuillez remplir tous les champs de texte requis", "OK");
                    loaders.IsVisible = false;
                    labels.IsVisible = false;
                    UserDialogs.Instance.HideLoading();
                }


            }

            else

            {

                await DisplayAlert("Désolé", "Vous n'ete pas Connecter a l'Internet. Veuillez vérifier l'état de votre réseau... Merci pour votre patience", "OK");
                loaders.IsVisible = false;
                labels.IsVisible = false;
                UserDialogs.Instance.HideLoading();

            }

        }


        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
        async void OnItemAdded(object sender, EventArgs e)
        {


            await TakePict();
        }


        async void OnMapsearch(object sender, EventArgs e)
        {

             await Navigation.PushAsync(new FormsMap
             {
                 //  BindingContext = new TodoItem()
             }); 

        }



        async void Onpickig(object sender, EventArgs e)
        {
            /* await Navigation.PushAsync(new RequestForelse
             {
                 //  BindingContext = new TodoItem()
             }); */

            await OpePict();
        }

        async Task OpePict()
        {


            try
            {
                _mediafile = await _appFunctions.BrowsePicture();

                if (_mediafile == null)

                    await DisplayAlert("Desole", "Impossible d'avoire une photos dans  votre  gallery.", "OK");

                else
                    Image.Source = _mediafile.Path;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }


        async Task TakePict()
        {


            try
            {
                _mediafile = await _appFunctions.TakePicture();

                if (_mediafile == null)
                    await DisplayAlert(null, "Desole vous n'avait pas une photos.", "OK");

                else
                    Image.Source = _mediafile.Path;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }


        async Task openFiles(FileResult audi)
        {
            // canceled
            if (audi == null)
            {
                //PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, audi.FileName);
            using (var stream = await audi.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);
           /// return newFile
           /// Image.Source = newFile;
            //PhotoPath = newFile;
        }


        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                //PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            Image.Source = newFile;
            //PhotoPath = newFile;
        }



       

        private async void playeraudio_Clicked_1(object sender, EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();

            if (status != Xamarin.Essentials.PermissionStatus.Granted)
                return;

            if (recorder.IsRecording)
            {

                await recorder.StopRecording();
                //    playeraudio.IsVisible = true;
                // player.Play(recorder.GetAudioFilePath());
           //     Preferences.Set("selAudio_key", recorder.GetAudioFilePath());
           
                playeraudio.Text = "Record New Audio";
                playeraudio.TextColor = Color.Black;
                try
                {
                    // Use default vibration length
                    Vibration.Vibrate();
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                    await DisplayAlert("Alert", "Vibrate is not supported on this device", "Continue");
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
                    await DisplayAlert("Alert", "Vibrate is not supported on this device", "Continue");
                }

                //  audioPlayer.Play(audioRecorderService.GetAudioFilePath());
             //   
              ///  var pather = recorder.GetAudioFilePath();
                await DisplayAlert("Alert", "Recorded audio is at: " + recorder.GetAudioFilePath(), "Continue");
                player.Play(recorder.GetAudioFilePath());
                //   var newFile = Path.Combine(FileSystem.CacheDirectory, recorder.GetAudioFilePath());
                /// using (var stream = await OpenReadAsync())
                //     using (var newStream = File.OpenWrite(newFile))
                //    await stream.CopyToAsync(newStream);
                //   await openFiles(FileResult photos);

            }
            else
            {
                try
                {
                    // Use default vibration length
                    Vibration.Vibrate();
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                    // await DisplayAlert("Alert", "Vibrate is not supported on this device", "Continue");
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
                    // await DisplayAlert("Alert", "Vibrate is not supported on this device", "Continue");
                }

                //   await audioRecorderService.StartRecording();
                playeraudio.Text = "Recording... Press to Finish";
                playeraudio.TextColor = Color.Red;
                  await recorder.StartRecording();
                //   await DisplayAlert("Alert", "Recorded audio is at: " + recorder.GetAudioFilePath(), "OK");

            }

        }
    }

}