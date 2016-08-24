using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Homeclick.Models
{
    public partial class Product
    {

        public Dictionary<string, object> ToArray()
        {

            PropertyInfo[] typeInfos = (typeof(Product)).GetProperties();
            Dictionary<string, object> child = new Dictionary<string, object>();
            foreach (PropertyInfo member in typeInfos)
            {
                if (member.CanRead && member.Name == "Product_detail")
                {
                    Dictionary<string, object> details = new Dictionary<string, object>();
                    ICollection<Product_detail> detailList = member.GetValue(this) as ICollection<Product_detail>;
                    foreach (Product_detail detail in detailList)
                    {
                        details.Add(detail.name, detail.value);

                    }
                    child.Add(member.Name, details);
                }

                else if (member.CanRead)
                {
                    child.Add(member.Name, member.GetValue(this));
                }
            }
            return child;

        }

        public Dictionary<string, object> ToArray(Product products)
        {

            PropertyInfo[] typeInfos = (typeof(Product)).GetProperties();
                Dictionary<string, object> child = new Dictionary<string, object>();
                foreach (PropertyInfo member in typeInfos)
                {
                    if (member.CanRead && member.Name == "Product_detail")
                    {
                        Dictionary<string, object> details = new Dictionary<string, object>();
                        ICollection<Product_detail> detailList = member.GetValue(products) as ICollection<Product_detail>;
                        foreach (Product_detail detail in detailList)
                        {
                            details.Add(detail.name, detail.value);

                        }
                        child.Add(member.Name, details);
                    }

                    //else if (member.CanRead)
                    //{
                    //    child.Add(member.Name, member.GetValue(p));
                    //}
                }
                return child;

        }

        public Dictionary<string, object> ToArray(Category products)
        {

            PropertyInfo[] typeInfos = (typeof(Category)).GetProperties();
            Dictionary<string, object> child = new Dictionary<string, object>();
            foreach (PropertyInfo member in typeInfos)
            {
                if (member.CanRead && member.Name == "Category_detail")
                {
                    Dictionary<string, object> details = new Dictionary<string, object>();
                    ICollection<Category_detail> detailList = member.GetValue(products) as ICollection<Category_detail>;
                    foreach (Category_detail detail in detailList)
                    {
                        details.Add(detail.name, detail.value);

                    }
                    child.Add(member.Name, details);
                }

                if (member.CanRead && member.Name == "Category_type")
                {
                    Dictionary<string, object> details = new Dictionary<string, object>();
                    ICollection<Category_type> detailList = member.GetValue(products) as ICollection<Category_type>;
                    foreach (Category_type detail in detailList)
                    {
                        details.Add(detail.name, detail.caption);

                    }
                    child.Add(member.Name, details);
                }

            }
            return child;

        }

        [NotMapped]
        public ICollection<Category> Categories
        {
            get
            {
                var categories = ModelHelper.getParents<Category>("Category", "Product", this.Id);
                return categories;
            }
       }

    }
}