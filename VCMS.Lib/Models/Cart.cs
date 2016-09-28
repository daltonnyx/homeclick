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
        public Product_Option product_Option { get; set; }
        public int quantity { get; set; }
        public decimal total
        {
            get { return Convert.ToInt32(product_Option.Product.DetailsToDictionary()[ProductDetailTypes.Price]) * quantity; }
        }

        public CartItem(Product_Option productOption)
        {
            id = Guid.NewGuid().ToString();
            this.product_Option = productOption;
        }
    }
}