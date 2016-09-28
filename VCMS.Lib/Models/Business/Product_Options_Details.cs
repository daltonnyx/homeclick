namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product_Options_Details
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Value { get; set; }

        [StringLength(128)]
        public string Caption { get; set; }

        public int? ProductOptionId { get; set; }

        [StringLength(128)]
        public string Type { get; set; }

        public int? TypeEnum { get; set; }

        public virtual Product_Option Product_Options { get; set; }
    }
}
