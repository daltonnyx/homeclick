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
    [MetadataType(typeof(Category_detail_meta))]
    [XafDisplayName("Chi tiết danh mục"),DefaultProperty("caption")]
    public partial class Category_detail : IDetail
    {
        public Category_detail(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
        [ImmediatePostData]
        public TypeEnum typeEnum
        {
            get { return (TypeEnum)this.type; }
            set { this.type = (Int16)value; }
        }
    }
    public class Category_detail_meta
    {
        [Browsable(false), DevExpress.Xpo.Key(true)]
        public Int32 Id
        {
            get;
            set;
        }

        [Browsable(false)]
        public Int32 Category_id
        {
            get;
            set;
        }
        [Size(SizeAttribute.Unlimited)]
        public string value
        {
            get;
            set;
        }
    }
}
