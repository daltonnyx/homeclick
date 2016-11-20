namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Departments")]
    public partial class Department
    {
        public Department()
        {
            Floors = new HashSet<Floor>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public virtual ICollection<Floor> Floors { get; set; }
    }
}
