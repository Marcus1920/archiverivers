using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Media.Abstractions;

namespace sos_solulutio.Models
{
    public class Profile
    {

        public string user_id { get; set; }
        public string nom { get; set; }
        public string postnom { get; set; }
        public string email { get; set; }
        public string profilePicture { get; set; }
        
        public string numero { get; set; }
        public string dateofbirth { get; set; }
        public string Interest { get; set; }
        public string occupation { get; set; }
        public MediaFile File { get; set; }
        public string organization { get; set; }
        public string logo { get; set; }
        public string organization_id { get; set; }

        
    }


}
