using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using VCMS.Lib.Models;
using VCMS.Lib.Common;

namespace Homeclick.Controllers
{
    public class CollectionController : Controller
    {
        public CategoryTypes CategoryType { get { return CategoryTypes.Collection; } }
        public ApplicationDbContext db = new ApplicationDbContext();

        public virtual ActionResult _Sidebar()
        {
            var categories = db.Categories.Where(c => c.Category_TypeId == (int)CategoryType).ToList();
            return PartialView(categories);
        }

        public ActionResult Index()
        {
            var maxItem = 3;
            var dic = new Dictionary<Category, IEnumerable<Post>>();
            var categories = db.Categories.Where(o => o.Category_TypeId == (int)CategoryTypes.Collection);
            foreach (var category in categories)
            {
                var posts = category.GetAllPost().PickRandom(maxItem);
                dic.Add(category, posts);
            }

            ViewBag.Slides = db.Categories.Find((int)SlideTypes.Collection).Slides;
            return View(dic);
        }

        public ActionResult Category(int category_id, int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            var category = db.Categories.Find(category_id);
            var list = category.GetAllPost();
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Detail(int category_id, int collection_id)
        {
            var category = db.Categories.Find(category_id);
            var collection = db.Posts.Find(collection_id);
            if (category == null || collection == null)
                return HttpNotFound();

            //some other collection
            category.Posts.Remove(collection);
            ViewBag.OtherCollections = category.Posts.PickRandom(3);

            return PartialView(collection);
        }
    }
}