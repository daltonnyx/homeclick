using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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
        private async Task<List<object>> upload(IEnumerable<HttpPostedFileBase> files, FileGroups fileGroup)
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
                        ext = newFile.Extension
                    });
                }
            }
            return result;
        }

        [HttpPost]
        public async Task<ActionResult> UploadP(IEnumerable<HttpPostedFileBase> files)
        {
            var result = await upload(files, FileGroups.ProductImage);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> UploadPV(IEnumerable<HttpPostedFileBase> files)
        {
            var result = await upload(files, FileGroups.ProductVariantImage);
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
    }
}
