using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.Lib.Models
{
    public interface OrderedModelBase
    {
        int? Order { get; set; }
    }
}
