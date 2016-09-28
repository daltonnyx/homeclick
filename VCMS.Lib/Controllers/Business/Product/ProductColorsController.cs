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
            ViewBag.Materials = Materials;
            return View(new ProductVariantsViewModel());
        }

        // POST: Manager/FileController/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductVariantsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = ViewModelToModel(viewModel);
                db.Product_Variants.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("List");
            }
            ViewBag.Materials = Materials;
            return View(viewModel);
        }

       public ActionResult Edit(int? id)
       {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = db.Product_Variants.Find(id);

            if (model == null)
                return HttpNotFound();

            var viewModel = ModelToViewModel(model);

            ViewBag.Materials = Materials;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductVariantsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = ViewModelToModel(viewModel);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.Materials = Materials;
            return View(viewModel);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Variant model = await db.Product_Variants.FindAsync(Convert.ToInt32(id));
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
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

        public ActionResult _ImageFile(string fileId)
        {
            var model = db.Files.Find(fileId);
            return PartialView(model);
        }

        private ProductVariantsViewModel ModelToViewModel(Product_Variant model)
        {
            var viewModel = new ProductVariantsViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Image = model.PreviewImageId,
            };
            return viewModel;
        }

        private Product_Variant ViewModelToModel(ProductVariantsViewModel viewModel)
        {
            var model = db.Product_Variants.Find(viewModel.Id);
            if (model == null)
            {
                model = new Product_Variant();
                model.CreateUserId = User.Identity.GetUserId();
                model.CreateTime = DateTime.Now;
            }

            model.Name = viewModel.Name;
            model.Description = viewModel.Description;

            var imageFile = db.Files.Find(viewModel.Image);
            if (imageFile != null && model.PreviewImage != imageFile)
                model.PreviewImageId = viewModel.Image;

            var category = db.Categories.Find((int)ProductVarianTypes.Material);
            if (!model.Categories.Contains(category))
                model.Categories.Add(category);

            return model;
        }

        private List<SelectListItem> Materials
        {
            get
            {
                var variants = db.Product_Variants;
                var materials = new List<SelectListItem>();
                foreach (var variant in variants)
                {
                    if (variant.VariantType == ProductVarianTypes.Material)
                        materials.Add(new SelectListItem { Text = variant.Name, Value = (variant.Id).ToString() });
                }
                return materials; 
            }
        }

        public JsonResult DataHandler(DTParameters param)
        {
            try
            {
                var dtsource = new List<ProductVariantsViewModel>();
                var product_Variants = db.Product_Variants;

                foreach (var product_Variant in product_Variants)
                {
                    if (product_Variant.VariantType == ProductVarianTypes.Color)
                    {
                        dtsource.Add(new ProductVariantsViewModel
                        {
                            Id = product_Variant.Id,
                            Name = product_Variant.Name,
                            Image = (product_Variant.PreviewImageId != null) ? product_Variant.PreviewImageId + product_Variant.PreviewImage.Extension : string.Empty
                        });
                    }
                }

                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<ProductVariantsViewModel, bool>> pre = (p => (search == null || (p.Name != null && p.Name.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.Name != null && p.Name.ToLower().Contains(columnSearch[1].ToLower()))));

                var resultSet = new ResultSet();
                List<ProductVariantsViewModel> data = resultSet.GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<ProductVariantsViewModel> result = new DTResult<ProductVariantsViewModel>
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
