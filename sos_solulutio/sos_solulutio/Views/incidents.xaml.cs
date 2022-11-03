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
        private readonly string BaseUrl = "http://shrouded-tundra-85417.herokuapp.com/api/incidents/create-incident";
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


                // Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                //    await DisplayAlert("Notification", location.Latitude + " permission exception", "OK");
             //   await DisplayAlert("Désolé", "Veuillez remplir tous les champs de texte requis ", "Ok");
    
                  
    
    var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet && current != NetworkAccess.ConstrainedInternet && current != NetworkAccess.None && current != NetworkAccess.Unknown)
            {
                try
                {
                    UserDialogs.Instance.ShowLoading("Veillez patiente votre  requête et entrain d’être traite   Merci pour votre patience …", MaskType.Black);

                    if (
                    String.IsNullOrWhiteSpace(descrition.Text) ||
                    String.IsNullOrWhiteSpace(Province.SelectedItem.ToString()) ||
                     String.IsNullOrWhiteSpace(Categories.SelectedItem.ToString()) ||
                      String.IsNullOrWhiteSpace(Adresse.Text) 

                    )



                    {

                        //   UserDialogs.Instance.ShowError("Complete les tous field", 2000);
                        await DisplayAlert("désolé", "Veuillez remplir tous les champs de texte requis ", "Ok");
                        UserDialogs.Instance.HideLoading();
                    }

                    var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(3));
                    cts = new CancellationTokenSource();
                    var location = await Geolocation.GetLocationAsync(request, cts.Token);


                    if (location != null)
                    {

                         

                            var user_id = Preferences.Get("user_id", "");

                        var content = new MultipartFormDataContent();

                        var values = new[]
                                   {


                            new KeyValuePair<string, string>("user_id",  user_id),
                            new KeyValuePair<string,  string>("Province", Province.SelectedItem.ToString()),
                            new KeyValuePair<string,  string>("ville", ville.SelectedItem.ToString()),
                            new KeyValuePair<string, string>("comments",descrition.Text),
                            new KeyValuePair<string, string>("long",  location.Longitude.ToString("N6").Replace(",", ".")),
                            new KeyValuePair<string, string>("lat",   location.Latitude.ToString("N6").Replace(",", ".")),
                            new KeyValuePair<string, string>("Commune",ville.SelectedItem.ToString()),
                            new KeyValuePair<string, string>("type",Categories.SelectedItem.ToString()),
                            new KeyValuePair<string, string>("subcategory",Subcategories.SelectedItem.ToString()),
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
                        // await DisplayAlert("Désolée", respond + "Connexion interrompue Veuillez vérifier votre Connexion donne", "OK");

                        if (respond == "OK")
                        {

                            loaders.IsVisible = false;
                            labels.IsVisible = false;
                            UserDialogs.Instance.HideLoading();
                            /// await DisplayAlert("Notification", "Votre Requete a etait recu Merci Patiente la police et en route", "OK");
                              descrition.Text = "";
                          //  _ = communes.SelectedIndex - 1;
                            _ = Province.SelectedIndex - 1;
                            _ = Categories.SelectedIndex - 1;
                            _ = ville.SelectedIndex - 1;
                            Image.Source="image_not_found.jpg";

                            await Navigation.PushAsync(new Thankyou
                            {
                           
                            });
                        }
                    
                
                    else
                        {

                            await DisplayAlert("Désolée", "Connexion interrompue Veuillez vérifier votre Connexion donne ", "OK");
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

                await DisplayAlert("Désolé", "Vous n'ete pas Connecter a l'Internet. Veuillez vérifier votre Connexion donne ... Merci pour votre patience", "OK");
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

                    await DisplayAlert("Desole", "Veuillez   recommencé  et choisies  une photos dans votre gallérie.", "OK");

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
                    await DisplayAlert("Desole", "Veuillez recommencé  et capture une photos  Merci pour votre patience...", "OK");

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
           
             //   playeraudio.Text = "Record New Audio";
              //  playeraudio.TextColor = Color.Black;
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
                //playeraudio.Text = "Recording... Press to Finish";
                //playeraudio.TextColor = Color.Red;
                  await recorder.StartRecording();
               
                //   await DisplayAlert("Alert", "Recorded audio is at: " + recorder.GetAudioFilePath(), "OK");

            }

        }

        private async void Province_SelectedIndexChanged(object sender, EventArgs e)
        {
            string subject = Province.SelectedItem.ToString();
            try
            {
                // Use default vibration length
                switch (subject)
                {

                    case "Lualaba":

                        ville.Items.Clear();
                        //  communes.Items.Clear();
                        ville.Items.Add("Kolwezi");
                        ville.Items.Add("Kasaji");
                        break;

                    case "Haut-Katanga":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Lubumbashi");
                        ville.Items.Add("Likasi");
                        break;

                     case "Bas-Uele":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Buta");
                        break;

                    case "Kongo central":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Matadi");
                        ville.Items.Add("Mbanza-Ngungu");
                        
                        break;

                    case "Bandundu":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Bandundu");
                        ville.Items.Add("Bolobo");

                        break;

                    case "Maniema":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Kindu");
                        ville.Items.Add("Kalima");

                        break;

                    case "Nord-Kivu":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Beni");
                        ville.Items.Add("Butembo");
                        ville.Items.Add("Goma");
                        break;

                         case "Tanganyika":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Kalemie");
                        ville.Items.Add("Kabalo");
                        
                        break;

                    case "Sud-Kivu":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Bukavu");
                        ville.Items.Add("Uvira");

                        break;

                    case "Kasaï central":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Kananga");
                        break;

                    case "Kasaï":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Tshikapa");
                        break;

                    case "Kasaï oriental":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Mbuji-Mayi");
                        break;

                    case "Nord-Ubangi":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Gbadolite");
                        break;

                    case "Tshopo":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Kisangani");
                        break;

                    case "Kinshasa":

                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Kinshasa");
                        break;

                    case "Haut-Uele":
                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Isiro");
                        break;

                    case "Haut-Lomami":
                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Kamina");
                        ville.Items.Add("Kaniama");
                        
                        break;

                    case "Équateur":
                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Mbandaka");
                        break;

                    case "Mai-Ndombe":
                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Inongo");
                        break;

                    case "Mongala":
                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Lisala");
                        break;

                    case "Tshuapa":
                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Boende");
                        break;

                    case "Kwango":
                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Kenge");
                        break;

                    case "Sud-Ubangi":
                        ville.Items.Clear();
                        // communes.Items.Clear();
                        ville.Items.Add("Gemena");
                        break;
                         
                    default:
                        Console.WriteLine("Subject is C#");
                        break;
                }

            }
            catch (Exception ex)
            {
                // Feature not supported on device
                 await DisplayAlert("Alert", "Exeption "+ex, "Continue");
            }




        }

        private async void infractions_SelectedIndexChanged(object sender, EventArgs e)
        {

            string subject = Categories.SelectedItem.ToString();
            try
            {
                // Use default vibration length
                switch (subject)
                {

                    case "Délits de vol":

                        Subcategories.Items.Clear();
                        //  communes.Items.Clear();
                        Subcategories.Items.Add("cambriolage");
                        Subcategories.Items.Add("vol à main armée");
                        Subcategories.Items.Add("vol avec violence");
                        Subcategories.Items.Add("vol à l'étalage");
                        Subcategories.Items.Add("assurance commerciale contre le vol");
                        break;

                    case "Crimes liés à la drogue":

                        Subcategories.Items.Clear();
                        //  communes.Items.Clear();
                        Subcategories.Items.Add("possession de drogue");
                        Subcategories.Items.Add("fabrication de drogue");
                        Subcategories.Items.Add("Trafic de drogue");

                        break;


                    case "Infractions routières":

                        Subcategories.Items.Clear();
                        //  communes.Items.Clear();
                        Subcategories.Items.Add("permis révoqué");
                        Subcategories.Items.Add("conduire sans permis");
                        Subcategories.Items.Add("accident ");
                        Subcategories.Items.Add("accidents avec délit de fuite");
                        Subcategories.Items.Add("conduite dangereuse");
                        Subcategories.Items.Add("agression véhiculaire");
                        break;

                    case "Fraude":

                        Subcategories.Items.Clear();
                        //  communes.Items.Clear();
                        Subcategories.Items.Add("chantage ");
                        Subcategories.Items.Add("blanchiment d'argent");
                        Subcategories.Items.Add("détournement de fonds");
                        Subcategories.Items.Add("évasion fiscale");
                        Subcategories.Items.Add("cybercriminalité");
                        break;


                    case "Agression sexuelle":

                        Subcategories.Items.Clear();
                        //  communes.Items.Clear();
                        Subcategories.Items.Add("menaces ");
                        Subcategories.Items.Add("embrasser");
                        Subcategories.Items.Add("pénétration");
                        Subcategories.Items.Add("viol avec forcé");

                        break;


                    


                    case "Mort d'homme":

                        Subcategories.Items.Clear();
                        //  communes.Items.Clear();
                        Subcategories.Items.Add("meurtre au premier degré");
                        Subcategories.Items.Add("meurtre au deuxième degré");
                        Subcategories.Items.Add("homicide involontaire");
                        Subcategories.Items.Add("homicide volontaire");

                        break;

                  


                    default:
                        Console.WriteLine("Subject is C#");
                        break;
                }

            }
            catch (Exception ex)
            {
                // Feature not supported on device
                await DisplayAlert("Alert", "Exeption " + ex, "Continue");
            }
        }
    }



}