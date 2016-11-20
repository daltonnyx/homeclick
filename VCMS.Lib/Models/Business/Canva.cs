namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Canva")]
    public partial class Canva
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public string JsonData { get; set; }

        public int? RoomId { get; set; }

        public int? CartId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UpdatedDate { get; set; }

        //public virtual Cart Cart { get; set; }

        public virtual Room Room { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
