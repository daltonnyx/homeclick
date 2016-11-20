using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCMS.Lib.Resources;

namespace VCMS.Lib.Models
{
    public abstract class BaseBusinessModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(128)]
        [Display(Name = "Name", ResourceType = typeof(Strings))]
        public string Name { get; set; }

        public string CreateUser { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
