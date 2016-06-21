using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homeclick.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Index()
        {
            ViewBag.MetaKeyword = "homeclick";
            ViewBag.MetaDescription = "project";
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

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Detail(int project_id, int detail_id)
        {
            return View();
        }
    }
}