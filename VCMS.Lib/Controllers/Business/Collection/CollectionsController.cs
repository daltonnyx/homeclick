using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class CollectionsController : PostsController
    {
        private IEnumerable<Category> GetCategories()
        {
            return db.Categories.Where(o => o.Category_TypeId == (int)CategoryTypes.Collection);
        }

        public override ActionResult List()
        {
            ViewBag.Categories = GetCategories();
            return View();
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        public override ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var category = db.Categories.Find(id);
            if (category == null)
                return HttpNotFound();
            ViewBag.Categories = GetCategories();
            return View(category);
        }

        public ActionResult Create(bool? success, string successObjectName)
        {
            ViewBag.Categories = GetCategories();
            ViewBag.Products = db.Products.Where(o => o.Status);
            ViewData["Success"] = success;
            ViewData["SuccessObjectName"] = successObjectName;
            return View(new CollectionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CollectionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = ViewModelToModel(viewModel);
                db.Posts.Add(model);
                if (db.SaveChanges() > 0)
                    return RedirectToAction("Create", new { success = true, successObjectName = model.Title });
            }
            ViewBag.Categories = GetCategories();
            ViewBag.Products = db.Products.Where(o => o.Status);
            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Post collection id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var post = db.Posts.Find(id);
            if (post == null)
                return HttpNotFound();
            var viewModel = ModelToViewModel(post);
            ViewBag.Categories = GetCategories();
            ViewBag.Products = db.Products.Where(o => o.Status);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CollectionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = ViewModelToModel(viewModel);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                if (db.SaveChanges() > 0)
                    return RedirectToAction("List");
            }
            ViewBag.Categories = GetCategories();
            ViewBag.Products = db.Products.Where(o => o.Status);
            return View(viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">File id</param>
        /// <returns></returns>
        public ActionResult _imageFile(string id)
        {
            var model = db.Files.Find(id);
            return PartialView(model);
        }

        public new CollectionViewModel ModelToViewModel(Post model)
        {
            var viewModel = new CollectionViewModel
            {
                postId = model.Id,
                title = model.Title,
                excerpt = model.Excerpt,
                htmlContent = model.Content,
                previewImageId = model.ImageId,
                previewImage = model.ImageFile.FullFileName,
                categoryIds = model.Categories.Select(o => o.Id).ToArray(),
                status = model.Status
            };
            //viewModel.products = model.Post_Products.ToDictionary(o => o.ProductOptionId.ToString(), o => o.Quantity);
            viewModel.imageFiles = model.Files.ToDictionary(o => o.Id, o => o.Extension);
            return viewModel;
        }

        public Post ViewModelToModel(CollectionViewModel viewModel)
        {
            var model = base.ViewModelToModel(viewModel);

            //var modelPost_Products = model.Post_Products.ToDictionary(o => o.ProductOptionId.ToString(), o => o.Quantity);
            //var viewModelPost_Products = viewModel.products ?? new Dictionary<string, int>();

            /*/Products
            var except = modelPost_Products.Select(o => o.Key).Except(viewModelPost_Products.Select(o => o.Key));
            foreach (var obj in except)
            {
                var post_product = model.Post_Products.FirstOrDefault(o => o.ProductOptionId == int.Parse(obj));
                if (post_product != null)
                    model.Post_Products.Remove(post_product);   
            }

            foreach (var obj in viewModelPost_Products)
            {
                var product = db.Products.Find(int.Parse(obj.Key));
                if (product != null)
                {
                    var post_Product = model.Post_Products.FirstOrDefault(o => o.ProductOptionId == product.Id);
                    if (post_Product != null)
                    {
                        if (post_Product.Quantity != obj.Value)
                        {
                            post_Product.Quantity = obj.Value;
                            db.Entry(post_Product).State = System.Data.Entity.EntityState.Modified;
                        }

                    }
                    else
                    {
                        var post_product = new Post_Product
                        {
                            ProductOptionId = product.Id,
                            Quantity = obj.Value
                        };
                        model.Post_Products.Add(post_product);
                    }
                }
            }
            */

            //Files
            var modelFiles = model.Files;
            var viewModelFiles = viewModel.imageFiles?? new Dictionary<string, string>();
            foreach (var modelFile in modelFiles)
            {
                var found = false;
                foreach (var viewModelFile in viewModelFiles)
                {
                    if (modelFile.Id == viewModelFile.Key)
                    {
                        found = true;
                        viewModelFiles.Remove(viewModelFile.Key);
                        break;
                    }
                }
                if (!found)
                    model.Files.Remove(modelFile);
            }

            foreach (var imageFile in viewModelFiles)
            {
                var file = db.Files.Find(imageFile.Key);
                if (file != null && !model.Files.Contains(file))
                    model.Files.Add(file);
            }

            return model;
        }

        public ActionResult Delete(int? id)
        {
            var result = "";
            var collection = db.Posts.Find(id);
            if (collection != null)
                try
                {
                    db.Files.Remove(collection.ImageFile);
                    foreach (var file in collection.Files.ToList())
                    {
                        db.Files.Remove(file);
                    }
                    db.Posts.Remove(collection);
                    db.SaveChanges();
                    result = "Success";
                }
                catch (Exception ex)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Delete failure!");
                    var innerException = ex.InnerException.InnerException;
                    if (innerException as SqlException != null)
                    {
                        sb.AppendLine("Error code: " + ((ex.InnerException.InnerException) as SqlException).Number);
                    }
                    result = sb.ToString();
                }
            else
                result = "Incorrect ID!";

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataHandler(DTParameters param, int? category_id)
        {
            try
            {
                var categories = (category_id != null) ?
                    db.Categories.Where(o => o.Id == category_id) :
                    db.Categories.Where(o => o.Category_TypeId == (int)CategoryTypes.Collection);

                var posts = new List<Post>();
                foreach (var category in categories)
                {
                    posts = posts.Union(category.GetAllPost()).ToList();
                }

                var dtsource = posts.Select(o => new dt_collection {
                    id = o.Id,
                    name = o.Title,
                    categories = o.Categories.Select(e => e.Name)
                });

                List<string> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<dt_collection, bool>> pre = (p => (search == null || (p.name != null && p.name.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.name != null && p.name.ToLower().Contains(columnSearch[1].ToLower()))));

                var resultSet = new ResultSet();
                List<dt_collection> data = resultSet.GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<dt_collection> result = new DTResult<dt_collection>
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

        #region [Product Option]
        public ActionResult AddOption(int postId, bool? success, string successObjectName)
        {
            var post = db.Posts.Find(postId);
            if (post == null)
                return HttpNotFound();

            ViewBag.Products = db.Products.Where(o => o.Status);
            ViewBag.Options = post.Post_ProductOptions;

            ViewData["Success"] = success;
            ViewData["SuccessObjectName"] = successObjectName;
            return View(new PostProductOptionViewModel { postId = postId, postName = post.Title});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOption(PostProductOptionViewModel viewModel)
        {
            var post = db.Posts.Find(viewModel.postId);
            ViewBag.Products = db.Products.Where(o => o.Status);
            ViewBag.Options = post.Post_ProductOptions;

            if (ModelState.IsValid)
            {
                var option = db.Product_Options.Find(viewModel.optionId);
                if (post != null && option != null)
                {
                    var model = new Post_Product
                    {
                        PostId = viewModel.postId,
                        Quantity = viewModel.quantity,
                        ProductOption = option,
                        CreateUserId = User.Identity.GetUserId(),
                        CreateTime = DateTime.Now
                    };
                    db.Post_Products.Add(model);
                    post.Post_ProductOptions.Add(model);
                    db.SaveChanges();

                    ModelState.Clear();
                    return RedirectToAction("AddOption", new { postId = viewModel.postId, success = true, successObjectName = model.ProductOption.Name});
                }
            }
            return View(viewModel);
        }

        public ActionResult EditOption(int postProductOptionId)
        {
            var postProductOption = db.Post_Products.Find(postProductOptionId);
            if (postProductOption == null)
                return HttpNotFound();

            var viewModel = new PostProductOptionViewModel()
            {
                postId = postProductOption.PostId,
                optionId = postProductOption.ProductOptionId,
                quantity = postProductOption.Quantity
            };
            ViewBag.Products = db.Products.Where(o => o.Status);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOption(PostProductOptionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var postProductOption = db.Post_Products.Find(viewModel.id);
                var option = db.Product_Options.Find(viewModel.optionId);
                if (postProductOption != null )
                {
                    postProductOption.Quantity = viewModel.quantity;
                    postProductOption.ProductOptionId = option.Id;
                    db.SaveChanges();

                    return RedirectToAction("AddOption", new { postId = viewModel.postId });
                }
            }
            ViewBag.Products = db.Products.Where(o => o.Status);
            return View(viewModel);
        }

        [HttpGet]
        public JsonResult RemoveOption(int postId, int optionId)
        {
            var Post_Product = db.Post_Products.Find(optionId);
            var post = db.Posts.Find(postId);
            db.Post_Products.Remove(Post_Product);
            var result = db.SaveChanges();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOptions(int postId, int productId)
        {
            var product = db.Products.Find(productId);
            dynamic result = null;
            if (product != null)
            {
                var post = db.Posts.Find(postId);
                var postOptions = post.Post_ProductOptions.Select(o => o.ProductOptionId);
                result = product.Product_Options.Where(o => o.Status && !postOptions.Contains(o.Id)).Select(o => new { id = o.Id, name = o.Name});
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
