namespace Homeclick.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class Project
    {
        // Your context has been configured to use a 'Project' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Homeclick.Models.Project' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Project' 
        // connection string in the application configuration file.
        public Project()
        {
            this.Items = new HashSet<ProjectItem>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string City
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }
        
        public string Size
        {
            get;
            set;
        }

        public string Investor
        {
            get;
            set;
        }

        public uint Apartments
        {
            get;
            set;
        }

        public string StartYear
        {
            get;
            set;
        }

        public string CompletedYear
        {
            get;
            set;
        }

        public string ArchitetualDesignAgency
        {
            get;
            set;
        }

        public string FurnitureDesignAgency
        {
            get;
            set;
        }

        public string ViewDesignAgency
        {
            get;
            set;
        }

        public string ConstructionAgency
        {
            get;
            set;
        }

        public string Manager
        {
            get;
            set;
        }


        public string DistributionAgency
        {
            get;
            set;
        }

        public string HtmlContent
        {
            get;
            set;
        }

        public string MetaKeyword
        {
            get;

            set;
        }

        public string MetaDescription
        {
            get;
            set;
        }
        public virtual ICollection<ProjectItem> Items { get; set; }

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

        public Nullable<int> CreatedById { get; set; }

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

        public Nullable<int> UpdatedById
        {
            get;
            set;
        }

        public virtual User UpdatedBy
        {
            get;
            set;
        }


    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}