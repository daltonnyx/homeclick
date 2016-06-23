using Homeclick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
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
        public ActionResult Feedback(Feedback f, int CaptchaValue, int? CaptchaAnswer)
        {
            if (CaptchaAnswer != null)
            {
                if (CaptchaValue != CaptchaAnswer)
                {
                    TempData["CaptchaError"] = "Sai kết quả";
                }
                else if (ModelState.IsValid)
                {
                    TempData["Notification"] = "Gửi thông tin phản hồi thành công, chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất. Xin cảm ơn!";

                    f.Send("temp@gmail.com");
                }
            }
            else
                TempData["CaptchaError"] = "Cần nhập vào kết quả";

            var rd = new Random();
            ViewBag.CaptchaStart = rd.Next(1, 10);
            ViewBag.CaptchaEnd = rd.Next(1, 10);

            return View("Index");
        }
    }
}