using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homeclick.vn.Module.BusinessObjects
{
    [Persistent("ProductDanhSachProducts_CategoryDanhSachCategories")]
    public class DanhSachProductsyDanhSachCategories : XPLiteObject
    {
        public DanhSachProductsyDanhSachCategories(Session session) : base(session) { }

        private int f_oid;
        
        [Key(true),Persistent("OID")]
        public int OID
        {
            get
            {
                return f_oid;
            }
            set
            {
                SetPropertyValue("OID", ref f_oid, value);
            }
        }

        private Product f_product;
        [Association, Persistent("DanhSachProducts")]
        public Product DanhSachProducts
        {
            get
            {
                return f_product;
            }
            set
            {
                SetPropertyValue<Product>("DanhSachProducts", ref f_product, value);
            }
        }

        
        private Category f_category;
        [Association, Persistent("DanhSachCategories")]
        public Category DanhSachCategories
        {
            get
            {
                return f_category;
            }
            set
            {
                SetPropertyValue<Category>("DanhSachCategories", ref f_category, value);
            }
        }

        private int f_quantity;
        public int Quantity
        {
            get
            {
                return f_quantity;
            }
            set
            {
                SetPropertyValue("Quantity", ref f_quantity, value);
            }
        }
    }
}
