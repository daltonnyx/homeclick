using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homeclick.Models
{
    public class ProductsLayoutsLink
    {
        public int ID { get; set; }
        public int Layout { get; set; }
        public int Product { get; set; }
        public int Total { get; set; }

        public virtual string ProductName { get; set; }
        public virtual int ProductValue { get; set; }
        public virtual int TotalValue { get; set; }
    }
}