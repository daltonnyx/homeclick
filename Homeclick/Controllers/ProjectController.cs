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
            var v = db.ProjectLayout_Collections.ToList();
            return View();
        }

        public ActionResult _Collections(int? collectionId)
        {
            return PartialView();
        }

        public ActionResult _CollectionDetails(int? collectionId)
        {
            return PartialView();
        }

        public ActionResult Details(int? ProjectId)
        {

            return View();
        }

        public ActionResult Detail(int project_id, int detail_id)
        {
            return View();
        }
    }
}