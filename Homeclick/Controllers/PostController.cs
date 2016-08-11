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

        [HttpPost]
        public ActionResult FilterP(int page = 1)
        {
            string[] models = Request.Form.GetValues("Categories[Models][]");
            string[] typologies = Request.Form.GetValues("Categories[Typologies][]");
            var post = db.Posts.AsQueryable();
            if (models != null && models.Count<string>() > 0)
                post = from post1 in post
                       where post1.Categories.Where(c => models.Contains<string>(c.Id.ToString())).Count<Category>() > 0
                       select post1;
            if (typologies != null && typologies.Count<string>() > 0)
                post = from post1 in post
                       where post1.Categories.Where(c => typologies.Contains<string>(c.Id.ToString())).Count<Category>() > 0
                       select post1;
            ViewBag.Title = "Bài viết";
            return PartialView(post.OrderBy<Post, string>(c => c.title).ToPagedList(page, 6));
        }

        public ActionResult Sidebar()
        {
            IList<Category> rooms = db.Categories.Where<Category>(c => c.Category_type.name == "collection").ToList();
            return PartialView("Sidebar", rooms);
        }

        public IView listproduct { get; set; }
    }
}