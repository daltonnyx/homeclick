using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.Lib.Models
{
    public class FileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Ext{ get; set; }
        public string FileType { get; set; }
        public string Size { get; set; }
        public string CreateTime { get; set; }
    }
}
