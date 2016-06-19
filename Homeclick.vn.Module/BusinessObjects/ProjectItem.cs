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
    [Persistent("ProjectItems")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112701.aspx).
    public class ProjectItem : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ProjectItem(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private int _id;

        [Key(true),Persistent("Id"),Browsable(false)]
        public int ID
        {
             get { return _id; }
            set { SetPropertyValue("ID", ref _id, value); }
        }

        private string _name;
        [Size(255)]
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        private string _description;
        [Size(255)]
        public string Description
        {
            get { return _description; }
            set { SetPropertyValue("Description", ref _description, value); }
        }

        private string _image;
        [Size(SizeAttribute.Unlimited)]
        public string Image
        {
            get { return _image; }
            set { SetPropertyValue("Image", ref _image, value); }
        }

        private int _order;

        public int Order
        {
            get { return _order; }
            set { SetPropertyValue("Order", ref _order, value); }
        }


        private Project _project;
        [Association("Project-ProjectItems"), Persistent("ProjectId")]
        public Project Project
        {
            get { return _project; }
            set { SetPropertyValue("Project", ref _project, value); }
        }

        [Association("Item-Layouts")]
        public XPCollection<ProjectLayout> Layouts
        {

            get
            {
                return GetCollection<ProjectLayout>("Layouts");
            }

        }


        private bool _lockedout;

        public bool LockedOut
        {
            get { return _lockedout; }
            set { SetPropertyValue("LockedOut", ref _lockedout, value); }
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
            set { SetPropertyValue<User>("UpdatedBy", ref _updatedby, value); }
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
