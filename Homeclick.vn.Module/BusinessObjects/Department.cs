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
    [MetadataType(typeof(Department_metadata))]
    [DefaultClassOptions,XafDefaultProperty("name"),XafDisplayName("Chung cư")]
    public partial class Department
    {
        public Department(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }
    public class Department_metadata
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
    }
}
