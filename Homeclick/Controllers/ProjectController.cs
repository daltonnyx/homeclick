using VCMS.Lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.Lib.Common;

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

        public JsonResult CollectionImages(int? collection_id)
        {
            var list = new List<string>();
            if (collection_id != null)
            {
                var collection = db.Posts.Find(collection_id);
                foreach (var item in collection.Post_Details.Where(o => o.Name == "popupImages"))
                {
                    list.Add(item.Value);
                }
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AjaxChildActionOnly]
        public PartialViewResult _RenderRooms(int? floor_id)
        {
            var floor = db.Floors.Find(floor_id);
            var rooms = floor.Rooms;
            return PartialView(rooms);
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