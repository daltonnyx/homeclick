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
    [MetadataType(typeof(Canva_metadata))]
    [DefaultClassOptions,XafDisplayName("Tự thiết kế"),DefaultProperty("DisplayName")]
    public partial class Canva
    {
        public Canva()
        {
            
        }

        public Canva(Session session) : base(session)
        { }

        public override void AfterConstruction() { base.AfterConstruction(); }

        public string DisplayName
        {
            get
            {
                if(UserId != null)
                    return "Canva " + UserId.UserName + " " + Id.ToString();
                return "Canva " + Id.ToString();
            }
        }
    }
    public class Canva_metadata
    {
        [Browsable(false),DevExpress.Xpo.Key(true)]
        public Int32 Id
        { get; set; }

        [Browsable(false)]
        public Int32 room_id
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
        public Int32 cart_id
        {
            get;
            set;
        }
    }
}
