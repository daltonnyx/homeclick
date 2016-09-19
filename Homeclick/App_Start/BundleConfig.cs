using System.Web;
using System.Web.Optimization;

namespace Homeclick
{
    public class BundleConfig
    {

        private static void RegisterCSS(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/lib/owl-carousel/css").Include(
                "~/Content/lib/owl-carousel/owl.carousel.css",
                "~/Content/lib/owl-carousel/owl.theme.css",
                "~/Content/lib/owl-carousel/owl.transitions.css"
                ));
        }

        private static void RegisterJS(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Content/lib/owl-carousel/js").Include(
                "~/Content/lib/owl-carousel/owl.carousel.min.js"
                ));
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            RegisterCSS(bundles);
            RegisterJS(bundles);

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js",
                    "~/Scripts/jquery.unobtrusive-ajax.js",
                    "~/Scripts/plugins/ResponsiveMenu/flaunt/flaunt.js",
                    "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/nivo-slider.css",
                    "~/Content/style.css",
                    "~/Content/site.css",
                    "~/Content/responsive.css",
                    "~/Content/fonts/fonts.css",
                    "~/Content/font-awesome/css/font-awesome.min.css",
                    "~/Scripts/plugins/ResponsiveMenu/flaunt/menu.css"
                      ));


        }
    }
}
