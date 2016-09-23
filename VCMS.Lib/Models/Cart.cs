using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VCMS.Lib.Models;

namespace VCMS.Lib.Models.Business
{
    public class CartItem
    {
        public string id { get; set; }
        public Product product { get; set; }
        public List<Product_Variant> variants { get; set; }
        public int quantity { get; set; }
        public decimal total
        {
            get { return Convert.ToInt32(product.DetailsToDictionary()[ProductDetailTypes.Price]) * quantity; }
        }

        public CartItem(Product product)
        {
            id = Guid.NewGuid().ToString();
            this.product = product;
        }
    }
}