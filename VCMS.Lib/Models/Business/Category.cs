namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            Category_details = new HashSet<Category_detail>();
            CategoryParents = new HashSet<Category>();
            CategoryChildren = new HashSet<Category>();
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; }

        public string description { get; set; }

        public int? Order { get; set; }

        public int? Category_typeId { get; set; }

        public virtual Category_Type Category_type { get; private set; }

        public virtual ICollection<Category_detail> Category_details { get; private set; }

        public virtual ICollection<Category> CategoryParents { get; private set; }

        public virtual ICollection<Category> CategoryChildren { get; private set; }

        public virtual ICollection<Product> Products { get; private set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<Product_Variant> ProductVariants { get; set; }
    }

    public partial class Category
    {
        public IList<Category> GetDescendantCategories(CategoryTypes categoryType)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                IList<Category> descendant = new List<Category>();
                string descendantType = categoryType.ToString().ToLower();
                var categories = db.Categories;
                descendant = (from cat in categories
                              where (
                                from product in cat.Products
                                where (from cat2 in product.Categories
                                       where cat2.Id == this.Id
                                       select cat2).Count<Category>() > 0
                                select product).Count<Product>() > 0 && cat.Category_type.Name == descendantType
                              select cat).ToList<Category>();
                return descendant;
            }
        }

        public Dictionary<string, string> Details
        {
            get
            {
                var details = new Dictionary<string, string>();
                foreach (var detail in Category_details)
                {
                    if (!details.ContainsKey(detail.name))
                        details.Add(detail.name, detail.value);
                }
                return details;
            }
        }
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
