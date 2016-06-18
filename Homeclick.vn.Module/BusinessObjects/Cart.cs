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
using System.ComponentModel.DataAnnotations.Schema;
namespace Homeclick.vn.Module.BusinessObjects
{
    [MetadataType(typeof(Cart_metadata))]
    [DefaultClassOptions,XafDisplayName("Giỏ hàng"),DefaultProperty("DisplayName")]
    public partial class Cart
    {
        public Cart(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }

        public string DisplayName
        {
            get
            {
                if(UserId != null)
                    return "Card No." + Id.ToString() + " of " + UserId.UserName;
                return "Card No." + Id.ToString();
            }
        }

        [DevExpress.Xpo.Association, Browsable(false)]
        public IList<DanhSachProductsDanhSachCarts> ProductToCartLinks
        {
            get
            {
                return GetList<DanhSachProductsDanhSachCarts>("ProductToCartLinks");
            }
        }

        [ManyToManyAlias("ProductToCartLinks", "DanhSachProducts")]
        public IList<Product> Products
        {
            get
            {
                return GetList<Product>("Products");
            }
        }
    }
    public class Cart_metadata
    {
        [Browsable(false), DevExpress.Xpo.Key(true)]
        public Int32 Id
        {
            get;
            set;
        }

        [Browsable(false)]
        public Int32 user_id
        {
            get;
            set;
        }

        [Browsable(false)]
        public short status
        {
            get;
            set;
        }

        [NotMapped, NonPersistent]
        public CartStatus StatusEnum
        {
            get { return (CartStatus)this.status; }
            set { this.status = (short)value; }
        }
    }
    public enum CartStatus
    {
        pending = 0,
        success = 1,
        dropped = 2,
        failed = 3,
    }
}
