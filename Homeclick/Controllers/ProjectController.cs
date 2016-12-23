using VCMS.Lib.Models;
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
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult _Sidebar()
        {
            var categories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProjectType);
            ViewData["cities"] = db.Cities;
            ViewData["states"] = db.Districts;

            return PartialView(categories);
        }

        public ActionResult Index(int? category_id)
        {
            var viewModel = db.Projects.Where(o => o.Status);
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
            var types = db.Categories.Where(o => o.CategoryParentId == 28);
            var cities = db.Cities.ToList();

            ViewBag.ProjectTypes = types;
            ViewBag.Cities = cities;

            var categories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProjectType);
            ViewBag.Categories = categories;

            return View();
        }
        /*
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
        */
        public ActionResult Details(int? category_id, int? project_id)
        {
            var project = db.Projects.Find(project_id);
            return View(project);
        }

        public ActionResult Collection(int? collection_id)
        {
            var collection = db.Posts.Find(collection_id);
            if (collection != null)
                return PartialView(collection);
            return HttpNotFound();
        }

        [HttpPost]
        public JsonResult GetCollections(int room_id)
        {
            var room = db.Rooms.Find(room_id);
            var result = new List<object>();
            foreach (var collection in room.Collections)
            {
                result.Add(new { id = collection.Id, name = collection.Title, image = collection.ImageFile.FullFileName });
            }
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetProjectsData(int? category_id)
        {
            IEnumerable<Project> projects;

            projects = category_id == null || category_id == -1 ? db.Projects.ToList() :
                                db.Projects.Where(o => o.CategoryId == category_id).ToList();

            var json = new List<object>();
            foreach (var project in projects)
            {
                json.Add(new
                {
                    id = project.Id,
                    name = project.Name,
                    image = project.PreviewImage.FullFileName,
                    address = project.Address,
                    state = project.DistrictId,

                    statu = Convert.ToInt32(project.Status),
                    type = project.CategoryId,
                });
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}