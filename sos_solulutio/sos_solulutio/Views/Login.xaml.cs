using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Newtonsoft.Json;
using sos_solulutio.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sos_solulutio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private readonly string BaseUrl = "http://shrouded-tundra-85417.herokuapp.com/api/incidents/login-student";
        public Login()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        void UpdateLocal(string img , string postnome, string prov, string comm, string addr, string phone, string emals)
        {
            Preferences.Set("imageUrl", img);
            Preferences.Set("surname", postnome);
            Preferences.Set("province", prov);
            Preferences.Set("commune", comm);
            Preferences.Set("address", addr);
            Preferences.Set("cellPhonumber", phone);
            Preferences.Set("email", emals);
        }
        private async void Register_ClickedAsync(object sender, EventArgs e)
        {

            if (
              String.IsNullOrWhiteSpace(Password.Text) ||
              String.IsNullOrWhiteSpace(Email.Text))
          
             
            {

                //   UserDialogs.Instance.ShowError("Complete les tous field", 2000);
                await DisplayAlert("Erreur désolé", "Veuillez remplir tous les champs de texte requis ", "Ok");

            }
            else
            {
                //  UserDialogs.Instance.ShowLoading("Patiente...");
                //  Users users = await App.Database.GetItemsLoginUserAsync();
                ////       System.Diagnostics.Debug.WriteLine("My Data" + Adresse.Text + "/n" + Nom.Text + "/n" + Cellphone.Text + "/n" + PostNom.Text + "/n" + email.Text + "/n" + "/n" + Zonee.SelectedItem.ToString() + "/n" + "/n" + Province.SelectedItem.ToString() + "/n");

                // await App.Database.SaveRaportsAsync(reports);
                try
                {
                    UserDialogs.Instance.ShowLoading("Loading....", MaskType.Gradient);

                  
                    var client = new HttpClient();
                    var contents = new MultipartContent();
                    var response = await client.PostAsync("http://stormy-wildwood-40195.herokuapp.com/api/incidents/login-student?email=" + Email.Text + "&password=" + Password.Text + "", contents);
                    var respond = await response.Content.ReadAsStringAsync();

                    LoginModel responses = JsonConvert.DeserializeObject<LoginModel>(respond);

                   //  await DisplayAlert("Notification", responses.msg, "Ok");
                    // await Navigation.PushModalAsync(new BuyersHome());



                    if (responses.msg=="OK")
                    {
                        UserDialogs.Instance.HideLoading();

                        Preferences.Set("user_id", responses.user_id.ToString());
                        Preferences.Set("name", responses.name.ToString());
                        Preferences.Set("surname", responses.surname.ToString());
                        Preferences.Set("image", responses.image.ToString());
                        Preferences.Set("cellPhonumber", responses.cellPhonumber.ToString());
                        Preferences.Set("email", responses.email.ToString());
                       Preferences.Set("logo", responses.logo.ToString());
                        Preferences.Set("organization", responses.organization.ToString());
                        /*
                       Preferences.Set("dateofbirth", responses.dateofbirth.ToString());
                       
                       Preferences.Set("province", responses.province.ToString());
                       */
                        ////  UpdateLocal(responses.image.ToString(), responses.surname.ToString(), responses.province.ToString(), responses.commune.ToString(), responses.address.ToString(), responses.cellPhonumber.ToString(), responses.email.ToString());
                        await Navigation.PopModalAsync();
                        await Navigation.PushModalAsync(new Maipage());
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Erro", responses.msg, "OK");
                        //  UserDialogs.Instance.HideLoading();

                    }
                }
                catch (Exception ex)
                {


                    await DisplayAlert("désolé Vous n'ete pas Connecter a l'Internet. Veuille  vérifier votre connection ... Merci pour votre patience ", ".", "OK");
                    
                  
                    UserDialogs.Instance.HideLoading();
                }
                finally
                {

                    // await DisplayAlert("Notification", "No Item found", "OK");
                }

                // UserDialogs.Instance.HideLoading();
            }

            //  await App.Database.SaveUsersAsync(useers);

        }

        private void xingin_Clicked(object sender, EventArgs e)
        {


          //  var scanner = new ZXing.Mobile.MobileBarcodeScanner();

            //var result = await scanner.Scan();

            //if (result != null)
             ///   Console.WriteLine("Scanned Barcode: " + result.Text);
        }

      
    }
}