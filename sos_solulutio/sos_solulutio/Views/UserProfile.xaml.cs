using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Media.Abstractions;
using sos_solulutio.ViewModels.logic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sos_solulutio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfile : ContentPage
    {
        private readonly UserProfileService _service = new UserProfileService();
        private string _url;
        private readonly HttpClient _client = new HttpClient();
        private HttpResponseMessage _response = new HttpResponseMessage();

        private readonly AppFunctions _appFunctions = new AppFunctions();
        private MediaFile _mediafile;
        public UserProfile()
        {
            BindingContext = this;
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            
            nameProfile.Text = Preferences.Get("name", "") + "  " + Preferences.Get("surname", "");
           //BannerImage.Source = Preferences.Get("logo", "");
           

            //UserProfiles.Source = Preferences.Get("image", "");
//UserProfiles.Source = "logos.png";
            // dateofbirth.Text = Preferences.Get("dateofbirth", "");
            email.Text = Preferences.Get("email", "");
            cellPhonumber.Text = Preferences.Get("cellPhonumber", "");
            surname.Text = Preferences.Get("surname", "");
            name.Text = Preferences.Get("name", "");

            base.OnAppearing();
        }




        public async Task GetUserProfile()
        {
            try
            {
                var profile = await _service.GetUser();

                BindingContext = profile;
            }
            catch (Exception)
            {
                await DisplayAlert("Error! ",
                    "Connection interrupted. Please check your network status, refresh the page or try again later.",
                    "OK");

                await Navigation.PopAsync();
            }
        }

        /*
                private async void TakePicture_Clicked(object sender, EventArgs e)
                {

                    _mediafile = await _appFunctions.TakePicture();

                    if (_mediafile == null)
                        await DisplayAlert(null, "Could not take picture.", "OK");

                    else
                   //     Image.Source = _mediafile.Path;
                }


                private async void BrowsePicture_Clicked__(object sender, EventArgs e)
                {
                    _mediafile = await _appFunctions.BrowsePicture();

                    if (_mediafile == null)
                        await DisplayAlert(null, "Could not get picture.", "OK");

                    else
                       // Image.Source = _mediafile.Path;
                }
        */



        private async void Save_OnClicked(object sender, EventArgs e)
        {


            if (_mediafile == null)
            {
                UserDialogs.Instance.Alert("OK", "Please complete all the fields and set a product picture.", "Ok");
            }
            UserDialogs.Instance.ShowLoading("Loading....", MaskType.Gradient);

            _url = "http://shrouded-tundra-85417.herokuapp.com//api/v1/updateProfile";
            var user_id = Preferences.Get("user_id", "");
            var content = new MultipartFormDataContent();

            var values = new[]
            {
                new KeyValuePair<string, string>("user_id",user_id),
          //      new KeyValuePair<string, string>("email", email.Text),
             //   new KeyValuePair<string, string>("cellphone", phone.Text),

            };

            foreach (var keyValuePair in values)
            {
                content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
            }

            if (_mediafile != null)
                content.Add(new StreamContent(_mediafile.GetStream()), "\"profilePicture\"", "\"profilePicture\"");

            _response = await _client.PostAsync(_url, content);

            if (_response.IsSuccessStatusCode)

            {
                UserDialogs.Instance.HideLoading();
                try
                {
                    var profile = await _service.GetUser();

                    BindingContext = profile;
                    await Navigation.PopAsync();
                }
                catch (Exception)
                {
                    await DisplayAlert("Error!",
                        "Connection interrupted. Please check your network status, refresh the page or try again later.",
                        "OK");

                    await Navigation.PopAsync();
                }

            }
            else
            {
               // UserDialogs.Instance.("erro ....");

            }





        }

    }
}