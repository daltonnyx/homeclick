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
using VCMS.Lib.Common;

namespace VCMS.Lib.Controllers
{
    using System.Data.Entity.Infrastructure;
    using System.Globalization;
    using static ConstantKeys;

    public class CategoriesController : BaseController
    {
        #region[Functions]
        public IQueryable<Category_Type> GetAllType()
        {
            return db.Category_types;
        }

        public IQueryable<Category> GetAllCategory(int? typeId)
        {
            return db.Categories;
        }
        #endregion

        public virtual ActionResult List(int? category_type_id)
        {
            var types = GetAllType();
            var selectedType = types.FirstOrDefault().Id.ToString();
            if (category_type_id != null)
                selectedType = category_type_id.ToString();
            else if (TempData.ContainsKey(CATEGORY_TYPE_SELECTED))
                selectedType = TempData[CATEGORY_TYPE_SELECTED].ToString();
            var modelState = new ModelState { Value = new ValueProviderResult(new string[] { selectedType }, selectedType, CultureInfo.CurrentCulture) };
            ModelState.Add(string.Format("{0}[{1}]", CREATE_CATEGORY_PARAM, CATEGORY_TYPE), modelState);
            ModelState.Add(CATEGORY_TYPE, modelState);
            ViewData[CATEGORY_TYPES] = types;
            return View();
        }

        [HttpPost]
        public JsonResult Create(Dictionary<string, string> CreateCategoryData)
        {
            var name = CreateCategoryData[NAME];
            var description = CreateCategoryData[DESCRIPTION];
            var parentCategory = CreateCategoryData[PARENT_CATEGORY] != string.Empty ? int.Parse(CreateCategoryData[PARENT_CATEGORY]) : 0;
            var categoryType = CreateCategoryData[CATEGORY_TYPE] != string.Empty ? int.Parse(CreateCategoryData[CATEGORY_TYPE]) : 0;

            if (name == string.Empty)
                return Json(new { MessageType = MessageTypes.Danger.ToString(), MessageContent = "Name can't be empty!" });
            var newCategory = new Category { Name = name, Description = description };
            db.Categories.Add(newCategory);

            //Add category to parent if parent field constant data.
            if (parentCategory != 0)
            {
                var parent = db.Categories.Find(parentCategory);
                if (parent != null)
                    parent.Children.Add(newCategory);
            }
            //Add category to type
            var type = db.Category_types.Find(categoryType);
            if (type != null)
                type.Categories.Add(newCategory);
            else
                return Json(new { MessageType = MessageTypes.Danger.ToString(), MessageContent = string.Format(TemplateStrings.MODEL_CREATE_FAIL, name) });

            if (db.SaveChanges() > 0)
                return Json(new { MessageType = MessageTypes.Success.ToString(), MessageContent = string.Format(TemplateStrings.MODEL_CREATE_SUCCESS, name) });
            return Json(new { MessageType = MessageTypes.Danger.ToString(), MessageContent = string.Format(TemplateStrings.MODEL_CREATE_FAIL, name) });
        }

        public ActionResult Edit(int? category_id)
        {
            if (category_id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var category = db.Categories.Find(category_id);
            if (category == null)
                return HttpNotFound();
            ViewData[CATEGORY_TYPES] = GetAllType();
            ViewData[CATEGORIES] = db.Categories.Where(o => o.CategoryTypeId == category.CategoryTypeId && o.Id != category.Id && o.CategoryParentId == null);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            var category = db.Categories.Find(model.Id);
            if (category == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(category).CurrentValues.SetValues(model);
                    db.SaveChanges();
                    var messages = new List<Message>();
                    messages.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Update successfully!" });
                    TempData[ACTION_RESULT_MESSAGES] = messages;
                    return RedirectToAction("List", new { category_type_id = category.CategoryTypeId});
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
                }
            }
            ViewData[CATEGORY_TYPES] = GetAllType();
            ViewData[CATEGORIES] = db.Categories.Where(o => o.CategoryTypeId == category.CategoryTypeId && o.Id != category.Id);
            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteConfirmed(int[] ids)
        {
            if (ids == null)
                return Json(new { MessageType = MessageTypes.Warning.ToString(), MessageContent = "Something wrong?" });
            var messageCollection = new List<object>();
            var successes = new List<string>();
            foreach (var id in ids)
            {
                var category = db.Categories.Find(id);
                if (category != null)
                    try
                    {
                        db.Entry(category).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        successes.Add(category.Name);
                    }
                    catch (DbUpdateException ex)
                    {
                        messageCollection.Add(new { MessageType = MessageTypes.Danger.ToString(), MessageContent = ex.Message });
                    }
            }
            if (successes.Count > 0)
                messageCollection.Add(new { MessageType = MessageTypes.Success.ToString(), MessageContent = string.Format(TemplateStrings.MODEL_DELETE_RESULT, "Categories", string.Join(", ", successes.ToArray()), successes.Count) });

            return Json(messageCollection);
        }

        #region[Datatables]
        private IQueryable<Category> FilterDbSource(Dictionary<string, string> args)
        {
            var categories = db.Categories.AsQueryable();
            if (args != null)
                foreach (var arg in args)
                {
                    if (arg.Key.Contains(CATEGORY_TYPE))
                    {
                        int value;
                        if (int.TryParse(arg.Value, out value))
                            categories = categories.Where(o => o.CategoryTypeId == value && o.CategoryParentId == null);
                    }
                }
            return categories;
        }

        private DTResult<T> GetDtResult<T>(DTParameters param, Dictionary<string, string> args) where T : Dictionary<string, object>, new()
        {
            var dtsource = new List<T>();
            var dbSource = FilterDbSource(args);

            var entityNodes = new List<EntityNode<Category>>();
            foreach (var source in dbSource)
            {
                entityNodes.AddRange(source.GetNodes(0));
            }

            foreach (var item in entityNodes)
            {
                dtsource.Add(new T
                {
                    {ID, item.Entity.Id },
                    {NAME, item.Entity.Name },
                    {DESCRIPTION, item.Entity.Description },
                    {LEVEL, item.Depth}
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
