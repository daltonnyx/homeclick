using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homeclick.vn.Module.BusinessObjects
{
    [Persistent("ProductDanhSachProducts_CartDanhSachCarts")]
    public class DanhSachProductsDanhSachCarts : XPLiteObject
    {
        public DanhSachProductsDanhSachCarts(Session session) : base(session) { }

        private int f_oid;
        [Key(true)]
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
        [Association]
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

        private Cart f_cart;
        [Association]
        public Cart DanhSachCarts
        {
            get
            {
                return f_cart;
            }
            set
            {
                SetPropertyValue<Cart>("DanhSachCarts", ref f_cart, value);
            }
        }

        private string f_color;

        public string Color
        {
            get
            {
                return f_color;
            }
            set
            {
                SetPropertyValue("Color", ref f_color, value);
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
