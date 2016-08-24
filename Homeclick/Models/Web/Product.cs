namespace Homeclick.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Product_detail = new HashSet<Product_detail>();
            Wishlists = new HashSet<Wishlist>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        public string content { get; set; }

        public DateTime? created_date { get; set; }

        public DateTime? updated_date { get; set; }

        public short? status { get; set; }

        public int? author_id { get; set; }

        public int? UserId { get; set; }

        public bool? featured { get; set; }

        [StringLength(100)]
        public string image { get; set; }

        public string excerpt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_detail> Product_detail { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}
