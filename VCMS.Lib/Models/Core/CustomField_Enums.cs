namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomField_Enums
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public int? CustomFieldId { get; set; }

        [StringLength(128)]
        public string CreateUserId { get; set; }

        public DateTime? CreateTime { get; set; }

        public virtual CustomField CustomField { get; set; }
    }
}
