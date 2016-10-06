namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class District
    {
        public District()
        {
            Projects = new HashSet<Project>();
        }

        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        public int? CityId { get; set; }

        public bool Status { get; set; }

        public virtual City City { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
