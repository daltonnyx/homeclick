using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homeclick.Models
{
    public class ProjectViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string city { get; set; }
        public int type { get; set; }
        public int statu { get; set; }
    }
}