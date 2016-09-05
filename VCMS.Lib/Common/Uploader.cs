using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VCMS.Lib.Models;

namespace VCMS.Lib.Common
{
    public static class Uploader
    {
        public static async Task<bool> Upload(CreateFileViewModel model, Controller controller)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var guid = Guid.NewGuid();
                    var fileExt = System.IO.Path.GetExtension(model.File.FileName);
                    var newFileName = guid.ToString();

                    var newFile = new File
                    {
                        Id = newFileName,
                        Name = model.Name,
                        Description = model.Description,
                        Extension = fileExt,
                        Size = model.File.ContentLength,
                        CreateUserId = controller.User.Identity.GetUserId(),
                        CreateTime = DateTime.Now,
                    };
                    var destinationFolder = "";
                    if (fileExt == ".png" || fileExt == ".jpg")
                    {
                        newFile.FileTypeId = (int)FileTypes.Image;
                        destinationFolder = Properties.Resources.UploadFolder_Image;
                    }
                    else
                        destinationFolder = Properties.Resources.UploadFolder_Other;

                    db.Files.Add(newFile);
                    await db.SaveChangesAsync();
                    var newPath = System.IO.Path.Combine(controller.Server.MapPath("~/" + destinationFolder), newFileName + fileExt);
                    model.File.SaveAs(newPath);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static async Task<File> Upload(HttpPostedFileBase file, Controller controller)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var guid = Guid.NewGuid();
                    var fileExt = System.IO.Path.GetExtension(file.FileName);
                    var newFileName = guid.ToString();

                    var newFile = new File
                    {
                        Id = newFileName,
                        Extension = fileExt,
                        Size = file.ContentLength,
                        CreateUserId = controller.User.Identity.GetUserId(),
                        CreateTime = DateTime.Now,
                    };
                    var destinationFolder = "";
                    if (fileExt == ".png" || fileExt == ".jpg")
                    {
                        newFile.FileTypeId = (int)FileTypes.Image;
                        destinationFolder = Properties.Resources.UploadFolder_Image;
                    }
                    else
                        destinationFolder = Properties.Resources.UploadFolder_Other;

                    db.Files.Add(newFile);
                    await db.SaveChangesAsync();
                    var newPath = System.IO.Path.Combine(controller.Server.MapPath("~/" + destinationFolder), newFileName + fileExt);
                    file.SaveAs(newPath);

                    return newFile;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
