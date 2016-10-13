using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Datatables;
using VCMS.Lib.Common;

namespace VCMS.Lib.Controllers
{
    using static ConstantKeys;

    public class CategoriesController : BaseController
    {
        public IQueryable<Category_Type> GetAllType()
        {
            return db.Category_types;
        }

        public virtual ActionResult List()
        {
            ViewData[CATEGORY_TYPES] = GetAllType();
            return View();
        }

        #region[Datatables]
        private IQueryable<Category> FilterDbSource(Dictionary<string, string> args)
        {
            var categories = db.Categories.AsQueryable();
            if (args != null)
                foreach (var arg in args)
                {
                    if (arg.Key.Contains(CATEGORY_TYPE))
                    {
                        int value;
                        if (int.TryParse(arg.Value, out value))
                            categories = categories.Where(o => o.CategoryTypeId == value);
                    }
                }
            return categories;
        }

        private DTResult<T> GetDtResult<T>(DTParameters param, Dictionary<string, string> args) where T : Dictionary<string, object>, new()
        {
            var dtsource = new List<T>();
            var dbSource = FilterDbSource(args);

            var hierarchyNodes = dbSource.AsHierarchy(o => o.Id, o => o.CategoryParentId);
            var sortedNodes = hierarchyNodes.OrderBy(o => o.Entity.Order);
            var ordered = new List<HierarchyNode<Category>>();
            foreach (var node in sortedNodes)
            {
                ordered.AddRange(node.NodesToListOrdered());
            }

            foreach (var item in ordered)
            {
                dtsource.Add(new T
                {
                    {"name", item.Entity.Name },
                    {"description", item.Entity.Description },
                    {"level", item.Depth - 1}
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
