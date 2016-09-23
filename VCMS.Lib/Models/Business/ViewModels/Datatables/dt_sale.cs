using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.Lib.Models.Business.Datatables
{
    public class dt_sale
    {
        public int id { get; set; }
        public string name { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public int status { get; set; }
    }
}
