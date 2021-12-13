using System;
using sos_solulutio.Services;
using sos_solulutio.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sos_solulutio
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();

            MainPage = new Maipage();
            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
