namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Posts_Products")]
    public partial class Post_Product : BaseModel
    {
        [Key]
        public new int Id { get; set; }

        public int Quantity { get; set; }

        public int PostId { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual Post Post { get; set; }
    }
}
