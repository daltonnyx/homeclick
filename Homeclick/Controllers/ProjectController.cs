using Homeclick.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homeclick.Controllers
{
    public class ProjectController : BaseController
    {
        public override CategoryTypes CategoryType
        {
            get
            {
                return CategoryTypes.ProjectType;
            }
        }

        public ActionResult Index(int? category_id)
        {
            var viewModel = db.Projects.Where(o => o.LockedOut == false);
            return View(viewModel);
        }

        public ActionResult Category(int category_id)
        {
            return View();
        }

        public ActionResult Map()
        {
            ViewBag.MetaKeyword = "homeclick";
            ViewBag.MetaDescription = "project";
            var types = db.Categories.Where(o => o.ParentCategoryId == 28);
            var cities = db.Cities.ToList();

            ViewBag.ProjectTypes = types;
            ViewBag.Cities = cities;

            var categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.ProjectType);
            ViewBag.Categories = categories;

            return View();
        }

        public ActionResult _CollectionDetails(int collection_id)
        {
            var query = string.Format("SELECT * FROM dbo.ProjectLayout_Collection_Product_Link WHERE ParentId = '{0}'", collection_id);
            var tableItems = db.Database.SqlQuery<ProjectLayout_Collection_Product_Link>(query).ToList();
            
            var products = new List<Product>();

            foreach (var product in tableItems)
            {
                var temp_p = db.Products.Find(product.ChildId);
                var temp_t = temp_p.ToArray();
                var detail = temp_t["Product_detail"] as Dictionary<string, object>;
                
                products.Add(temp_p);

                product.ProductName = temp_p.name;
                product.ProductValue = Convert.ToInt32(detail["gia"]);
                product.TotalValue = product.Quantity * product.ProductValue;
            }

            ViewBag.Products = products;
            ViewBag.TableItems = tableItems;

            var l = db.ProjectLayout_Collections.ToList();
            var v = l.SingleOrDefault(o => o.Id == collection_id);
       
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