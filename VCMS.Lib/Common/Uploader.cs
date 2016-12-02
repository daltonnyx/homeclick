using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        public static Image CreateThumbnail(this Image image, Size thumbnailSize)
        {
            int scaledWidth = thumbnailSize.Width;
            int scaledHeight = thumbnailSize.Height;
            int scaledLeft = (thumbnailSize.Width - scaledWidth) / 2;
            int scaledTop = (thumbnailSize.Height - scaledHeight) / 2;

            // For portrait mode, adjust the vertical top of the crop area so that we get more of the top area
            if (scaledWidth < scaledHeight && scaledHeight > thumbnailSize.Height)
            {
                scaledTop = (thumbnailSize.Height - scaledHeight) / 4;
            }

            Rectangle cropArea = new Rectangle(scaledLeft, scaledTop, scaledWidth, scaledHeight);

            System.Drawing.Image thumbnail = new Bitmap(thumbnailSize.Width, thumbnailSize.Height);
            using (Graphics thumbnailGraphics = Graphics.FromImage(thumbnail))
            {
                thumbnailGraphics.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                thumbnailGraphics.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraphics.DrawImage(image, cropArea);
            }
            return thumbnail;
        }

        public static async Task<bool> Upload(CreateFileViewModel model, FileGroups fileGroup, Controller controller)
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

                    var categoryG = await db.Categories.FindAsync((int)fileGroup);
                    newFile.Categories.Add(categoryG);

                    if (fileExt == ".png" || fileExt == ".jpg")
                    {
                        var categoryT = await db.Categories.FindAsync((int)FileTypes.Image);
                        newFile.Categories.Add(categoryT);
                        destinationFolder = Properties.Resources.UploadFolder_Image;
                    }
                    else
                    {
                        var categoryT = await db.Categories.FindAsync((int)FileTypes.Other);
                        newFile.Categories.Add(categoryT);
                        destinationFolder = Properties.Resources.UploadFolder_Other;
                    }

                    db.Files.Add(newFile);
                    await db.SaveChangesAsync();
                    var newPath = System.IO.Path.Combine(controller.Server.MapPath("~/" + destinationFolder), newFileName + fileExt);
                    model.File.SaveAs(newPath);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<File> Upload(HttpPostedFileBase file, FileGroups fileGroup, Controller controller)
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

                    var categoryG = await db.Categories.FindAsync((int)fileGroup);
                    newFile.Categories.Add(categoryG);

                    if (fileExt == ".png" || fileExt == ".jpg" || fileExt == ".gif" || fileExt == ".svg")
                    {
                        var categoryT = await db.Categories.FindAsync((int)FileTypes.Image);
                        newFile.Categories.Add(categoryT);
                        destinationFolder = Properties.Resources.UploadFolder_Image;
                    }
                    else
                    {
                        var categoryT = await db.Categories.FindAsync((int)FileTypes.Other);
                        newFile.Categories.Add(categoryT);
                        destinationFolder = Properties.Resources.UploadFolder_Other;
                    }

                    db.Files.Add(newFile);
                    await db.SaveChangesAsync();
                    if(!System.IO.Directory.Exists(controller.Server.MapPath("~/" + destinationFolder)))
                    {
                        System.IO.Directory.CreateDirectory(controller.Server.MapPath("~/" + destinationFolder));
                    }
                    var newPath = System.IO.Path.Combine(controller.Server.MapPath("~/" + destinationFolder), newFileName + fileExt);
                    file.SaveAs(newPath);

                    //thumb
                    if (fileExt == ".png" || fileExt == ".jpg" || fileExt == ".gif")
                        using (Image img = Image.FromFile(newPath))
                        {
                            var thumb = img.CreateThumbnail(new Size(256, Convert.ToInt32((img.Height / (img.Width / 256)))));
                            thumb.Save(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(newPath), "thumb", System.IO.Path.GetFileName(newPath)));
                        }

                    return newFile;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void DeleteFile(File file, Controller controller)
        {
            var destinationFolder = "";
            if (file.Extension == ".png" || file.Extension == ".jpg")
                destinationFolder = Properties.Resources.UploadFolder_Image;
            else
                destinationFolder = Properties.Resources.UploadFolder_Other;

            string fullPath = controller.Request.MapPath("~/" + destinationFolder + "//" + file.Id + file.Extension);
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
            var thumbPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fullPath), "thumb", System.IO.Path.GetFileName(fullPath));
            if (System.IO.File.Exists(thumbPath))
                System.IO.File.Delete(thumbPath);
        }
    }
}
