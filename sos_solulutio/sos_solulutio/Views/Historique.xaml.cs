using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using sos_solulutio.Models;
using sos_solulutio.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sos_solulutio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Historique : ContentPage
    {
        private ObservableCollection<Reports> _station;
        private readonly ReportsService _service = new ReportsService();


        public Historique()
        {
           
            BindingContext = this;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await GetPosts();

        }

        private async void CountriesSearchBar_TextChangedAsync(object sender, TextChangedEventArgs e)
        {
            //  var countriesSearched = viewModel.Items.Where(c => c.Text.Contains(SearchStation.Text));
            //ItemsListView.ItemsSource = countriesSearched;
            List<Reports> poststList = await _service.GetAllPosts();
            var source = poststList.Where(c => c.Name.Contains(SearchStation.Text));
            if (source == null)

                notFound.Text = "Aucun  résulta trouver";

            else
                listView.ItemsSource = source;

        }
        void OnItemAdded(object sender, EventArgs e)
        {

        }
        public async Task GetPosts()
        {


            try
            {
                SearchStation.IsVisible = true;
                notFound.IsVisible = true;
                notFound.Text = "Loading...";
                var poststList = await _service.GetAllPosts();


                _station = new ObservableCollection<Reports>(poststList);
                listView.ItemsSource = _station;

                listView.IsVisible = _station.Any();
                notFound.IsVisible = !listView.IsVisible;
            }
            catch (Exception)
            {
                /*await DisplayAlert("Connection Error!",
                    "Please check your network status, refresh the page (pull down) or try again later.",
                    "OK");*/
            }
            finally
            {
                notFound.Text = "";
                SearchStation.IsVisible = false;
                UserDialogs.Instance.Toast("Aucun  résulta trouver vérifier votre connections", TimeSpan.FromSeconds(15));

            }


            /*
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet && current != NetworkAccess.ConstrainedInternet && current != NetworkAccess.None && current != NetworkAccess.Unknown)
            {



                try
            {

                   UserDialogs.Instance.ShowLoading("Patiente nous actualisons vos données.Merci pour votre patience …", MaskType.Black);

                    SearchStation.IsVisible=false;
                notFound.IsVisible = true;
              // notFound.Text = "Patiente nous actualisons vos données";

               var poststList = await _service.GetAllPosts();
                  

                _station = new ObservableCollection<Reports>(poststList);
                listView.ItemsSource = _station;

                listView.IsVisible = _station.Any();
                notFound.IsVisible = !listView.IsVisible;
                UserDialogs.Instance.HideLoading();

                }
                catch (Exception)
            {
                await DisplayAlert("Désolé !",
                    "Veuillez vérifier votre connexion donne  et  réessayer plus tard... ",
                    "OK");
                    UserDialogs.Instance.HideLoading();
                }
            finally
            {
                
               // SearchStation.IsVisible = false;
              //  UserDialogs.Instance.Toast("Aucun  résultat trouver vérifier votre connections", TimeSpan.FromSeconds(7));
                    UserDialogs.Instance.HideLoading();
                    var poststLists = new List<Reports>()
            {
                new Reports { Name ="Goro", Surname = "Mbayo", CellPhonumber="+243992203123",Email="marcus@gmail.com"
                ,Address="14 wangata comune de manika",category="Fraude",comments="Maison cambriole perte de téléphone Samsung et un post téléviseur plus une sommées 2000Fc " ,Commune="manika", Province="Lualaba"
                ,status="Attente",ville="Kolwezi", HopitalLogo="logo.png",ID=1,Lat=1111245,Lng=11111},

            };
                  
                    _station = new ObservableCollection<Reports>(poststLists);
                    listView.ItemsSource = _station;

                    listView.IsVisible = _station.Any();
                    notFound.IsVisible = !listView.IsVisible;

                    UserDialogs.Instance.HideLoading();

                }

            }
            else

                //SearchStation.IsVisible = false;
                // UserDialogs.Instance.Toast("Aucun  résultat trouver vérifier votre connections", TimeSpan.FromSeconds(7));
                Plugin.Badge.Abstractions.TabBadge.SetBadgeText(this, "1");
                Plugin.Badge.Abstractions.TabBadge.SetBadgeColor(this, Color.DarkOliveGreen);
            
            UserDialogs.Instance.HideLoading();

            var poststListss = new List<Reports>()
            {
                new Reports { Name ="Goro", Surname = "Mbayo", CellPhonumber="+243992203123",Email="marcus@gmail.com"
                ,Address="14 wangata comune de manika",category="Vols",comments="Maison cambriole perte de téléphone Samsung et un post téléviseur plus une sommées 2000Fc " ,Commune="manika", Province="Lualaba"
                ,status="Attente",ville="Kolwezi", HopitalLogo="logo.png",ID=1,Lat=1111245,Lng=11111},

            };

       

            _station = new ObservableCollection<Reports>(poststListss);
            
            listView.ItemsSource = _station;

            listView.IsVisible = _station.Any();
            notFound.IsVisible = !listView.IsVisible;
            UserDialogs.Instance.HideLoading();
            */
        }

        private async void ItemsListView_Refreshing(object sender, EventArgs e)
        {
            await GetPosts();

            listView.EndRefresh();
        }
        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem == null)
                return;

            var reports = e.SelectedItem as Reports;
            await Navigation.PushAsync(new ReportDetails(reports));

            listView.SelectedItem = null;

          
        }


    }
}