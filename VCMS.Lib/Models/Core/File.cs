namespace VCMS.Lib.Models
{
    using Business;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Spatial;

    public partial class File : BaseModel
    {
        [Display(Name = "FileName", ResourceType = typeof(Strings))]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Extension { get; set; }

        public int? FileTypeId { get; set; }

        public long Size { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Product_Variant> Product_Variants { get; set; }

        [Display(Name = "FileType", ResourceType = typeof(Strings))]
        public virtual Category FileType { get; set; }

        [ForeignKey("CreateUserId")]
        public virtual ApplicationUser CreateUser { get; set; }

        [ForeignKey("UpdateUserId")]
        public virtual ApplicationUser UpdateUser { get; set; }
    }

    public enum FileTypes { Image = 74 }
}
