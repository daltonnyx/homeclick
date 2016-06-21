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
    public class HomeController : Controller
    {
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        public ActionResult Index()
        {
            var v = db.Projects.ToList();
            return View();
        }

        //Slider home
        public PartialViewResult Slider()
        {
            return PartialView();
        }


        // Show product feature

        public ActionResult Feature_Product(int? page)
        {
            //    //Số sản phẩm trên trang
            int product_number = 4;
            ////    //tao số biến trong trang
            int pageNumber = (page ?? 1);
            
            IList<Product> product;
            product = db.Products.Where(m => m.featured == true).ToList<Product>();
            if (Request.IsAjaxRequest())
            {
                return View( product.OrderBy(n => n.name).ToPagedList(pageNumber, product_number));
            }
            return View("Feature_Product", product.OrderBy(n => n.name).ToPagedList(pageNumber, product_number));
        }
        //Product Detail
        
        
        //Form Newletter
        public ActionResult Newletter()
        {
            return View();
        }

        // Search formp
        
        public ActionResult Search_form(string search_name)
        {
            var result =db.Products.ToList();
            if (!String.IsNullOrEmpty(search_name))
            {
                result=result.Where(p=>p.name.ToUpper().Contains(search_name.ToUpper())).ToList();
                
            }
            return View(result);
        }
        
        public ActionResult Sidebar()
        {
            ViewBag.catId = RouteData.Values["id"];
            Dictionary<string, IList<Category>> blocks = new Dictionary<string, IList<Category>>();
            var queryModel = from category in db.Categories
                             where category.Category_type.name == "model"
                             select category;
            blocks.Add("models",queryModel.ToList());
            var queryTypology = from category in db.Categories
                                where category.Category_type.name == "typology"
                                select category;
            blocks.Add("typologies", queryTypology.ToList());
            var queryTypes = from category in db.Categories
                             where category.Category_type.name == "vat_lieu"
                             select category;

            blocks.Add("types", queryTypes.ToList());
            return PartialView(blocks);
        }
    }

}