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
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Persistent("ProductsLayoutsLink")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112701.aspx).
    public class ProductsLayoutsLinkTable : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ProductsLayoutsLinkTable(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private int _id;

        [Key(true), Browsable(false)]
        public int ID
        {
            get { return _id; }
            set { SetPropertyValue("ID", ref _id, value); }
        }

        private Product _product;
        [Association]
        public Product Product
        {
            get { return _product; }
            set { SetPropertyValue<Product>("Product", ref _product, value); }
        }

        private ProjectLayout _layout;
        [Association]
        public ProjectLayout Layout
        {
            get
            {
                return _layout;
            }

            set { SetPropertyValue<ProjectLayout>("Layout", ref _layout, value); }
        }


    }
}
