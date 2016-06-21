using Homeclick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homeclick.Controllers
{
    public class LienheController : Controller
    {
        // GET: Lienhe
        public ActionResult Index()
        {
            var rd = new Random();
            ViewBag.CaptchaStart = rd.Next(1, 10);
            ViewBag.CaptchaEnd = rd.Next(1, 10);

            return View();
        }

        [HttpPost]
        public ActionResult Feedback(Feedback f)
        {
            if (ModelState.IsValid && f.CaptchaResult == ((int)ViewBag.CaptchaStart + (int)ViewBag.CaptchaEnd))
            {

            }

            var rd = new Random();
            ViewBag.CaptchaStart = rd.Next(1, 10);
            ViewBag.CaptchaEnd = rd.Next(1, 10);

            return View("Index");
        }
    }
}