namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("User_Details")]
    public partial class User_Detail
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Value { get; set; }

        [StringLength(128)]
        public string Caption { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(128)]
        public string Type { get; set; }

        public string TypeEnum { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
