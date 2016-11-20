namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Post_Details : BaseModel
    {
        [Key]
        public new int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string Value { get; set; }

        [StringLength(100)]
        public string Caption { get; set; }

        public int? PostId { get; set; }

        public short? Type { get; set; }

        public Post Post { get; set; }
    }
}
