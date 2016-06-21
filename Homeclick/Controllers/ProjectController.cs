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
            var l = db.ProjectLayout_Collections.ToList();
            var v = l.SingleOrDefault(o => o.Id == collectionId);

            var productsTable = new List<Models.Project_ProductsTable>();
            
            return PartialView(v);
        }

        public ActionResult Details(int? ProjectId)
        {
            
            var l = db.Projects.ToList();
            var v = l.SingleOrDefault(o => o.Id == ProjectId);

            var layouts = db.ProjectItems.Where(o => o.ProjectId == ProjectId).ToList();
            ViewBag.Layouts = layouts;

            var firstLayoutId = layouts.FirstOrDefault().Id;
            ViewBag.firstLayoutId = firstLayoutId;

            return View(v);
        }

        
    }
}