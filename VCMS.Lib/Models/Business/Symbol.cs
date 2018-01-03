using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Reflection;

namespace VCMS.Lib.Models
{
    [Table("Symbol")]
    public class Symbol : BaseModel
    {

        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? ProductTypeId { get; set; }

        [StringLength(128)]
        public string SvgId { get; set; }

        [ForeignKey("ProductTypeId")]
        public virtual Product_Type ProductType { get; set; }

        [ForeignKey("SvgId")]
        public virtual File Svg { get; set; }
    }
}
