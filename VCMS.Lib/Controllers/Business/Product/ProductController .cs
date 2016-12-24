using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        #region[Methods]
        private IEnumerable<Category> GetCategories()
        {
            return db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.Model);
        }

        private IEnumerable<CustomField> GetCustomFields()
        {
            return db.CustomFields.Where(o => o.CategoryId == (int)CustomFieldType.Product);
        }

        private IEnumerable<Product_Type> GetProductTypes()
        {
            return db.Product_Types;
        }

        private List<Product_Detail> DicToDetails(Dictionary<string,object> dic, IEnumerable<CustomField> customfields)
        {
            var result = new List<Product_Detail>();
            foreach (var item in dic)
            {
                var pattern = @"(\[(?:.*?)\])";
                var fieldNames = System.Text.RegularExpressions.Regex.Split(item.Key, pattern).FirstOrDefault();
                var customfield = customfields.FirstOrDefault(o => o.Name == fieldNames.Split('-').FirstOrDefault());
                var detail = new Product_Detail
                {
                    Name = customfield.Name,
                    Type = customfield.ValueType,
                };
                var value = (item.Value as string[]);
                if (customfield.Type == (int)FieldTypes.File)
                    detail.FileId = value.FirstOrDefault();
                else if (customfield.Type == (int)FieldTypes.Select)
                    detail.EnumId = int.Parse(value.FirstOrDefault());
                else
                    detail.Value = value.FirstOrDefault() != string.Empty ? value.FirstOrDefault() : null;

                result.Add(detail);
            }
            return result;
        }
        #endregion

        private List<ProductVarianTypes> productVariantTypes
        {
            get
            {
                var productVariantTypes = new List<ProductVarianTypes>();
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

        public ActionResult Create()
        {
            ViewData[ConstantKeys.CATEGORIES] = this.GetCategories();
            ViewData[ConstantKeys.CUSTOM_FIELDS] = this.GetCustomFields();
            ViewData[ConstantKeys.PRODUCT_TYPES] = this.GetProductTypes();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model)
        {
            var messageCollection = new List<Message>();
            var customfields = this.GetCustomFields();
            if (ModelState.IsValid)
            {
                //Add product to selected categories
                foreach (var item in model.SelectedCategories.Where(o => o.Value))
                {
                    var category = db.Categories.Find(int.Parse(item.Key));
                    category.Products.Add(model);
                }

                var userId = User.Identity.GetUserId();
                var currentTimeUtc = DateTime.UtcNow;

                //Add details
                foreach (var detail in model.Product_Details)
                {
                        detail.CreateUserId = userId;
                        detail.CreateTime = currentTimeUtc;
                        db.Entry(detail).State = System.Data.Entity.EntityState.Added;
                }

                model.CreateUserId = User.Identity.GetUserId();
                model.CreateTime = DateTime.UtcNow;

                db.Products.Add(model);
                db.SaveChanges();

                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Create successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;
                return RedirectToAction("Edit", new { id = model.Id });
            }

            ViewData[ConstantKeys.CATEGORIES] = this.GetCategories();
            ViewData[ConstantKeys.CUSTOM_FIELDS] = customfields;
            ViewData[ConstantKeys.PRODUCT_TYPES] = this.GetProductTypes();
            return View(model);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = db.Products.Find(Convert.ToInt32(id));
            if (model == null)
                return HttpNotFound();

            ViewData[ConstantKeys.CATEGORIES] = this.GetCategories();
            ViewData[ConstantKeys.CUSTOM_FIELDS] = this.GetCustomFields();
            ViewData[ConstantKeys.PRODUCT_TYPES] = this.GetProductTypes();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            var messageCollection = new List<Message>();
            var customfields = this.GetCustomFields();
            if (ModelState.IsValid)
            {
                var modelTarget = db.Products.Find(model.Id);
                foreach (var item in model.SelectedCategories)
                {
                    var category = db.Categories.Find(int.Parse(item.Key));
                    if (category.Posts.Select(o => o.Id).Contains(modelTarget.Id))
                    {
                        if (!item.Value)
                            category.Products.Remove(modelTarget);
                    }
                    else if (item.Value)
                        category.Products.Add(modelTarget);
                }

                var userId = User.Identity.GetUserId();
                var currentTimeUtc = DateTime.UtcNow;

                //Remove absent details
                var existDetailId = modelTarget.Product_Details.Select(o => o.Id);
                var exceptDetailIds = existDetailId.Except(model.Product_Details.Select(o => o.Id)).ToList();
                foreach (var exceptDetailId in exceptDetailIds)
                {
                    var details = db.Product_details.Find(exceptDetailId);
                    if (details != null)
                        db.Entry(details).State = System.Data.Entity.EntityState.Deleted;
                }

                //Modify or add details
                foreach (var detail in model.Product_Details)
                {
                    if (detail.Id != 0)
                    {
                        var detailTarget = modelTarget.Product_Details.FirstOrDefault(o => o.Id == detail.Id);
                        db.Entry(detailTarget).CurrentValues.SetValues(detail);
                        db.Entry(detailTarget).Property("ProductId").IsModified = false;
                        db.Entry(detailTarget).Property("CreateUserId").IsModified = false;
                        db.Entry(detailTarget).Property("CreateTime").IsModified = false;
                    }
                    else
                    {
                        var detailTarget = modelTarget.Product_Details.FirstOrDefault(o => o.Name == detail.Name);
                        if (detailTarget != null)
                        {
                            detailTarget.FileId = detail.FileId;
                            db.Entry(detailTarget).Property("ProductId").IsModified = false;
                            db.Entry(detailTarget).Property("CreateUserId").IsModified = false;
                            db.Entry(detailTarget).Property("CreateTime").IsModified = false;
                        }
                        else
                        {
                            detail.CreateUserId = userId;
                            detail.CreateTime = currentTimeUtc;
                            detail.ProductId = model.Id;
                            db.Entry(detail).State = System.Data.Entity.EntityState.Added;
                            modelTarget.Product_Details.Add(detail);
                        }
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

            ViewData[ConstantKeys.CATEGORIES] = this.GetCategories();
            ViewData[ConstantKeys.CUSTOM_FIELDS] = customfields;
            ViewData[ConstantKeys.PRODUCT_TYPES] = this.GetProductTypes();
            return View(model);
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

        #region [Options]
        private IEnumerable<Category> GetVariantTypes()
        {
            return db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProductVariant);
        }


        public ActionResult CreateOption(int? product_id)
        {
            if (product_id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = db.Products.Find(product_id);
            if (product == null)
                return HttpNotFound();

            ViewBag.VariantCategories = GetVariantTypes();
            ViewBag.Options = product.Product_Options;
            ViewData["ProductName"] = product.Name;
            return View();
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
                productName = option.Product.Name,
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

        #region[Datatables]
        private IQueryable<Product> FilterDbSource(Dictionary<string, string> args)
        {
            var result = db.Products.AsQueryable();
            if (args != null)
                foreach (var arg in args)
                {
                    if (arg.Key.Contains(ConstantKeys.CATEGORIES))
                    {
                        int value;
                        if (int.TryParse(arg.Value, out value))
                            result = result.Where(o => o.Categories.Select(c => c.Id).Contains(value));
                    }
                }
            return result;
        }

        private DTResult<T> GetDtResult<T>(DTParameters param, Dictionary<string, string> args) where T : Dictionary<string, object>, new()
        {
            var dtsource = new List<T>();
            var dbSource = FilterDbSource(args);
            foreach (var item in dbSource)
            {
                var options = item.Product_Options.Select(o => new Dictionary<string, object>()
                {
                    { ConstantKeys.ID, o.Id },
                    { ConstantKeys.NAME, o.Name },
                    { ConstantKeys.QUANTITY, o.Product_Options_Details.FirstOrDefault(d => d.Name == "so_luong" ).Value ?? "Stocking"}
                });

                var categories = item.Categories.Select(o => new Dictionary<string, object>()
                {
                    { ConstantKeys.ID, o.Id },
                    { ConstantKeys.NAME, o.Name },
                });

                dtsource.Add(new T
                {
                    {ConstantKeys.ID, item.Id },
                    {ConstantKeys.NAME, item.Name },
                    {ConstantKeys.PREVIEW_IMAGE, item.Image?.FullFileName },
                    {ConstantKeys.EXCERPT, item.Excerpt },
                    {ConstantKeys.PRODUCT_TYPE, item.ProductType?.Name },
                    {ConstantKeys.PRODUCT_OPTIONS, options },
                    {ConstantKeys.CATEGORIES, categories },
                    {ConstantKeys.FEATURE, item.Featured },
                    {ConstantKeys.STATUS, item.Status },
                    {ConstantKeys.AUTHOR, item.CreateUser?.UserName },
                    {ConstantKeys.TIME, item.CreateTime }
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