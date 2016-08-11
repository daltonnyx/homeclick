using Homeclick.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;

namespace Homeclick.Controllers
{
    public class CategoryController : BaseController
    {
        public override CategoryTypes CategoryType { get { return CategoryTypes.Model; } }

        public ActionResult Typologies()
        {
            var typologies = db.Categories.Where(c => c.Category_typeId == (int)CategoryTypes.Typology).ToList();
            return View(typologies);
        }
    }

}