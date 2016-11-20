using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VCMS.Lib.Common;
using VCMS.Lib.Models;

namespace VCMS.Lib.Controllers
{
    using static ConstantKeys;

    public class CustomFieldsController : BaseController
    {
        public IQueryable<Category> GetCategories()
        {
            return db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.CustonFields);
        }

        public ActionResult List()
        {
            ViewData[CATEGORIES] = GetCategories();
            return View();
        }

        #region[Datatables]
        private IQueryable<CustomField> FilterDbSource(Dictionary<string, string> args)
        {
            var customsFields = db.CustomFields.AsQueryable();
            if (args != null)
                foreach (var arg in args)
                {
                    if (arg.Key.Contains(CATEGORY + "-"))
                    {
                        int value;
                        if (int.TryParse(arg.Value,out value))
                            customsFields = customsFields.Where(o => o.CategoryId == value);
                    }
                }
            return customsFields;
        }

        private DTResult<T> GetDtResult<T>(DTParameters param, Dictionary<string, string> args) where T : Dictionary<string, object>, new()
        {
            var dtsource = new List<T>();
            var dbSource = FilterDbSource(args);
            foreach (var source in dbSource)
            {
                dtsource.Add(new T
                {
                    {"name", source.Name },
                    {"label", source.Label },
                    {"type", source.FieldType.ToString() },
                    {"pHolder", source.PlaceHolder },
                    {"status", source.Status }
                });
            }

            return JDatatables<T>.GetDTResult(param, dtsource);
        }

        public JsonResult DataHandler(DTParameters param, Dictionary<string, string> args)
        {
            try
            {
                var result = GetDtResult<Dictionary<string, object>>(param, args);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        #endregion
    }
}
