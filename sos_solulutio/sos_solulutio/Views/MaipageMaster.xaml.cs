using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sos_solulutio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MaipageMaster : ContentPage
    {
        public ListView ListView;

        public MaipageMaster()
        {
            InitializeComponent();

            BindingContext = new MaipageMasterViewModel();
            ListView = MenuItemsListView;
        }
        protected override void OnAppearing()
        {
            infoMame.Text="Version"+ AppInfo.Version.ToString();
            UserName.Text ="";
            UserName.Text = Preferences.Get("organization", "")+" Pty Ltd";
            //ProfilePic.Source = Preferences.Get("logo", "");
            ProfilePic.Source = "logood.png";
            base.OnAppearing();
        }
        class MaipageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MaipageMasterMenuItem> MenuItems { get; set; }

            public MaipageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MaipageMasterMenuItem>(new[]
                {
                    
                    new  MaipageMasterMenuItem { Id = 0, Title = "Alert SOS", IconSource="ic_add_alert_black_36dp.png", TargetType = typeof(panicsos) },
                    new  MaipageMasterMenuItem { Id = 1, Title = "Appel SOS", IconSource="ic_call_black_24dp.png", TargetType = typeof(Callsos) },
                    new MaipageMasterMenuItem { Id = 4, Title = "Profile", IconSource="ic_nature_people_black_36dp.png", TargetType = typeof(UserProfile) },

                   new MaipageMasterMenuItem { Id = 2, Title = "Reporter un Incident" , IconSource="ic_create_new_folder_black_36dp.png", TargetType = typeof(incidents) },
                    new MaipageMasterMenuItem { Id = 3, Title = "Historique des Reportages" , IconSource="ic_library_books_black_48dp.png", TargetType = typeof(HistoryTabs) },

                   // new MaipageMasterMenuItem { Id = 3, Title = "Attendancy" , IconSource="ic_person_black_36dp.png", TargetType = typeof(Attendancy) },
              //      new MaipageMasterMenuItem { Id = 3, Title = "Rotation des Shift" , IconSource="ic_history_black_36dp.png", TargetType = typeof(RoasterTabs) },


 

                      new MaipageMasterMenuItem { Id = 4, Title = "Aboute",  IconSource="ic_hearing_black_48dp.png", TargetType = typeof(Apropos) },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}