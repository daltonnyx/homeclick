namespace VCMS.Lib.Models
{
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
            //Files = new HashSet<File>();
            //Children = new HashSet<Product_Variant>();
            Categories = new HashSet<Category>();
        }

        public new int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Description { get; set; }

        public string PreviewImageId { get; set; }

        //public int? ParentId { get; set; }

        [ForeignKey("PreviewImageId")]
        public virtual File PreviewImage { get; set; }

        //public virtual Product_Variant Parent { get; set; }

        //public virtual ICollection<Product> Products { get; set; }

        //public virtual ICollection<Product_Variant> Children { get; set; }

        //public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Product_Option> Product_Options { get; set; }
    }

    public partial class Product_Variant
    {
       public ProductVarianTypes VariantType
        {
            get
            {
                var categories = this.Categories.Where(o => o.Category_TypeId == (int)CategoryTypes.ProductVariant);
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
    }

    public enum ProductVarianTypes { Default = -1, Material = 77, Color = 76 }
}
