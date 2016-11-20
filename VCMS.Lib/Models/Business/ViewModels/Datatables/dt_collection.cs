using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.Lib.Models.Datatables
{
    public class dt_collection
    {
        public int id { get; set; }
        public string name { get; set; }
        public IEnumerable<string> categories { get; set; }
    }
}
