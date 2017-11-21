using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Homeclick
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Areas.Manager.ManagerBundlesConfig.RegisterBundles(BundleTable.Bundles);
            App_Start.ExtendAreaViewEngine engine = new App_Start.ExtendAreaViewEngine();
            engine.AddViewLocationFormat("~/Areas/Manager/Views/{1}/{0}.cshtml");
            engine.AddViewLocationFormat("~/Areas/Manager/Views/{1}/{0}.vbhtml");
            engine.AddViewLocationFormat("~/Areas/Manager/Views/Shared/{1}/{0}.cshtml");
            engine.AddViewLocationFormat("~/Areas/Manager/Views/Shared/{1}/{0}.vbhtml");

            // Add a shared location too, as the lines above are controller specific
            engine.AddPartialViewLocationFormat("~/Areas/Manager/Views/{0}.cshtml");
            engine.AddPartialViewLocationFormat("~/Areas/Manager/Views/{0}.vbhtml");
            engine.AddPartialViewLocationFormat("~/Areas/Manager/Views/Shared/{0}.cshtml");
            engine.AddPartialViewLocationFormat("~/Areas/Manager/Views/Shared/{0}.vbhtml");
            ViewEngines.Engines.Add(engine);
        }
        /*
        public MvcApplication() // constructor
        {
            PreRequestHandlerExecute += new EventHandler(OnPreRequestHandlerExecute);
            EndRequest += new EventHandler(OnEndRequest);
        }

        protected void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpContextProvider.OnBeginRequest();   // preserves HttpContext.Current for use across async/await boundaries.            
        }

        protected void OnEndRequest(object sender, EventArgs e)
        {
            HttpContextProvider.OnEndRequest();
        }
        */
    }
}
