using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homeclick.Models
{
    public class ProjectLayout_Collection
    {

        public ProjectLayout_Collection()
        {
            this.Products = new HashSet<Product>();
        }
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

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

        public string HtmlContent
        {
            get;
            set;
        }

        public Nullable<int> LayoutId;


        public virtual ProjectItem Layout
        {
            get;
            set;
        }

        
        public virtual ICollection<Product> Products
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

        public Nullable<int> UpdatedById;

        public virtual User UpdatedBy
        {
            get;
            set;
        }
    }
}