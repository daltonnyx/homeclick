using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homeclick.Models
{
    public class CollectionViewModel
    {
        public int id { get; set; }
        public int categoryId { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string link { get; set; }
    }
}