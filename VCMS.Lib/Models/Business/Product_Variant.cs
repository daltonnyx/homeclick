namespace VCMS.Lib.Models
{
    using Business;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product_Variants")]
    public partial class Product_Variant : BaseModel
    {
        public Product_Variant()
        {
            Files = new HashSet<File>();
        }

        public new int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Description { get; set; }

        public string Image { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("CreateUserId")]
        public virtual ApplicationUser CreateUser { get; set; }

        [ForeignKey("UpdateUserId")]
        public virtual ApplicationUser UpdateUser { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Product_Variant> Parents { get; set; }

        public virtual ICollection<Product_Variant> Children { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }

    public enum ProductVarianTypes { Material = 77, Color = 76 }
}
