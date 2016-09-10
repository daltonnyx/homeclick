namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category_detail
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        public string value { get; set; }

        [StringLength(100)]
        public string caption { get; set; }

        public int? CategoryId { get; set; }

        public int? type { get; set; }

        public virtual Category Category { get; set; }
    }

    public static class CategoryDetailTypes
    {
        public const string Icon = "Icon";
    }
}
