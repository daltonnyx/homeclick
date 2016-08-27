using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Homeclick.Models
{
    public partial class Category
    {

        vinabits_homeclickEntities db = new vinabits_homeclickEntities();     

        public static IList<Category_detail> detail(Category category)
        {
            PropertyInfo[] typeInfos = (typeof(Product)).GetProperties();
            IList<Category_detail> child = new List<Category_detail>();
            foreach (PropertyInfo member in typeInfos)
            {
                if (member.CanRead && member.Name == "Category_detail")
                {
                    Dictionary<string, object> details = new Dictionary<string, object>();
                    ICollection<Category_detail> detailList = member.GetValue(category) as ICollection<Category_detail>;
                    child = detailList.ToList<Category_detail>();
                    break;
                }
            }
            return child;
        }

        public Category_detail getDetail(string name)
        {
            return null;
        }

        public object getDetailValue(string name)
        {
            IList<Category_detail> details = this.Category_detail.ToList<Category_detail>();
            if (details.Count == 0)
                return string.Empty;
            Category_detail detail = details.Where<Category_detail>(d => d.name == name).FirstOrDefault();
            if (detail == null)
                return string.Empty;
            return detail.value;
        }

        public IList<Product> getProducts()
        {
            return this.Products.ToList();
        }

        public IList<T> getItems<T>() where T: class
        {
            var posts = ModelHelper.GetObjectListByCategory<T>(this.Id);
            return posts;
        }

        public IList<Category> getDescendantCategories()
        {
            IList<Category> descendant = new List<Category>();
            string descendantType = "material";
            if(this.Category_type.name == "model")
            {
                descendantType = "typology";
            }


            descendant = (from cat in db.Categories
                          where (
                            from product in cat.Products
                            where (from cat2 in product.Categories
                                   where cat2.Id == this.Id
                                   select cat2).Count<Category>() > 0
                            select product).Count<Product>() > 0 && cat.Category_type.name == descendantType
                          select cat).ToList<Category>();
            return descendant;
        }

        public IList<Category> getDescendantCategories(CategoryTypes categoryType)
        {
            IList<Category> descendant = new List<Category>();
            string descendantType = categoryType.ToString().ToLower();

            descendant = (from cat in db.Categories
                          where (
                            from product in cat.Products
                            where (from cat2 in product.Categories
                                   where cat2.Id == this.Id
                                   select cat2).Count<Category>() > 0
                            select product).Count<Product>() > 0 && cat.Category_type.name == descendantType
                          select cat).ToList<Category>();
            return descendant;
        }

        public IList<Category> GetCategoryChilds()
        {
            var list = ModelHelper.getChildren<Category>(typeof(Category).Name, typeof(Category).Name,this.Id).OrderBy(o => o.name).ToList();
            var sortedList = list.OrderBy(o => o.Order).ToList();
            return sortedList;
        }


        public string getActionLink()
        {
            string type = "Typology";
            if (this.Category_type.name == "model")
                type = "Model";
            else if (this.Category_type.name == "collection")
                return String.Format("/BoSuuTap/Detail/{0}", this.Id);
            return String.Format("/Category/{1}/{0}",this.Id, type);
        }
    }
}