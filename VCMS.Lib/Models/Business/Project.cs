namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            Project_Details = new HashSet<Project_Details>();
        }

        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? DistrictId { get; set; }

        [StringLength(128)]
        public string PreviewImageId { get; set; }

        public int? CategoryId { get; set; }

        public bool Status { get; set; }

        [AllowHtml]
        public string HtmlContent { get; set; }

        [StringLength(128)]
        public string CreateUserId { get; set; }

        public virtual File PreviewImage { get; set; }

        public virtual Category Category { get; set; }

        public virtual District District { get; set; }

        public virtual ICollection<File> Files { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

        public virtual ICollection<Project_Details> Project_Details { get; set; }

        [NotMapped]
        public string[] FileIds { get; set; }

    }
}
