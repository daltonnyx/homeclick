namespace Homeclick.Models.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("City")]
    public partial class City
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [StringLength(100)]
        public string description { get; set; }

        [StringLength(100)]
        public string ship_price { get; set; }

        [StringLength(100)]
        public string postal_code { get; set; }
    }
}
