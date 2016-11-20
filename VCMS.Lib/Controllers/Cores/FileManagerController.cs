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
using Microsoft.AspNet.Identity;
using VCMS.Lib.Common;
using System.Text;
using System.Data.SqlClient;

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

        [HttpDelete]
        public JsonResult Delete(string id)
        {
            var result = "";
            var file = db.Files.Find(id);
            if (file != null)
                try
                {
                    db.Files.Remove(file);
                    Uploader.DeleteFile(file, this);
                    db.SaveChanges();
                    result = "Success";
                }
                catch (Exception ex)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Delete failure!");
                    var innerException = ex.InnerException.InnerException;
                    if (innerException as SqlException != null)
                    {
                        sb.AppendLine("Error code: " + ((ex.InnerException.InnerException) as SqlException).Number);
                    }
                    result = sb.ToString();
                }
            else
                result = "Incorrect ID!";

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFiles()
        {
            var files = db.Files.OrderByDescending(o => o.CreateTime);
            var result = new List<object>();
            foreach (var file in files)
            {
                var fullpath = "~/" + System.IO.Path.Combine(Properties.Resources.UploadFolder_Image, "thumb", file.FullFileName);
                var hasThumb = false;
                if (System.IO.File.Exists(Server.MapPath(fullpath)))
                    hasThumb = true;
                result.Add(new
                {
                    filename = file.Id,
                    ext = file.Extension,
                    thumb = hasThumb,
                });
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFileByPath(string src)
        {
            var result = 0;
            var id = System.IO.Path.GetFileNameWithoutExtension(src);
            var file = db.Files.Find(id);
            if (file != null)
            {
                db.Files.Remove(file);
                result = db.SaveChanges();
            }
            return Json(result);
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
    }
}
