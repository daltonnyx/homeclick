using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homeclick.Models
{
    public class Giohang
    {
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        public int iProduct_id { get; set; }
        public string iTenProduct { get; set; }
        public string iImage { get; set; }
        public decimal iPrince { get; set; }
        public int iNumberProduct { get; set; }
        public decimal iTotal
        {
            get { return iPrince * iNumberProduct; }
        }

        public Giohang(int id)
        {
            iProduct_id = id;
            Product product = db.Products.Single(n => n.Id == id);
            Dictionary<string, object> arrayItem = product.ToArray(product);
            Dictionary<string, object> details = arrayItem["Product_detail"] as Dictionary<string, object>;
            iTenProduct = product.name;
            iImage = product.image;
            iPrince = Convert.ToDecimal(details["gia"]);
            //iNumberProduct = 2;
        }
    }
}