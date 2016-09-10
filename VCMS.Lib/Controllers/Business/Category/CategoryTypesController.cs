using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Business;
using VCMS.Lib.Models.Business.Datatables;

namespace VCMS.Lib.Controllers
{
    public class CategoryTypesController : BaseController
    {
        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var categoryType = db.Category_types.Find(id);
            if (categoryType == null)
                return HttpNotFound();

            var categories = db.Categories.Where(o => o.Category_typeId == id);
            ViewBag.Categories = categories;
            return View(categoryType);
        }
    }
}
