namespace VCMS.Lib.Models
{
    using Business;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;

    public partial class File : BaseModel
    {
        [StringLength(128)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public int? FileTypeId { get; set; }

        public long? Size { get; set; }

        public virtual Category FileType { get; set; }

        [ForeignKey("CreateUserId")]
        public virtual ApplicationUser CreateUser { get; set; }

        [ForeignKey("UpdateUserId")]
        public virtual ApplicationUser UpdateUser { get; set; }
    }
}
