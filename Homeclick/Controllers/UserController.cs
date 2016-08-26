using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Homeclick.Models;


namespace Homeclick.Controllers
{
    public class UserController : Controller
    {
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        //
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveCanvas(string data)
        {
            if(User.Identity.GetUserId() != null)
            {
                //save action goes here
                string jsonData = data;
                User user = db.Users.Find(Convert.ToInt32(User.Identity.GetUserId()));
                Canva canvas = db.Canvas.Create();
                canvas.User = user;
                canvas.json_data = jsonData;
                db.SaveChanges();
                Response.StatusCode = 202;
                return Content(canvas.Id.ToString());
            }
            else
            {
                Response.StatusCode = 403;
                return Content(string.Empty);
            }
        }
    }
}