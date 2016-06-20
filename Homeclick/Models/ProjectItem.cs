using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homeclick.Models
{
    public partial class ProjectItem
    {
        public ProjectItem()
        {
            this.Layouts = new HashSet<ProjectLayout_Collection>();
        }

        public int Id { get; set; }

        public string Name { get; set;}

      
        public string Description
        {
            get;
            set;
        }

        public string Image
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }

        public Nullable<int> ProjectId { get; set; }
        
        public virtual Project Project
        {
            get;
            set;
        }

        public virtual ICollection<ProjectLayout_Collection> Layouts
        {

            get;
            set;

        }
        
        public bool LockedOut
        {
            get;
            set;
        }

        public Nullable<DateTime> CreatedDate
        {
            get;

            set;
        }

        public Nullable<int> CreatedById;

        public virtual User CreatedBy
        {
            get;
            set;
        }

        public Nullable<DateTime> UpdatedDate
        {
            get;
            set;
        }

        public Nullable<int> UpdatedById { get; set; }

        public virtual User UpdatedBy
        {
            get;
            set;
        }
    }
}