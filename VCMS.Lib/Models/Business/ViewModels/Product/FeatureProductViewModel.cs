using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCMS.Lib.Resources;

namespace VCMS.Lib.Models.Business
{
    public class FeatureProductViewModel
    {
        [Required]
        [Display(Name = "Product", ResourceType = typeof(Strings))]
        public int ProductId { get; set; }
    }
}
