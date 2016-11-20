using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Common;
using VCMS.Lib.Models;
using VCMS.Lib.Models.Datatables;

namespace VCMS.Lib.Controllers
{
    public class ProductOptionsController : BaseController
    {
        #region[Methods]
        private IEnumerable<CustomField> GetCustomFields()
        {
            return db.CustomFields.Where(o => o.CategoryId == (int)CustomFieldType.ProductOption);
        }

        private IEnumerable<Category> GetVariantTypes()
        {
            return db.Categories.Where(o => o.CategoryTypeId == (int)CategoryTypes.ProductVariant);
        }

        #endregion

        public ActionResult List(int product_id)
        {
            var product = db.Products.Find(product_id);
            if (product == null)
                return HttpNotFound();

            ViewData["Options"] = product.Product_Options;
            ViewData["ProductName"] = product.Name;
            return View();
        }

        public ActionResult Create(int product_id)
        {
            var product = db.Products.Find(product_id);
            if (product == null)
                return HttpNotFound();

            ViewData["ProductName"] = product.Name;
            ViewData[ConstantKeys.PRODUCT_VARIANT_TYPES] = GetVariantTypes();
            ViewData[ConstantKeys.CUSTOM_FIELDS] = GetCustomFields();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product_Option model)
        {
            var product = db.Products.Find(model.ProductId);
            var messageCollection = new List<Message>();

            if (ModelState.IsValid)
            {
                if (model.variantTypes != null)
                {
                    model.Product_Variants = new List<Product_Variant>();
                    foreach (var variantType in model.variantTypes)
                    {
                        foreach (var variantId in variantType.Value)
                        {
                            var variant = db.Product_Variants.Find(variantId);
                            model.Product_Variants.Add(variant);
                        }
                    }
                }

                var userId = User.Identity.GetUserId();
                var currentTimeUtc = DateTime.UtcNow;
                foreach (var detail in model.Product_Options_Details)
                {
                    detail.CreateUserId = userId;
                    detail.CreateTime = currentTimeUtc;
                    db.Entry(detail).State = System.Data.Entity.EntityState.Added;
                }
                model.CreateUserId = userId;
                model.CreateTime = currentTimeUtc;

                db.Entry(model).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Create successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;

                return RedirectToAction("Edit", new {id = model.Id });
            }

            ViewData["ProductName"] = product.Name;
            ViewData[ConstantKeys.PRODUCT_VARIANT_TYPES] = GetVariantTypes();
            ViewData[ConstantKeys.CUSTOM_FIELDS] = GetCustomFields();

            if (model.PreviewImageId != null)
                model.PreviewImage = db.Files.Find(model.PreviewImageId);

            foreach (var item in model.Product_Options_Details)
            {
                if (item.Type == ((int)FieldTypes.File).ToString())
                {
                    item.File = db.Files.Find(item.FileId);
                }
            }
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OptionId">Option Id</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var option = db.Product_Options.Find(id);
            if (option == null)
                return HttpNotFound();

            ViewData[ConstantKeys.PRODUCT_VARIANT_TYPES] = GetVariantTypes();
            ViewData[ConstantKeys.CUSTOM_FIELDS] = GetCustomFields();
            return View(option);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product_Option model)
        {
            var modelTarget = db.Product_Options.Find(model.Id);
            var messageCollection = new List<Message>();

            if (ModelState.IsValid)
            {
                modelTarget.Product_Variants.Clear();
                if (model.variantTypes != null)
                    foreach (var variantType in model.variantTypes)
                    {
                        foreach (var variantId in variantType.Value)
                        {
                            var variant = db.Product_Variants.Find(variantId);
                            modelTarget.Product_Variants.Add(variant);
                        }
                    }

                var userId = User.Identity.GetUserId();
                var currentTimeUtc = DateTime.UtcNow;

                //Remove absent details
                var existDetailId = modelTarget.Product_Options_Details.Select(o => o.Id);
                var exceptDetailIds = existDetailId.Except(model.Product_Options_Details.Select(o => o.Id)).ToList();
                foreach (var exceptDetailId in exceptDetailIds)
                {
                    var details = db.Product_Options_Details.Find(exceptDetailId);
                    db.Entry(details).State = System.Data.Entity.EntityState.Deleted;
                }

                //Modify or add details
                foreach (var detail in model.Product_Options_Details)
                {
                    if (detail.Id != 0)
                    {
                        var detailTarget = modelTarget.Product_Options_Details.FirstOrDefault(o => o.Id == detail.Id);
                        db.Entry(detailTarget).CurrentValues.SetValues(detail);
                        db.Entry(detailTarget).Property("ProductOptionId").IsModified = false;
                        db.Entry(detailTarget).Property("CreateUserId").IsModified = false;
                        db.Entry(detailTarget).Property("CreateTime").IsModified = false;
                    }
                    else
                    {
                        detail.CreateUserId = userId;
                        detail.CreateTime = currentTimeUtc;
                        db.Entry(detail).State = System.Data.Entity.EntityState.Added;
                    }
                }

                db.Entry(modelTarget).CurrentValues.SetValues(model);
                db.Entry(modelTarget).Property("CreateUserId").IsModified = false;
                db.Entry(modelTarget).Property("CreateTime").IsModified = false;
                db.SaveChanges();

                messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Update successfully!" });
                TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;
                return RedirectToAction("Edit", new { id = model.Id });
            }

            ViewData[ConstantKeys.PRODUCT_VARIANT_TYPES] = GetVariantTypes();
            ViewData[ConstantKeys.CUSTOM_FIELDS] = GetCustomFields();

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int[] ids, string returnUrl)
        {
            var messageCollection = new List<Message>();
            foreach (var id in ids)
            {
                var model = db.Product_Options.Find(id);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            }
            db.SaveChanges();

            messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = "Delete successfully!" });
            TempData[ConstantKeys.ACTION_RESULT_MESSAGES] = messageCollection;
            return Redirect(returnUrl);
        }
    }
}