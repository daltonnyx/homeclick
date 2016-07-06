using Homeclick.Models;
using System;
using System.Collections.Generic;
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

            var v = db.Projects.ToList();
            return View(v);
        }

        public ActionResult _Collections(int? layoutId)
        {
            var v = db.ProjectLayout_Collections.Where(o => o.LayoutId == layoutId).ToList();
            return PartialView(v);
        }

        public ActionResult _CollectionDetails(int? collectionId)
        {
            if (collectionId == null)
            {

            }

            var query = string.Format("SELECT * FROM dbo.ProductsLayoutsLink WHERE Layout = '{0}'", collectionId);
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
            var v = l.SingleOrDefault(o => o.Id == collectionId);
       
            return PartialView(v);
        }

        public ActionResult Details(int? ProjectId)
        {

            var layouts = db.ProjectItems.Where(o => o.ProjectId == ProjectId && o.Category.Id == 25).ToList();
            ViewBag.Layouts = layouts;

            var firstLayoutId = layouts.FirstOrDefault().Id;
            ViewBag.firstLayoutId = firstLayoutId;



            var thumbnails = db.ProjectItems.Where(o => o.ProjectId == ProjectId && o.Category.Id == 24).ToList();
            ViewBag.Thumbnails = thumbnails;

            var v = db.Projects.SingleOrDefault(o => o.Id == ProjectId);
            return View(v);
        }

        
    }
}