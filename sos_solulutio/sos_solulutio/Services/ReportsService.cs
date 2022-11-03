using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sos_solulutio.Models;
using Xamarin.Essentials;

namespace sos_solulutio.Services
{
    public class ReportsService
    {
        public string BaseUrl = "http://shrouded-tundra-85417.herokuapp.com/api/incidents/";
        private string _url;
        private readonly HttpClient _client = new HttpClient();
        private HttpResponseMessage _response = new HttpResponseMessage();


        public async Task<List<Reports>> GetAllPosts()
        {
            _url = BaseUrl;

            var user_id = Preferences.Get("user_id", "");
            var json = await _client.GetStringAsync(_url + "get-incidentbyid?Id=" + user_id);
            var stationtList = JsonConvert.DeserializeObject<List<Reports>>(json);

            foreach (var post in stationtList)
            {
                if (post.HopitalLogo == null)
                    post.HopitalLogo = "hopitalogo.png";
            }

            return stationtList;
        }


        // get accepted cases

        public async Task<List<Reports>> GetAccepted()
        {
            _url = BaseUrl;

            var user_id = Preferences.Get("user_id", "");
            var json = await _client.GetStringAsync(_url + "get-incidentbyid-accpter?Id=" +user_id);
            var stationtList = JsonConvert.DeserializeObject<List<Reports>>(json);

            foreach (var post in stationtList)
            {
                if (post.HopitalLogo == null)
                    post.HopitalLogo = "hopitalogo.png";
            }

            return stationtList;
        }


        // get Close cases

        public async Task<List<Reports>> GetFemer()
        {
            _url = BaseUrl;

            var user_id = Preferences.Get("user_id", "");
            var json = await _client.GetStringAsync(_url + "get-incidentbyid-ferme?Id=" + user_id);
            var stationtList = JsonConvert.DeserializeObject<List<Reports>>(json);

            foreach (var post in stationtList)
            {
                if (post.HopitalLogo == null)
                    post.HopitalLogo = "hopitalogo.png";
            }

            return stationtList;
        }


        // get Close cases

        public async Task<List<Reports>> GetIgnore()
        {
            _url = BaseUrl;

            var user_id = Preferences.Get("user_id", "");
            var json = await _client.GetStringAsync(_url + "get-incidentbyid-ignore?Id=" + user_id);
            var stationtList = JsonConvert.DeserializeObject<List<Reports>>(json);

            foreach (var post in stationtList)
            {
                if (post.HopitalLogo == null)
                    post.HopitalLogo = "hopitalogo.png";
            }

            return stationtList;
        }
    }
}