namespace Homeclick.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Wishlist")]
    public partial class Wishlist
    {
        public int Id { get; set; }

        public int? user_id { get; set; }

        [StringLength(100)]
        public string title { get; set; }

        [StringLength(100)]
        public string created_date { get; set; }

        [StringLength(100)]
        public string description { get; set; }

        public int? UserId { get; set; }

        public int? product_id { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
