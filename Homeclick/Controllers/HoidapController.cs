using Homeclick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homeclick.Controllers
{
    public class HoidapController : Controller
    {
        // GET: Hoidap
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AskedQuestion(Feedback f)
        {
            if (ModelState.IsValid)
            {
                TempData["Notification"] = "Câu hỏi của bạn đã được gởi đi, chúng tôi sẽ trả lời thắc mắc của bạn trong thời gian sớm nhất. Xin cảm ơn!";

                f.Send("temp@gmail.com");
            }
            return View("Index");
        }
    }
}