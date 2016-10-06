using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCMS.Lib.Common;
using VCMS.Lib.Models;

namespace VCMS.Lib.Controllers
{
    [AuthorizationService]
    public abstract class BaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();

        public ApplicationDbContext GetDbContext()
        {
            return db;
        }

        public ActionResult _imageFile(string imageId)
        {
            var imageFile = db.Files.Find(imageId);
            if (imageFile != null)
            {
                return PartialView(imageFile);
            }
            return null;
        }
    }
}