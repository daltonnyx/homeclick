using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.Lib.Models.Business
{
    public abstract class BaseBusinessModel
    {
        [Key]
        public int Id { get; set; }

        public string CreateUser { get; set; }
        public DateTime? CreateTime { get; set; }

        public string UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
