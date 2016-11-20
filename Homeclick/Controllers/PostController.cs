using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homeclick.Models;
using PagedList;
using PagedList.Mvc;
using System.Collections;
using System.Reflection;

namespace Homeclick.Controllers
{
    public class PostController : Controller
    {
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        // GET: Post
        public ActionResult Index()
        {
            var categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.Postcat).ToList();
            ViewBag.categories = categories;
            return View();
        }

        public ActionResult Category(int category_id)
        {
            var posts = GetPostByCategory(category_id);
            return View(posts);
        }

        public ActionResult _SideBar()
        {
            var categories = db.Categories.Where(o => o.Category_typeId == (int)CategoryTypes.Postcat).ToList();
            return PartialView(categories);
        }

        public ActionResult Details(int category_id, int post_id)
        {
            var categoryParent = db.Categories.Find(category_id);
            var othersPost = GetPostByCategory(categoryParent.Id);
            ViewBag.OthersPost = othersPost.PickRandom(3);

            var post = db.Posts.Find(post_id);
            return View(post);
        }

        private IEnumerable<Post> GetPostByCategory(int category_id)
        {
            var posts = new List<Post>();
            var category = db.Categories.Where(o => o.Id == category_id).FirstOrDefault();
            var categoryChildren = category.GetCategoryChilds();
            if (categoryChildren.Count > 0)
            {
                foreach (var child in categoryChildren)
                {
                    var temp = child.getItems<Post>();
                    posts.AddRange(temp);
                }
            }
            else
            {
                posts = category.getItems<Post>().ToList();
            }
            return posts;
        }

        public ActionResult Sidebar()
        {
            IList<Category> rooms = db.Categories.Where<Category>(c => c.Category_type.name == "collection").ToList();
            return PartialView("Sidebar", rooms);
        }

        public IView listproduct { get; set; }
    }
}