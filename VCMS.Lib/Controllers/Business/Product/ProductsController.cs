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
using VCMS.Lib.Models.Datatables;

namespace VCMS.Lib.Controllers
{
    public class ProductsController : BaseController
    {
        private List<ProductVarianTypes> productVariantTypes
        {
            get
            {
                var productVariantTypes = new List<ProductVarianTypes>();
                productVariantTypes.Add(ProductVarianTypes.Color);
                productVariantTypes.Add(ProductVarianTypes.Material);
                return productVariantTypes;
            }
        }

        private List<CategoryTypes> ProductCategoryTypes
        {
            get
            {
                var categoryTypes = new List<CategoryTypes>();
                categoryTypes.Add(CategoryTypes.Model);
                categoryTypes.Add(CategoryTypes.Typology);
                return categoryTypes;
            }
        }

        public ActionResult List()
        {
            var dic = new Dictionary<string, Dictionary<string, int>>();
            ProductCategoryTypes.ForEach(
                o => dic.Add(o.ToString(), db.Categories.Where(c => c.CategoryTypeId == (int)o).ToDictionary(e => e.Name, e => e.Id)));
            ViewBag.Dic = dic;
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Typologies = Typologies;
            ViewBag.Rooms = Rooms;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newProduct = ViewModelToModel(model);
                db.Products.Add(newProduct);
                await db.SaveChangesAsync();

                model = new ProductViewModel();
                ViewData["Success"] = true;
                return View();
            }

            ViewBag.Typologies = Typologies;
            ViewBag.Rooms = Rooms;
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

            ViewBag.Typologies = Typologies;
            ViewBag.Rooms = Rooms;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = ViewModelToModel(model);
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("List");
            }

            ViewBag.Typologies = Typologies;
            ViewBag.Rooms = Rooms;
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

        public ActionResult _imageFile(string id)
        {
            var model = db.Files.Find(id);
            if (model == null)
                return HttpNotFound();

            return PartialView(model);
        }

