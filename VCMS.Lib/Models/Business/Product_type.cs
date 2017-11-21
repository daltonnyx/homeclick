namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product_Types")]
    public partial class Product_Type : BaseModel
    {
        public Product_Type()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Caption { get; set; }

        [StringLength(128)]
        public string ImageId { get; set; }

        public virtual File Image { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
