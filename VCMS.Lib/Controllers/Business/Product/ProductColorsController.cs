using Microsoft.AspNet.Identity;
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

namespace VCMS.Lib.Controllers
{
    public class ProductColorsController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }


        // GET: Manager/FileController/Create
        public ActionResult Create()
        {
            return View(new CreateProductVariantsViewModel());
        }

        // POST: Manager/FileController/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateProductVariantsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var guid = Guid.NewGuid();
                var fileExt = System.IO.Path.GetExtension(model.File.FileName);
                var newFileName = guid.ToString();

                var fileLink = "Areas/Manager/Uploads/Images/" + newFileName + fileExt;
                var newFile = new File
                {
                    Name = model.Name,
                    Description = model.Description,
                    Extension = fileLink,
                    CreateUserId = User.Identity.GetUserId(),
                    CreateTime = DateTime.Now,
                    FileTypeId = 74
                };

                var newColor = new Product_Variant
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = fileLink,
                    CreateUserId = User.Identity.GetUserId(),
                    CreateTime = DateTime.Now,
                };

                db.Product_Variants.Add(newColor);
                db.Files.Add(newFile);

                await db.SaveChangesAsync();
                var newPath = System.IO.Path.Combine(Server.MapPath("~/Areas/Manager/Uploads/Images"), newFileName + fileExt);
                model.File.SaveAs(newPath);

                return RedirectToAction("List");
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Variant color = await db.Product_Variants.FindAsync(Convert.ToInt32(id));
            if (color == null)
            {
                return HttpNotFound();
            }
            return View(color);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Product_Variant color = await db.Product_Variants.FindAsync(Convert.ToInt32(id));
            db.Product_Variants.Remove(color);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        public JsonResult DataHandler(DTParameters param)
        {
            try
            {
                var dtsource = db.Product_Variants.Where(o => o.CategoryId == (int)ProductVarianTypes.Color).Select(o => new ColorViewModel {
                    Id = o.Id,
                    Name = o.Name,
                    Image = o.Image
                }).ToList();

                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<ColorViewModel, bool>> pre = (p => (search == null || (p.Name != null && p.Name.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.Name != null && p.Name.ToLower().Contains(columnSearch[1].ToLower()))));

                var resultSet = new ResultSet();
                List<ColorViewModel> data = resultSet.GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<ColorViewModel> result = new DTResult<ColorViewModel>
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
