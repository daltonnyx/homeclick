namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomField : BaseModel
    {
        public CustomField()
        {
            CustomField_Enums = new HashSet<CustomField_Enums>();
        }

        public new int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(256)]
        public string Label { get; set; }

        [StringLength(256)]
        public string PlaceHolder { get; set; }

        public int? Type { get; set; }

        public bool Status { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<CustomField_Enums> CustomField_Enums { get; set; }
    }

    public partial class CustomField
    {
        public FieldTypes FieldType { get { return (FieldTypes)Type; } }
    }

    public enum FieldTypes { Text, Number, Image, Select, Date }
}
