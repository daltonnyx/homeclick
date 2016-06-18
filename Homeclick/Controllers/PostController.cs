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
            return View();
        }

        public ActionResult Detail(int id)
        {
            Category category = db.Categories.Where(c => c.Id == id).First();
            return View("Detail", category);
        }

        public ActionResult ListPost(int? page)
        {
            //    //Số sản phẩm trên trang
            int product_number = 8;
            ////    //tao số biến trong trang
            int pageNumber = (page ?? 1);
            IList<Post> listpost;
            listpost = db.Posts.ToList();
            if (Request.IsAjaxRequest())
            {
                return View(listpost.OrderBy(n => n.title).ToPagedList(pageNumber, product_number));
            }
            return View("ListPost", listpost.OrderBy(n => n.title).ToPagedList(pageNumber, product_number));
        }

        public ViewResult Post_Detail(int? id)
        {
            var post = db.Posts.Find(id);
            return View(post);
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