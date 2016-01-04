using System.Web;
using System.Web.Optimization;

namespace Blog
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Windows Metro Style
            bundles.Add(new StyleBundle("~/bundles/metro").Include(
                "~/Content/metro.css",
                "~/Content/metro-*"
            ));

            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-{version}.min.map"
            ));

            // Ajax
            bundles.Add(new ScriptBundle("~/bundles/ajax").Include(
                "~/Scripts/jquery.unobtrusive-ajax.js"
            ));

            // Плагин валидации
            bundles.Add(new ScriptBundle("~/bundles/validate").Include(
                "~/Scripts/jquery.validate*"
            ));

            // Элементы пользовательского интерфейса
            bundles.Add(new ScriptBundle("~/bundles/widgets").Include(
                "~/Scripts/jquery.dataTables.js",
                "~/Scripts/metro.js"
            ));
        }
    }
}