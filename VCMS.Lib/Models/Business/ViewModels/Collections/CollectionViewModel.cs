using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VCMS.Lib.Models
{
    public class CollectionViewModel : PostViewModel
    {
        [Required]
        public Dictionary<string,int> products { get; set; }   

        public Dictionary<string, string> imageFiles { get; set; }

        public int? discountAmount { get; set; }

        public List<int> danhmuc { get; set; }
    }
}
