namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Slide : BaseModel
    {
        public new int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(128)]
        public string ImageFileId { get; set; }

        public string Link { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("ImageFileId")]
        public File ImageFile { get; set; }

        public Category Category { get; set; }
    }
}
