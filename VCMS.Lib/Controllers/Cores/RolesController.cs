using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Common;
using VCMS.Lib.Models;

namespace VCMS.Lib.Controllers
{
    using static ConstantKeys;
    using static TemplateStrings;

    public class RolesController : UserManageController
    {
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole { Name = model.Name };
                var result = await RoleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    var messageCollection = new List<Message>();
                    messageCollection.Add(new Message { MessageType = MessageTypes.Success, MessageContent = string.Format("<strong>{0}</strong> has been created!", role.Name)});
                    TempData[ACTION_RESULT_MESSAGES] = messageCollection;
                    return RedirectToAction("Create");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(string[] ids)
        {
            var messageCollection = new List<Message>();
            var successes = new List<string>();
            foreach (var id in ids)
            {
                var role = await RoleManager.FindByIdAsync(id);
                if (role != null)
                    try
                    {
                        var result = RoleManager.Delete(role);
                        if (!result.Succeeded)
                            foreach (var err in result.Errors)
                            {
                                messageCollection.Add(new Message { MessageType = MessageTypes.Danger, MessageContent = err });
                            }
                        else
                            successes.Add(string.Format(HTML_STRONG, role.Name));
                    }
                    catch (Exception)
                    {
                        messageCollection.Add(new Message { MessageType = MessageTypes.Danger, MessageContent = string.Format(MODEL_DELETE_ERROR_UNKNOW, role.Name) });
                    }
            }
            if (successes.Count != 0)
                messageCollection.Add(new Message { MessageType = MessageTypes.Warning, MessageContent = string.Format(MODEL_DELETE_RESULT, "Roles", string.Join(", ", successes.ToArray()), successes.Count) });

            TempData[ACTION_RESULT_MESSAGES] = messageCollection;
            return RedirectToAction("List");
        }

        public async Task<ActionResult> Edit(string role_id)
        {
            var user = await RoleManager.FindByIdAsync(role_id);
            if (user != null)
            {
                var model = new RoleViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                };
                return View(model);
            }
            return RedirectToAction("Dashboard", "Page");
        }

        //
        // POST: /Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(model.Id);
                role.Name = model.Name;

                var result = await RoleManager.UpdateAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("List");

                AddErrors(result);
            }
            return View(model);
        }

        #region[Datatables]
        private DTResult<T> getResult<T>(DTParameters param) where T : Dictionary<string, object>, new()
        {
            var dtsource = new List<T>();
            var roles = db.Roles;
            foreach (var role in roles)
            {
                var obj = new T {
                        {"id", role.Id },
                        {"name", role.Name},
                        {"users", role.Users.Count}
                    };
                dtsource.Add(obj);
            }
            return JDatatables<T>.GetDTResult(param, dtsource);
        }

        public JsonResult DataHandler(DTParameters param)
        {
            try
            {
                var result = getResult<Dictionary<string, object>>(param);
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
