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
    [MetadataType(typeof(Room_metadata))]
    [DefaultClassOptions]
    [XafDisplayName("Căn hộ"),DefaultProperty("name")]
    public partial class Room
    {
        public Room(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }
    public class Room_metadata
    {
        [Browsable(false), DevExpress.Xpo.Key(true)]
        public Int32 Id
        { get; set; }

        [Browsable(false)]
        public Int32 floor_id
        {
            get;
            set;
        }

        
        public string coordinates
        {
            get;
            set;
        }
    }
}
