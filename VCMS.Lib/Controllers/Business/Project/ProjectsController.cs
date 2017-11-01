using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Common;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Datatables;

namespace VCMS.Lib.Controllers
{
    public class ProjectsController : BaseController
    {
        private IEnumerable<City> GetCities()
        {
            var result = db.Cities.Where(o => o.Status);
            return result;
        }

        private IEnumerable<District> GetDistricts()
        {
            var result = db.Districts;
            return result;
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProjectType); ;
            ViewData[ConstantKeys.CITIES] = GetCities();
            ViewData[ConstantKeys.DISTRICTS] = GetDistricts();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project model)
        {
            var messageCollection = new List<Message>();
            if (ModelState.IsValid)
            {
                db.Projects.Add(model);
                db.SaveChanges();

                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Create successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;
                return RedirectToAction("Edit", new { project_id = model.Id });
            }
            ViewBag.Categories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProjectType); ;
            ViewData[ConstantKeys.CITIES] = GetCities();
            ViewData[ConstantKeys.DISTRICTS] = GetDistricts();
            return View(model);
        }

        public ActionResult Edit(int project_id)
        {
            var project = db.Projects.Find(project_id);
            if (project == null)
                return HttpNotFound();
            ViewBag.Categories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProjectType); ;
            ViewData[ConstantKeys.CITIES] = GetCities();
            ViewData[ConstantKeys.DISTRICTS] = GetDistricts();
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project model)
        {
            
            var messageCollection = new List<Message>();
            if (ModelState.IsValid)
            {
                var modelTarget = db.Projects.Find(model.Id);
                db.Entry(modelTarget).CurrentValues.SetValues(model);
                db.SaveChanges();

                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Update successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;
                return RedirectToAction("Edit", new { project_id = model.Id });
            }

            ViewBag.Categories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProjectType); ;
            ViewData[ConstantKeys.CITIES] = GetCities();
            ViewData[ConstantKeys.DISTRICTS] = GetDistricts();
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int[] ids)
        {
            var messageCollection = new List<Message>();
            foreach (var id in ids)
            {
                var model = db.Projects.Find(id);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();

            messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Delete successfully!" });
            TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;
            return RedirectToAction("List");
        }

        #region[Datatables]
        private IQueryable<Project> FilterDbSource(Dictionary<string, string> args)
        {
            var result = db.Projects.AsQueryable();
            if (args != null)
                foreach (var arg in args)
                {

                }
            return result;
        }

        private DTResult<T> GetDtResult<T>(DTParameters param, Dictionary<string, string> args) where T : Dictionary<string, object>, new()
        {
            var dtsource = new List<T>();
            var dbSource = FilterDbSource(args);

            foreach (var item in dbSource)
            {
                dtsource.Add(new T
                {
                    {ConstantKeys.ID, item.Id },
                    {ConstantKeys.NAME, item.Name },
                });
            }
            return JDatatables<T>.GetDTResult(param, dtsource);
        }

        public JsonResult DataHandler(DTParameters param, Dictionary<string, string> args)
        {
            try
            {
                var result = GetDtResult<Dictionary<string, object>>(param, args);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        #endregion
    }
}
