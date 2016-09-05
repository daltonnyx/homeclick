﻿using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class UrlHelperExtender
    {
        public static string GetBaseUrl(this UrlHelper helper)
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }

        public static string GetImageUploadFolder(this UrlHelper helper)
        {
            return helper.GetBaseUrl() + VCMS.Lib.Properties.Resources.UploadFolder_Image + "/";
        }
    }
}
