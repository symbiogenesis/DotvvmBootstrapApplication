using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(RingDownCentralConsole.Startup))]
namespace RingDownCentralConsole
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IdentityConfig.ConfigureAuth(app);
        }
    }
}
