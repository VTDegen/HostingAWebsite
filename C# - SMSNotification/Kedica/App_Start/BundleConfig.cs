using System.Web.Optimization;

namespace SMSNotification
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            string googleFonts = "https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700";
            string[] LayoutJS = new string[]
            {
                "~/Content/assets/js/bluebird.core.js",
                "~/Content/assets/plugins/jquery/jquery-3.3.1.min.js",
                "~/Content/assets/plugins/jquery-ui/jquery-ui.min.js",
                "~/Content/assets/plugins/bootstrap/js/bootstrap.bundle.min.js",
                "~/Content/assets/plugins/slimscroll/jquery.slimscroll.js",
                "~/Content/assets/plugins/js-cookie/js.cookie.js",
                "~/Content/assets/js/jquery.cookie.min.js",
                "~/Content/assets/js/theme/default.min.js",
                "~/Content/assets/js/lodash.min.js",
                "~/Content/assets/js/apps.min.js",
                "~/Content/assets/js/Menu.js",
                "~/Content/assets/js/Custom.js"
            };
            string[] LayoutCSS = new string[]
            {
                "~/Content/assets/plugins/jquery-ui/jquery-ui.min.css",
                "~/Content/assets/plugins/bootstrap/css/bootstrap.min.css",
                "~/Content/assets/plugins/font-awesome/css/all.min.css",
                "~/Content/assets/plugins/animate/animate.min.css",
                "~/Content/assets/css/default/style.min.css",
                "~/Content/assets/css/default/style-responsive.min.css",
                "~/Content/assets/css/Custom.css"
            };
            string[] DataTblJS = new string[]
            {
                "~/Content/assets/plugins/DataTables/media/js/jquery.dataTables.js",
                "~/Content/assets/plugins/DataTables/media/js/dataTables.bootstrap.min.js",
                "~/Content/assets/plugins/DataTables/extensions/Responsive/js/dataTables.responsive.min.js",
                "~/Content/assets/js/demo/table-manage-fixed-header.demo.min.js",
            };
            string[] DataTblCSS = new string[]
            {
                "~/Content/assets/plugins/DataTables/media/css/dataTables.bootstrap.min.css",
                "~/Content/assets/plugins/DataTables/extensions/Responsive/css/responsive.bootstrap.min.css"
            };
            string[] TrxJS = new string[]
            {
                "~/Content/assets/plugins/iziToast/dist/js/iziToast.min.js",
                "~/Content/assets/plugins/iziModal/js/iziModal.js",
                "~/Content/assets/plugins/Parsleyjs/dist/parsley.min.js",
                "~/Content/assets/js/Classes/common/Message.js",
                "~/Content/assets/js/Classes/common/Formatter.js",
                "~/Content/assets/js/Classes/common/CustomUI.js",
                "~/Content/assets/js/Classes/common/Data.js"
            };
            string[] TrxCSS = new string[]
            {
                "~/Content/assets/plugins/iziToast/dist/css/iziToast.min.css",
                "~/Content/assets/plugins/iziModal/css/iziModal.css",
                "~/Content/assets/plugins/Parsleyjs/src/parsley.min.css"
            };
            bundles.Add(new StyleBundle("~/Home-CSS", googleFonts)
                     .Include(LayoutCSS)
                        .Include(DataTblCSS)
                        .Include(TrxCSS)
                       .Include(
                            "~/Content/assets/plugins/DataTables/extensions/Select/css/select.bootstrap.min.css",
                            "~/Content/assets/plugins/select2/dist/css/select2.css"
                        )
            );

            bundles.Add(new ScriptBundle("~/Home-JS")
                     .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Content/assets/plugins/select2/dist/js/select2.full.js",
                        "~/Content/assets/js/Dashboard.js"
                    )
            );
            bundles.Add(new StyleBundle("~/Login-CSS", googleFonts)
                  .Include(LayoutCSS)
                        .Include(DataTblCSS)
                        .Include(TrxCSS)
                       .Include(
                            "~/Content/assets/plugins/DataTables/extensions/Select/css/select.bootstrap.min.css",
                            "~/Content/assets/plugins/select2/dist/css/select2.css"
                        )
            );

            bundles.Add(new ScriptBundle("~/Login-JS")
                     .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Content/assets/plugins/select2/dist/js/select2.full.js",
                        "~/Content/assets/js/Login.js"
                    )
            );
            RegisterMasterMaintenaceBundles(bundles, LayoutJS, LayoutCSS, TrxJS, TrxCSS, DataTblJS, DataTblCSS);
            RegisterTransactionBundles(bundles, LayoutJS, LayoutCSS, TrxJS, TrxCSS, DataTblJS, DataTblCSS);
            RegisterReportBundles(bundles, LayoutJS, LayoutCSS, TrxJS, TrxCSS, DataTblJS, DataTblCSS);
            RegisterAuditTrailBundles(bundles, LayoutJS, LayoutCSS, TrxJS, TrxCSS, DataTblJS, DataTblCSS);
            RegisterRestoreInformationBundles(bundles, LayoutJS, LayoutCSS, TrxJS, TrxCSS, DataTblJS, DataTblCSS);
        }
        public static void RegisterMasterMaintenaceBundles(BundleCollection bundles, string[] LayoutJS, string[] LayoutCSS, string[] TrxJS, string[] TrxCSS, string[] DataTblJS, string[] DataTblCSS)
        {
            bundles.Add(new StyleBundle("~/UserMaster-CSS")
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
                    .Include(
                        "~/Content/assets/plugins/DataTables/extensions/Select/css/select.bootstrap.min.css",
                        "~/Content/assets/plugins/select2/dist/css/select2.css")
                    );
            bundles.Add(new ScriptBundle("~/UserMaster-Js")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Content/assets/plugins/DataTables/extensions/Select/js/dataTables.select.min.js",
                        "~/Content/assets/plugins/select2/dist/js/select2.full.js",
                        "~/Areas/MasterMaintenance/Scripts/UserMaster.js"
                    )
            );
            bundles.Add(new StyleBundle("~/PageMaster-CSS")
                   .Include(LayoutCSS)
                   .Include(DataTblCSS)
                   .Include(TrxCSS)
                   .Include(
                       "~/Content/assets/plugins/DataTables/extensions/Select/css/select.bootstrap.min.css",
                       "~/Content/assets/plugins/select2/dist/css/select2.css")
                   );
            bundles.Add(new ScriptBundle("~/PageMaster-Js")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Content/assets/plugins/DataTables/extensions/Select/js/dataTables.select.min.js",
                        "~/Content/assets/plugins/select2/dist/js/select2.full.js",
                        "~/Areas/MasterMaintenance/Scripts/PageMaster.js"
                    )
            );
            

            bundles.Add(new StyleBundle("~/GeneralMaster-CSS")
                 .Include(LayoutCSS)
                 .Include(DataTblCSS)
                 .Include(TrxCSS)
                 .Include(
                     "~/Content/assets/plugins/DataTables/extensions/Select/css/select.bootstrap.min.css",
                     "~/Content/assets/plugins/select2/dist/css/select2.css")
                 );
            bundles.Add(new ScriptBundle("~/GeneralMaster-Js")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Content/assets/plugins/DataTables/extensions/Select/js/dataTables.select.min.js",
                        "~/Content/assets/plugins/select2/dist/js/select2.full.js",
                        "~/Areas/MasterMaintenance/Scripts/GeneralMaster.js"
                    )
            );

        }
        public static void RegisterTransactionBundles(BundleCollection bundles, string[] LayoutJS, string[] LayoutCSS, string[] TrxJS, string[] TrxCSS, string[] DataTblJS, string[] DataTblCSS)
        {
            bundles.Add(new StyleBundle("~/SendingAnnouncement-CSS")
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
                    .Include(
                        "~/Content/assets/plugins/DataTables/extensions/Select/css/select.bootstrap.min.css",
                        "~/Content/assets/plugins/select2/dist/css/select2.css")
                    );
            bundles.Add(new ScriptBundle("~/SendingAnnouncement-Js")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Content/assets/plugins/DataTables/extensions/Select/js/dataTables.select.min.js",
                        "~/Content/assets/plugins/select2/dist/js/select2.full.js",
                        "~/Areas/Transaction/Scripts/SendingAnnouncement.js"
                    )
            );
            bundles.Add(new StyleBundle("~/Announcement-CSS")
                   .Include(LayoutCSS)
                   .Include(DataTblCSS)
                   .Include(TrxCSS)
                   .Include(
                          "~/Content/assets/css/default/app.min.css"
                        )
                   );
                  
            bundles.Add(new ScriptBundle("~/Announcement-Js")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Areas/Transaction/Scripts/Announcement.js"
                    )
                    );

        }
        public static void RegisterReportBundles(BundleCollection bundles, string[] LayoutJS, string[] LayoutCSS, string[] TrxJS, string[] TrxCSS, string[] DataTblJS, string[] DataTblCSS)
        {
            bundles.Add(new StyleBundle("~/ReportGeneration-CSS")
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
                    .Include(
                        "~/Content/assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css",
                        "~/Content/assets/plugins/DataTables/extensions/Select/css/select.bootstrap.min.css",
                        "~/Content/assets/plugins/select2/dist/css/select2.css")
                    );
            bundles.Add(new ScriptBundle("~/ReportGeneration-Js")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                 "~/Content/assets/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js",
                        "~/Content/assets/plugins/DataTables/extensions/Select/js/dataTables.select.min.js",
                        "~/Content/assets/plugins/select2/dist/js/select2.full.js",
                        "~/Areas/Report/Scripts/ReportGeneration.js"
                    )
            );

        }
        public static void RegisterAuditTrailBundles(BundleCollection bundles, string[] LayoutJS, string[] LayoutCSS, string[] TrxJS, string[] TrxCSS, string[] DataTblJS, string[] DataTblCSS)
        {
            bundles.Add(new StyleBundle("~/UserLogs-CSS")
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
                    .Include(
                        "~/Content/assets/plugins/DataTables/extensions/Select/css/select.bootstrap.min.css",
                        "~/Content/assets/plugins/select2/dist/css/select2.css")
                    );
            bundles.Add(new ScriptBundle("~/UserLogs-Js")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Content/assets/plugins/DataTables/extensions/Select/js/dataTables.select.min.js",
                        "~/Content/assets/plugins/select2/dist/js/select2.full.js",
                        "~/Areas/AuditTrail/Scripts/UserLogs.js"
                    )
            );

        }
        public static void RegisterRestoreInformationBundles(BundleCollection bundles, string[] LayoutJS, string[] LayoutCSS, string[] TrxJS, string[] TrxCSS, string[] DataTblJS, string[] DataTblCSS)
        {
            bundles.Add(new StyleBundle("~/RestoreInformation-CSS")
                    .Include(LayoutCSS)
                    .Include(DataTblCSS)
                    .Include(TrxCSS)
                    .Include(
                        "~/Content/assets/plugins/DataTables/extensions/Select/css/select.bootstrap.min.css",
                        "~/Content/assets/plugins/select2/dist/css/select2.css")
                    );
            bundles.Add(new ScriptBundle("~/RestoreInformation-Js")
                    .Include(LayoutJS)
                    .Include(DataTblJS)
                    .Include(TrxJS)
                    .Include(
                        "~/Content/assets/plugins/DataTables/extensions/Select/js/dataTables.select.min.js",
                        "~/Content/assets/plugins/select2/dist/js/select2.full.js",
                        "~/Areas/RestoreInformation/Scripts/RestoreInformation.js"
                    )
            );

        }

    }
}
