using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homeclick.Models;

namespace Homeclick.Controllers
{
    public class BoSuuTapController : Controller
    {
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        //
        // GET: /Bosutap/
        public ActionResult Index()
        {
            IList<Category> rooms = db.Categories.Where<Category>(c => c.Category_type.name == "collection").ToList();
            ViewBag.slides = rooms.Select<Category, string>(c => c.Category_detail.Where<Category_detail>(d => d.name == "image").First().value);
            return View(rooms);
        }


        public ActionResult Detail(int id)
        {
            Category category = db.Categories.Where(c => c.Id == id).First();
            return View("Detail", category);
        }
        public ActionResult ListCategory()
        {
            List<Category> listcategory = db.Categories.ToList();
            return PartialView(listcategory);
        }
        public ActionResult CategoryProduct(int categoryid)
        {
            List<Product> listproduct;
            if (categoryid == 0)
            {
                 listproduct = db.Products.ToList();
            }
            else
            {
                Category category = db.Categories.Where(n => n.Id == categoryid) as Category;
                 listproduct = db.Products.Where(n => n.Categories.Contains(category)).ToList();

                //Dictionary<string, object> details = arrayItem["Product_detail"] as Dictionary<string, object>;
                //@item.Product_detail.Count();
                
            }
            return View(listproduct);
        }
        public ActionResult Sidebar()
        {
            IList<Category> rooms = db.Categories.Where<Category>(c => c.Category_type.name == "collection").ToList();
            
            return PartialView("Sidebar", rooms);
        }

        public IView listproduct { get; set; }
    }
}