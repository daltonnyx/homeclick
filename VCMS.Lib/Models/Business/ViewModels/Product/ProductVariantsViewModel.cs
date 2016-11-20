using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VCMS.Lib.Resources;

namespace VCMS.Lib.Models
{
    public class ProductVariantsViewModel
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int? VariantParent { get; set; }

        [Display(Name = "Category", ResourceType =typeof(Strings))]
        public int? CategoryId { get; set; }

        [Display(Name = "Categories", ResourceType = typeof(Strings))]
        public int[] CategoryIds { get; set; }
    }
}
