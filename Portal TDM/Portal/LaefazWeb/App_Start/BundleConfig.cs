
using System.Web.Optimization;

namespace TDMWeb
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Asset/Vendor/bootstrap/css")
                                .Include("~/Assets/Vendor/bootstrap/css/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Asset/Vendor/AdminLTE/css")
                                .Include("~/Assets/Vendor/AdminLTE/css/AdminLTE.css",
                                         "~/Assets/Vendor/AdminLTE/css/skins/skin-green.css"));

            bundles.Add(new StyleBundle("~/Asset/Vendor/Datatables/css")
                                .Include("~/Assets/Vendor/Datatables/css/dataTables.bootstrap.css")
                                .Include("~/Assets/Vendor/Datatables/css/jquery.dataTables.min.css")
                                .Include("~/Assets/Vendor/Datatables/css/dataTables.bootstrap.css")
                                .Include("~/Assets/Vendor/Datatables/css/responsive.dataTables.min.css"));

            bundles.Add(new StyleBundle("~/Asset/Vendor/Select2/css")
                                .Include("~/Assets/Vendor/Select2/css/select2.css"));

            bundles.Add(new StyleBundle("~/Asset/Vendor/multi-select/css")
                                .Include("~/Assets/Vendor/multi-select/css/multi-select.css"));


            bundles.Add(new StyleBundle("~/Asset/Vendor/ColorSelect/css")
                                .Include("~/Assets/Vendor/ColorSelect/css/bootstrap-colorselector.css"));

            bundles.Add(new StyleBundle("~/Asset/Vendor/FontAwesome/css")
                                .Include("~/Assets/Vendor/FontAwesome/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Asset/Vendor/Ionicons/css")
                                .Include("~/Assets/Vendor/Ionicons/css/ionicons.css"));

            bundles.Add(new StyleBundle("~/Asset/Vendor/jquery-upload-file/css")
                                .Include("~/Assets/Vendor/jquery-upload-file/jquery.uploadfile.css"));

            bundles.Add(new StyleBundle("~/Asset/Styles/css")
                                .Include("~/Assets/Styles/LAEFaz.css", "~/Assets/Vendor/daterangepicker/css/LAEFaz.css"));

            bundles.Add(new StyleBundle("~/Asset/Vendor/fullcalendar-3.9.0")
                               .Include("~/Assets/Vendor/fullcalendar-3.9.0/fullcalendar.print.css")
                               .Include("~/Assets/Vendor/fullcalendar-3.9.0/fullcalendar.css"));

            bundles.Add(new StyleBundle("~/Asset/Vendor/Wizard/css")
                                .Include("~/Assets/Vendor/Wizard/css/paper-bootstrap-wizard.css"));
                                //.Include("~/Assets/Vendor/Wizard/css/themify-icons.css"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/Asset/Vendor/modernizr").Include(
                        "~/Assets/Vendor/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Asset/Vendor/bootstrap/js")
                                .Include("~/Assets/Vendor/bootstrap/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/Asset/Vendor/js")
                                //.Include("~/Assets/Vendor/jquery-2.2.3.js")
                                .Include("~/Assets/Vendor/jquery.validate.js")
                                .Include("~/Assets/Vendor/jquery.validate.messages_pt_BR.js")
                                .Include("~/Assets/Vendor/jquery.slimscroll.js")
                                .Include("~/Assets/Vendor/additional-methods.js")
                                .Include("~/Assets/Vendor/fastclick.js")
                                .Include("~/Assets/Vendor/modernizr-2.6.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery_3_3_1").Include("~/Assets/Vendor/jquery-3.3.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery_2_2_3").Include("~/Assets/Vendor/jquery-2.2.3.js"));

            bundles.Add(new ScriptBundle("~/Script/Jquery")
                .Include("~/Scripts/jquery-2.2.3.intellisense.js")
                .Include("~/Scripts/jquery-ui-1.11.4.js")
                .Include("~/Scripts/jquery-ui-1.11.4.min.js"));

            bundles.Add(new ScriptBundle("~/Asset/Vendor/bootstrap-daterangepicker")
                .Include("~/Assets/Vendor/bootstrap-daterangepicker/daterangepicker.css"));

            bundles.Add(new ScriptBundle("~/Asset/Vendor/AdminLTE/js")
                                .Include("~/Assets/Vendor/AdminLTE/js/app.js"));

            bundles.Add(new ScriptBundle("~/Asset/Vendor/Datatables/js")
                                .Include("~/Assets/Vendor/Datatables/js/jquery.dataTables.js")
                                .Include("~/Assets/Vendor/Datatables/js/jquery.dataTables.min.js")
                                .Include("~/Assets/Vendor/Datatables/js/dataTables.bootstrap.js")
                                .Include("~/Assets/Vendor/Datatables/js/dataTables.checkboxes.js")
                                .Include("~/Assets/Vendor/Datatables/js/dataTables.responsive.min.js")
                                .Include("~/Assets/Vendor/Datatables/js/dataTables.fixedHeader.js"));

            bundles.Add(new ScriptBundle("~/Asset/Vendor/DatatablesEditor/js")
                                .Include("~/Assets/Vendor/Datatables/js/dataTables.editor.min.js"));

            bundles.Add(new ScriptBundle("~/Asset/Vendor/Select2/js")
                                .Include("~/Assets/Vendor/Select2/js/select2.full.js"));

            bundles.Add(new ScriptBundle("~/Asset/Vendor/multi-select/js")
                                .Include("~/Assets/Vendor/multi-select/js/jquery.multi-select.js"));

            bundles.Add(new ScriptBundle("~/Asset/Vendor/ColorSelect/js")
                    .Include("~/Assets/Vendor/ColorSelect/js/bootstrap-colorselector.js"));

            bundles.Add(new ScriptBundle("~/Asset/Vendor/jquery-upload-file/js")
                                .Include("~/Assets/Vendor/jquery-upload-file/jquery.uploadfile.js"));

            bundles.Add(new ScriptBundle("~/Asset/Scripts/js")
                                .Include("~/Assets/Scripts/daterangepicker.js")
                                .Include("~/Assets/Scripts/LAEFaz.js")
                                .Include("~/Assets/Scripts/moment.min.js")
                                .Include("~/Assets/Scripts/heatmap.js"));

            //scripts e css do calendario
            bundles.Add(new ScriptBundle("~/Asset/Vendor/fullcalendar-3.9.0")
                                .Include("~/Assets/Vendor/fullcalendar-3.9.0/fullcalendar.js"));

            bundles.Add(new ScriptBundle("~/Asset/Vendor/Wizard/js")
                                .Include("~/Assets/Vendor/Wizard/js/jquery.bootstrap.wizard.js")
                                .Include("~/Assets/Vendor/Wizard/js/paper-bootstrap-wizard.js"));

            

            
        }
    }
}