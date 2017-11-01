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
using VCMS.Lib.Common;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Datatables;

namespace VCMS.Lib.Controllers
{
    using System.Globalization;
    using static ConstantKeys;
    public class PostsController : BaseController
    {
        public virtual IQueryable<Category> GetCategories(bool parentOnly = false)
        {
            var result = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.Postcat);
            if (parentOnly)
                return result.Where(o => o.CategoryParentId == null);
            return result;
        }

        public virtual ActionResult List(int? category_id)
        {
            var categories = this.GetCategories();
            var selectedCategory = category_id != null ? category_id.ToString() : categories.FirstOrDefault().Id.ToString();
            if (category_id != null)
            {
                var modelState = new ModelState { Value = new ValueProviderResult(new string[] { selectedCategory }, selectedCategory, CultureInfo.CurrentCulture) };
                ModelState.Add(ConstantKeys.CATEGORIES, modelState);
            }
            ViewData[ConstantKeys.CATEGORIES] = categories;
            return View();
        }

        public virtual ActionResult Create()
        {
            ViewData[CATEGORIES] = GetCategories(true);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(Post model)
        {
            var messageCollection = new List<Message>();
            if (ModelState.IsValid)
            {
                model.CreateUserId = User.Identity.GetUserId();
                model.CreateTime = DateTime.UtcNow;
                db.Posts.Add(model);
                foreach (var item in model.SelectedCategories.Where(o => o.Value))
                {
                    var category = db.Categories.Find(int.Parse(item.Key));
                    category.Posts.Add(model);
                }
                db.SaveChanges();
                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Create successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;
                return RedirectToAction("Edit", new { id = model.Id });
            }
            ViewData[CATEGORIES] = GetCategories(true);
            return View(model);
        }


        public virtual ActionResult Edit(int id)
        {
            var model = db.Posts.Find(id);
            if (model == null)
                return HttpNotFound();
            ViewData[CATEGORIES] = GetCategories(true);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(Post model)
        {
            var messageCollection = new List<Message>();
            if (ModelState.IsValid)
            {
                var modelTarget = db.Posts.Find(model.Id);

                foreach (var item in model.SelectedCategories)
                {
                    var category = db.Categories.Find(int.Parse(item.Key));
                    if (category.Posts.Select(o => o.Id).Contains(modelTarget.Id))
                    {
                        if (!item.Value)
                            category.Posts.Remove(modelTarget);
                    }
                    else if (item.Value)
                        category.Posts.Add(modelTarget);
                }

                modelTarget.Files.Clear();
                if (model.SlideImages != null)
                {
                    foreach (var item in model.SlideImages)
                    {
                        var file = db.Files.Find(item);
                        modelTarget.Files.Add(file);
                    }
                }

                //Remove absent details
                var existDetailId = modelTarget.Post_Details.Select(o => o.Id);
                var exceptDetailIds = existDetailId.Except(model.Post_Details.Select(o => o.Id)).ToList();
                foreach (var exceptDetailId in exceptDetailIds)
                {
                    var details = db.Post_Details.Find(exceptDetailId);
                    if (details != null)
                        db.Entry(details).State = System.Data.Entity.EntityState.Deleted;
                }

                //Modify or add details
                foreach (var detail in model.Post_Details)
                {
                    if (detail.Id != 0)
                    {
                        var detailTarget = modelTarget.Post_Details.FirstOrDefault(o => o.Id == detail.Id);
                        db.Entry(detailTarget).CurrentValues.SetValues(detail);
                        db.Entry(detailTarget).Property("PostId").IsModified = false;
                        db.Entry(detailTarget).Property("CreateUserId").IsModified = false;
                        db.Entry(detailTarget).Property("CreateTime").IsModified = false;
                    }
                    else
                    {
                        detail.CreateUserId = User.Identity.GetUserId();
                        detail.CreateTime = DateTime.UtcNow;
                        detail.PostId = model.Id;
                        db.Entry(detail).State = System.Data.Entity.EntityState.Added;
                        modelTarget.Post_Details.Add(detail);
                    }
                }

                db.Entry(modelTarget).CurrentValues.SetValues(model);
                db.Entry(modelTarget).Property("CreateUserId").IsModified = false;
                db.Entry(modelTarget).Property("CreateTime").IsModified = false;
                db.SaveChanges();

                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Update successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;
                return RedirectToAction("Edit", new { id = model.Id });
            }
            ViewData[CATEGORIES] = GetCategories(true);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult ListFilter(FormCollection collection)
        {
            return RedirectToAction("List", new { category_id = collection[ConstantKeys.CATEGORIES]});
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int[] ids)
        {
            var messageCollection = new List<Message>();
            foreach (var id in ids)
            {
                var model = db.Posts.Find(id);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();

            messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Delete successfully!" });
            TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;
            return RedirectToAction("List");
        }

        #region[Datatables]
        private IQueryable<Post> FilterDbSource(Dictionary<string, string> args)
        {
            var posts = db.Posts.AsQueryable();
            if (args != null)
                foreach (var arg in args)
                {
                    if (arg.Key.Contains(CATEGORIES))
                    {
                        int value;
                        if (int.TryParse(arg.Value, out value))
                            posts = posts.Where(o => o.Categories.Select(c => c.Id).Contains(value));
                    }
                }
            return posts;
        }

        private DTResult<T> GetDtResult<T>(DTParameters param, Dictionary<string, string> args) where T : Dictionary<string, object>, new()
        {
            var dtsource = new List<T>();
            var dbSource = FilterDbSource(args);
            var o = new object();
            foreach (var item in dbSource)
            {
                dtsource.Add(new T
                {
                    {ID, item.Id },
                    {TITLE, item.Title },
                    {EXCERPT, item.Excerpt },
                    {AUTHOR, item.CreateUser?.UserName },
                    {CATEGORIES, item.Categories.Select(c => new { id = c.Id, name = c.Name}) },
                    {STATUS, item.Status },
                    {TIME, item.CreateTime }
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
