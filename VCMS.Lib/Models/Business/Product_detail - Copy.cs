namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product_Details")]
    public partial class Product_Detail : Entity_Detail
    {
        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }

    public static class ProductDetailTypes
    {
        public const string Size = "kich_thuoc";
        public const string Warranty = "bao_hanh";
        public const string Price = "gia";
        public const string Weight = "nang";
        public const string MadeIn = "xuat_xu";    
        public const string UnitType = "dvt";//đơn vị tính
        public const string Quantity = "so_luong";
    }
}
