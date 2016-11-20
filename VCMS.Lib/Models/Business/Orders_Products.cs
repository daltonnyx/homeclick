namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders_Products
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int? VariantId { get; set; }

        public int? Quantity { get; set; }

        public int? SaleId { get; set; }

        public virtual Order Order { get; set; }

        public virtual Sale Sale { get; set; }

        [ForeignKey("VariantId")]
        public Product_Variant Product_Variant { get; set; }
    }
}
