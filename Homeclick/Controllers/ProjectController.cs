using Homeclick.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homeclick.Controllers
{
    public class ProjectController : Controller
    {
        Homeclick.Models.vinabits_homeclickEntities db = new Models.vinabits_homeclickEntities();
        // GET: Project
        public ActionResult Index()
        {
            ViewBag.MetaKeyword = "homeclick";
            ViewBag.MetaDescription = "project";
            var types = db.Categories.Where(o => o.parent_id == 28);
            var cities = db.Cities.ToList();

            ViewBag.ProjectTypes = types;
            ViewBag.Cities = cities;
            var categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.ProjectType);
            ViewBag.Categories = categories;
            var v = db.Projects.ToList();
            return View(v);
        }

        public ActionResult Category(int category_id)
        {
            ViewBag.MetaKeyword = "homeclick";
            ViewBag.MetaDescription = "project";

            var types = db.Categories.Where(o => o.parent_id == 28);
            var cities = db.Cities.ToList();

            ViewBag.ProjectTypes = types;
            ViewBag.Cities = cities;

            var categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.ProjectType);
            ViewBag.Categories = categories;

            var v = db.Projects.Where(o => o.CategoryId == category_id).ToList();
            return View(v);
        }

        public ActionResult Map()
        {
            ViewBag.MetaKeyword = "homeclick";
            ViewBag.MetaDescription = "project";
            var types = db.Categories.Where(o => o.parent_id == 28);
            var cities = db.Cities.ToList();

            ViewBag.ProjectTypes = types;
            ViewBag.Cities = cities;

            var categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.ProjectType);
            ViewBag.Categories = categories;
            return View();
        }

        

        public ActionResult _CollectionDetails(int collection_id)
        {
            if (collection_id == null)
            {

            }

            var query = string.Format("SELECT * FROM dbo.ProductsLayoutsLink WHERE Layout = '{0}'", collection_id);
            var tableItems = db.Database.SqlQuery<ProductsLayoutsLink>(query).ToList();

            var products = new List<Product>();

            foreach (var product in tableItems)
            {
                var temp_p = db.Products.Find(product.Product);
                var temp_t = temp_p.ToArray();
                var detail = temp_t["Product_detail"] as Dictionary<string, object>;
                
                products.Add(temp_p);

                product.ProductName = temp_p.name;
                product.ProductValue = Convert.ToInt32(detail["gia"]);
                product.TotalValue = product.Total * product.ProductValue;
            }

            ViewBag.Products = products;
            ViewBag.TableItems = tableItems;

            var l = db.ProjectLayout_Collections.ToList();
            var v = l.SingleOrDefault(o => o.Id == collection_id);
       
            return PartialView(v);
        }

        public ActionResult _Projects (int? typeId, string text)
        {
            var list = db.Projects.Where(o => o.LockedOut == true).ToList();

            var v = typeId != null ? db.Projects.Where(o => o.CategoryId == typeId).ToList() : db.Projects.ToList();

            if (text != null)
            {
                var b = new List<Project>();
                foreach (var item in v)
                {
                    if (item.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                        b.Add(item);
                }
                v = b;
            }
            ViewBag.SearchText = text;
            return PartialView(v);
        }

        public ActionResult Details(int category_id, int project_id)
        {
            var layouts = db.ProjectItems.Where(o => o.ProjectId == project_id && o.Category.Id == 25).ToList();
            ViewBag.Layouts = layouts;

            var firstLayoutId = layouts.FirstOrDefault().Id;
            ViewBag.firstLayoutId = firstLayoutId;


            var thumbnails = db.ProjectItems.Where(o => o.ProjectId == project_id && o.Category.Id == 24).ToList();
            ViewBag.Thumbnails = thumbnails;

            var project = db.Projects.ToList();
            var v = project.SingleOrDefault(o => o.Id == project_id);
            
            return View(v);
        }
    }
}