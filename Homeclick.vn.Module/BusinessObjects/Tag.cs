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
    [MetadataType(typeof(Tag_metadata))]
    [DefaultClassOptions]
    [XafDisplayName("Thẻ"),DefaultProperty("name")]
    public partial class Tag
    {
        public Tag(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }
    public class Tag_metadata
    {
        [Browsable(false), DevExpress.Xpo.Key(true)]
        public Int32 Id
        {
            get;
            set;
        }
    }
}
