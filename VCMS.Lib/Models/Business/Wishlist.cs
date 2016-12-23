using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.Lib.Models
{
    [Table("Wishlist")]
    public partial class Wishlist
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string title { get; set; }

        [StringLength(100)]
        public string created_date { get; set; }

        [StringLength(100)]
        public string description { get; set; }

        public string UserId { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
