using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace Homeclick.vn.Module.BusinessObjects
{
    [MetadataType(typeof(Product_detail_meta))]
    [DefaultProperty("caption"),XafDisplayName("Chi tiết sản phẩm")]
    public partial class Product_detail : IDetail
    {
        public Product_detail(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }

        public Product_detail() { }

        public TypeEnum typeEnum
        {
            get { return (TypeEnum)this.type; }
            set { this.type = (Int16)value; }
        }

    }
    public class Product_detail_meta
    {
        [Browsable(false), DevExpress.Xpo.Key(true)]
        public Int32 Id
        {
            get;
            set;
        }
    }
    public enum TypeEnum
    {
        str = 0,
        upload = 1,
        multiupload = 2
    }
}