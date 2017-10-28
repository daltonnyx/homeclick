using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homeclick.App_Start
{
    public class ExtendAreaViewEngine : RazorViewEngine
    {
        public void AddViewLocationFormat(string paths)
        {
            List<string> existingPaths = new List<string>(ViewLocationFormats);
            existingPaths.Add(paths);

            this.ViewLocationFormats = existingPaths.ToArray();
        }

        public void AddPartialViewLocationFormat(string paths)
        {
            List<string> existingPaths = new List<string>(PartialViewLocationFormats);
            existingPaths.Add(paths);

            this.PartialViewLocationFormats = existingPaths.ToArray();
        }
    }
}