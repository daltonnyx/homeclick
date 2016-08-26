using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.Lib.Models
{
    public class BaseViewModel
    {
        public string Id { get; set; }
    }

    public class BaseModel
    {
        public BaseModel()
        {
            var guid = Guid.NewGuid();
            this.Id = guid.ToString();
        }

        [Key]
        public string Id { get; set; }
    }
}
