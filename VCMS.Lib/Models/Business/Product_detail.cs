namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product_detail
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Value { get; set; }

        [StringLength(128)]
        public string Caption { get; set; }

        public int ProductId { get; set; }

        [StringLength(128)]
        public string Type { get; set; }

        public int? TypeEnum { get; set; }

        public virtual Product Product { get; set; }
    }

    public static class ProductDetailTypes
    {
        public const string Size = "kich_thuoc";
        public const string Warranty = "bao_hanh";
        public const string Price = "gia";
        public const string Weight = "nang";
        public const string MadeIn = "xuat_xu";
    }
}
