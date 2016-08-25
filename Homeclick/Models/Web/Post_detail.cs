namespace Homeclick.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Post_detail
    {
        public int Id { get; set; }

        public int? post_id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        public string value { get; set; }

        [StringLength(100)]
        public string caption { get; set; }

        public int? PostId { get; set; }

        public short? type { get; set; }

        public int? typeEnum { get; set; }

        public virtual Post Post { get; set; }
    }
}
