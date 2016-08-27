using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DevExpress.Persistent.Base;


namespace Homeclick.vn.Module.BusinessObjects
{
    [MetadataType(typeof(Category_metadata))]
    [DefaultClassOptions,XafDisplayName("Danh mục"),DefaultProperty("name")]
    public partial class Category
    {
        public Category(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }


        [DevExpress.Xpo.Association, Browsable(false)]
        public IList<DanhSachProductsyDanhSachCategories> ProductToCategoryLinks
        {
            get
            {
                return GetList<DanhSachProductsyDanhSachCategories>("ProductToCategoryLinks");
            }
        }

        [ManyToManyAlias("ProductToCategoryLinks", "DanhSachProducts")]
        public IList<Product> Products
        {
            get
            {
                return GetList<Product>("Products");
            }
        }
    }
    public class Category_metadata
    {
        [Browsable(false), DevExpress.Xpo.Key(true)]
        public Int32 Id
        {
            get;
            set;
        }

        [Browsable(false)]
        public Int32 parent_id
        {
            get;
            set;
        }

        [Browsable(false)]
        public Int32 Category_type_id
        {
            get;
            set;
        }
    }
}
