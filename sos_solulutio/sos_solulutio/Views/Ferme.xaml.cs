using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sos_solulutio.Models;
using sos_solulutio.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sos_solulutio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ferme : ContentPage
    {
        private ObservableCollection<Reports> _station;
        private readonly ReportsService _service = new ReportsService();
        public Ferme()
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
            List<Reports> poststList = await _service.GetFemer();
            var source = poststList.Where(c => c.Name.Contains(SearchStation.Text));
            listView_ferme.ItemsSource = source;

        }
        void OnItemAdded(object sender, EventArgs e)
        {

        }
        public async Task GetPosts()
        {
            try
            {
                notFound.IsVisible = true;
                notFound.Text = "Loading...";

                var poststList = await _service.GetFemer();

                _station = new ObservableCollection<Reports>(poststList);
                listView_ferme.ItemsSource = _station;

                listView_ferme.IsVisible = _station.Any();
                notFound.IsVisible = !listView_ferme.IsVisible;
            }
            catch (Exception)
            {
                await DisplayAlert("Connection Error!",
                    "Please check your network status, refresh the page (pull down) or try again later.",
                    "OK");
            }
            finally
            {
                notFound.Text = "No posts found.";
            }
        }

        private async void ItemsListView_Refreshing(object sender, EventArgs e)
        {
            await GetPosts();

            listView_ferme.EndRefresh();
        }
        async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem == null)
                return;

            var reports = e.SelectedItem as Reports;
            await Navigation.PushAsync(new ReportDetails(reports));

            listView_ferme.SelectedItem = null;


        }


    }
}