using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homeclick.Models
{
    public class CategoriesLink
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
    }
}