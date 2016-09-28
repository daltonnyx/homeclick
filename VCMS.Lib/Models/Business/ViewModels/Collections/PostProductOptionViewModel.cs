using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VCMS.Lib.Models.Business
{
    public class PostProductOptionViewModel
    {
        public int id { get; set; }
        [Required]
        public int? postId { get; set; }

        public string postName { get; set; }
        [Required]
        public int? optionId { get; set; }
        [Required]
        public int quantity { get; set; }
    }
}
