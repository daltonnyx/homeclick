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
    public class SaleController : BaseController
    {
        public enum SaleStatus { Unexpired, Expried }

        public virtual ActionResult List()
        {
            //sử dụng dictionary cho tìm kiếm dữ liệu ở jquery databales
            //xem DataHandler function
            var dic = new Dictionary<string, Dictionary<string, object>>();

            //tìm kiếm thông qua tình trạng của sale, 
            var statusPair = new Dictionary<string, object>();
            statusPair.Add(SaleStatus.Unexpired.ToString(), (int)SaleStatus.Unexpired);
            statusPair.Add(SaleStatus.Expried.ToString(), (int)SaleStatus.Expried);

            dic.Add("status", statusPair);
            ViewBag.Dic = dic;
            return View();
        }

        public ActionResult Create(bool? success, string successObjectName)
        {
            ViewData["success"] = success;
            ViewData["successObjectName"] = successObjectName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SaleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = new Sale
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Percent = viewModel.percent,
                    StartDate = Convert.ToDateTime(viewModel.startDate).ToUniversalTime(),
                    EndDate = Convert.ToDateTime(viewModel.endDate).ToUniversalTime(),
                    CreateUserId = User.Identity.GetUserId(),
                    CreateTime = DateTime.Now.ToUniversalTime(),
                    CategoryId = (int)SaleType.Product
                };
                db.Sales.Add(model);
                db.SaveChanges();
                return RedirectToAction("Create", new { success = true, successObjectName = model.Name });
            }
            return View(viewModel);
        }

        public ActionResult Edit(int? id)
        {
            object result = 0;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var sale = db.Sales.Find(id);
            if (sale == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            var viewModel = new SaleViewModel
            {
                saleId = sale.Id,
                Name = sale.Name,
                Description = sale.Description,
                percent = sale.Percent,
                startDate = sale.StartDate.ToLocalTime().ToString("yyyy-MM-ddThh:mm"),
                endDate = sale.EndDate.ToLocalTime().ToString("yyyy-MM-ddThh:mm"),
                status = sale.Status
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SaleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = db.Sales.Find(viewModel.saleId);
                if (model != null)
                {
                    model.Name = viewModel.Name;
                    model.Description = viewModel.Description;
                    model.Percent = viewModel.percent;
                    model.StartDate = Convert.ToDateTime(viewModel.startDate).ToUniversalTime();
                    model.EndDate = Convert.ToDateTime(viewModel.endDate).ToUniversalTime();
                    model.Status = viewModel.status;
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }
            return View(viewModel);
        }

        public JsonResult Lock(int id)
        {
            var sale = db.Sales.Find(id);
            object result;
            if (sale != null)
            {
                sale.Status = false;
                db.Entry(sale).State = System.Data.Entity.EntityState.Modified;
                result = db.SaveChanges();
            }
            else
                result = (int)HttpStatusCode.NotFound;

            return Json(result);
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var sale = db.Sales.Find(id);
            object result;
            if (sale != null)
            {
                try
                {
                    db.Sales.Remove(sale);
                    result = db.SaveChanges();
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
            else
                result = (int)HttpStatusCode.NotFound;
            return Json(result);
        }
    }
}
