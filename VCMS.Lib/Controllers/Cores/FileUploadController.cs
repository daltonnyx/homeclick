using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VCMS.Lib.Common;

namespace VCMS.Lib.Controllers
{
    public class FileUploadController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult> Upload(IEnumerable<HttpPostedFileBase> files)
        {
            var result = new List<object>();
            foreach (var file in files)
            {
                var newFile = await Uploader.Upload(file, this);
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
