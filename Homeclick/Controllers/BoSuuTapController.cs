using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homeclick.Models;
using PagedList;

namespace Homeclick.Controllers
{
    public class BoSuuTapController : Controller
    {
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        //
        // GET: /Bosutap/
        public ActionResult Index()
        {
            /*
            IList<Category> rooms = db.Categories.Where<Category>(c => c.Category_type.name == "collection").ToList();
            ViewBag.slides = rooms.Select<Category, string>(c => c.Category_detail.Where<Category_detail>(d => d.name == "image").First().value);
            return View(rooms);*/
            var maxItem = 6;
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

            var query = string.Format("SELECT * FROM dbo.ProductsLayoutsLink WHERE Layout = '{0}'", collection_id);
            var tableItems = db.Database.SqlQuery<ProductsLayoutsLink>(query).ToList();

            var products = new List<Product>();

            foreach (var product in tableItems)
            {
                var temp_p = db.Products.Find(product.Product);
                var temp_t = temp_p.ToArray();
                var detail = temp_t["Product_detail"] as Dictionary<string, object>;

                products.Add(temp_p);

                product.ProductName = temp_p.name;
                product.ProductValue = Convert.ToInt32(detail["gia"]);
                product.TotalValue = product.Total * product.ProductValue;
            }

            ViewBag.Products = products;
            ViewBag.TableItems = tableItems;

            var model = db.ProjectLayout_Collections.Find(collection_id);
            return PartialView(model);
        }

        public ActionResult ListCategory()
        {
            List<Category> listcategory = db.Categories.ToList();
            return PartialView(listcategory);
        }
        public ActionResult CategoryProduct(int categoryid)
        {
            List<Product> listproduct;
            if (categoryid == 0)
            {
                 listproduct = db.Products.ToList();
            }
            else
            {
                Category category = db.Categories.Where(n => n.Id == categoryid) as Category;
                 listproduct = db.Products.Where(n => n.Categories.Contains(category)).ToList();

                //Dictionary<string, object> details = arrayItem["Product_detail"] as Dictionary<string, object>;
                //@item.Product_detail.Count();
                
            }
            return View(listproduct);
        }
        public ActionResult Sidebar()
        {
            IList<Category> rooms = db.Categories.Where<Category>(c => c.Category_type.name == "collection").ToList();
            
            return PartialView("Sidebar", rooms);
        }

        public ActionResult List(int category_id, int? page)
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

        public IView listproduct { get; set; }
    }
}