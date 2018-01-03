using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Common;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Datatables;


namespace VCMS.Lib.Controllers
{
    public class SymbolsController : BaseController
    {

        #region [Methods]
        private IEnumerable<Product_Type> GetProductTypes()
        {
            return db.Product_Types;
        }
        #endregion

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewData[ConstantKeys.PRODUCT_TYPES] = this.GetProductTypes();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Symbol model)
        {
            var messageCollection = new List<Message>();
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var currentTimeUtc = DateTime.UtcNow;
                model.CreateUserId = userId;
                model.CreateTime = currentTimeUtc;
                db.Symbols.Add(model);
                db.SaveChanges();

                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Create successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;

                return RedirectToAction("Edit", new { symbol_id = model.Id });
            }
            ViewData[ConstantKeys.PRODUCT_TYPES] = this.GetProductTypes();
            return View();
        }

        public ActionResult Edit(int symbol_id)
        {
            ViewData[ConstantKeys.PRODUCT_TYPES] = this.GetProductTypes();
            var model = db.Symbols.Find(symbol_id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Symbol model)
        {
            var messageCollection = new List<Message>();
            if (ModelState.IsValid)
            {
                var modelTarget = db.Symbols.Find(model.Id);
                db.Entry(modelTarget).CurrentValues.SetValues(model);
                db.SaveChanges();

                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Update successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;

                return RedirectToAction("Edit", new { symbol_id = model.Id });
            }
            ViewData[ConstantKeys.PRODUCT_TYPES] = this.GetProductTypes();
            return View();
        }

        #region[Datatables]
        private IQueryable<Symbol> FilterDbSource(Dictionary<string, string> args)
        {
            var result = db.Symbols.AsQueryable();
            if (args != null)
                foreach (var arg in args)
                {

                }
            return result;
        }

        private DTResult<T> GetDtResult<T>(DTParameters param, Dictionary<string, string> args) where T : Dictionary<string, object>, new()
        {
            var dtsource = new List<T>();
            var dbSource = FilterDbSource(args);

            foreach (var item in dbSource)
            {
                dtsource.Add(new T
                {
                    {ConstantKeys.ID, item.Id },
                    {ConstantKeys.PREVIEW_IMAGE, item.Svg.FullFileName },
                    {ConstantKeys.NAME, item.Name },
                    {ConstantKeys.PRODUCT_TYPE, item.ProductType.Name }
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
