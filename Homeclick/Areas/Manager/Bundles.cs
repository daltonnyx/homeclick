using System.Web.Optimization;

namespace Homeclick.Areas.Manager
{
    public class ManagerBundlesConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/manager/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/manager/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/manager/flaty").Include(
                    "~/Areas/Manager/Content/Themes/FLATY/jquery.slimscroll.min.js",
                    "~/Areas/Manager/Content/Themes/FLATY/jquery.cookies.js",
                    "~/Areas/Manager/Content/Themes/FLATY/flaty.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/manager/datatables").Include(
                    "~/Areas/Manager/Content/Themes/Datatables/jquery.dataTables.js",
                    "~/Areas/Manager/Content/Themes/Datatables/dataTables.bootstrap.js"
                    ));

            bundles.Add(new StyleBundle("~/bundles/manager/datatables/css").Include(
                    "~/Areas/Manager/Content/Themes/Datatables/jquery.dataTables.css"
                    ));

            bundles.Add(new StyleBundle("~/content/manager/css").Include(
                      "~/Content/font-awesome/css/font-awesome.min.css",
                      "~/Content/bootstrap.css",
                      "~/Areas/Manager/Content/Themes/Admin.css"
                      ));
        }
    }
}