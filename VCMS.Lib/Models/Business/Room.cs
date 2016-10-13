namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Room")]
    public partial class Room
    {
        public Room()
        {
            Canvas = new HashSet<Canva>();
        }

        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Coordinates { get; set; }

        public string CanvasData { get; set; }

        public int? FloorId { get; set; }

        public virtual ICollection<Canva> Canvas { get; set; }

        public virtual Floor Floor { get; set; }
    }
}
