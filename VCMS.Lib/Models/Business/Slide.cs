namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Slide 
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(128)]
        public string ImageFileId { get; set; }

        public string Link { get; set; }

        public int? CategoryId { get; set; }

        public bool Status { get; set; }

        [ForeignKey("ImageFileId")]
        public virtual File ImageFile { get; set; }

        public virtual Category Category { get; set; }

        public DateTime? CreateTime { get; set; }

        [StringLength(128)]
        public string CreateUserId { get; set; }

        [ForeignKey("CreateUserId")]
        public virtual ApplicationUser CreateUser { get; set; }
    }

    public enum SlideTypes { Home = 122, Collection = 123 }
}
