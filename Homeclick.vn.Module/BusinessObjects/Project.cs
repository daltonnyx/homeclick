using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace Homeclick.vn.Module.BusinessObjects
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Persistent("Projects")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112701.aspx).
    public class Project : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Project(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private int _id;

        [Persistent("Id"),Key(true),Browsable(false)]
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                SetPropertyValue("ID", ref _id, value);
            }
        }

        private string _name;
        [Size(255)]
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        private string _city;
        [Size(128)]
        public string City
        {
            get { return _city; }
            set { SetPropertyValue("City", ref _city, value); }
        }

        private string _state;
        [Size(128)]
        public string State
        {
            get { return _state; }
            set { SetPropertyValue("State", ref _state, value); }
        }

        private string _address;
        [Size(255)]
        public string Address
        {
            get { return _address; }
            set { SetPropertyValue("State", ref _address, value); }
        }

        private string _size;

        public string Size
        {
            get { return _size; }
            set { SetPropertyValue("Size", ref _size, value); }
        }

        private string _investor;
        [Size(255)]
        public string Investor
        {
            get { return _investor; }
            set { SetPropertyValue("Investor", ref _investor, value); }
        }

        private uint _apartments;

        public uint  Apartments
        {
            get { return _apartments; }
            set { SetPropertyValue("Apartments", ref _apartments, value); }
        }

        private string _startyear;
        [Size(4)]
        public string StartYear
        {
            get { return _startyear; }
            set { SetPropertyValue("StartYear", ref _startyear, value); }
        }

        private string _completedyear;
        [Size(4)]
        public string CompletedYear
        {
            get { return _completedyear; }
            set { SetPropertyValue("CompletedYear", ref _completedyear, value); }
        }

        private string _architetualdesignagency;
        [Size(255)]
        public string ArchitetualDesignAgency
        {
            get { return _architetualdesignagency; }
            set { SetPropertyValue("ArchitetualDesignAgency", ref _architetualdesignagency, value); }
        }

        private string _furnituredesignagency;
        [Size(255)]
        public string FurnitureDesignAgency
        {
            get { return _furnituredesignagency; }
            set { SetPropertyValue("FurnitureDesignAgency", ref _furnituredesignagency, value); }
        }

        private string _viewdesignagency;
        [Size(255)]
        public string ViewDesignAgency
        {
            get { return _viewdesignagency; }
            set { SetPropertyValue("ViewDesignAgency", ref _viewdesignagency, value); }
        }

        private string _contructionagency;
        [Size(255)]
        public string ConstructionAgency
        {
            get
            {
                return _contructionagency;
            }
            set { SetPropertyValue("ConstructionAgency", ref _contructionagency, value); }
        }

        private string _manager;
        [Size(255)]
        public string Manager
        {
            get
            {
                return _manager;
            }
            set
            {
                SetPropertyValue("Manager", ref _manager, value);
            }
        }

        private string _distributionagency;
        [Size(255)]
        public string DistributionAgency
        {
            get
            {
                return _distributionagency;
            }
            set
            {
                SetPropertyValue("DistributionAgency", ref _distributionagency, value);
            }
        }

        private string _htmlcontent;
        [Size(SizeAttribute.Unlimited)]
        public string HtmlContent
        {
            get
            {
                return _htmlcontent;
            }
            set
            {
                SetPropertyValue("HtmlContent", ref _htmlcontent, value);
            }
        }

        private string _metakeyword;
        [Size(255)]
        public string MetaKeyword
        {
            get
            {
                return _metakeyword;
            }

            set
            {
                SetPropertyValue("MetaKeyword", ref _metakeyword, value);
            }
        }

        private string _metadescription;
        [Size(255)]
        public string MetaDescription
        {
            get
            {
                return _metadescription;
            }
            set
            {
                SetPropertyValue("MetaDescription", ref _metadescription, value);
            }
        }

        [Association("Project-ProjectItems")]
        public XPCollection<ProjectItem> Items
        {
            get { return GetCollection<ProjectItem>("Items"); }
        }

        private bool _lockedout;

        public bool LockedOut
        {
            get
            {
                return _lockedout;
            }

            set
            {
                SetPropertyValue("LockedOut", ref _lockedout, value);
            }
        }

        private DateTime _createddate;

        public DateTime CreatedDate
        {
            get
            {
                return _createddate;
            }

            set
            {
                SetPropertyValue("CreatedDate", ref _createddate, value);
            }
        }

        private User _createdby;

        public User CreatedBy
        {
            get { return _createdby; }
            set { SetPropertyValue<User>("CreatedBy", ref _createdby, value); }
        }

        private DateTime _updateddate;

        public DateTime UpdatedDate
        {
            get { return _updateddate; }
            set { SetPropertyValue("UpdatedDate", ref _updateddate, value); }
        }

        private User _updatedby;

        public User UpdatedBy
        {
            get { return _updatedby; }
            set { SetPropertyValue<User>("UpdatedBy", ref _updatedby, value);    }
        }


        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}
