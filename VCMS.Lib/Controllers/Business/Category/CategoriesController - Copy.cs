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
    public class CategoriesController : BaseController
    {
        public virtual ActionResult List()
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

        public virtual ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var categoryType = db.Category_types.Find(id);
            if (categoryType == null)
                return HttpNotFound();

            var categories = db.Categories.Where(o => o.CategoryTypeId == id);
            ViewBag.Categories = categories;
            return View(categoryType);
        }


        public virtual ActionResult CreateCategory(int? id, bool? success, string successObjectName)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var parent = db.Categories.Find(id);
            if (parent == null)
                return HttpNotFound();

            var viewModel = new CategoryViewModel
            {
                ParentId = parent.Id,
                ParentName = parent.Name,
                TypeName = parent.CategoryType.Name,
            };

            if (success == true)
            {
                ViewData["Success"] = success;
                ViewData["SuccessObjectName"] = successObjectName;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult CreateCategory(CategoryViewModel viewModel)
        {
            var parentCategory = db.Categories.Find(viewModel.ParentId);

            if (parentCategory != null && ModelState.IsValid)
            {
                var model = new Category
                {
                    Name = viewModel.Name,
                    Description = viewModel.Desciption,
                };
                model.CategoryParents.Add(parentCategory);
                db.Categories.Add(model);
                db.SaveChanges();
                return RedirectToAction("CreateCategory", new { id = parentCategory.Id, success = true, successObjectName = model.Name });
            }
            return View(viewModel);
        }

        /// <summary>
        /// Edit a category for type.
        /// </summary>
        /// <param name="id">Target category-parent Id</param>
        /// <returns></returns>
        public ActionResult EditCategory(int? id, int? categoryId)
        {
            if (id == null || categoryId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var parent = db.Categories.Find(id);
            var category = db.Categories.Find(categoryId);
            if (parent == null || category == null)
                return HttpNotFound();

            var viewModel = new CategoryViewModel
            {
                CategoryId = category.Id,
                Name = category.Name,
                Desciption = category.Description,
                ParentId = parent.Id,
                ParentName = parent.Name,
                TypeId = parent.CategoryTypeId ?? null,
                TypeName = parent.CategoryType?.Name ?? null
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult EditCategory(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var type = db.Category_types.Find(viewModel.TypeId);
                var parent = db.Categories.Find(viewModel.ParentId);
                if (type != null && parent !=null)
                {
                    var category = db.Categories.Find(viewModel.CategoryId);
                    if (category != null && parent.CategoryChildren.Contains(category))
                    {
                        category.Name = viewModel.Name;
                        category.Description = viewModel.Desciption;
                        db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                        var result = db.SaveChanges();
                        if (result > 0)
                            return RedirectToAction("Index", new { id = parent.Id });
                    }
                }
            }
            return View(viewModel);
        }


        /// <summary>
        /// Delete a category with given id.
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns></returns>
        public ActionResult DeleteCategory (int? id)
        {
            var result = "";
            var category = db.Categories.Find(id);
            if (category != null)
                try
                {
                    db.Categories.Remove(category);
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
    }
}
