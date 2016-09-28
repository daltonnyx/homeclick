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

        /// <summary>
        /// Danh sách sản phẩm đang trong giảm giá
        /// </summary>
        /// <param name="id">Sale id</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var sale = db.Sales.Find(id);
            if (sale == null)
                return HttpNotFound();

            return View(sale);
        }

        /// <summary>
        /// Thêm sản phẩm vào giảm giá
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddProduct(int? id, bool? success, string successObjectName)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var sale = db.Sales.Find(id);
            if (sale == null)
                return HttpNotFound();

            ViewData["Success"] = success;
            ViewData["SuccessObjectName"] = successObjectName;

            ViewBag.Products = db.Products.Where(o => !o.Sales.Select(e => e.Id).Contains(sale.Id));
            return View(sale);
        }

        [HttpPost]
        public ActionResult AddProduct(int? saleId, int? productId)
        {
            if (saleId == null || productId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var sale = db.Sales.Find(saleId);
            var product = db.Products.Find(productId);
            if (sale == null || product == null)
                return HttpNotFound();

            sale.Products.Add(product);
            db.Entry(sale).State = System.Data.Entity.EntityState.Modified;
            if (db.SaveChanges() > 0)
                return RedirectToAction("AddProduct", new { id = saleId, success = true, successObjectName = product.name});

            return RedirectToAction("AddProduct", new { id = saleId });
        }

        /// <summary>
        /// bỏ sản phẩm khỏi giảm giá
        /// </summary>
        /// <param name="saleId">Sale id</param>
        /// <param name="productId">Product id</param>
        /// <returns></returns>
        public JsonResult RemoveProduct(int? saleId, int? productId)
        {
            object result = 0;
            if (saleId == null || productId == null)
                return Json(new HttpStatusCodeResult(HttpStatusCode.BadRequest), JsonRequestBehavior.AllowGet);
            var sale = db.Sales.Find(saleId);
            var product = db.Products.Find(productId);
            if (sale == null || product == null)
                return Json(result = new HttpStatusCodeResult(HttpStatusCode.NotFound), JsonRequestBehavior.AllowGet);

            if (sale.Products.Select(o => o.Id).Contains(product.Id))
            {
                sale.Products.Remove(product);
                db.Entry(sale).State = System.Data.Entity.EntityState.Modified;
                result = db.SaveChanges();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DataHandler(DTParameters param, Dictionary<string, object> args)
        {
            try
            {
                var sale = db.Sales.Where(o=> o.CategoryId == (int)SaleType.Product).ToList();
                if (args != null)
                    foreach (var arg in args)
                    {
                        if (arg.Key == "status")
                            switch (Convert.ToInt32(arg.Value))
                            {
                                case (int)SaleStatus.Unexpired: sale = sale.Where(o => o.EndDate > DateTime.Now).ToList();
                                    break;
                                case (int)SaleStatus.Expried: sale = sale.Where(o => o.EndDate < DateTime.Now).ToList();
                                    break;
                                default:
                                    break;
                            }
                    }

                var dtsource = sale.Select(o => new dt_sale
                {
                    id = o.Id,
                    name = o.Name,
                    percent = o.Percent,
                    startdate = o.StartDate.ToString(),
                    enddate = o.EndDate.ToString(),
                    status = Convert.ToInt32(o.Status),
                    products = o.Products.Count
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
