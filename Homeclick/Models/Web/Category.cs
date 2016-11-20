namespace Homeclick.Models
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            Category_detail = new HashSet<Category_detail>();
            CategoryParents = new HashSet<Category>();
            CategoryChildren = new HashSet<Category>();
            Projects = new HashSet<Project>();
            ProjectItems = new HashSet<ProjectItem>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        public string description { get; set; }

        public int? Order { get; set; }

        public int? ParentCategoryId { get; set; }

        public int? Category_typeId { get; set; }

        public virtual Category_type Category_type { get; private set; }

        public virtual ICollection<Category_detail> Category_detail { get; private set; }

        public virtual ICollection<Category> CategoryParents { get; private set; }
        public virtual ICollection<Category> CategoryChildren { get; private set; }

        public virtual ICollection<Project> Projects { get; private set; }

        public virtual ICollection<ProjectItem> ProjectItems { get; private set; }

        public virtual ICollection<Product> Products { get; private set; }
    }

    public class CategoryEntityConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryEntityConfiguration()
        {
            this.HasMany(e => e.Category_detail)
                .WithOptional(e => e.Category)
                .WillCascadeOnDelete();

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
    }
}
