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
    [MetadataType(typeof(Wishlist_metadata))]
    [XafDisplayName("Wishlist"), DefaultProperty("DisplayName")]
    public partial class Wishlist
    {
        public Wishlist(Session session) : base(session) { }

        public string DisplayName
        {
            get {
                if(UserId != null)
                    return "Wishlist No." + this.Id.ToString() + " of " + this.UserId.UserName;
                return "Wishlist No." + this.Id.ToString();
            }
        }

        public override void AfterConstruction() { base.AfterConstruction(); }
    }
    public class Wishlist_metadata
    {
        [Browsable(false), DevExpress.Xpo.Key(true)]
        public Int32 Id
        {
            get;
            set;
        }

        [Browsable(false)]
        public Int32 user_id
        { get; set; }
    }
}
