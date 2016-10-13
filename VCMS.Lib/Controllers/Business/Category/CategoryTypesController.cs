using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Datatables;

namespace VCMS.Lib.Controllers
{
    public class CategoryTypesController : CategoriesController
    {
        public override ActionResult List()
        {
            var types = db.Category_types.ToList();
            var viewModels = types.Select(o => new CategoryTypeViewModel
            {
                Id = o.Id,
                Name = o.Name,
                Count = o.Categories.Count
            });

            return View(viewModels);
        }

        public ActionResult CreateType(bool? success, string susccessObjName = "")
        {
            if (success == true)
            {
                ViewData["Success"] = true;
                ViewData["SuccessObjName"] = susccessObjName;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateType(CategoryTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = new Category_Type
                {
                    Name = viewModel.Name,
                    Description = viewModel.Desciption,
                    CreateUserId = User.Identity.GetUserId(),
                    CreateTime = DateTime.Now
                };
                db.Category_types.Add(model);
                db.SaveChanges();
                return RedirectToAction("CreateType", new { success = true, susccessObjName = model.Name});
            }
            return View(viewModel);
        }

        public ActionResult EditType(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var type = db.Category_types.Find(id);
            if (type == null)
                return HttpNotFound();

            var viewModel = new CategoryTypeViewModel
            {
                Name = type.Name,
                Desciption = type.Description,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditType(CategoryTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var type = db.Category_types.Find(viewModel.Id);
                if (type != null)
                {
                    type.Name = viewModel.Name;
                    type.Description = viewModel.Desciption;
                    db.Entry(type).State = System.Data.Entity.EntityState.Modified;
                    var result = db.SaveChanges();
                    if (result > 0)
                        return RedirectToAction("List");
                }
            }
            return View(viewModel);
        }

        /// <summary>
        /// Delete a category with given id.
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns></returns>
        public ActionResult DeleteType(int? id)
        {
            var result = "";
            var type = db.Category_types.Find(id);
            if (type != null)
                try
                {
                    db.Category_types.Remove(type);
                    db.SaveChanges();
                    result = "Success";
                }
                catch (Exception ex)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Delete failure!");
                    sb.AppendLine("Error code: " + ((ex.InnerException.InnerException) as SqlException).Number);
                    result = sb.ToString();
                }
            else
                result = "Incorrect ID!";

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create category for type.
        /// </summary>
        /// <param name="id">Target category-type Id</param>
        /// <param name="success">After successfully added</param>
        /// <param name="susccessObjName">Name of object added</param>
        /// <returns></returns>
        public override ActionResult CreateCategory(int? id, bool? success, string susccessObjName = "")
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var type = db.Category_types.Find(id);
            if (type == null)
                return HttpNotFound();

            if (success == true)
            {
                ViewData["Success"] = true;
                ViewData["AddedCategoryName"] = susccessObjName;
            }
            return View(new CategoryViewModel {TypeId = type.Id, TypeName = type.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult CreateCategory(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var type = db.Category_types.Find(viewModel.TypeId);
                if (type != null)
                {
                    var category = new Category
                    {
                        Name = viewModel.Name,
                        Description = viewModel.Desciption,
                        CreateUserId = User.Identity.GetUserId(),
                        CreateTime = DateTime.Now
                    };
                    category.CategoryType = type;
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("CreateCategory", new { id = type.Id, success = true, susccessObjName = category.Name });
                } 
            }
            return View(viewModel);
        }

        /// <summary>
        /// Edit a category for type.
        /// </summary>
        /// <param name="id">Target category-type Id</param>
        /// <returns></returns>
        public ActionResult EditCategoryForType(int? id, int? categoryId)
        {
            if (id == null || categoryId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var type = db.Category_types.Find(id);
            var category = db.Categories.Find(categoryId);
            if (type == null || category == null)
                return HttpNotFound();


            var viewModel = new CategoryViewModel
            {
                CategoryId = category.Id,
                Name = category.Name,
                Desciption = category.Description,
                TypeName = type.Name,
                TypeId = type.Id
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategoryForType(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var type = db.Category_types.Find(viewModel.TypeId);
                if (type != null)
                {
                    var category = db.Categories.Find(viewModel.CategoryId);
                    if (category != null)
                    {
                        category.Name = viewModel.Name;
                        category.Description = viewModel.Desciption;
                        db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                        var result = db.SaveChanges();
                        if (result > 0)
                            return RedirectToAction("Index", new { id = type.Id });
                    }
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult EditCategory(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var type = db.Category_types.Find(viewModel.TypeId);
                var parent = db.Categories.Find(viewModel.ParentId);
                if (type != null && parent != null)
                {
                    var category = db.Categories.Find(viewModel.CategoryId);
                    if (category != null)
                    {
                        category.Name = viewModel.Name;
                        category.Description = viewModel.Desciption;
                        db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                        var result = db.SaveChanges();
                        if (result > 0)
                            return RedirectToAction("Index", new { id = type.Id });
                    }
                }
            }
            return View(viewModel);
        }
    }
}
