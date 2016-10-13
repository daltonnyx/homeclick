using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Common;
using VCMS.Lib.Models;

namespace VCMS.Lib.Controllers
{
    using static ConstantKeys;
    using static TemplateStrings;

    public class UsersController: UserManageController 
    {
        private string[] Getroles()
        {
            return RoleManager.Roles.Select(o => o.Name).ToArray();
        }

        public const int CustomfieldTypeId = 125;
        private IEnumerable<CustomField> GetCustomFields(bool getAll = false)
        {
            IEnumerable<CustomField> result = new List<CustomField>();
            var customfieldType = db.Categories.Find(CustomfieldTypeId);
            if (customfieldType != null)
                result = getAll ? customfieldType.CustomFields : customfieldType.CustomFields.Where(o => o.Status);
            return result;
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewData[ROLES] = Getroles();
            ViewData[CUSTOM_FIELDS] = GetCustomFields();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model, Dictionary<string, string> customfield)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                if(customfield != null)
                {
                    foreach (var obj in customfield)
                    {
                        var detail = new User_Detail
                        {
                            Name = obj.Key,
                            Value = obj.Value,
                        };
                        user.Details.Add(detail);
                    }
                }
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.RoleList.Count() > 0)
                        result = await UserManager.AddToRolesAsync(user.Id, model.RoleList);

                    if (result.Succeeded)
                        return RedirectToAction("List");
                }
                AddErrors(result);
            }
            ViewData[ROLES] = Getroles();
            ViewData[CUSTOM_FIELDS] = GetCustomFields();
            return View(model);
        }

        public async Task<ActionResult> Edit(string user_id)
        {
            var user = await UserManager.FindByIdAsync(user_id);
            if (user == null)
                return HttpNotFound();

            var model = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                LockoutEnabled = user.LockoutEnabled,
            };

            var userRoles = await UserManager.GetRolesAsync(user_id);
            model.RoleList = userRoles.ToArray();

            if (model.LockoutEnabled)
            {
                model.LockoutEndDate = (await UserManager.GetLockoutEndDateAsync(user_id)).DateTime.ToLocalTime();
                if (model.LockoutEndDate == DateTime.MinValue || model.LockoutEndDate < DateTime.Now)
                    model.LockoutEndDate = null;
            }

            foreach (var detail in user.DetailsToDictionary())
            {
                ModelState.Add(string.Format("{0}[{1}]", CUSTOM_FIELD, detail.Key), new ModelState { Value = new ValueProviderResult(new string[] { detail.Value }, detail.Value, CultureInfo.CurrentCulture) });
            }
            ViewData[ROLES] = Getroles();
            ViewData[CUSTOM_FIELDS] = GetCustomFields();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel model, Dictionary<string, string> customfield)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(model.Id);
                user.Email = model.Email;
                user.UserName = model.UserName;
                //Custom field
                if (customfield != null)
                {
                    user.Details.ToList().ForEach(o =>
                    {
                        if (customfield.ContainsKey(o.Name))
                        {
                            o.Value = customfield[o.Name];
                            customfield.Remove(o.Name);
                        }
                        else
                            user.Details.Remove(o);
                    });

                    foreach (var obj in customfield)
                    {
                        user.Details.Add(new User_Detail { Name = obj.Key, Value = obj.Value, });
                    }
                }

                UserManager.Update(user);
                //Change password
                if (model.Password != null)
                {
                    UserManager.RemovePassword(user.Id);
                    UserManager.AddPassword(user.Id, model.Password);
                }
                //Set lockout enabled
                UserManager.SetLockoutEnabled(user.Id, model.LockoutEnabled);
                //Set lockout end time
                if (model.LockoutEnabled)
                {
                    var lockoutEndDate = (model.LockoutEndDate != null) ? new DateTimeOffset(model.LockoutEndDate.Value.ToUniversalTime()) : DateTimeOffset.MinValue;
                    UserManager.SetLockoutEndDate(user.Id, lockoutEndDate);
                }
                //Remove current role and add new roles
                UserManager.RemoveFromRoles(user.Id, UserManager.GetRoles(user.Id).ToArray());
                UserManager.AddToRoles(user.Id, model.RoleList);

                return RedirectToAction("List");
            }

            ViewData[ROLES] = Getroles();
            ViewData[CUSTOM_FIELDS] = GetCustomFields();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(string[] ids)
        {
            var messageCollection = new List<Message>();
            var successes = new List<string>();
            foreach (var user_id in ids)
            {
                var user = await UserManager.FindByIdAsync(user_id);
                var username = user.UserName;
                try
                {
                    var result = UserManager.Delete(user);
                    if (!result.Succeeded)
                        foreach (var err in result.Errors)
                        {
                            messageCollection.Add(new Message { MessageType = MessageTypes.Danger, MessageContent = err });
                        }
                    else
                        successes.Add(string.Format(HTML_STRONG, user.UserName));
                }
                catch (Exception)
                {
                    messageCollection.Add(new Message { MessageType = MessageTypes.Danger, MessageContent = string.Format(MODEL_DELETE_ERROR_UNKNOW, user.UserName) });
                }
            }
            if (successes.Count != 0)
                messageCollection.Add(new Message { MessageType = MessageTypes.Warning, MessageContent = string.Format(MODEL_DELETE_RESULT, "Roles", string.Join(", ", successes.ToArray()), successes.Count) });

            TempData[ACTION_RESULT_MESSAGES] = messageCollection;
            return RedirectToAction("List");
        }

        #region[Datatables]
        private DTResult<T> getResult<T>(DTParameters param) where T : Dictionary<string, object>, new()
        {
            var dtsource = new List<T>();
            var users = db.Users;
            foreach (var user in users)
            {
                var details = user.DetailsToDictionary();

                var name = string.Empty;
                if (details.ContainsKey("last_name"))
                    name = details["last_name"];
                if (details.ContainsKey("first_name"))
                    name += " " + details["first_name"];
                DateTime? lockout = UserManager.GetLockoutEndDate(user.Id).DateTime.ToLocalTime();

                var obj = new T {
                        {"id", user.Id },
                        {"name", name},
                        {"username", user.UserName},
                        {"email", user.Email},
                        {"lockout", lockout <= DateTime.Now || lockout <= DateTime.MinValue.ToLocalTime() ? null : lockout}
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
