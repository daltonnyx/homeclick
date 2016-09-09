namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category_type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category_type()
        {
            Categories = new HashSet<Category>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [StringLength(100)]
        public string caption { get; set; }

        public int? typeFor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
    }

    public enum CategoryTypes { Material = 3, Model = 2, Typology = 1, LifeStype = 4, Collection = 5, Masonry = 15, Postcat = 16, ProjectType = 18, FileType = 20, ProductVariant = 21, FileGroup = 22 }
}
