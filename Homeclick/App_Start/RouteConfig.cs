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
            /*
            //Project
            routes.MapRoute(
                name: "ProjectsMap",
                url: "Project/Map",
                defaults: new { controller = "Project", action = "Map" }
            );

            routes.MapRoute(
            name: "Project",
            url: "Project/Details/{projectId}",
            defaults: new { controller = "Project", action = "Details", projectId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ProjectCollectionDetails",
                url: "View/Project/{action}/{collection_id}",
                defaults: new { controller = "Project", action = "Details" }
            );


            routes.MapRoute(
                name: "ProjectCategory",
                url: "Project/{category_id}",
                defaults: new { controller = "Project", action = "Category" }
            );

            routes.MapRoute(
                name: "ProjectDetails",
                url: "Project/{category_id}/{project_id}",
                defaults: new { controller = "Project", action = "Details" }
            );

            //JSON
            routes.MapRoute(
                 name: "Json",
                 url: "JSON/{action}",
                 defaults: new { controller = "JSON" }
             );

            //San pham
            routes.MapRoute(
                 name: "sanpham",
                 url: "sanpham/{action}",
                 defaults: new { controller = "sanpham" }
             );

            routes.MapRoute(
                 name: "page",
                 url: "page/{action}",
                 defaults: new { controller = "page"}
             );

            routes.MapRoute(
                 name: "Typologies",
                 url: "category/Typologies",
                 defaults: new { controller = "Category", action = "Typologies" }
             );

            routes.MapRoute(
                 name: "DefaultForCategory",
                 url: "{controller}/{category_id}",
                 defaults: new { action = "Category"}
             );

            routes.MapRoute(
                name: "CollectionDetail",
                url: "{controller}/{category_id}/{collection_id}",
                defaults: new {action = "Detail"}
            );

            routes.MapRoute(
                name: "CollectionList",
                url: "BoSuuTap/{category_id}",
                defaults: new { controller = "BoSuuTap", action = "List" }
            );

            routes.MapRoute(
                name: "Product",
                url: "Product/{model_id}/{typo_id}/{mate_id}",
                defaults: new { controller = "Category", action = "Filter", model_id = UrlParameter.Optional, typo_id = UrlParameter.Optional, mate_id = UrlParameter.Optional}
            );

 */
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
