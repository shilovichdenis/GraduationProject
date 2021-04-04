using System.Web;
using System.Web.Optimization;

namespace CourseProject
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/Slick/slick.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // готово к выпуску, используйте средство сборки по адресу https://modernizr.com, чтобы выбрать только необходимые тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/ScrptSection.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/slick").Include(
                      "~/Content/Slick/slick.css",
                      "~/Content/Slick/slick-theme.css"));

            bundles.Add(new StyleBundle("~/Content/Home").Include(
                      "~/Content/HomePage.css"));


            bundles.Add(new StyleBundle("~/Account/Registration").Include(
                "~/Content/Account/Registration.css"));

            bundles.Add(new StyleBundle("~/Account/Log").Include(
                "~/Content/Account/Login.css"));


            bundles.Add(new StyleBundle("~/Admin/View").Include(
                "~/Content/Admin/ViewSt.css",
                "~/Content/Admin/InterfaceStyle.css"));

            bundles.Add(new StyleBundle("~/Admin/Manage").Include(
                "~/Content/Admin/ManageStyle.css",
                "~/Content/Admin/InterfaceStyle.css"));


            bundles.Add(new StyleBundle("~/Teacher/View").Include(
                "~/Content/Teacher/View.css",
                "~/Content/Teacher/Interface.css"));

            bundles.Add(new StyleBundle("~/Teacher/Manage").Include(
                "~/Content/Teacher/Manage.css",
                "~/Content/Teacher/Interface.css"));


            bundles.Add(new StyleBundle("~/Student/View").Include(
                "~/Content/Student/View.css",
                "~/Content/Student/InterfaceStyle.css"));


            bundles.Add(new ScriptBundle("~/Student/Script").Include(
                "~/Scripts/Student/Scripts.js"));
        }
    }
}
