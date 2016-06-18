using Homeclick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Collections;
using System.Reflection;

namespace Homeclick.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListOfRooms()
        {
            IList<Category> rooms = db.Categories.Where<Category>(c => c.Category_type.name == "model").ToList();
            return PartialView("ListOfRooms", rooms);
        }

        public ActionResult Models()
        {

            IList<Category> rooms = db.Categories.Where<Category>(c => c.Category_type.name == "model").ToList();
            ViewBag.Title = "Phòng";
            return View(rooms);
        }

        public ActionResult Filter(int? model_id = null, int? typo_id = null, int? mate_id = null,int page = 1)
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
            if(typo_id != null)
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

            var model = products.OrderBy<Product, string>(p => p.name).ToPagedList<Product>(page, 20);
            return View(model);
        }

        public ActionResult Model(int id, int page = 1)
        {
            Category model = db.Categories.Find(id);
            IList<Product> query = model.Products.ToList<Product>();
            ViewBag.title = model.name;
            var products = query.OrderBy<Product, string>(p => p.name).ToPagedList<Product>(page, 20);
            return View(products);
        }

        public ActionResult Typologies()
        {
            IList<Category> typologies = db.Categories.Where<Category>(t => t.Category_type.name == "typology").ToList();
            return View(typologies);
        }

        public ActionResult Typology(int id, int page = 1)
        {
            Category cat = db.Categories.Find(id);
            ViewBag.Title = cat.name;
            var query = from product in db.Products
                        where product.Categories.Where<Category>(c => c.Id == id).Count<Category>() >= 1
                        select product;
            var products = query.OrderBy<Product, string>(p => p.name).ToPagedList<Product>(page, 20);
            return View(products);
        }
    }

}