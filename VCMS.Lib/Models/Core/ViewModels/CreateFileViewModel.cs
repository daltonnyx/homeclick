using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VCMS.Lib.Models
{
    public class CreateFileViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase File { get; set; }
    }
}
