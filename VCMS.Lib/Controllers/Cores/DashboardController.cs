using System.Web.Mvc;
using VCMS.Lib.Common;

namespace VCMS.Lib.Controllers
{
    public class DashboardController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}