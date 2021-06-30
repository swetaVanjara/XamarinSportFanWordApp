using System.Web;
using System.Web.Optimization;

namespace Fanword.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            BundleTable.EnableOptimizations = true;

            #region CSS

            bundles.Add(new StyleBundle("~/Bundle/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            #endregion

            #region Script Bundles

            bundles.Add(new ScriptBundle("~/bundles/Scripts/jquery").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery-2.2.1.js"
                        ));

            bundles.Add(new ScriptBundle("~/Bundles/Scripts/jqueryval").Include(
                       "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Bundles/Scripts/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));




            bundles.Add(new ScriptBundle("~/Bundles/Scripts/DefaultJs").Include(
                   "~/Scripts/moment.js",
                    "~/Scripts/kendo.modernizr.custom.js",
                    "~/Scripts/linq.js"
            ));



            bundles.Add(new ScriptBundle("~/Bundles/Scripts/AngularJs").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-animate.min.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/AngularPromiseButtons/angular-promise-buttons.js",
                "~/Scripts/angular-moment.js",
                "~/Scripts/elastic.js",
                "~/Scripts/angular-ui-router.js"
                ));

            bundles.Add(new ScriptBundle("~/Bundles/FanwordAngularAppJs").Include("~/TypeScripts/App/Fanword.App.js"));


            bundles.Add(new ScriptBundle("~/Bundles/PocoModels").IncludeDirectory("~/TypeScripts/PocoModels", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/Directives").IncludeDirectory("~/TypeScripts/Directives", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/Sports").IncludeDirectory("~/TypeScripts/Sports", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/Schools").IncludeDirectory("~/TypeScripts/Schools", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/Teams").IncludeDirectory("~/TypeScripts/Teams", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/UserManagement").IncludeDirectory("~/TypeScripts/UserManagement", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/RssFeeds").IncludeDirectory("~/TypeScripts/RssFeeds", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/Rankings").IncludeDirectory("~/TypeScripts/Rankings", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/NewsNotifications").IncludeDirectory("~/TypeScripts/NewsNotifications", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/Events").IncludeDirectory("~/TypeScripts/Events", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/Admin/Advertising").IncludeDirectory("~/TypeScripts/Advertising/Admins", "*.js", true));
            bundles.Add(new ScriptBundle("~/Bundles/Advertising").IncludeDirectory("~/TypeScripts/Advertising/Advertisers", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/ContentManagement").IncludeDirectory("~/TypeScripts/ContentManagement", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/ContentSources").IncludeDirectory("~/TypeScripts/ContentSources", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/AdvertiserRegistration").IncludeDirectory("~/TypeScripts/AdvertiserRegistration", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/ContentSourceAdmin").IncludeDirectory("~/TypeScripts/ContentSourceAdmin", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/Comments").IncludeDirectory("~/TypeScripts/Comments", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/Likes").IncludeDirectory("~/TypeScripts/Likes", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/UserAdmins").IncludeDirectory("~/TypeScripts/UserAdmins", "*.js", true));

            bundles.Add(new ScriptBundle("~/Bundles/Dashboard").IncludeDirectory("~/TypeScripts/Dashboard", "*.js", true));

            #endregion

            #region Metronic Bundles

            bundles.Add(new StyleBundle("~/Bundle/Theme/MetronicGlobalCss").Include(
               "~/Theme/assets/global/plugins/uniform/css/uniform.default.css",
                   "~/Theme/assets/global/css/components-rounded.min.css",
                   "~/Theme/assets/global/css/plugins.min.css",
                   "~/Theme/assets/layouts/layout/css/layout.min.css",
                   "~/Theme/assets/layouts/layout/css/themes/default.min.css", // this should be changed based on user, for now its just this
                   "~/Theme/assets/layouts/layout/css/custom.min.css",
                   "~/Theme/assets/global/plugins/simple-line-icons/simple-line-icons.min.css"
               ));

            bundles.Add(new ScriptBundle("~/Bundles/Theme/MetronicGlobalJs").Include(
                "~/Theme/assets/global/plugins/js.cookie.min.js",
                "~/Theme/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                "~/Theme/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/Theme/assets/global/plugins/jquery.blockui.min.js",
                "~/Theme/assets/global/plugins/uniform/jquery.uniform.min.js",
                "~/Theme/assets/global/plugins/bootstrap-switch/js/boostrap-switch.min.js",
                "~/Theme/assets/global/scripts/app.min.js",
                "~/Theme/assets/layouts/layout/scripts/layout.min.js",
                "~/Theme/assets/layouts/global/scripts/quick-sidebar.min.js"
               ));

            #endregion

        }
    }
}
