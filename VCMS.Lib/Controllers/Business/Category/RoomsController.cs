using System;
using System.Collections.Generic;
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
    public class RoomsController : BaseController
    {
        private const CategoryTypes parentType = CategoryTypes.Model;
        private const CategoryTypes childType = CategoryTypes.Typology;

        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var cRoom = db.Categories.Find(id);
            if (cRoom == null || cRoom.CategoryTypeId != (int)CategoryTypes.Model)
                return HttpNotFound();

            var typologies = cRoom.CategoryChildren;
            ViewBag.Typologies = typologies;
            return View(cRoom);
        }

        public ActionResult Add(int? id, bool? success, string successObjectName)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = db.Categories.Find(id);
            if (category == null || category.CategoryTypeId != (int)parentType)
                return HttpNotFound();
 
            ViewBag.Typologies = GetDistinctCategories(category); ;
            ViewBag.CategoryParent = category;
            ViewData["Success"] = success;
            ViewData["SuccessObjectName"] = successObjectName;
            return View(new ParentChildViewModel { ParentId = category.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ParentChildViewModel viewModel)
        {
            var parent = db.Categories.Find(viewModel.ParentId);
            if (parent == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                if (parent?.CategoryTypeId == (int)parentType)
                {
                    var child = db.Categories.Find(viewModel.ChildId);
                    if (child?.CategoryTypeId == (int)childType && !parent.CategoryChildren.Contains(child))
                    {
                        parent.CategoryChildren.Add(child);
                        db.Entry(parent).State = System.Data.Entity.EntityState.Modified;
                        if (db.SaveChanges() > 0)
                            return RedirectToAction("Add", new { id = parent.Id, success = true, successObjectName = child.Name });
                    }
                }
            }

            ViewBag.Typologies = GetDistinctCategories(parent);
            ViewBag.CategoryParent = parent;
            return View(viewModel);
        }

        public async Task<ActionResult> Remove(int? roomId, int? typoId)
        {
            int result = 0;
            var parent = db.Categories.Find(roomId);
            if (parent?.CategoryTypeId == (int)parentType)
            {
                var child = db.Categories.Find(typoId);
                if (child?.CategoryTypeId == (int)childType && parent.CategoryChildren.Contains(child))
                {
                    parent.CategoryChildren.Remove(child);
                    result = await db.SaveChangesAsync();
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<SelectListItem> GetDistinctCategories(Category parent)
        {
            var typologies = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.Typology).ToList();
            var result = typologies.Except(parent.CategoryChildren).Select(o => new SelectListItem { Text = o.Name, Value = o.Id.ToString() });
            return result;
        }
    }
}
