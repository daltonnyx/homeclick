namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public Product()
        {
            Product_detail = new HashSet<Product_detail>();
            Categories = new HashSet<Category>();
            Product_Variants = new HashSet<Product_Variant>();
            Files = new HashSet<File>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        public string content { get; set; }

        [Column("status")]
        public bool Status { get; set; }

        public bool? featured { get; set; }

        [StringLength(100)]
        public string image { get; set; }

        public string excerpt { get; set; }

        public string CreateUserId { get; set; }

        public DateTime CreateTime { get; set; }

        public virtual ICollection<Product_detail> Product_detail { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Product_Variant> Product_Variants { get; set; }

        public virtual ICollection<File> Files { get; set; }

        [ForeignKey("CreateUserId")]
        public virtual ApplicationUser CreateUser { get; set; }
    }
    /*5205555.
     * 6
    public class ProductEntityConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductEntityConfiguration()
        {
            this.HasMany(e => e.Product_detail)
                .WithOptional(e => e.Product)
                .WillCascadeOnDelete();

            this.HasMany(e => e.Categories)
                .WithMany(e => e.Products)
                .Map(cs =>
                {
                    cs.MapLeftKey("ChildId");
                    cs.MapRightKey("ParentId");
                    cs.ToTable("Category_Product_Link");
                });
        }
    }
    */
}
