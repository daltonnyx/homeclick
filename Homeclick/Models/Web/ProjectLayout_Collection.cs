namespace Homeclick.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProjectLayout_Collection
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string Image { get; set; }

        public string HtmlContent { get; set; }

        public int? LayoutId { get; set; }

        public string Area { get; set; }

        public bool? LockedOut { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public virtual ProjectItem ProjectItem { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
