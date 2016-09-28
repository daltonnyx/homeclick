namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sale")]
    public partial class Sale : BaseModel
    {
        public Sale()
        {
            Orders_Products = new HashSet<Orders_Products>();
            Products = new HashSet<Product>();
        }

        public new int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int Percent { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int CategoryId { get; set; }

        public bool Status { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public virtual ICollection<Orders_Products> Orders_Products { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

    public enum SaleType { Default = -1, Product = 120, Collection = 121 }
}
