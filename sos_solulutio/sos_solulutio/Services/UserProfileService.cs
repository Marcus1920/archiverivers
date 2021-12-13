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
    class UserProfileService
    {
    }
}
public class UserProfileService
{
    private string _url;
    public string BaseUrl = "http://stormy-wildwood-40195.herokuapp.com/api/incidents/getUserProfile?user_id";
    private readonly HttpClient _client = new HttpClient();

    private HttpResponseMessage _response = new HttpResponseMessage();
    public async Task<Profile> GetUser()
    {
        var user_id = Preferences.Get("user_id", "");
    
        _url = "http://stormy-wildwood-40195.herokuapp.com/api/incidents/getUserProfile?user_id=" + user_id;

        var json = await _client.GetStringAsync(_url);

        var user = JsonConvert.DeserializeObject<Profile>(json);

        return user;
    }


}

