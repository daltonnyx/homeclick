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
    public class SanPhamController : Controller
    {
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();

        public ActionResult Index(int? mode_id, int? typo_id, int? page)
        {
            //    Số sản phẩm trên trang
            //    tao số biến trong trang
            int pageNumber = (page ?? 1);

            if (mode_id == null)
                mode_id = 1;

            IList<Product> listproduct = Filter(mode_id, typo_id, null, pageNumber);

            ViewBag.type = "all";
            return View(listproduct);
        }

        public ActionResult ListProduct(int? mode_id, int? typo_id, int? page)
        {
            //    Số sản phẩm trên trang
            int product_number = 8;
            //    tao số biến trong trang
            int pageNumber = (page ?? 1);

            if (mode_id == null)
                mode_id = 1;

            IList<Product> listproduct = Filter(mode_id, typo_id,null, pageNumber);

            if (Request.IsAjaxRequest())
            {
                return View(listproduct.OrderBy(n => n.name).ToPagedList(pageNumber, product_number));
            }
            return View("ListProduct", listproduct.OrderBy(n => n.name).ToPagedList(pageNumber, product_number));
        }

        public PartialViewResult SliderSanPham()
        {
            return PartialView();
        }

        public ActionResult ListCategory()
        {


            List<Category> listcategory = db.Categories.ToList();
            return View(listcategory);
        }

        public ActionResult Sidebar(string type = "all")
        {
            /*
            IList<Category> list = new List<Category>();
            switch (type)
            {
                case "all":
                    ViewBag.SideTitle = "Danh mục";
                    list = db.Categories.Where<Category>(c => c.Category_type.typeFor == 0).OrderBy<Category,int>(c => c.Category_type.Id).ToList();
                    break;
                case "model":
                    ViewBag.SideTitle = "Phòng";
                    list = db.Categories.Where<Category>(c => c.Category_type.name == "model" && c.Category_type.typeFor == 0).OrderBy<Category, int>(c => c.Category_type.Id).ToList();
                    break;
                case "typology":
                    ViewBag.SideTitle = "Chủng loại";
                    list = db.Categories.Where<Category>(c => c.Category_type.name == "typology" && c.Category_type.typeFor == 0).OrderBy<Category, int>(c => c.Category_type.Id).ToList();
                    break;
                default:
                    break;
            }
            */
            IList<Category> list = db.Categories.Where<Category>(c => c.Category_type.name == "model").ToList();

            ViewBag.type = type;
            return PartialView(list);
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
                            where product.Product_type.Id == type_id && product.status == 1
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

        public List<Product> Filter(int? model_id = null, int? typo_id = null, int? mate_id = null, int page = 1)
        {
            var products = db.Products.AsQueryable();
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

        public JsonResult ProductsJson(int? model_id = null, int? typo_id = null, int? mate_id = null)
        {
            var list = this.Filter(model_id, typo_id, mate_id);
            var json = new List<object>();
            foreach (var item in list)
            {
                var arrayItem = item.ToArray();
                var details = arrayItem["Product_detail"] as Dictionary<string, object>;

                var materialList = new List<object>();
                var materials = ModelHelper.GetProductCategories(CategoryTypes.Material, item.Id);

                var typo = ModelHelper.GetProductCategories(CategoryTypes.Typology, item.Id).FirstOrDefault();

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

        [HttpPost]
        public ActionResult FilterProduct(int page = 1)
        {
            string[] models = Request.Form.GetValues("Categories[Models][]");
            string[] typologies = Request.Form.GetValues("Categories[Typologies][]");
            var products = db.Products.AsQueryable();
            if(models != null && models.Count<string>() > 0)
            products = from product in products
                           where product.Categories.Where(c => models.Contains<string>(c.Id.ToString())).Count<Category>() > 0
                           select product;
            if(typologies != null && typologies.Count<string>() > 0)
            products = from product in products
                       where product.Categories.Where(c => typologies.Contains<string>(c.Id.ToString())).Count<Category>() > 0
                       select product;
            ViewBag.Title = "Sản phẩm";
            return PartialView(products.OrderBy<Product, string>(c => c.name).ToPagedList(page, 6));
        }

        public ViewResult Product_Detail(int? id)
        {
            var product = db.Products.Find(id);
            ViewBag.Title = product.name;
            ViewBag.url = "http://demo.vinabits.com.vn/homeclick/admin";
            return View(product);
        }

        public PartialViewResult AjaxProductDetail(int id)
        {
            var product = db.Products.Find(id);
            return PartialView(product);
        }
	}
}