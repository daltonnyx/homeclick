namespace Homeclick.Models
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

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
