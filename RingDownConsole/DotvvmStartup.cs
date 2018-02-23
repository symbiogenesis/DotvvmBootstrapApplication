using DotVVM.Framework.Configuration;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ResourceManagement;
using DotvvmBootstrapApplication.Models.Enums;

namespace DotvvmBootstrapApplication
{
    public class DotvvmStartup : IDotvvmStartup
    {
        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureRoutes(config);
            ConfigureControls(config);
            ConfigureResources(config);
        }

        private void ConfigureRoutes(DotvvmConfiguration config)
        {
            config.RouteTable.Add(nameof(Routes.ExampleView), "", "Views/example.dothtml");
            config.RouteTable.Add(nameof(Routes.AdminSettings), "admin/settings", "Views/Admin/settings.dothtml");
            config.RouteTable.Add(nameof(Routes.AdminUsers), "admin/users", "Views/Admin/users.dothtml");
            config.RouteTable.Add(nameof(Routes.Login), "login", "Views/login.dothtml");

            // Uncomment the following line to auto-register all dothtml files in the Views folder
            // config.RouteTable.AutoDiscoverRoutes(new DefaultRouteStrategy(config));
        }

        private void ConfigureControls(DotvvmConfiguration config)
        {
            // register code-only controls and markup controls
            config.Markup.AddCodeControls("dot", typeof(BootstrapDataPager));

            //config.AddBusinessPackConfiguration();
            //config.AddReactBridgeConfiguration();
            //config.AddBootstrapConfiguration(new DotvvmBootstrapOptions() { IncludeBootstrapResourcesInPage = false });
            //config.AddContribTypeAheadConfiguration();
            //config.AddContribMultiSelectConfiguration();
            //config.AddContribCkEditorMinimalConfiguration();
        }

        private void ConfigureResources(DotvvmConfiguration config)
        {
            // register custom resources and adjust paths to the built-in resources
            config.Resources.Register(ResourceConstants.JQueryResourceName, new ScriptResource { Location = new UrlResourceLocation("https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"), RenderPosition = ResourceRenderPosition.Head });
        }
    }
}
