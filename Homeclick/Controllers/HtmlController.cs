using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homeclick.Models;

namespace Homeclick.Controllers
{
    public class HtmlController : Controller
    {
        //
        // GET: /Html/
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Phong()
        {
            IList<Category> rooms = db.Categories.Where<Category>(c => c.Category_type.name == "collection").ToList();
            ViewBag.slides = rooms.Select<Category, string>(c => c.Category_detail.Where<Category_detail>(d => d.name == "image").First().value);
            return View(rooms);
            return View();
        }
        public ActionResult Keotha()
        {
            return View();
        }
        public ActionResult Thietke1()
        {
            return View();
        }
        public ActionResult Thietke2()
        {
            return View();
        }
        public ActionResult AllTypologies()
        {
            return View();
        }
        public ActionResult Lienhe()
        {
            return View();
        }
        public ActionResult Thietkemau()
        {
            return View();
        }
        public ActionResult Hoidap()
        {
            return View();
        }
        public ActionResult Faqs()
        {
            return View();
        }

        public ActionResult Thietkenoithat()
        {
            return View();
        }

        public ActionResult Thietkekientruc()
        {
            return View();
        }
        public ActionResult Sanphamkhac()
        {
            return View();
        }
        public ActionResult Chair()
        {
            return View();
        }

        public ActionResult Power_2()
        {
            return View();
        }
        public ActionResult Blog()
        {
            return View();
        }
        public ActionResult SingleBlog()
        {
            return View();
        }
        public ActionResult Canthietke()
        {
            return View();
        }
        public ActionResult Tuthietke()
        {
            return View();
        }

        public ActionResult duphong1()
        {
            return View();
        }
        public ActionResult duphong2()
        {
            return View();
        }
    }

}