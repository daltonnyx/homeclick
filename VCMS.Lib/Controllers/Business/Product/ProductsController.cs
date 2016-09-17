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
using VCMS.Lib.Models.Business.Datatables;

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
            ViewBag.Typologies = Typologies;
            ViewBag.Colors = Colors;
            ViewBag.Rooms = Rooms;
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            }

            ViewBag.Typologies = Typologies;
            ViewBag.Colors = Colors;
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
            ViewBag.Colors = Colors;
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
            ViewBag.Colors = Colors;
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

        public ActionResult _ImageFiles(string fileId, int colorId, bool? isSelected, int[] colors)
        {
            var model = db.Files.Find(fileId);
            var iColors = new List<Product_Variant>();
            if (colors != null)
                foreach (var str in colors)
                {
                    var color = db.Product_Variants.Find(str);
                    iColors.Add(color);
                }

            ViewBag.Selected = (isSelected != null) ? isSelected : false;
            ViewBag.SelectedColor = colorId;
            ViewBag.Colors = iColors;

            return PartialView(model);
        }

        public IEnumerable<Product_Variant> Colors
        {
            get
            {
                var variants = db.Product_Variants;
                var colors = new List<Product_Variant>();
                foreach (var variant in variants)
                {
                    if (variant.VariantType == ProductVarianTypes.Color)
                        colors.Add(variant);
                }
                return colors;
            }
        }

        public List<SelectListItem> Rooms
        {
            get
            {
                var rooms = db.Categories.Where(o => o.Category_TypeId == (int)CategoryTypes.Model)
                    .Select(o => new SelectListItem { Text = o.Name, Value = (o.Id).ToString() }).ToList();
                return rooms;
            }
        }

        public List<SelectListItem> Typologies
        {
            get
            {
                var rooms = db.Categories.Where(o => o.Category_TypeId == (int)CategoryTypes.Typology)
                    .Select(o => new SelectListItem { Text = o.Name, Value = (o.Id).ToString() }).ToList();
                return rooms;
            }
        }

        public JsonResult DataHandler(DTParameters param)
        {
            try
            {
                var products = db.Products;
                var dtsource = products.Select(o => new dt_product {
                    id = o.Id,
                    name = o.name,
                    img = o.Image.Id + o.Image.Extension,
                    status = o.Status
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

        public ActionResult GetTypologies(int[] roomIds)
        {
            var result = new List<object>();
            var typoLists = new List<IEnumerable<Category>>();
            foreach (var roomId in roomIds)
            {
                var cRoom = db.Categories.Find(roomId);
                if (cRoom?.Category_TypeId == (int)CategoryTypes.Model)
                {
                    var typologies = cRoom.CategoryChildren.Where(o => o.Category_TypeId == (int)CategoryTypes.Typology);
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

                //price
                if (viewModel.Weight != null)
                    dic.Add(ProductDetailTypes.Weight, viewModel.Weight);

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
                        default:
                            break;
                    }
                }
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
                    foreach (var ImageFile in viewModel.ImageFiles)
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
                    foreach (var imageFile in viewModel.ImageFiles)
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

            //Images
            foreach (var obj in viewModel.ImageFiles)
            {
                var file = db.Files.Find(obj.Key);
                if (file != null)
                {
                    if (obj.Key == viewModel.PreviewImage)
                        model.Image = file;
                    else if (!model.Files.Contains(file))
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

            return model;
        }

        public ProductViewModel ModelToViewModel(Product model)
        {
            var viewModel = new ProductViewModel
            {
                Name = model.name,
                PreviewImage = model.ImageId,
                Colors = model.Product_Variants.Select(o => o.Id).ToArray(),
                Description = model.content,
                IColors = model.Product_Variants,
                Id = model.Id,
                Status = model.Status
            };

            foreach (var variant in model.Product_Variants)
            {
                foreach (var file in variant.Files)
                {
                    if (model.Files.Contains(file))
                        if (!viewModel.ImageFiles.ContainsKey(file.Id))
                            viewModel.ImageFiles.Add(file.Id, variant.Id);
                }
            }

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
    }
}