namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Reflection;

    [Table("Product")]
    public partial class Product : BaseModel
    {
        public Product()
        {
            Product_Details = new HashSet<Product_Detail>();
            Categories = new HashSet<Category>();
            Product_Options = new HashSet<Product_Option>();
            Files = new HashSet<File>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string Content { get; set; }
        
        public int? ProductTypeId { get; set; }

        [Column("status")]
        public bool Status { get; set; }

        [Column("featured")]
        public bool Featured { get; set; }

        [StringLength(128)]
        public string ImageId { get; set; }

        public string Excerpt { get; set; }

        public virtual Product_Type ProductType { get; set; }

        [ForeignKey("ImageId")]
        public virtual File Image { get; set; }

        public virtual ICollection<Product_Detail> Product_Details { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Product_Option> Product_Options { get; set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<Post_Product> Post_Products { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }

    public partial class Product
    {

        [NotMapped]
        public Dictionary<string, bool> SelectedCategories { get; set; }

        public Sale CurrentSale
        {
            get
            {
                var sale = Sales.Count > 0 ? Sales.FirstOrDefault(o => o.EndDate > DateTime.UtcNow) : null;
                return sale;
            }
        }

        public Category Typology {
            get
            {
                var typology = Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.Typology).FirstOrDefault();
                return typology;
            }
        }

        public IEnumerable<Category> Rooms
        {
            get
            {
                var room = Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.Model);
                return room;
            }
        }

        public IEnumerable<Product_Variant> Materials
        {
            get
            {
                var materials = new List<Product_Variant>();
                foreach (var Product_Option in Product_Options)
                {
                    foreach (var Product_Variant in Product_Option.Product_Variants)
                    {
                        if (Product_Variant.VariantType == ProductVarianTypes.Material && !materials.Select(o => o.Id).Contains(Product_Variant.Id))
                            materials.Add(Product_Variant);
                    }
                }
                return materials;
            }
        }

        public Dictionary<string, object> DetailsToDictionary()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var detail in Product_Details)
            {
                if (!dic.ContainsKey(detail.Name)) {
                    if (detail.Name == "data" || detail.Name == "pattern")
                    {
                        dic.Add(detail.Name, detail.FileId);
                    }
                    else
                    {
                        dic.Add(detail.Name, detail.Value);
                    }
                    
                }
            }
            return dic;
        }

        public List<VCMS.Lib.Models.Product_Variant> SimpleProductOptions()
        {
            return new List<VCMS.Lib.Models.Product_Variant>();
        }
    }

    public enum UnitOfProduct { Default, Meter, SquareMeter }


    public class ProductEntityConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductEntityConfiguration()
        {
            this.HasMany(e => e.Product_Details)
                .WithOptional(e => e.Product);

            this.HasMany(e => e.Product_Options)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete();

            this.HasMany(e => e.Categories)
                .WithMany(e => e.Products)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Product_Category_Link");
                });

            this.HasMany(e => e.Files)
                .WithMany(e => e.Products)
                .Map(cs =>
                {
                    cs.MapLeftKey("ParentId");
                    cs.MapRightKey("ChildId");
                    cs.ToTable("Product_Files_Link");
                });
        }
    }
}
