using System.Web.Mvc;
using VCMS.Lib.Common;

namespace VCMS.Lib.Controllers
{
    public class PagesController : BaseController
    {
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}