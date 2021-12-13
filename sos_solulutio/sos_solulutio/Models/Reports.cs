using System;
using System.Collections.Generic;
using System.Text;

namespace sos_solulutio.Models
{
    public class Reports
    {
        public int ID { get; internal set; }
        public object HopitalLogo { get; internal set; }
        public string category { get; set; }
        public string comments { get; set; }
        public string Name { get; set; }
        public string status { get; set; }
        public string Surname { get; set; }
        public string CellPhonumber { get; set; }
        public Double Lat { get; set; }
        public Double Lng { get; set; }
        public string Email { get; set; }
        public string Province { get; set; }
        public string Commune { get; set; }
        public string Address { get; set; }
    }
}
