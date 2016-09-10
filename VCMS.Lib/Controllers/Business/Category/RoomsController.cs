using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Business;
using VCMS.Lib.Models.Business.Datatables;

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
            if (cRoom == null || cRoom.Category_typeId != (int)CategoryTypes.Model)
                return HttpNotFound();

            var typologies = cRoom.CategoryChildren;
            ViewBag.Typologies = typologies;
            return View(cRoom);
        }

        public ActionResult Add(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = db.Categories.Find(id);
            if (category == null || category.Category_typeId != (int)parentType)
                return HttpNotFound();
 
            ViewBag.Typologies = GetDistinctCategories(category); ;
            ViewBag.CategoryParent = category;
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
                if (parent?.Category_typeId == (int)parentType)
                {
                    var child = db.Categories.Find(viewModel.ChildId);
                    if (child?.Category_typeId == (int)childType && !parent.CategoryChildren.Contains(child))
                    {
                        parent.CategoryChildren.Add(child);
                        db.Entry(parent).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        ViewData["Success"] = true;
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
            if (parent?.Category_typeId == (int)parentType)
            {
                var child = db.Categories.Find(typoId);
                if (child?.Category_typeId == (int)childType && parent.CategoryChildren.Contains(child))
                {
                    parent.CategoryChildren.Remove(child);
                    result = await db.SaveChangesAsync();
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<SelectListItem> GetDistinctCategories(Category parent)
        {
            var typologies = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.Typology).ToList();
            var result = typologies.Except(parent.CategoryChildren).Select(o => new SelectListItem { Text = o.Name, Value = o.Id.ToString() });
            return result;
        }
    }
}
