using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Homeclick
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Project",
            url: "Project/Details/{projectId}",
            defaults: new { controller = "Project", action = "Details", projectId = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "Product",
                url: "Product/{model_id}/{typo_id}/{mate_id}",
                defaults: new { controller = "Category", action = "Filter", model_id = UrlParameter.Optional, typo_id = UrlParameter.Optional, mate_id = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
