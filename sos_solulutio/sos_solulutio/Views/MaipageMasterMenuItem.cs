using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sos_solulutio.Views
{
    public class MaipageMasterMenuItem
    {
        public MaipageMasterMenuItem()
        {
            TargetType = typeof(MaipageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
        public String IconSource { get; set; }
    }
}