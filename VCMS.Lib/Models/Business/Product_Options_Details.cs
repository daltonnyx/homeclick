namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product_Options_Details : Entity_Detail
    {
        public int? ProductOptionId { get; set; }

        public virtual Product_Option Product_Options { get; set; }
    }
}
