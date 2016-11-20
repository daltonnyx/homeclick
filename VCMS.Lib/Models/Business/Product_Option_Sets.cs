namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Product_Option_Sets")]
    public partial class Product_Option_Set : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductOptionId { get; set; }

        [ForeignKey("ProductOptionId")]
        public virtual Product_Option ProductOption { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }



    public partial class Product_Option_Set
    {
        public int TotalValue
        {
            get
            {
                var totalValue = int.Parse(ProductOption.Product.Product_detail.FirstOrDefault(o => o.Name == ProductDetailTypes.Price).Value) * Quantity;
                return totalValue;
            }
        }
    }
}
