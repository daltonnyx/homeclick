using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Business;
using VCMS.Lib.Models.Business.ViewModels;
using System.Linq.Expressions;

namespace VCMS.Lib.Controllers
{
    public class ProductColorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Manager/ProductColors
        public ActionResult Index()
        {
            return View();
        }

        // GET: Manager/ProductColors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Manager/ProductColors/Create
        public ActionResult Create()
        {
            ViewBag.Category_typeId = new SelectList(db.Category_types, "Id", "name");
            return View();
        }

        // POST: Manager/ProductColors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,name,description,Order,ParentCategoryId,Category_typeId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Category_typeId = new SelectList(db.Category_types, "Id", "name", category.Category_typeId);
            return View(category);
        }

        // GET: Manager/ProductColors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_typeId = new SelectList(db.Category_types, "Id", "name", category.Category_typeId);
            return View(category);
        }

        // POST: Manager/ProductColors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,name,description,Order,ParentCategoryId,Category_typeId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Category_typeId = new SelectList(db.Category_types, "Id", "name", category.Category_typeId);
            return View(category);
        }

        // GET: Manager/ProductColors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Manager/ProductColors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult DataHandler(DTParameters param)
        {
            try
            {
                var categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.ProductColor);
                var dtsource = new List<ProductColorViewModel>();

                foreach (var category in categories)
                {
                    var details = category.Category_detail;
                    var temp = new ProductColorViewModel
                    {
                        Id = category.Id,
                        Name = category.name,
                        Image = details.FirstOrDefault(o => o.name == "colour").value,
                    };
                    dtsource.Add(temp);
                }

                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<ProductColorViewModel, bool>> pre = (p => (search == null || (p.Name != null && p.Name.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.Name != null && p.Name.ToLower().Contains(columnSearch[1].ToLower()))));

                var resultSet = new ResultSet();
                List<ProductColorViewModel> data = resultSet.GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<ProductColorViewModel> result = new DTResult<ProductColorViewModel>
                {
                    draw = param.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
