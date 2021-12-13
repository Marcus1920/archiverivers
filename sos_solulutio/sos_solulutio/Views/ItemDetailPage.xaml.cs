using System.ComponentModel;
using sos_solulutio.ViewModels;
using Xamarin.Forms;

namespace sos_solulutio.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}