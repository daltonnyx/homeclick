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
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Details(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
                return HttpNotFound();

            return View(project);
        }

        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProjectType); ;
            ViewBag.Cities = db.Cities.Where(o => o.Status);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project model)
        {
            if (ModelState.IsValid)
            {
                foreach (var fileId in model.FileIds ?? new string[] { })
                {
                    var file = db.Files.Find(fileId);
                    if (file != null)
                        model.Files.Add(file);
                }
                db.Projects.Add(model);
                db.SaveChanges();
                return RedirectToAction("Details", new { projectId = model.Id});
            }
            ViewBag.Categories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProjectType); ;
            ViewBag.Cities = db.Cities.Where(o => o.Status);
            return View(model);
        }

        public ActionResult Edit(int project_id)
        {
            var project = db.Projects.Find(project_id);
            if (project == null)
                return HttpNotFound();
            ViewBag.Categories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProjectType); ;
            ViewBag.Cities = db.Cities.Where(o => o.Status);
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project model)
        {
            if (ModelState.IsValid)
            {
                var project = db.Projects.Find(model.Id);
                if (project == null)
                    return HttpNotFound();
                db.Entry(project).CurrentValues.SetValues(model);

                project.Files.Clear();
                foreach (var fileId in model.FileIds ?? new string[] { })
                {
                    var file = db.Files.Find(fileId);
                    if (file != null)
                        model.Files.Add(file);
                }
                db.Entry(project).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { projectId = model.Id });
            }
            ViewBag.Categories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProjectType); ;
            ViewBag.Cities = db.Cities.Where(o => o.Status);
            return View(model);
        }

        [HttpDelete]
        public ActionResult Delete(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
                return HttpNotFound();

            if (project.PreviewImage != null)
            {
                if (!Helper.HasAnyRelation(project.PreviewImage))
                {
                    Uploader.DeleteFile(project.PreviewImage, this);
                    db.Files.Remove(project.PreviewImage);
                }
            }

            foreach (var file in project.Files)
            {
                if (!Helper.HasAnyRelation(file))
                {
                    Uploader.DeleteFile(file, this);
                    db.Files.Remove(file);
                }
            }

            db.Projects.Remove(project);
            db.SaveChanges();
            return View("List");
        }

        [HttpPost]
        public JsonResult DataHandler(DTParameters param, Dictionary<string, string> args)
        {
            try
            {
                var projects = db.Projects.ToList();
                if (args != null)
                    foreach (var arg in args)
                    {
                        int n;
                        if (int.TryParse(arg.Value, out n))
                        {
                            if (arg.Key == "category")
                                projects = projects.Where(o => o.Id == n).ToList();
                        }
                    }

                var dtsource = projects.Select(o => new dt_project {
                    id = o.Id, name = o.Name,
                    img = o.PreviewImage != null ? o.PreviewImage.Id + o.PreviewImage.Extension : null,
                    status = o.Status});

                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<dt_project, bool>> pre = (p => (search == null || (p.name != null && p.name.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.name != null && p.name.ToLower().Contains(columnSearch[1].ToLower()))));

                var resultSet = new ResultSet();
                List<dt_project> data = resultSet.GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<dt_project> result = new DTResult<dt_project>
                {
                    draw = param.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