        public List<SelectListItem> Rooms
        {
            get
            {
                var rooms = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.Model)
                    .Select(o => new SelectListItem { Text = o.Name, Value = (o.Id).ToString() }).ToList();
                return rooms;
            }
        }

        public List<SelectListItem> Typologies
        {
            get
            {
                var rooms = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.Typology)
                    .Select(o => new SelectListItem { Text = o.Name, Value = (o.Id).ToString() }).ToList();
                return rooms;
            }
        }

        public ActionResult GetTypologies(int[] roomIds)
        {
            var result = new List<object>();
            var typoLists = new List<IEnumerable<Category>>();
            foreach (var roomId in roomIds)
            {
                var cRoom = db.Categories.Find(roomId);
                if (cRoom?.CategoryTypeId == (int)CategoryTypes.Model)
                {
                    var typologies = cRoom.CategoryChildren.Where(o => o.CategoryTypeId == (int)CategoryTypes.Typology);
                    typoLists.Add(typologies);
                }
            }
            var commonList = Common.Helper.FindCommon<Category>(typoLists);

            foreach (var item in commonList)
            {
                result.Add(new
                {
                    id = item.Id,
                    name = item.Name
                });
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public Product ViewModelToModel(ProductViewModel viewModel)
        {
            var files = db.Files;
            var categories = db.Categories;

            var model = db.Products.Find(viewModel.Id);
            if (model == null)
            {
                model = new Product();
                model.CreateUserId = User.Identity.GetUserId();
                model.CreateTime = DateTime.Now;

                var dic = new Dictionary<string, string>();
                //size
                if (viewModel.Size != null)
                    dic.Add(ProductDetailTypes.Size, viewModel.Size);

                //Warranty
                if (viewModel.Warranty != null)
                    dic.Add(ProductDetailTypes.Warranty, viewModel.Warranty);

                //Madein
                if (viewModel.MadeIn != null)
                    dic.Add(ProductDetailTypes.MadeIn, viewModel.MadeIn);

                //price
                if (viewModel.Price != null)
                    dic.Add(ProductDetailTypes.Price, viewModel.Price);

                //weight
                if (viewModel.Weight != null)
                    dic.Add(ProductDetailTypes.Weight, viewModel.Weight);

                //price
                if (viewModel.UnitType != null)
                    dic.Add(ProductDetailTypes.UnitType, viewModel.UnitType);

                foreach (var item in dic)
                {
                    var detail = new Product_detail { Name = item.Key, Value = item.Value };
                    detail.Product = model;
                    db.Product_details.Add(detail);
                }
            }
            else
            {

                foreach (var detail in model.Product_detail)
                {
                    var newValue = "";
                    switch (detail.Name)
                    {
                        case ProductDetailTypes.Price:
                            newValue = viewModel.Price;
                            if (detail.Value != newValue)
                            {
                                detail.Value = newValue;
                                db.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                            }
                            break;
                        case ProductDetailTypes.Size:
                            newValue = viewModel.Size;
                            if (detail.Value != newValue)
                            {
                                detail.Value = newValue;
                                db.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                            }
                            break;
                        case ProductDetailTypes.Warranty:
                            newValue = viewModel.Warranty;
                            if (detail.Value != newValue)
                            {
                                detail.Value = newValue;
                                db.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                            }
                            break;
                        case ProductDetailTypes.MadeIn:
                            newValue = viewModel.MadeIn;
                            if (detail.Value != newValue)
                            {
                                detail.Value = newValue;
                                db.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                            }
                            break;
                        case ProductDetailTypes.Weight:
                            newValue = viewModel.Weight;
                            if (detail.Value != newValue)
                            {
                                detail.Value = newValue;
                                db.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                            }
                            break;
                        case ProductDetailTypes.UnitType:
                            newValue = viewModel.UnitType;
                            if (detail.Value != newValue)
                            {
                                detail.Value = newValue;
                                db.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                            }
                            break;
                        default:
                            break;
                    }
                }

                /*
                // old variants data
                var dic = new Dictionary<string, int>();
                foreach (var variant in model.Product_Variants)
                {
                    foreach (var file in variant.Files)
                    {
                        if (model.Files.Contains(file))
                            if (!dic.ContainsKey(file.Id))
                                dic.Add(file.Id, variant.Id);
                    }
                }

                // delete old data
                foreach (var obj in dic)
                {
                    var found = false;
                    foreach (var ImageFile in viewModel.imageFiles)
                    {
                        if (obj.Key == ImageFile.Key && obj.Value == ImageFile.Value)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        var file = files.Find(obj.Key);
                        if (files != null)
                        {
                            var variant = model.Product_Variants.FirstOrDefault(o => o.Id == obj.Value);
                            if (variant != null)
                            {
                                variant.Files.Remove(file);
                                db.Entry(variant).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }
                }
                            */
            }

            model.Status = viewModel.Status;
            model.name = viewModel.Name;
            model.content = viewModel.Description;

            if (model.Typology.Id != viewModel.TypologyTypeId)
            {
                model.Categories.Remove(model.Typology);
                var category = categories.Find(viewModel.TypologyTypeId);
                model.Categories.Add(category);
            }

            var modelRooms = model.Rooms;
            foreach (var modelRoom in modelRooms)
            {
                var found = false;
                foreach (var roomId in viewModel.RoomIds)
                {
                    if (modelRoom.Id == roomId)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    model.Categories.Remove(modelRoom);
            }

            foreach (var room in viewModel.RoomIds)
            {
                var category = categories.Find(room);
                if (category != null)
                    if (!model.Categories.Contains(category))
                        model.Categories.Add(category);
            }
            /*
            //colors
            var modelVariants = model.Product_Variants;
            foreach (var variant in modelVariants)
            {
                var found = false;
                foreach (var color in viewModel.Colors)
                {
                    if (variant.Id == color)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    model.Product_Variants.Remove(variant);
                    foreach (var imageFile in viewModel.imageFiles)
                    {
                        var file = db.Files.Find(imageFile.Key);
                        if (file != null)
                            if (variant.Files.Contains(file))
                            {
                                variant.Files.Remove(file);
                                db.Entry(variant).State = System.Data.Entity.EntityState.Modified;
                            }
                    }
                }
            }

            foreach (var color in viewModel.Colors)
            {
                var selectedColor = db.Product_Variants.Find(color);
                if (selectedColor != null)
                    if (!model.Product_Variants.Contains(selectedColor))
                        model.Product_Variants.Add(selectedColor);
            }
            */
            //proview image
            if (model.ImageId != viewModel.previewImageId)
                model.ImageId = viewModel.previewImageId;
            /*
            //Images
            foreach (var obj in viewModel.imageFiles)
            {
                var file = db.Files.Find(obj.Key);
                if (file != null)
                {
                    if (!model.Files.Contains(file))
                    {
                        model.Files.Add(file);
                        var variant = db.Product_Variants.Find(obj.Value);
                        if (variant != null)
                        {
                            if (!variant.Files.Contains(file))
                            {
                                variant.Files.Add(file);
                                db.Entry(variant).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }
                }
            }
            */

            return model;
        }

        public ProductViewModel ModelToViewModel(Product model)
        {
            var viewModel = new ProductViewModel
            {
                Name = model.name,
                previewImageId = model.ImageId,
                previewImage = model.Image != null ? model.Image.FullFileName : string.Empty,
                Description = model.content,
                Id = model.Id,
                Status = model.Status
            };

            if (model.Typology != null)
                viewModel.TypologyTypeId = model.Typology.Id;
            if (model.Rooms != null)
                viewModel.RoomIds = model.Rooms.Select(o => o.Id).ToArray();

            foreach (var detail in model.Product_detail)
            {
                switch (detail.Name)
                {
                    case ProductDetailTypes.Price:
                        viewModel.Price = detail.Value;
                        break;
                    case ProductDetailTypes.Size:
                        viewModel.Size = detail.Value;
                        break;
                    case ProductDetailTypes.Warranty:
                        viewModel.Warranty = detail.Value;
                        break;
                    case ProductDetailTypes.MadeIn:
                        viewModel.MadeIn = detail.Value;
                        break;
                    case ProductDetailTypes.Weight:
                        viewModel.Weight = detail.Value;
                        break;
                    default:
                        break;
                }
            }
            return viewModel;
        }

        [HttpPost]
        public JsonResult DataHandler(DTParameters param, Dictionary<string, string> args)
        {
            try
            {
                var products = db.Products.ToList();
                if (args != null)
                    foreach (var arg in args)
                    {
                        int number;
                        switch (arg.Key.ToLower())
                        {
                            case "sale":
                                if (int.TryParse(arg.Value, out number))
                                    products = products.Where(o => o.Sales.Select(e => e.Id).Contains(number)).ToList();
                                break;
                            case "model":
                                if (int.TryParse(arg.Value, out number))
                                    products = products.Where(o => o.Categories.Select(e => e.Id).ToList().Contains(number)).ToList();
                                break;
                            case "typology":
                                if (int.TryParse(arg.Value, out number))
                                    products = products.Where(o => o.Categories.Select(e => e.Id).ToList().Contains(number)).ToList();
                                break;
                        }
                    }

                var dtsource = products.Select(o => new dt_product
                {
                    id = o.Id,
                    name = o.name,
                    img = o.Image != null ? o.Image.FullFileName : "NoImageFound.png",
                    status = o.Status,
                    options = o.Product_Options.ToDictionary(e => e.Name, e => int.Parse(e.Product_Options_Details.FirstOrDefault(q => q.Name == ProductDetailTypes.Quantity).Value))
                }).ToList();

                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<dt_product, bool>> pre = (p => (search == null || (p.name != null && p.name.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.name != null && p.name.ToLower().Contains(columnSearch[1].ToLower()))));

                var resultSet = new ResultSet();
                List<dt_product> data = resultSet.GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<dt_product> result = new DTResult<dt_product>
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

        #region [Options]
        public ActionResult CreateOption(int? productId, bool? success, string successObjectName)
        {
            if (productId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = db.Products.Find(productId);
            if (product == null)
                return HttpNotFound();

            ViewData["Success"] = success;
            ViewData["SuccessObjectName"] = successObjectName;
            var option = new ProductOptionViewModel
            {
                productId = product.Id,
                productName = product.name
            };

            var variantCategories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProductVariant);
            ViewBag.VariantCategories = variantCategories;

            var options = product.Product_Options;
            ViewBag.Options = options;

            return View(option);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOption(ProductOptionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = new Product_Option
                {
                    Name = viewModel.name,
                    Description = viewModel.description,
                    ProductId = viewModel.productId,
                    PreviewImageId = viewModel.previewImageId,
                    Status = viewModel.status,
                    Files = new List<File>(),
                    CreateUserId = User.Identity.GetUserId(),
                    CreateTime = DateTime.Now
                };

                if (viewModel.variants != null)
                    foreach (var variant in viewModel.variants)
                    {
                        foreach (var variantId in variant.Value)
                        {
                            var variantModel = db.Product_Variants.Find(variantId);
                            if (variantModel != null)
                                variantModel.Product_Options.Add(model);
                        }
                    }

                if (viewModel.imageFiles != null)
                    foreach (var file in viewModel.imageFiles)
                    {
                        var fileModel = db.Files.Find(file);
                        if (fileModel != null)
                            model.Files.Add(fileModel);
                    }

                if (viewModel.details != null)
                    foreach (var pair in viewModel.details)
                    {
                        if (pair.Key != string.Empty && pair.Value != string.Empty)
                        {
                            var detailModel = new Product_Options_Details
                            {
                                Name = pair.Key,
                                Value = pair.Value,
                            };
                            db.Product_Options_Details.Add(detailModel);
                            model.Product_Options_Details.Add(detailModel);
                        }
                    }

                db.Product_Options.Add(model);
                db.SaveChanges();
                return RedirectToAction("CreateOption", new { productId = model.ProductId, success = true, successObjectName = model.Name });
            }

            var variantCategories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProductVariant);
            ViewBag.VariantCategories = variantCategories;

            var product = db.Products.Find(viewModel.productId);
            var options = product.Product_Options;
            ViewBag.Options = options;

            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OptionId">Option Id</param>
        /// <returns></returns>
        public ActionResult EditOption(int? OptionId)
        {
            if (OptionId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var option = db.Product_Options.Find(OptionId);
            if (option == null)
                return HttpNotFound();

            var viewModel = new ProductOptionViewModel
            {
                id = option.Id,
                description = option.Description,
                details = option.Product_Options_Details.ToDictionary(o => o.Name, o => o.Value),
                imageFiles = option.Files.Select(o => o.Id).ToArray(),
                name = option.Name,
                previewImage = option.PreviewImage != null ? option.PreviewImage.FullFileName : string.Empty,
                previewImageId = option.PreviewImageId,
                productId = option.ProductId,
                productName = option.Product.name,
                status = option.Status,
                variants = new Dictionary<string, int[]>()
            };

            foreach (var item in productVariantTypes)
            {
                var variants = new List<Product_Variant>();
                foreach (var variant in option.Product_Variants)
                {
                    if (variant.VariantType == item)
                        variants.Add(variant);
                }
                viewModel.variants.Add(item.ToString(), variants.Select(o => o.Id).ToArray());
            }

            var variantCategories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProductVariant);
            ViewBag.VariantCategories = variantCategories;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOption(ProductOptionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = db.Product_Options.Find(viewModel.id);
                model.Name = viewModel.name;
                model.Description = viewModel.description;
                model.ProductId = viewModel.productId;
                model.PreviewImageId = viewModel.previewImageId;
                model.Status = viewModel.status;

                model.Product_Variants.Clear();
                if (viewModel.variants != null)
                    foreach (var variant in viewModel.variants)
                    {
                        foreach (var variantId in variant.Value)
                        {
                            var variantModel = db.Product_Variants.Find(variantId);
                            if (variantModel != null)
                                variantModel.Product_Options.Add(model);
                        }
                    }

                model.Files.Clear();
                if (viewModel.imageFiles != null)
                    foreach (var file in viewModel.imageFiles)
                    {
                        var fileModel = db.Files.Find(file);
                        if (fileModel != null)
                            model.Files.Add(fileModel);
                    }

                model.Product_Options_Details.Clear();
                if (viewModel.details != null)
                    foreach (var pair in viewModel.details)
                    {
                        if (pair.Key != string.Empty && pair.Value != string.Empty)
                        {
                            var detailModel = new Product_Options_Details
                            {
                                Name = pair.Key,
                                Value = pair.Value,
                            };
                            db.Product_Options_Details.Add(detailModel);
                            model.Product_Options_Details.Add(detailModel);
                        }
                    }
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() > 0)
                    return RedirectToAction("CreateOption", new { productId = model.ProductId });
            }
            var variantCategories = db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProductVariant);
            ViewBag.VariantCategories = variantCategories;

            return View(viewModel);
        }

        [HttpDelete]
        public JsonResult DeleteOption(int? id)
        {
            object result;
            try
            {
                var option = db.Product_Options.Find(id);
                var product = option.Product;
                foreach (var file in option.Files)
                {
                    option.Files.Remove(file);
                    db.Files.Remove(file);
                    Uploader.DeleteFile(file, this);
                }

                if (option.PreviewImage != null)
                {
                    db.Files.Remove(option.PreviewImage);
                    Uploader.DeleteFile(option.PreviewImage, this);
                }

                product.Product_Options.Remove(option);
                db.Product_Options.Remove(option);
                result = db.SaveChanges() > 0 ? "Success" : "Fail";
            }
            catch (Exception ex)
            {
                result = ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}