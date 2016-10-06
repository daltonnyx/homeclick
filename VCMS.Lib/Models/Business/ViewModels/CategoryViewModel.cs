using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.Lib.Models
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Desciption { get; set; }

        public int Order { get; set; }

        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        public int? TypeId { get; set; }

        public string TypeName { get; set; }
    }
}
