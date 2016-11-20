namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        public Order()
        {
            Orders_Products = new HashSet<Orders_Products>();
        }

        public int Id { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(128)]
        public string UserName { get; set; }

        [StringLength(128)]
        public string UserEmail { get; set; }

        [StringLength(128)]
        public string UserPhone { get; set; }

        public DateTime? Date { get; set; }

        public bool? Status { get; set; }

        public string Comment { get; set; }

        public virtual ICollection<Orders_Products> Orders_Products { get; set; }
    }
}
