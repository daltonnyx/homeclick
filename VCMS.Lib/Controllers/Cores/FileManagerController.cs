using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VCMS.Lib.Models;
using System.Linq.Expressions;
using VCMS.Lib.Models.Business;
using Microsoft.AspNet.Identity;
using VCMS.Lib.Common;

namespace VCMS.Lib.Controllers
{
    public class FileManagerController : UserManageController
    {
        
        // GET: Manager/FileController
        public ActionResult List()
        {
            return View();
        }

        // GET: Manager/FileController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = await db.Files.FindAsync(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // GET: Manager/FileController/Create
        public ActionResult Create()
        {
            return View(new CreateFileViewModel());
        }

        // POST: Manager/FileController/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var uploadResult = await Uploader.Upload(model, FileGroups.Other, this);
                if (uploadResult)
                    return RedirectToAction("List");
            }
            return View(model);
        }

        // GET: Manager/FileController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = await db.Files.FindAsync(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: Manager/FileController/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Link,FileTypeId,Size,CreateUserId,CreateTime,UpdateUserId,UpdateTime")] File file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(file).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(file);
        }

        [HttpPost]
        public void Delete(string id)
        {
            File file = db.Files.Find(id);
            if (file != null)
            {
                Uploader.DeleteFile(file, this);
                db.Files.Remove(file);
                db.SaveChanges();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult DataHandler(DTParameters param)
        {
            try
            {
                //var imageTypeId = (int)CategoryTypes.FileImage;
                var files = db.Files;

                var dtsource = new List<FileViewModel>();

                foreach (var file in files)
                {
                    dtsource.Add(new FileViewModel
                    {
                        Id = file.Id,
                        Name = file.Id,
                        Ext = file.Extension,
                        FileType = file.FileType.ToString(),
                        Size = file.Size.ToString(),
                        CreateTime = file.CreateTime.ToString()
                    });
                }


                List<String> columnSearch = new List<string>();

                foreach (var col in param.Columns)
                {
                    columnSearch.Add(col.Search.Value);
                }
                var search = param.Search.Value;

                Expression<Func<FileViewModel, bool>> pre = (p => (search == null || (p.Name != null && p.Name.ToLower().Contains(search.ToLower())))
                && (columnSearch[0] == null || (p.Name != null && p.Name.ToLower().Contains(columnSearch[1].ToLower()))));

                List<FileViewModel> data = new ResultSet().GetResult(pre, param.SortOrder, param.Start, param.Length, dtsource);

                var jsonResult = new List<object>();
                int count = new ResultSet().Count(pre, dtsource);
                DTResult<FileViewModel> result = new DTResult<FileViewModel>
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
