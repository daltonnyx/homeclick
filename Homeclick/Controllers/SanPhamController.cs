using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homeclick.Models;
using PagedList;
using PagedList.Mvc;
using System.Collections;
using System.Reflection;

namespace Homeclick.Controllers
{
    public class SanPhamController : BaseController
    {
        public override CategoryTypes CategoryType { get { return CategoryTypes.Model; } }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Category(int category_id)
        {
            return View();
        }

        public ActionResult ListCategory()
        {
            List<Category> listcategory = db.Categories.ToList();
            return View(listcategory);
        }

        public ActionResult CanvasList(int? cat_id, int? type_id)
        {
            IList<Product> products = (from product in db.Products
                                       where product.status == 1
                                      select product).ToList<Product>();

            if(cat_id != null)
            {
                products = db.Categories.Find(cat_id).Products.Where<Product>(p => p.status == 1).ToList<Product>();
            }
            if(type_id != null)
            {
                products = (from product in products
                            where product.status == 1
                            select product).ToList<Product>();
            }
            return PartialView(products);
        }

        public ActionResult CategoryProduct(int? categoryid)
        {
            List<Product> listproduct;
            if (categoryid == null)
            {
                listproduct = db.Products.ToList();
            }
            else
            {
                Category category = db.Categories.Where(n => n.Id == categoryid) as Category;
                listproduct = db.Products.Where(n => n.Categories.Contains(category)).ToList();
            }
            return View(listproduct);
        }

        public List<Product> Filter(int? model_id = null, int? typo_id = null, int? mate_id = null)
        {
            var temp = db.Products.ToList();
            var products = temp.AsQueryable();
            if (model_id != null)
            {
                products = from p in products
                           where (from c in p.Categories
                                  where c.Id == model_id
                                  select c).Count() > 0
                           select p;
            }
            if (typo_id != null && typo_id > 0)
            {
                products = from p in products
                           where (from c in p.Categories
                                  where c.Id == typo_id
                                  select c).Count() > 0
                           select p;
            }
            if (mate_id != null)
            {
                products = from p in products
                           where (from c in p.Categories
                                  where c.Id == mate_id
                                  select c).Count() > 0
                           select p;
            }

            var list = products.OrderBy<Product, string>(p => p.name).ToList();
            return list;
        }

        public JsonResult ProductsJson(int? category_id = null, int? typo_id = null, int? mate_id = null)
        {
            var list = this.Filter(category_id, typo_id, mate_id);
            var json = new List<object>();
            foreach (var item in list)
            {
                var arrayItem = item.ToArray();
                var details = arrayItem["Product_detail"] as Dictionary<string, object>;

                var materialList = new List<object>();
                var materials = item.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.Material);

                var typo = item.Categories.FirstOrDefault(o => o.Category_typeId == (int)CategoryTypes.Typology);

                foreach (var material in materials)
                {
                    materialList.Add(new
                    {
                        id = material.Id
                    });
                }

                json.Add(new
                {
                    id = item.Id,
                    name = item.name,
                    image = item.image,
                    value = Convert.ToInt32(details["gia"]),
                    materials = materialList,
                    typo = typo.Id
                });
            }
            return Json(json, JsonRequestBehavior.AllowGet);     
        }

        /// <summary>
        /// taking all of the material relating to the category
        /// </summary>
        /// <param name="category_id">Id of the category</param>
        /// <param name="model_id">If 'category_id' is -1 and 'model_id' has been set, this will getting materials contained in the model</param>
        /// <returns></returns>
        public JsonResult GetMetarialsJson(int category_id, int? model_id)
        {
            IList<Category> metarials;
            var json = new List<object>();
            var model = db.Categories.Find((category_id == -1 && model_id != null) ? model_id : category_id);

            if (model != null)
            {
                metarials = model.getDescendantCategories(CategoryTypes.Material);
                foreach (var metarial in metarials)
                {
                    json.Add(new
                    {
                        id = metarial.Id,
                        name = metarial.name
                    });
                }
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Product_Detail(int? id)
        {
            var product = db.Products.Find(id);
            ViewBag.Title = product.name;
            return View(product);
        }

        public PartialViewResult AjaxProductDetail(int id)
        {
            var product = db.Products.Find(id);
            return PartialView(product);
        }
	}
}