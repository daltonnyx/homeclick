namespace Homeclick.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectItem")]
    public partial class ProjectItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectItem()
        {
            ProjectItem1 = new HashSet<ProjectItem>();
            ProjectLayout_Collection = new HashSet<ProjectLayout_Collection>();
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string Image { get; set; }

        public string AreaCoords { get; set; }

        public int? Order { get; set; }

        public int? ProjectId { get; set; }

        public bool? LockedOut { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public int? CategoryId { get; set; }

        public int? ParentId { get; set; }


        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public virtual Project Project { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectItem> ProjectItem1 { get; set; }

        public virtual ProjectItem ProjectItem2 { get; set; }

        public virtual User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectLayout_Collection> ProjectLayout_Collection { get; set; }
    }
}
