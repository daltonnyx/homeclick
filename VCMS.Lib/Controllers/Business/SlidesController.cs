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
using VCMS.Lib.Models.Business;
using VCMS.Lib.Models.Business.Datatables;

namespace VCMS.Lib.Controllers
{
    public class SlidesController : BaseController
    {
        public ActionResult List(int categoryId)
        {
            var category = db.Categories.Find(categoryId);
            if (!CheckCategoryIsType(category.Category_TypeId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        public ActionResult Create(int categoryId, bool? success, string successObjectName)
        {
            var category = db.Categories.Find(categoryId);
            if (!CheckCategoryIsType(category.Category_TypeId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (category == null)
                return HttpNotFound();
            ViewBag.Category = category;
            ViewData["Success"] = success;
            ViewData["SuccessObjectName"] = successObjectName;
            return View(new Slide { CategoryId = category.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slide model)
        {
            var category = db.Categories.Find(model.CategoryId);
            if (ModelState.IsValid)
            {
                if (!CheckCategoryIsType(category.Category_TypeId))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                model.CreateUserId = User.Identity.GetUserId();
                model.CreateTime = DateTime.Now;
                db.Slides.Add(model);
                db.SaveChanges();
                return RedirectToAction("Create", new { categoryId = model.CategoryId, success = true, successObjectName = model.Name });
            }
            ViewBag.Category = category;
            return View(model);
        }

        public ActionResult Edit(int categoryId, int slideId)
        {
            var category = db.Categories.Find(categoryId);
            var slide = db.Slides.Find(slideId);
            if (!CheckCategoryIsType(category.Category_TypeId) || !category.Slides.Select(o => o.Id).Contains(slide.Id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (category == null || slide == null)
                return HttpNotFound();

            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slide model)
        {
            if (ModelState.IsValid)
            {
                var category = db.Categories.Find(model.CategoryId);
                if (!CheckCategoryIsType(category.Category_TypeId) || !category.Slides.Select(o => o.Id).Contains(model.Id))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var modelInDb = db.Slides.Find(model.Id);
                db.Entry(modelInDb).CurrentValues.SetValues(model);
                db.Entry(modelInDb).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List", new { categoryId = model.CategoryId});
            }
            return View(model);
        }
        [HttpGet]
        public JsonResult Delete(int slideId)
        {
            var slide = db.Slides.Find(slideId);
            object result = -1;
            if (slide != null)
            {
                if (slide.ImageFile != null)
                {
                    Uploader.DeleteFile(slide.ImageFile, this);
                    db.Files.Remove(slide.ImageFile);
                }
                db.Slides.Remove(slide);
                result = db.SaveChanges();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region [Functions]
        private bool CheckCategoryIsType(int? typeId)
        {
            return typeId == (int)CategoryTypes.Slide;
        }
        #endregion
    }
}
