using System.Web.Optimization;

namespace Homeclick.Areas.Manager
{
    public class ManagerBundlesConfig
    {
        public static void RegisterJS(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/manager/jquery").Include(
                "~/areas/manager/content/lib/jquery/jquery-1.11.0.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/manager/bootstrap").Include(
                "~/Areas/Manager/Content/lib/bootstrap/bootstrap.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/manager/flaty").Include(
                "~/Areas/Manager/Content/lib/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/Areas/Manager/Content/lib/jquery-cookies/jquery.cookies.js",
                "~/Areas/Manager/Content/js/flaty/flaty.js",
                "~/Areas/Manager/Content/js/admin.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/manager/datatables").Include(
                "~/Areas/Manager/Content/lib/datatables/jquery.dataTables.js",
                "~/Areas/Manager/Content/lib/Datatables/dataTables.bootstrap.js"
                ));
        }

        public static void RegisterCSS(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/manager/datatables/css").Include(
                //"~/Areas/Manager/Content/lib/datatables/jquery.dataTables.css",
                "~/Areas/Manager/Content/lib/datatables/dataTables.bootstrap.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/manager/bootstrap/css").Include(
                "~/areas/manager/content/lib/bootstrap/bootstrap.min.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/manager/font-awesome/css").Include(
                "~/content/font-awesome/css/font-awesome.min.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/manager/css").Include(
                 "~/areas/manager/content/css/flaty/flaty.css",
                 "~/areas/manager/content/css/flaty/flaty-responsive.css",
                 "~/areas/manager/content/css/utility.css",
                 "~/areas/manager/content/css/admin.css"
                 ));
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterJS(bundles);
            RegisterCSS(bundles);
        }
    }
}