namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project_Details
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string name { get; set; }

        public string value { get; set; }

        [StringLength(128)]
        public string caption { get; set; }

        public int? ProjectId { get; set; }

        [StringLength(128)]
        public string type { get; set; }

        public int? typeEnum { get; set; }

        public virtual Project Project { get; set; }
    }
}
