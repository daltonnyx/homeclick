using Microsoft.AspNet.Identity;
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
    public class ProductSaleController : SaleController
    {
        public override ActionResult List()
        {
            return base.List();
        }


        public JsonResult DataHandler(DTParameters param, Dictionary<string, object> args)
        {
            try
            {
                var sale = db.Sales.Where(o=> o.CategoryId == (int)SaleType.Product).ToList();
                if (args != null)
                    foreach (var arg in args)
                    {
                        if (arg.Key == "expired")
                            if (arg.Value is bool)
                                sale = !(bool)arg.Value ? sale.Where(o => o.EndDate < DateTime.Now).ToList() : sale.Where(o => o.EndDate > DateTime.Now).ToList();
                    }

                var dtsource = sale.Select(o => new dt_sale
                {
                    id = o.Id,
                    name = o.Name,
                    startdate = o.StartDate.ToString(),
                    enddate = o.EndDate.ToString(),
                    status = Convert.ToInt32(o.Status)
                }).ToList();

                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<dt_sale, bool>> pre = (p => (search == null || (p.name != null && p.name.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.name != null && p.name.ToLower().Contains(columnSearch[1].ToLower()))));

                var resultSet = new ResultSet();
                List<dt_sale> data = resultSet.GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<dt_sale> result = new DTResult<dt_sale>
                {
                    draw = param.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
