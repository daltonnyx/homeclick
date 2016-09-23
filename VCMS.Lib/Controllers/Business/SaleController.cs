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
        public enum SaleStatus { All, Unexpired, Expried }

        public virtual ActionResult List()
        {
            //sử dụng dic cho tìm kiếm dữ liệu ở jquery databales
            var dic = new Dictionary<string, object>();

            //tìm kiếm thông qua tình trạng của sale, 
            var statusPair = new Dictionary<string, object>();
            //tất cả sale
            statusPair.Add(SaleStatus.All.ToString(), SaleStatus.All);
            //sale chưa hết hạn
            statusPair.Add(SaleStatus.Unexpired.ToString(), SaleStatus.Unexpired);
            //sale đã hết hạn
            statusPair.Add(SaleStatus.Expried.ToString(), SaleStatus.Expried);

            dic.Add("status", statusPair);
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
                    StartDate = viewModel.startDate,
                    EndDate = viewModel.endDate,
                    CreateUserId = User.Identity.GetUserId(),
                    CreateTime = DateTime.Now,
                    CategoryId = (int)SaleType.Product
                };
                db.Sales.Add(model);
                db.SaveChanges();
                return RedirectToAction("Create", new { success = true, successObjectName = model.Name });
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
