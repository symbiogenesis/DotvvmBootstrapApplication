using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace DotvvmBootstrapApplication.WebAPI
{
    public static class Program
    {
        public static void Main(string[] args) => BuildWebHost(args).Run();

        public static IWebHost BuildWebHost(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                            .UseKestrel()
                            .UseIISIntegration()
                            .UseSerilog()
                            .UseStartup<Startup>()
                            .UseUrls("http://localhost:3456")
                            .Build();
    }
}
