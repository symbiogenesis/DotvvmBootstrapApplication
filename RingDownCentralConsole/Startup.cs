using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RingDownCentralConsole.Startup))]
namespace RingDownCentralConsole
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
