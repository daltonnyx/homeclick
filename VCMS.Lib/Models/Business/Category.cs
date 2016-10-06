namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Category")]
    public partial class Category : BaseModel
    {
        public Category()
        {
            Category_details = new HashSet<Category_detail>();
            CategoryParents = new HashSet<Category>();
            CategoryChildren = new HashSet<Category>();
            Products = new HashSet<Product>();
        }

        [Key]
        public new int Id { get; set; }

        [Column("name")]
        [StringLength(128)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? Order { get; set; }

        public int? Category_TypeId { get; set; }

        public virtual Category_Type Category_Type { get; set; }

        public virtual ICollection<Category_detail> Category_details { get; set; }

        public virtual ICollection<Category> CategoryParents { get; set; }

        public virtual ICollection<Category> CategoryChildren { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<Product_Variant> ProductVariants { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Slide> Slides { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
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
                                select product).Count<Product>() > 0 && cat.Category_Type.Name == descendantType
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

        public List<Post> GetAllPost()
        {
            var posts = new List<Post>();
            posts.AddRange(Posts);
            foreach (var child in CategoryChildren)
            {
                posts.AddRange(child.GetAllPost());
                posts = posts.Distinct().ToList();
            }
            return posts;
        }

        public List<Product> GetAllProduct()
        {
            var products = new List<Product>();
            products.AddRange(Products);
            foreach (var child in CategoryChildren)
            {
                products.AddRange(child.GetAllProduct());
                products = products.Distinct().ToList();
            }
            return products;
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
