﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private async Task<List<dynamic>> uploads(IEnumerable<HttpPostedFileBase> files, FileGroups fileGroup, string imagesFolderPath)
        {
            var result = new List<dynamic>();
            foreach (var file in files)
            {
                var newFile = await Uploader.Upload(file, fileGroup, this);
                if (newFile != null)
                {
                    var resultItem = new
                    {
                        oldFileName = file.FileName,
                        newFileName = newFile.Id,
                        ext = newFile.Extension,
                        link = imagesFolderPath + newFile.Id + newFile.Extension
                    };
                    result.Add(resultItem);
                }
            }
            return result;
        }

        private async Task<dynamic> upload(HttpPostedFileBase file, FileGroups fileGroup, string imagesFolderPath)
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

        [HttpPost]
        public async Task<ActionResult> UploadP(IEnumerable<HttpPostedFileBase> files)
        {
            var imagesFolderPath = Url.GetImageUploadFolder();
            var result = await uploads(files, FileGroups.ProductImage, imagesFolderPath);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> UploadPV(IEnumerable<HttpPostedFileBase> files)
        {
            var imagesFolderPath = Url.GetImageUploadFolder();
            var result = await uploads(files, FileGroups.ProductVariantImage, imagesFolderPath);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file, int fileGroup)
        {

            var imagesFolderPath = Url.GetImageUploadFolder();
            var result = await upload(file, (FileGroups)fileGroup, imagesFolderPath);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> Uploads(IEnumerable<HttpPostedFileBase> files, int fileGroup)
        {
            var imagesFolderPath = Url.GetImageUploadFolder();
            var result = await uploads(files, (FileGroups)fileGroup, imagesFolderPath);
            return Json(result);
        }
    }
}
