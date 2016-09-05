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
using FileSystemData.BusinessObjects;

namespace Homeclick.vn.Module.BusinessObjects
{
    [MetadataType(typeof(Floor_metadata))]
    [DefaultClassOptions,XafDisplayName("Tầng căn hộ"),DefaultProperty("name")]
    public partial class Floor
    {
        public Floor(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }

    public class Floor_metadata
    {
        [Browsable(false), DevExpress.Xpo.Key(true)]
        public Int32 Id
        {
            get;
            set;
        }
        [Browsable(false)]
        public Int32 block_id
        {
            get;
            set;
        }
    }
}
