namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            Category_detail = new HashSet<Category_detail>();
            CategoryParents = new HashSet<Category>();
            CategoryChildren = new HashSet<Category>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        public string description { get; set; }

        public int? Order { get; set; }

        public int? Category_typeId { get; set; }

        public virtual Category_type Category_type { get; private set; }

        public virtual ICollection<Category_detail> Category_detail { get; private set; }

        public virtual ICollection<Category> CategoryParents { get; private set; }

        public virtual ICollection<Category> CategoryChildren { get; private set; }

        public virtual ICollection<Product> Products { get; private set; }

        public virtual ICollection<File> Files { get; set; }
    }
    /*
    public class CategoryEntityConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryEntityConfiguration()
        {
            this.HasMany(e => e.Category_detail)
                .WithOptional(e => e.Category)
                .WillCascadeOnDelete();

            this.HasMany(e => e.Files)
                .WithOptional(e => e.FileType)
                .HasForeignKey(e => e.FileTypeId);

            //Parents
            this.HasMany(e => e.CategoryParents)
                .WithMany(e => e.CategoryChildren)
                .Map(cs =>
                {
                    cs.MapLeftKey("ChildId");
                    cs.MapRightKey("ParentId");
                    cs.ToTable("Category_Category_Link");
                });
        }
    }        */
}
