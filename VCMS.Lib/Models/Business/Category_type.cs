namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category_Type : BaseModel
    {
        public Category_Type()
        {
            Categories = new HashSet<Category>();
        }

        public new int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Description { get; set; }

        public int? TypeFor { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }

    public enum CategoryTypes { Default = -1, Material = 3, Model = 2, Typology = 1, LifeStype = 4, Collection = 5, Masonry = 15, Postcat = 16, ProjectType = 18, FileType = 20, ProductVariant = 21, FileGroup = 22, Slide = 28 }
}
