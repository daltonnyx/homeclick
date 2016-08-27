namespace Homeclick.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Room")]
    public partial class Room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Room()
        {
            Canvas = new HashSet<Canva>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [StringLength(100)]
        public string description { get; set; }

        public int? floor_id { get; set; }

        public string coordinates { get; set; }

        public string canvas_data { get; set; }

        public int? FloorId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Canva> Canvas { get; set; }

        public virtual Floor Floor { get; set; }
    }
}
