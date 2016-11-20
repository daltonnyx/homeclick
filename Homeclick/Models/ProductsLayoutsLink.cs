using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homeclick.Models
{
    public class ProjectLayout_Collection_Product_Link
    {
        public int Id { get; set; }
        public int PatentId { get; set; }
        public int ChildId { get; set; }
        public int Quantity { get; set; }

        public virtual string ProductName { get; set; }
        public virtual int ProductValue { get; set; }
        public virtual int TotalValue { get; set; }
    }
}