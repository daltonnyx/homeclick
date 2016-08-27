namespace Homeclick.Models
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

        public int? room_id { get; set; }

        public int? user_id { get; set; }

        public string json_data { get; set; }

        public int? cart_id { get; set; }

        public int? RoomId { get; set; }

        public int? CartId { get; set; }

        public int? UserId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UpdatedDate { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual Room Room { get; set; }

        public virtual User User { get; set; }
    }
}
