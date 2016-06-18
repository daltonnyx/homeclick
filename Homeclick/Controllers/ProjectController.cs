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
            return View();
        }

        public ActionResult Index(int id)
        {
            return View("ProjectIndex");
        }

        public ActionResult Sidebar()
        {
            return View();
        }

        public ActionResult Detail(int project_id, int detail_id)
        {
            return View();
        }
    }
}