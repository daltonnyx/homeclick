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
    [MetadataType(typeof(Post_detail_metadata))]
    [XafDisplayName("Chi tiết bài viết"),DefaultProperty("caption")]
    public partial class Post_detail : IDetail
    {
        public Post_detail(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }

        public TypeEnum typeEnum
        {
            get { return (TypeEnum)this.type; }
            set { this.type = (Int16)value; }
        }
    }
    public class Post_detail_metadata
    {
        [Browsable(false), DevExpress.Xpo.Key(true)]
        public Int32 Id
        {
            get;
            set;
        }

        [Browsable(false)]
        public Int32 post_id
        {
            get;
            set;
        }
    }
}
