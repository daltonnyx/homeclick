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
    public class FloorsController : BaseController
    {
        public IEnumerable<Project> GetProjects()
        {
            var result = db.Projects;
            return result;
        }

        public IEnumerable<Department> GetDepartments()
        {
            var result = db.Departments;
            return result;
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewData[ConstantKeys.PROJECTS] = this.GetProjects();
            ViewData[ConstantKeys.DEPARTMENTS] = this.GetDepartments();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Floor model)
        {
            var messageCollection = new List<Message>();
            if (ModelState.IsValid)
            {
                db.Floors.Add(model);
                db.SaveChanges();

                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Create successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;

                return RedirectToAction("Edit", new { floor_id = model.Id });
            }
            ViewData[ConstantKeys.PROJECTS] = this.GetProjects();
            ViewData[ConstantKeys.DEPARTMENTS] = this.GetDepartments();
            return View();
        }

        public ActionResult Edit(int floor_id)
        {
            ViewData[ConstantKeys.PROJECTS] = this.GetProjects();
            ViewData[ConstantKeys.DEPARTMENTS] = this.GetDepartments();
            var model = db.Floors.Find(floor_id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Floor model)
        {
            var messageCollection = new List<Message>();
            if (ModelState.IsValid)
            {
                var modelTarget = db.Floors.Find(model.Id);
                db.Entry(modelTarget).CurrentValues.SetValues(model);
                db.SaveChanges();

                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent= "Update successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;

                return RedirectToAction("Edit", new { floor_id = model.Id});         
            }
            ViewData[ConstantKeys.PROJECTS] = this.GetProjects();
            ViewData[ConstantKeys.DEPARTMENTS] = this.GetDepartments();
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int[] ids)
        {
            var messageCollection = new List<Message>();
            foreach (var id in ids)
            {
                var model = db.Floors.Find(id);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();

            messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Delete successfully!" });
            TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;
            return RedirectToAction("List");
        }

        #region[Datatables]
        private IQueryable<Floor> FilterDbSource(Dictionary<string, string> args)
        {
            var result = db.Floors.AsQueryable();
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
                    {ConstantKeys.DEPARTMENT, item.Department.Name},
                    {ConstantKeys.PROJECT, item.Department.Project.Name }
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
