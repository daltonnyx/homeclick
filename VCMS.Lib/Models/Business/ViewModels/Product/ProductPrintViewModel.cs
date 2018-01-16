using System;
using DevExpress.Xpo;

namespace VCMS.Lib.Models
{

    public class ProductPrintViewModel
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string ImgUrl
        {
            get;
            set;
        }

        public int Price
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }

        public int Amount
        {
            get
            {
                return this.Price * this.Quantity;
            } 
        }

        public string getPrice()
        {
            return string.Format("{0:#,##0} vnđ", this.Price);
        }

        public string getAmount()
        {
            return string.Format("{0:#,##0} vnđ", this.Amount);
        }

        public Product getRaw()
        {
            return db.Products.Find(this.Id);
        }
    }

}