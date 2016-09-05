using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homeclick.vn.Module.BusinessObjects
{
    public interface IDetail
    {
        short type { get; set; }
        string name { get; set; }
        string value { get; set; }
    }
}
