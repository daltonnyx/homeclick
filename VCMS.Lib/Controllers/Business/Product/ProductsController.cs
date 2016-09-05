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
    public class ProductsController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }


        public ActionResult Create()
        {
            var colors = db.Product_Variants.Where(o=> o.CategoryId == (int)ProductVarianTypes.Color);
            var categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.Typology);
            ViewBag.Colors = colors;
            ViewBag.Categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.Typology);
            return View();
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newProduct = ViewModelToModel(model);
                db.Products.Add(newProduct);
                await db.SaveChangesAsync();

                model = new CreateProductViewModel();
                ViewData["Success"] = true;
            }
            var colors = db.Product_Variants.Where(o => o.CategoryId == (int)ProductVarianTypes.Color);
            var categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.Typology);
            ViewBag.Colors = colors;
            ViewBag.Categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.Typology);
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(Convert.ToInt32(id));
            
            if (product == null)
            {
                return HttpNotFound();
            }
            var viewModel = ModelToViewModel(product);

            var colors = db.Product_Variants.Where(o => o.CategoryId == (int)ProductVarianTypes.Color);
            var categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.Typology);
            ViewBag.Colors = colors;
            ViewBag.Categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.Typology);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = ViewModelToModel(model);
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
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
            Product product = await db.Products.FindAsync(Convert.ToInt32(id));
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var product = db.Products.Find(int.Parse(id));
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        public ActionResult _Colors(int colorId)
        {
            var model = db.Product_Variants.Find(colorId);
            return PartialView(model);
        }

        public ActionResult _ImageFiles(string fileId, int colorId, int[] colors)
        {
            var model = db.Files.Find(fileId);
            var iColors = new List<Product_Variant>();
            foreach (var str in colors)
            {
                var color = db.Product_Variants.Find(str);
                iColors.Add(color);
            }
            ViewBag.SelectedColor = colorId;
            ViewBag.Colors = iColors;
            return PartialView(model);
        }

        public JsonResult DataHandler(DTParameters param)
        {
            try
            {
                var dtsource = db.Products.Select(o => new ProductViewModel {
                    Id = o.Id,
                    Name = o.name,
                    Image = o.image,
                    Status = o.Status
                }).ToList();

                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<ProductViewModel, bool>> pre = (p => (search == null || (p.Name != null && p.Name.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.Name != null && p.Name.ToLower().Contains(columnSearch[1].ToLower()))));

                var resultSet = new ResultSet();
                List<ProductViewModel> data = resultSet.GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<ProductViewModel> result = new DTResult<ProductViewModel>
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

        public Product ViewModelToModel(CreateProductViewModel viewModel)
        {
            var files = db.Files;

            Product newProduct = new Product
            {
                Id = viewModel.Id,
                name = viewModel.Name,
                content = viewModel.Description,
                image = viewModel.PreviewImage,
                CreateUserId = User.Identity.GetUserId(),
                CreateTime = DateTime.Now
            };

            //colors
            foreach (var color in viewModel.Colors)
            {
                var selectedColor = db.Product_Variants.Find(color);
                newProduct.Product_Variants.Add(selectedColor);
            }

            //Images
            foreach (var obj in viewModel.ImageFiles)
            {
                var file = db.Files.Find(obj.Key);
                if (file != null)
                {
                    newProduct.Files.Add(file);
                    var variant = db.Product_Variants.Find(obj.Value);
                    if (variant != null)
                        variant.Files.Add(file);
                }
            }

            //size
            if (viewModel.Size != null)
            {
                var pSize = new Product_detail
                {
                    name = "kich_thuoc",
                    value = viewModel.Size,
                };
                pSize.Product = newProduct;
                db.Product_details.Add(pSize);
            }

            //Warranty
            if (viewModel.Warranty != null)
            {
                var detail = new Product_detail
                {
                    name = "bao_hanh",
                    value = viewModel.Warranty,
                };
                detail.Product = newProduct;
                db.Product_details.Add(detail);
            }

            //Madein
            if (viewModel.MadeIn != null)
            {
                var detail = new Product_detail
                {
                    name = "xuat_xu",
                    value = viewModel.MadeIn,
                };
                detail.Product = newProduct;
                db.Product_details.Add(detail);
            }

            //price
            if (viewModel.Price != null)
            {
                var detail = new Product_detail
                {
                    name = "gia",
                    value = viewModel.Price,
                };
                detail.Product = newProduct;
                db.Product_details.Add(detail);
            }

            //price
            if (viewModel.Weight != null)
            {
                var detail = new Product_detail
                {
                    name = "nang",
                    value = viewModel.Weight,
                };
                detail.Product = newProduct;
                db.Product_details.Add(detail);
            }

            return newProduct;
        }

        public CreateProductViewModel ModelToViewModel(Product model)
        {
            var viewModel = new CreateProductViewModel
            {
                Name = model.name,
                Colors = model.Product_Variants.Select(o => o.Id).ToArray(),
                Description = model.content,
                IColors = model.Product_Variants,
                Id = model.Id,
            };

            foreach (var detail in model.Product_detail)
            {
                switch (detail.name)
                {
                    case "gia":
                        viewModel.Price = detail.value;
                        break;
                    case "kich_thuoc":
                        viewModel.Size = detail.value;
                        break;
                    case "bao_hanh":
                        viewModel.Warranty = detail.value;
                        break;
                    case "xuat_xu":
                        viewModel.MadeIn = detail.value;
                        break;
                    default:
                        break;
                }
            }
            return viewModel;
        }
    }
}
