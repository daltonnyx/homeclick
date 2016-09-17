using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Business;

namespace Homeclick.Controllers
{
    public class SanPhamController : Controller
    {
        public  CategoryTypes CategoryType { get { return CategoryTypes.Model; } }

        public ApplicationDbContext db = new ApplicationDbContext();

        public virtual ActionResult _Sidebar()
        {
            var categories = db.Categories.Where(c => c.Category_TypeId == (int)CategoryType).ToList();
            return PartialView(categories);
        }

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
                                       where product.Status == true
                                      select product).ToList<Product>();

            if(cat_id != null)
            {
                products = db.Categories.Find(cat_id).Products.Where<Product>(p => p.Status == true).ToList<Product>();
            }
            if(type_id != null)
            {
                products = (from product in products
                            where product.Status == true
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
                var tProducts = new List<Product>();
                foreach (var product in products)
                {
                    foreach (var material in product.Materials)
                    {
                        var found = false;
                        foreach (var category in material.Categories)
                        {
                            if (category.Id == mate_id)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            tProducts.Add(product);
                            break;
                        }
                    }
                }
                products = products.Intersect(tProducts);
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
                var details = item.DetailsToDictionary();
                var typo = item.Categories.FirstOrDefault(o => o.Category_TypeId == (int)CategoryTypes.Typology);

                var materialList = new List<object>();
                var tList = new List<Product>();
                tList.Add(item);
                var materials = GetCMaterialOfProducts(tList);

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
                    image = item.Image.FullFileName,
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
        /// <param name="typo_id">Id of the category</param>
        /// <param name="model_id">If 'category_id' is -1 and 'model_id' has been set, this will getting materials contained in the model</param>
        /// <returns></returns>       
        public JsonResult GetMetarialsJson(int? model_id, int? typo_id)
        {
            var resuilt = new List<object>();
            var products = this.Filter(model_id, typo_id);
            var cMaterials = GetCMaterialOfProducts(products);

            foreach (var cMaterial in cMaterials)
            {
                resuilt.Add(new
                {
                    id = cMaterial.Id,
                    name = cMaterial.Name
                });
            }
            return Json(resuilt, JsonRequestBehavior.AllowGet);
        }

        private List<Category> GetCMaterialOfProducts(IEnumerable<Product> products)
        {
            var pMaterials = new List<Product_Variant>();
            var cMaterials = new List<Category>();
            foreach (var product in products)
            {
                pMaterials = pMaterials
                    .Concat(product.Materials.ToList())
                    .Distinct()
                    .ToList();
            }
            foreach (var pMaterial in pMaterials)
            {
                cMaterials = cMaterials
                    .Concat(pMaterial.Categories.Where(o => o.Category_TypeId == (int)CategoryTypes.Material))
                    .Distinct()
                    .ToList();
            }
            return cMaterials;
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

        public ActionResult GetImages(int? variantId, int? productId)
        {
            var result = new List<object>();
            if (variantId != null && productId != null)
            {
                var product = db.Products.Find(productId);
                if (product != null)
                {
                    var color = db.Product_Variants.Find(variantId);
                    if (color != null)
                    {
                        var commonItem = product.Files.Intersect(color.Files);
                        foreach (var item in commonItem)
                        {
                            result.Add(item.FullFileName);
                        }
                    }
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}