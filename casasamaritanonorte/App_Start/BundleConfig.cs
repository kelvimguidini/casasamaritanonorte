using System.Web;
using System.Web.Optimization;

namespace casasamaritanonorte
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/CustomValidation.js",
                        "~/Scripts/methods_pt.js"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender. Em seguida, quando estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/Vendor").Include(
                      "~/vendor/jquery/jquery.min.js",
                      "~/vendor/bootstrap/js/bootstrap.bundle.min.js",
                      "~/vendor/datatables/jquery.dataTables.min.js",
                      "~/vendor/datatables/dataTables.bootstrap4.min.js",
                      "~/vendor/jquery-easing/jquery.easing.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                    "~/Scripts/sb-admin-2.min.js",
                    "~/vendor/chart.js/Chart.min.js",
                    "~/Scripts/demo/chart-area-demo.js",
                    "~/Scripts/demo/chart-pie-demo.js",
                    "~/Scripts/jquery-ui/jquery-ui.js",
                    "~/Scripts/jquery.mask.js",
                    "~/Scripts/jquery.maskMoney.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/scriptSite").Include(
                      "~/Scripts/js/popper.min.js",
                      "~/Scripts/js/bootstrap.min.js",
                      "~/Scripts/js/plugins.js",
                      "~/Scripts/js/active.js",
                      "~/Scripts/jquery.mask.js",
                      "~/Scripts/jquery.maskMoney.min.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/vendor/fontawesome-free/css/all.min.css",
                      "~/Scripts/jquery-ui/jquery-ui.css",
                      "~/Content/multfile.css",
                      "~/Content/sb-admin-2.min.css"));


            bundles.Add(new StyleBundle("~/Content/cssSite").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/plugins.css",
                      "~/Content/css/style.css",
                      "~/Content/css/custom.css")); //< script src = "js/vendor/modernizr-3.5.0.min.js" ></ script >
        }
    }
}
