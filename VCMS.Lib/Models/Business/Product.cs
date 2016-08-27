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

        public virtual ICollection<Product_detail> Product_detail { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }

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
}
