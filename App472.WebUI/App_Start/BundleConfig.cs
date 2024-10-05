using System.Web;
using System.Web.Optimization;

namespace App472.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css"
                ,"~/Content/ErrorStyles.css"
                ,"~/Content/Site.css"
            ));

            //Use the development version of Modernizr to develop with and learn from. Then, when your ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new Bundle("~/bundles/js").Include(
                "~/Scripts/jquery-3.4.1.js",

                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",

                //"~/Scripts/modernizr-2.8.3.js",
                "~/Scripts/bootstrap.bundle.js" // bundled version is needed for Popper, which gives us dropdown buttons.
            ));

        }
    }
}
