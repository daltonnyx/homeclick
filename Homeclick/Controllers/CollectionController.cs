using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homeclick.Models;
using PagedList;

namespace Homeclick.Controllers
{
    public class CollectionController : BaseController
    {
        public override CategoryTypes CategoryType { get { return CategoryTypes.Collection; } }

        public ActionResult Index()
        {
            var maxItem = 3;
            var dic = new Dictionary<string, IEnumerable<CollectionViewModel>>();
            var collectionCategories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.Collection);
            var allItem = new List<CollectionViewModel>();
            foreach (var collectionCategory in collectionCategories)
            {
                var collections = ModelHelper.GetObjectListByCategory<ProjectLayout_Collection>(collectionCategory.Id);
                var randomItems = collections.PickRandom(maxItem);
                var viewModel = new List<CollectionViewModel>();

                foreach (var item in randomItems)
                {
                    var temp = item.ToViewModel();
                    temp.categoryId = collectionCategory.Id;
                    viewModel.Add(temp);
                    allItem.Add(temp);
                }
                
                dic.Add(collectionCategory.name, viewModel);
            }
            ViewBag.Slides = allItem.PickRandom(5);
            return View(dic);
        }

        public ActionResult Detail(int category_id, int collection_id)
        {
            var collections = ModelHelper.GetObjectListByCategory<ProjectLayout_Collection>(category_id);
            var othercollectionsPick5 = collections.PickRandom(5);
            var otherCollectionViewModels = new List<CollectionViewModel>();
            foreach (var collection in othercollectionsPick5)
            {
                var temp = collection.ToViewModel();
                temp.categoryId = category_id;
                otherCollectionViewModels.Add(temp);
            }
            ViewBag.OtherCollections = otherCollectionViewModels;

            var query = string.Format("SELECT * FROM dbo.ProjectLayout_Collection_Product_Link WHERE ParentId = '{0}'", collection_id);
            var tableItems = db.Database.SqlQuery<ProjectLayout_Collection_Product_Link>(query).ToList();
            var products = new List<Product>();

            foreach (var product in tableItems)
            {
                var temp_p = db.Products.Find(product.ChildId);
                var temp_t = temp_p.ToArray();
                var detail = temp_t["Product_detail"] as Dictionary<string, object>;

                products.Add(temp_p);

                product.ProductName = temp_p.name;
                product.ProductValue = Convert.ToInt32(detail["gia"]);
                product.TotalValue = product.Quantity * product.ProductValue;
            }

            ViewBag.Products = products;
            ViewBag.TableItems = tableItems;

            var model = db.ProjectLayout_Collections.Find(collection_id);
            return PartialView(model);
        }

        public ActionResult Category(int category_id, int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            var list = ModelHelper.GetObjectListByCategory<ProjectLayout_Collection>(category_id);
            var ViewModel = new List<CollectionViewModel>();

            foreach (var item in list)
            {
                var temp = item.ToViewModel();
                temp.categoryId = category_id;
                ViewModel.Add(temp);
            }

            return View(ViewModel.ToPagedList(pageNumber, pageSize));
        }
    }
}