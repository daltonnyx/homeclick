namespace VCMS.Lib.Models.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product_Variants
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product_Variants()
        {
            Product_Variants1 = new HashSet<Product_Variants>();
        }

        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Description { get; set; }

        [StringLength(128)]
        public string ImageId { get; set; }

        public int? ParentId { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(128)]
        public string CreateUserId { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(128)]
        public string UpdateUserId { get; set; }

        public DateTime? UpdateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Variants> Product_Variants1 { get; set; }

        public virtual Product_Variants Product_Variants2 { get; set; }
    }
}
