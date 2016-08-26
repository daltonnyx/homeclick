namespace Homeclick.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Project")]
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            ProjectItems = new HashSet<ProjectItem>();
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int? CityId { get; set; }

        public int? StateId { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Size { get; set; }

        [StringLength(255)]
        public string Investor { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Apartments { get; set; }

        [StringLength(4)]
        public string StartYear { get; set; }

        [StringLength(4)]
        public string CompletedYear { get; set; }

        [StringLength(255)]
        public string ArchitetualDesignAgency { get; set; }

        [StringLength(255)]
        public string FurnitureDesignAgency { get; set; }

        [StringLength(255)]
        public string ViewDesignAgency { get; set; }

        [StringLength(255)]
        public string ConstructionAgency { get; set; }

        [StringLength(255)]
        public string Manager { get; set; }

        [StringLength(255)]
        public string DistributionAgency { get; set; }

        public string HtmlContent { get; set; }

        [StringLength(255)]
        public string MetaKeyword { get; set; }

        [StringLength(255)]
        public string MetaDescription { get; set; }

        public bool? LockedOut { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public string Image { get; set; }

        public int? CategoryId { get; set; }

        public bool? Completed { get; set; }

        [StringLength(50)]
        public string MapLocation { get; set; }


        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public virtual City City { get; set; }

        public virtual User User { get; set; }

        public virtual State State { get; set; }

        public virtual User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectItem> ProjectItems { get; set; }
    }
}
