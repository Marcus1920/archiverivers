using System;
using OneSignalSDK.Xamarin;
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

            //DependencyService.Register<MockDataStore>()FormsMap(); //;


            MainPage = new Maipage();

            OneSignal.Default.Initialize("0d97eca9-35e4-4ba6-93b5-7dd5cef14ee4");
            OneSignal.Default.PromptForPushNotificationsWithUserResponse();
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
