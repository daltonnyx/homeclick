namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Floors")]
    public partial class Floor
    {
        public Floor()
        {
            Rooms = new HashSet<Room>();
        }

        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Description { get; set; }

        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
