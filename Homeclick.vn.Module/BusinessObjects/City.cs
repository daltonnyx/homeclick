using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homeclick.vn.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Thành phố"),DefaultProperty("name")]
    public partial class City
    {
        public City(Session session) : base(session) { }
    }
}
