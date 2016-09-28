namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Posts_Products")]
    public partial class Post_Product : BaseModel
    {
        [Key]
        public new int Id { get; set; }

        public int Quantity { get; set; }

        public int? PostId { get; set; }

        public int ProductOptionId { get; set; }

        [ForeignKey("ProductOptionId")]
        public virtual Product_Option ProductOption { get; set; }

        public virtual Post Post { get; set; }
    }

    public partial class Post_Product
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
