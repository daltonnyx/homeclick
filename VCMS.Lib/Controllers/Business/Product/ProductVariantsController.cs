using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Common;
using VCMS.Lib.Models;

namespace VCMS.Lib.Controllers
{
    public class ProductVariantsController : BaseController
    {
        public ActionResult List(int? category_id)
        {
            if (category_id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var category = db.Categories.Find(category_id);
            if (category == null || category.CategoryTypeId != (int)CategoryTypes.ProductVariant)
                return HttpNotFound();
            return View(category);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryId">variant type Id(category)</param>
        /// <returns></returns>
        public ActionResult Create(int? categoryId)
        {
            if (categoryId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var category = db.Categories.Find(categoryId);
            if (category == null || category.CategoryTypeId != (int)CategoryTypes.ProductVariant)
                return HttpNotFound();

            ViewBag.Categories = Categories;
            return View(new ProductVariantsViewModel { CategoryId = category.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductVariantsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = ViewModelToModel(viewModel);
                db.Product_Variants.Add(model);

                await db.SaveChangesAsync();

                //Return after success
                ViewData["Success"] = true;
                ViewData["SuccessObjectName"] = model.Name;
                ViewBag.Categories = Categories;
                return View(new ProductVariantsViewModel());
            }
            ViewBag.Categories = Categories;
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

            ViewBag.Categories = Categories;
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

            ViewBag.Categories = Categories;
            return View(viewModel);
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var model = db.Product_Variants.Find(id);
            object result;
            if (model != null)
            {
                try
                {
                    if (model.PreviewImage != null)
                    {
                        Uploader.DeleteFile(model.PreviewImage, this);
                        db.Files.Remove(model.PreviewImage);
                    }

                    db.Product_Variants.Remove(model);
                    result = db.SaveChanges() > 0 ? "Success" : "Fail";
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            else
            {
                result = HttpStatusCode.NotFound;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
                CategoryId = (int)model.VariantType,
                Image = model.PreviewImageId,
            };

            var cMaterials = model.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.Material);
            var materials = new List<int>();
            foreach (var category in cMaterials)
            {
                materials.Add(category.Id);
            }

            viewModel.CategoryIds = materials.ToArray();
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

            var category = db.Categories.Find(viewModel.CategoryId);
            if (!model.Categories.Contains(category))
                model.Categories.Add(category);

            foreach (var categoryId in viewModel.CategoryIds)
            {
                category = db.Categories.Find(categoryId);
                if (category != null)
                {
                    if (!model.Categories.Contains(category))
                        model.Categories.Add(category);
                }
            }

            return model;
        }

        private List<SelectListItem> Categories
        {
            get
            {
                var categories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.Material)
                    .Select(o => new SelectListItem { Text = o.Name, Value = (o.Id).ToString() }).ToList();
                return categories;
            }
        }

        public JsonResult DataHandler(DTParameters param, Dictionary<string,string> args)
        {
            try
            {
                var dtsource = new List<ProductVariantsViewModel>();
                var product_Variants = db.Product_Variants;
                if (args != null)
                    foreach (var arg in args)
                    {
                        int number;
                        if (int.TryParse(arg.Value, out number))
                        {
                            if (arg.Key == ((int)CategoryTypes.ProductVariant).ToString())
                                foreach (var variant in product_Variants)
                                {
                                    if (variant.VariantType == (ProductVarianTypes)number)
                                        dtsource.Add(new ProductVariantsViewModel
                                        {
                                            Id = variant.Id,
                                            Name = variant.Name
                                        });
                                }
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
