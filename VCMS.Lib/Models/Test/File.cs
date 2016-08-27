namespace VCMS.Lib.Models.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class File
    {
        public string Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public int? FileTypeId { get; set; }

        public long? Size { get; set; }

        [StringLength(128)]
        public string CreateUserId { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(128)]
        public string UpdateUserId { get; set; }

        public DateTime? UpdateTime { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual AspNetUser AspNetUser1 { get; set; }

        public virtual Category Category { get; set; }
    }
}
