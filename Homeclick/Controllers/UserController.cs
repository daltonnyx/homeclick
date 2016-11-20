using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Homeclick.Models;
using System.Web.Script.Serialization;

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

        public ActionResult SaveCanvas(int? id, string data, string name)
        {
            if(User.Identity.GetUserId() != null)
            {
                if(id == null) { 
                    //save action goes here
                    string jsonData = data;
                    User user = db.Users.Find(Convert.ToInt32(User.Identity.GetUserId()));
                    Canva canvas = db.Canvas.Create();
                    canvas.User = user;
                    canvas.Name = name;
                    canvas.UpdatedDate = DateTime.Now;
                    canvas.json_data = jsonData;
                    db.Canvas.Add(canvas);
                    db.SaveChanges();
                    Response.StatusCode = 202;
                    return Content(canvas.Id.ToString());
                }
                else
                {
                    Canva canvas = db.Canvas.Find(id);
                    canvas.Name = name;
                    canvas.json_data = data;
                    canvas.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    Response.StatusCode = 202;
                    return Content(canvas.Id.ToString());
                }
            }
            else
            {
                Response.StatusCode = 403;
                return Content(string.Empty);
            }
        }

        public ActionResult LoadCanvas(int? id)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            if (id == null) { 
            if (User.Identity.GetUserId() != null)
                {
                    //load action goes here
                    User user = db.Users.Find(Convert.ToInt32(User.Identity.GetUserId()));
                    IList<object> canvas = db.Canvas.Where(c => c.User.ID == user.ID).Select(c => new { id = c.Id, Name = c.Name, UpdatedDate = c.UpdatedDate }).ToList<dynamic>()
                        //Convert date to string
                        .Select(o => new { id = o.id, name = o.name, UpdatedDate = o.UpdatedDate.ToString("dd-MM-yyyy")}).ToList<object>();
                    
                    //IList<object> jsObj = new List<object>();
                    //foreach(Canva c in canvas)
                    //{
                    //    jsObj.Add(new { id=c.Id, name = c.name, UpdatedDate = c.UpdatedDate.Value.ToString("dd-MM-yyyy")});
                    //}
                    Response.StatusCode = 202;
                    return Content(jsonSerializer.Serialize(canvas));
                }
                else
                {
                    Response.StatusCode = 403;
                    return Content(string.Empty);
                }
            }
            else
            {
                Canva canva = db.Canvas.Find(id);
                if(canva == null)
                {
                    Response.StatusCode = 404;
                    return Content("File not found!");
                }
                else
                {
                    var jsdata = new { id = canva.Id, json_data = canva.json_data, name = canva.Name };
                    Response.StatusCode = 200;
                    return Content(jsonSerializer.Serialize(jsdata));
                }
            }
        }

        public ActionResult AddWishlist(int id)
        {
            Response.ContentType = "text/plaintext";
            if (User.Identity.GetUserId() != null)
            {
                int userId = Convert.ToInt32(User.Identity.GetUserId());
                var wishlists = db.Wishlists.Where(w => w.User.ID == userId && w.Product.Id == id);
                //
                if(wishlists.Count() == 0)
                {
                    Wishlist wishlist = db.Wishlists.Create();
                    wishlist.Product = db.Products.Find(id);
                    wishlist.User = db.Users.Find(userId);
                    wishlist.title = "#" + DateTime.Now.ToString("dd-MM-yyyy") + "-" + wishlist.Product.name;
                    wishlist.created_date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    db.Wishlists.Add(wishlist);
                    db.SaveChanges();
                    Response.StatusCode = 202;
                    return Content(wishlist.Id.ToString());
                }
                else
                {
                    Wishlist wishlist = wishlists.First();
                    Response.StatusCode = 204;
                    return Content(wishlist.Id.ToString());
                }
                
            }
            else
            {
                Response.StatusCode = 403;
                return Content(string.Empty);
            }
        }

        public ActionResult LoadWishlists()
        {
            if (User.Identity.GetUserId() != null)
            {
                int userid = Convert.ToInt32(User.Identity.GetUserId());
                IList<Wishlist> wishlists = db.Wishlists.Where(w => w.UserId == userid).ToList();
                return PartialView(wishlists);
            }
            else
            {
                return Content("<p>Đăng nhập để xem Wishlist!</p>");
            }
        }
    }
}