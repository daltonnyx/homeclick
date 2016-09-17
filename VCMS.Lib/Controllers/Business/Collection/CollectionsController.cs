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
        public override ActionResult List()
        {
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
            return View(category);
        }

        public ActionResult Create(bool? success, int? successObjectName)
        {
            ViewBag.Categories = db.Categories.Where(o => o.Category_TypeId == (int)CategoryTypes.Collection && o.CategoryParents.Count > 0);
            ViewBag.Products = db.Products.Where(o => o.Status);
            return View(new CollectionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CollectionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = ViewModelToModel(viewModel);
                if (db.SaveChanges() > 0)
                    RedirectToAction("Create", new { success = true, successObjectName = model.Title });
            }
            ViewBag.Categories = db.Categories.Where(o => o.Category_TypeId == (int)CategoryTypes.Collection && o.CategoryParents.Count > 0);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Product id</param>
        /// <param name="quantity">Product quantity</param>
        /// <returns></returns>
        public ActionResult _product(int? id, int? quantity)
        {
            if (id == null)
                return new HttpNotFoundResult();

            var model = db.Products.Find(id);

            if (quantity == null)
                quantity = 1;

            ViewData["Quantity"] = quantity;
            return PartialView(model);
        }

        public new CollectionViewModel ModelToViewModel(Post model)
        {
            var viewModel = base.ModelToViewModel(model) as CollectionViewModel;
            viewModel.products = model.Post_Products.ToDictionary(o => o.ProductId.ToString(), o => o.Quantity);
            return viewModel;
        }

        public Post ViewModelToModel(CollectionViewModel viewModel)
        {
            var model = base.ViewModelToModel(viewModel);
            var discountAmount = new Post_Details
            {
                Name = "discount_amount",
                Value = viewModel.discountAmount != null ? viewModel.discountAmount.ToString() : "0",
            };
            model.Post_Details.Add(discountAmount);
            foreach (var obj in viewModel.products)
            {
                var product = db.Products.Find(obj.Key);
                if (product != null)
                {
                    var post_product = new Post_Product
                    {
                        ProductId = product.Id,
                        Quantity = obj.Value
                    };
                    model.Post_Products.Add(post_product);
                }
            }
            foreach (var fileId in viewModel.imageFiles)
            {
                var file = db.Files.Find(fileId);
                if (file != null)
                    model.Files.Add(file);
            }
            return model;
        }

        public JsonResult DataHandler(DTParameters param)
        {
            try
            {
                var posts = db.Posts;
                var dtsource = new List<dt_collection>();
                foreach (var post in posts)
                {
                    if (post.PostType == PostTypes.Collection)
                        dtsource.Add(new dt_collection
                        {
                            id = post.Id,
                            name = post.Title,
                        });
                }

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

    }
}
