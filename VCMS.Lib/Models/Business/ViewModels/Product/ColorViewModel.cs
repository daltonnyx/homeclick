using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCMS.Lib.Resources;

namespace VCMS.Lib.Models
{
    public class ColorViewModel : BaseBusinessModel
    {
        [StringLength(128)]
        [Display(Name = "Description", ResourceType = typeof(Strings))]
        public string Description { get; set; }

        [Display(Name = "Image", ResourceType = typeof(Strings))]
        public string Image { get; set; }
    }
}
