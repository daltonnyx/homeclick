namespace VCMS.Lib.Models.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomField
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomField()
        {
            CustomField_Enums = new HashSet<CustomField_Enums>();
        }

        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Label { get; set; }

        [StringLength(256)]
        public string PlaceHolder { get; set; }

        public int? Type { get; set; }

        [StringLength(128)]
        public string ValueType { get; set; }

        public bool? Multiple { get; set; }

        public bool? Status { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(128)]
        public string CreateUserId { get; set; }

        public DateTime? CreateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomField_Enums> CustomField_Enums { get; set; }
    }
}
