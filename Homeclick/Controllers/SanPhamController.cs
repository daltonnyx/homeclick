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
        //
        // GET: /SanPham/
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        //
        // GET: /Bosutap/

        public ActionResult Index()
        {
            ViewBag.type = "all";
            return View();
        }

        public ActionResult ListProduct(int? page)
        {
            //    //Số sản phẩm trên trang
            int product_number = 8;
            ////    //tao số biến trong trang
            int pageNumber = (page ?? 1);
            IList<Product> listproduct;
            listproduct = db.Products.ToList();
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

        public ActionResult CategoryProduct(int categoryid)
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