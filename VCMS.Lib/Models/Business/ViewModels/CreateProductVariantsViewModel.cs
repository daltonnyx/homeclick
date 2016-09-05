using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VCMS.Lib.Models
{
    public class CreateProductVariantsViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase File { get; set; }
        public string[] VariantParents { get; set; }
    }
}
