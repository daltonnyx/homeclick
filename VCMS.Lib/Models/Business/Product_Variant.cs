namespace VCMS.Lib.Models
{
    using Business;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Product_Variants")]
    public partial class Product_Variant : BaseModel
    {
        public Product_Variant()
        {
            Files = new HashSet<File>();
            Children = new HashSet<Product_Variant>();
            Categories = new HashSet<Category>();
        }

        public new int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Description { get; set; }

        [Column("ImageId")]
        public string ImageFileId { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ImageFileId")]
        public virtual File ImageFile { get; set; }

        public virtual Product_Variant Parent { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Product_Variant> Children { get; set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }

    public partial class Product_Variant
    {
       public ProductVarianTypes VariantType
        {
            get
            {
                var categories = this.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.ProductVariant);
                var type = ProductVarianTypes.Default;
                if (categories.Count() > 0)
                {
                    var category = categories.FirstOrDefault();
                    switch (category.Id)
                    {
                        case (int)ProductVarianTypes.Material:
                            type = ProductVarianTypes.Material;
                            break;
                        case (int)ProductVarianTypes.Color:
                            type = ProductVarianTypes.Color;
                            break;
                    }
                }
                return type;
            }
        }

        public List<Product_Variant> GetAllChildren()
        {
            var allChildren = new List<Product_Variant>();
            foreach (var child in Children)
            {
                allChildren.Add(child);
                allChildren.AddRange(child.GetAllChildren() );
            }
            return allChildren;
        }
    }

    public enum ProductVarianTypes { Default = -1, Material = 77, Color = 76 }
}
