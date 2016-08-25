namespace Homeclick.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product_detail
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        public string value { get; set; }

        [StringLength(100)]
        public string caption { get; set; }

        public int? ProductId { get; set; }

        [StringLength(100)]
        public string type { get; set; }

        public int? typeEnum { get; set; }

        public virtual Product Product { get; set; }
    }
}
