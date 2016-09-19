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

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class BaseModel
    {
        public BaseModel()
        {
            var guid = Guid.NewGuid();
            this.Id = guid.ToString();
        }

        [Key]
        [StringLength(128)]
        public string Id { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(128)]
        public string CreateUserId { get; set; }

        [ForeignKey("CreateUserId")]
        public virtual ApplicationUser CreateUser { get; set; }

        //[StringLength(128)]
        //public string UpdateUserId { get; set; }
        //public DateTime? UpdateTime { get; set; }
        //[ForeignKey("UpdateUserId")]
        //public virtual ApplicationUser UpdateUser { get; set; }
    }
}
