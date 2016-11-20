using Homeclick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homeclick.Controllers
{
    public abstract class BaseController : Controller
    {
        public vinabits_homeclickEntities db = new vinabits_homeclickEntities();
        public abstract CategoryTypes CategoryType { get; }

        public virtual ActionResult _Sidebar()
        {
            var categories = db.Categories.Where(c => c.Category_typeId == (int)CategoryType).ToList();
            return PartialView(categories);
        }
    }
}