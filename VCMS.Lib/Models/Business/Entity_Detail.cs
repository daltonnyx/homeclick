namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Entity_Detail : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Caption { get; set; }

        public string Value { get; set; }

        [StringLength(128)]
        public string FileId { get; set; }

        public int? EnumId { get; set; }

        [StringLength(128)]
        public string Type { get; set; }

        public virtual File File { get; set; }

        public virtual CustomField_Enum Enum { get; set; }
    }
}
