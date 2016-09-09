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
    public class FeatureProductsController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add()
        {
            ViewBag.Products = Products;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FeatureProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = db.Products.Find(viewModel.ProductId);
                if (model != null)
                {
                    model.Featured = true;
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        public void Remove(int? id)
        {
            var model = db.Products.Find(id);
            if (model != null)
            {
                model.Featured = false;
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<SelectListItem> Products {
            get
            {
                var products = db.Products.Where(o => !o.Featured).Select(o => new SelectListItem { Text = o.name, Value = (o.Id).ToString() }).ToList();
                return products;
            }
        }

        public JsonResult DataHandler(DTParameters param)
        {
            try
            {
                var products = db.Products.Where(o => o.Featured == true);
                var dtsource = products.Select(o => new dt_product
                {
                    id = o.Id,
                    name = o.name,
                    img = o.Image.Id + o.Image.Extension,
                    status = o.Status
                }).ToList();

                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<dt_product, bool>> pre = (p => (search == null || (p.name != null && p.name.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.name != null && p.name.ToLower().Contains(columnSearch[1].ToLower()))));

                var resultSet = new ResultSet();
                List<dt_product> data = resultSet.GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<dt_product> result = new DTResult<dt_product>
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
