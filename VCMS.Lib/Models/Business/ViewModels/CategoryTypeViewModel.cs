using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.Lib.Models.Business
{
    public class CategoryTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Desciption { get; set; }

        public int Count { get; set; }
    }
}
