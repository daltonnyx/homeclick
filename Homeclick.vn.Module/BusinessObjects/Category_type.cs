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
    [MetadataType(typeof(Category_type_metadata))]
    [DefaultClassOptions]
    [XafDisplayName("Kiểu danh mục"),DefaultProperty("caption")]
    public partial class Category_type
    {
        public Category_type(Session session) : base(session) { }

        public override void AfterConstruction() { base.AfterConstruction(); }

        public TypeFor typeFor
        {
            get { return (TypeFor)this.type_for; }
            set { this.type_for = (short)value; }
        }
    }
    public class Category_type_metadata
    {
        [Browsable(false), DevExpress.Xpo.Key(true)]
        public Int32 Id
        {
            get;
            set;
        }
    }

    public enum TypeFor
    {
        product = 0,
        other = 9,
    }
}
