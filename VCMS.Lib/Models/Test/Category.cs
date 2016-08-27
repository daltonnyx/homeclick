namespace VCMS.Lib.Models.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            Category1 = new HashSet<Category>();
            Files = new HashSet<File>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        public string description { get; set; }

        public int? Order { get; set; }

        public int? ParentCategoryId { get; set; }

        public int? Category_typeId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Category1 { get; set; }

        public virtual Category Category2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> Files { get; set; }
    }
}
