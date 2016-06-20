using Homeclick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homeclick.Controllers
{
    public class DesignController : Controller
    {

        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        //
        // GET: /Design/
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult DesignSlider()
        {
            return PartialView();
        }

        public ActionResult Design()
        {
            return View();
        }

        public ActionResult getDepartments(int? id = null)
        {
            var model = db.Departments.Where<Department>(d => d.ParentDepartmentId == id).ToList<Department>();
            return PartialView("Departments", model);
        }

        public ActionResult getFloors(int? id)
        {
            var model = db.Floors.Where<Floor>(f => f.Department.Id == id).ToList<Floor>();
            return PartialView("Floors", model);
        }
        public ActionResult Floor(int? floor_id)
        {
            floor_id = Convert.ToInt16(Request["floor"].ToString());
            var model = db.Floors.Find(floor_id);
            return View(model);
        }

        public ActionResult Canvas(int? id)
        {
            var categoryTypes = db.Category_type.ToList();

            ViewBag.SelectedTypeId = categoryTypes.FirstOrDefault().Id;
            ViewBag.CategoryTypes = categoryTypes;

            var model = db.Canvas.Find(id);

            return PartialView(model);
        }

        public ActionResult _CategoryOptions(int? CategoryTypeId)
        {
            var categories = db.Categories.Where(o => o.Category_typeId == CategoryTypeId).ToList();
            return PartialView(categories);
        }

	}
}