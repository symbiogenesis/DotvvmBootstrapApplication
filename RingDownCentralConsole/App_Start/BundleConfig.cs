using System.Web.Optimization;
using System.Web.UI;

namespace RingDownCentralConsole
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            const string jQueryVersion = "3.3.1";

            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-" + jQueryVersion + ".min.js", //your path will be ignored
                DebugPath = "~/Scripts/jquery-" + jQueryVersion + ".js",  //your path will be ignored 
                CdnPath = "http://code.jquery.com/jquery.min.js",
                CdnDebugPath = "http://code.jquery.com/jquery-latest.js",
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "window.jQuery"
            });

            const string jQueryUiVersion = "1.12.1";

            ScriptManager.ScriptResourceMapping.AddDefinition("jquery-ui", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-ui-" + jQueryUiVersion + ".min.js", //your path will be ignored
                DebugPath = "~/Scripts/jquery-ui-" + jQueryUiVersion + ".js",  //your path will be ignored 
                CdnPath = $"http://code.jquery.com/ui/{jQueryUiVersion}/jquery-ui.min.js",
                CdnDebugPath = $"http://code.jquery.com/ui/{jQueryUiVersion}/jquery-ui.js",
                CdnSupportsSecureConnection = true
            });

            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/WebParts.js"));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));
        }
    }
}