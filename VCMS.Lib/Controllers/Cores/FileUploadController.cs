using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VCMS.Lib.Common;
using VCMS.Lib.Models;

namespace VCMS.Lib.Controllers
{
    public class FileUploadController : BaseController
    {
        private async Task<List<object>> uploads(IEnumerable<HttpPostedFileBase> files, FileGroups fileGroup)
        {
                var result = new List<object>();
            foreach (var file in files)
            {
                var newFile = await Uploader.Upload(file, fileGroup, this);
                if (newFile != null)
                {
                    result.Add(new
                    {
                        oldFileName = file.FileName,
                        newFileName = newFile.Id,
                        ext = newFile.Extension,
                    });
                }
            }
            return result;
        }

        private async Task<dynamic> upload(HttpPostedFileBase file, FileGroups fileGroup, string imagesFolderPath)
        {
            try
            {
                var newFile = await Uploader.Upload(file, fileGroup, this);
                var result = new
                {
                    oldFileName = file.FileName,
                    newFileName = newFile.Id,
                    ext = newFile.Extension,
                    link = imagesFolderPath + newFile.Id + newFile.Extension,
                };
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<ActionResult> UploadP(IEnumerable<HttpPostedFileBase> files)
        {
            var result = await uploads(files, FileGroups.ProductImage);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> UploadPV(IEnumerable<HttpPostedFileBase> files)
        {
            var result = await uploads(files, FileGroups.ProductVariantImage);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file, int fileGroup)
        {
            try
            {
                var imagesFolderPath = Url.GetImageUploadFolder();
                var result = await upload(file, (FileGroups)fileGroup, imagesFolderPath);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<ActionResult> Uploads(IEnumerable<HttpPostedFileBase> files, int fileGroup)
        {
            var result = await uploads(files, (FileGroups)fileGroup);
            return Json(result);
        }

        public async void DeleteFile(string id)
        {
            var file = db.Files.Find(id);
            if (file != null)
            {
                db.Files.Remove(file);
                await db.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteFileByPath(string src)
        {
            var result = 0;
            var id = Path.GetFileNameWithoutExtension(src);
            var file = db.Files.Find(id);
            if (file != null)
            {
                db.Files.Remove(file);
                result = await db.SaveChangesAsync();
            }
            return Json(result);
        }
    }
}
