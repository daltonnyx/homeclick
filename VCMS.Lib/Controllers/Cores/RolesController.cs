using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
                    return RedirectToAction("List");
                }
                AddErrors(result);
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(string role_id)
        {
            var role = await RoleManager.FindByIdAsync(role_id);
            if (role != null)
            {
                var model = new RoleViewModel { Id = role.Id, Name = role.Name };
                return View(model);
            }
            return RedirectToAction("Dashboard", "Pages");
        }

        //
        // POST: /Users/DeleteComfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(model.Id);
                if (role != null)
                {
                    var result = await RoleManager.DeleteAsync(role);
                    if (result.Succeeded)
                        return RedirectToAction("List");
                    AddErrors(result);
                }
                else
                {
                    AddErrors("Role not found!");
                }
            }
            return View(model);
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

        public JsonResult DataHandler(DTParameters param)
        {
            try
            {
                var dtsource = db.Roles.Select(o => new RoleViewModel { Id = o.Id, Name = o.Name}).ToList();

                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<RoleViewModel, bool>> pre = (p => (search == null || (p.Name != null && p.Name.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.Name != null && p.Name.ToLower().Contains(columnSearch[1].ToLower()))));

                var resultSet = new ResultSet();
                List<RoleViewModel> data = resultSet.GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<RoleViewModel> result = new DTResult<RoleViewModel>
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
