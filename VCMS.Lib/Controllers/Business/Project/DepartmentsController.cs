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
    public class DepartmentsController : BaseController
    {
        public IEnumerable<Project> GetProjects()
        {
            var result = db.Projects;
            return result;
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewData[ConstantKeys.PROJECTS] = this.GetProjects();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department model)
        {
            var messageCollection = new List<Message>();
            if (ModelState.IsValid)
            {
                db.Departments.Add(model);
                db.SaveChanges();

                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Create successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;

                return RedirectToAction("Edit", new { department_id = model.Id });
            }
            ViewData[ConstantKeys.PROJECTS] = this.GetProjects();
            return View();
        }

        public ActionResult Edit(int department_id)
        {
            ViewData[ConstantKeys.PROJECTS] = this.GetProjects();
            var model = db.Departments.Find(department_id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department model)
        {
            var messageCollection = new List<Message>();
            if (ModelState.IsValid)
            {
                var department = db.Departments.Find(model.Id);
                db.Entry(department).CurrentValues.SetValues(model);
                db.SaveChanges();

                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent= "Update successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;

                return RedirectToAction("Edit", new { department_id  = model.Id});         
            }
            ViewData[ConstantKeys.PROJECTS] = this.GetProjects();
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int[] ids)
        {
            var messageCollection = new List<Message>();
            foreach (var id in ids)
            {
                var model = db.Departments.Find(id);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();

            messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Delete successfully!" });
            TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;
            return RedirectToAction("List");
        }

        #region[Datatables]
        private IQueryable<Department> FilterDbSource(Dictionary<string, string> args)
        {
            var department = db.Departments.AsQueryable();
            if (args != null)
                foreach (var arg in args)
                {

                }
            return department;
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
                    {ConstantKeys.PROJECT, item.Project.Name},
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
