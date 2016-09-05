using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VCMS.Lib.Models;

namespace VCMS.Lib.Controllers
{
    public class UsersController: UserManageController 
    {
        private string[] roles
        {
            get
            {
                var roles = RoleManager.Roles.Select(o => o.Name).ToArray();
                return roles;
            }
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewData["Roles"] = roles;
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                System.Security.Claims.Claim cl = new System.Security.Claims.Claim("", "");
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.Roles.Count() > 0)
                        result = await UserManager.AddToRolesAsync(user.Id, model.Roles);

                    if (result.Succeeded)
                        return RedirectToAction("List");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            ViewData["Roles"] = roles;
            return View(model);
        }

        public async Task<ActionResult> Edit(string user_id)
        {
            var user = await UserManager.FindByIdAsync(user_id);
            if (user != null)
            {
                var model = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Lockout = await UserManager.IsLockedOutAsync(user_id),
                };

                var userRoles = await UserManager.GetRolesAsync(user_id);
                model.Roles = userRoles.ToArray();

                if (model.Lockout)
                    model.LockoutEndDate = (await UserManager.GetLockoutEndDateAsync(user_id)).DateTime.ToLocalTime();

                ViewData["Roles"] = roles;
                return View(model);
            }
            return RedirectToAction("Dashboard", "Page");
        }

        //
        // POST: /Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id);
                user.Email = model.Email;
                user.UserName = model.UserName;

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var lockoutEndDate = model.Lockout ? (model.LockoutEndDate != null ? new DateTimeOffset(model.LockoutEndDate.Value.ToUniversalTime()) : DateTimeOffset.MaxValue)
                        : DateTimeOffset.MinValue;

                    result = await UserManager.SetLockoutEndDateAsync(user.Id, lockoutEndDate);
                    if (result.Succeeded)
                    {
                        var oldRoles = (await UserManager.GetRolesAsync(user.Id)).ToArray();
                        result = await UserManager.RemoveFromRolesAsync(user.Id, oldRoles);
                        if (result.Succeeded)
                        {
                            if (model.Roles != null)
                                result = await UserManager.AddToRolesAsync(user.Id, model.Roles);

                            if (result.Succeeded)
                                return RedirectToAction("List");
                        }
                    }
                }
                AddErrors(result);
            }

            ViewData["Roles"] = roles;
            return View(model);
        }


        public async Task<ActionResult> ChangePassword(string user_id)
        {
            var user = await UserManager.FindByIdAsync(user_id);
            if (user != null)
            {
                var model = new ChangePasswordViewModel
                {
                    Id = user.Id,
                };
                return View(model);
            }
            return RedirectToAction("Dashboard", "Pages");
        }

        //
        // POST: /Users/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.RemovePasswordAsync(model.Id);
                if (result.Succeeded)
                {
                    result = await UserManager.AddPasswordAsync(model.Id, model.NewPassword);
                    if (result.Succeeded)
                        return RedirectToAction("List");
                }
                AddErrors(result);
            }

            return View(model);
        }

        public async Task<ActionResult> Delete(string user_id)
        {
            var user = await UserManager.FindByIdAsync(user_id);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("Dashboard", "Page");
        }

        //
        // POST: /Users/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteComfirmed(string user_id)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(user_id);
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("List");
                AddErrors(result);
            }
            return View(user_id);
        }

        public async Task<ActionResult> Details(string user_id)
        {
            var user = await UserManager.FindByIdAsync(user_id);
            if (user != null)
            {
                var model = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Lockout = await UserManager.IsLockedOutAsync(user.Id)
                };

                if (model.Lockout)
                    model.LockoutEndDate = user.LockoutEndDateUtc.Value.ToLocalTime();

                return View(model);
            }
            return RedirectToAction("Dashboard", "Page");
        }

        public async Task<JsonResult> DataHandler(DTParameters param)
        {
            try
            {
                var dtsource = db.Users.Select(o => new UserViewModel
                {
                    Id = o.Id,
                    UserName = o.UserName,
                    Email = o.Email,
                }).ToList();

                foreach (var user in dtsource)
                {
                    user.Lockout = await UserManager.IsLockedOutAsync(user.Id);
                }

                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<UserViewModel, bool>> pre = (p => (search == null || (p.UserName != null && p.UserName.ToLower().Contains(search.ToLower()) || p.Email != null && p.Email.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.UserName != null && p.UserName.ToLower().Contains(columnSearch[0].ToLower())))
                && (columnSearch[1] == null || (p.Email != null && p.Email.ToLower().Contains(columnSearch[1].ToLower()))));

                List<UserViewModel> data = new ResultSet().GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<UserViewModel> result = new DTResult<UserViewModel>
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
